using System;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Maths;
using Object = UnityEngine.Object;

namespace Utils_Project.Scene
{
    public class ULoadSceneManager : MonoBehaviour, ILoadSceneManagerStructureRead<ILoadPercentListener>
    {
        [Title("Types")] 
        [SerializeField] 
        private MainSceneLoadTransitionWrapper mainLoadScreenHandler = new MainSceneLoadTransitionWrapper();
        [SerializeReference] 
        private ILoadScenePercentHandler combatLoadScreenHandler;

        
        [Title("Params")]
        [SerializeField, SuffixLabel("deltas"), Range(.5f, 12f)] 
        private float showLoadScreenDeltaSpeed = 4f;
        [SerializeField, SuffixLabel("deltas"), Range(.5f, 12f)] 
        private float hideLoadScreenDeltaSpeed = 1f;

        

        [Title("Listeners"), ShowIf("_percentListeners")] 
        private ICollection<IScreenLoadListener> _percentListeners;

        private ICollection<ILoadPercentListener> _loadPercentListeners;

        private void Awake()
        {
            mainLoadScreenHandler.OnAwake();
            combatLoadScreenHandler?.OnAwake();

            gameObject.SetActive(false);
            LoadSceneManagerSingleton.Injection(this);

            DoAsyncFuncInitializations();
            DoSimpleTransitionInitializations();

            _percentListeners = LoadSceneManagerSingleton.ScreenLoadListeners;
            _loadPercentListeners = LoadSceneManagerSingleton.LoadPercentListeners;
        }

        public ILoadPercentListener MainLoadType => mainLoadScreenHandler;
        public ILoadPercentListener CombatLoadType => combatLoadScreenHandler;





        public bool IsLoadingScene() => _fillingCoroutineHandle.IsRunning;
        private CoroutineHandle _fillingCoroutineHandle;
        public void DoSceneTransition(
            LoadSceneParameters parameters,
            LoadSceneParameters.ISceneLoadCallback listener)
        {
            if(IsLoadingScene()) return;

            _fillingCoroutineHandle =
                Timing.RunCoroutine(_SceneSwapSequence(parameters, listener));
        }

        //// LOAD SCENE SEGMENT
        #region LOAD SCENE
        private AsyncOperation _currentLoadingAsyncOperation;
        private Func<bool> _isFinishAsyncOperation;
        private Func<float> _asyncOperationPercent;

        private IEnumerator<float> _SceneSwapSequence(
            LoadSceneParameters loadParameters,
            LoadSceneParameters.ISceneLoadCallback feedbackListener)
        {
            loadParameters.ExtractValues(
                out var sceneName,
                out var type,
                out var isAdditive, out var deltaModifier);

            var actions = new AsyncActions(DoLoadAsync, _isFinishAsyncOperation, _asyncOperationPercent);
            return _TransitionSequence(type, deltaModifier, actions, feedbackListener, loadParameters.OnLoadDelay);

            void DoLoadAsync()
            {
                var loadSceneMode = isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single;
                _currentLoadingAsyncOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            }
        }

        private void DoAsyncFuncInitializations()
        {
            if (_isFinishAsyncOperation == null) _isFinishAsyncOperation = IsFinish;
            if (_asyncOperationPercent == null) _asyncOperationPercent = CurrentPercent;


            bool IsFinish() => _currentLoadingAsyncOperation.isDone;
            float CurrentPercent() => _currentLoadingAsyncOperation.progress;
        }
        #endregion



        //// JUST SIMPLE TRANSITION SEGMENT
        private Func<bool> _isSimpleTransitionFinish;
        private Func<float> _simpleTransitionPercent;
        private float _currentSimpleTransitionTimerThreshold;
        private float _currentSimpleTransitionTimer;

        private void DoSimpleTransitionInitializations()
        {
            _isSimpleTransitionFinish = IsWaitFinish;
            _simpleTransitionPercent = CurrentLoad;

            bool IsWaitFinish()
            {
                _currentSimpleTransitionTimer += Time.deltaTime;
                return _currentSimpleTransitionTimer > _currentSimpleTransitionTimerThreshold;
            }

            float CurrentLoad() => _currentSimpleTransitionTimer / _currentSimpleTransitionTimerThreshold;
        }

        public void DoJustScreenTransition(
            float waitUntilHide, bool fromLeft, float deltaModifier = 1,
            LoadSceneParameters.ISceneLoadCallback feedbackListener = null)
        {
            Timing.KillCoroutines(_fillingCoroutineHandle);

            const float minTimerThreshold = .2f;

            _currentSimpleTransitionTimer = 0;
            _currentSimpleTransitionTimerThreshold = (waitUntilHide > minTimerThreshold) 
                ? waitUntilHide 
                : minTimerThreshold;
            _fillingCoroutineHandle = Timing.RunCoroutine(_DoScreenTransition(), Segment.RealtimeUpdate);

            IEnumerator<float> _DoScreenTransition()
            {
                var targetType = LoadSceneParameters.GetMainLoadType(fromLeft);
                var actions = new AsyncActions(null, _isSimpleTransitionFinish, _simpleTransitionPercent);
                return _TransitionSequence(targetType, deltaModifier, actions, feedbackListener);
            }
        }

        #region Transition Operation

        private static void ExtractCallersValues(LoadSceneParameters.ISceneLoadCallback listener,
            out ISceneLoadFirstLastCallListener firstLastListener,
            out ISceneHiddenListener hiddenListener,
            out ISceneLoadingListener tickingListener)
        {
            firstLastListener = null;
            hiddenListener = null;
            tickingListener = null;

            if (listener == null) return;

            if (listener is ISceneLoadFirstLastCallListener firstLastCall)
                firstLastListener = firstLastCall;
            if (listener is ISceneHiddenListener transition)
                hiddenListener = transition;
            if (listener is ISceneLoadingListener loadingListener)
                tickingListener = loadingListener;
        }

        private void _GetTransitionType(
            LoadSceneParameters.LoadType type,
            out IEnumerator<float> transitionIn, out IEnumerator<float> transitionOut,
            out ILoadScenePercentHandler handler,
            float deltaModifier)
        {
            switch (type)
            {
                case LoadSceneParameters.LoadType.CombatLoad:
                //todo
                default:
                    transitionIn = _FadeInMainLoadScreen(deltaModifier);
                    transitionOut = _FadeOutMainLoadScreen(deltaModifier);
                    handler = mainLoadScreenHandler;
                    break;
            }
        }

        private IEnumerator<float> _TransitionSequence(
            LoadSceneParameters.LoadType type,
            float deltaModifier,
            AsyncActions feedback,
            LoadSceneParameters.ISceneLoadCallback feedbackListener,
            float delay = 0)
        {
            ExtractCallersValues(feedbackListener,
                out var firstLastListener,
                out var hiddenListener,
                out var tickingListener);
            var onHideAction = feedback.OnHideSceneFeedback;
            var isFinishFunc = feedback.IsFinishFunc;
            var currentPercentFunc = feedback.CurrentPercent;

            _GetTransitionType(type,
                out var transitionIn,
                out var transitionOut,
                out var handler,
                deltaModifier);

            gameObject.SetActive(true);
            yield return Timing.WaitForOneFrame;
            CallInitialEvents();

            yield return Timing.WaitUntilDone(transitionIn);

            hiddenListener?.OnStartLoading();
            onHideAction?.Invoke();

            while (!isFinishFunc())
            {
                yield return Timing.WaitForOneFrame;
                var currentLoad = currentPercentFunc.Invoke();
                CallTickingEvents(currentLoad);
            }


            yield return Timing.WaitForOneFrame;
            yield return Timing.WaitForSeconds(delay);
            CallLastEvents();
            yield return Timing.WaitUntilDone(transitionOut);


            firstLastListener?.OnFinishTransition();
            gameObject.SetActive(false);


            void CallInitialEvents()
            {
                handler.OnStartTransition(type);
                firstLastListener?.OnStartTransition();
                foreach (var listener in _percentListeners)
                {
                    listener.OnShowLoadScreen(type);
                }
            }
            void CallTickingEvents(float currentLoad)
            {
                foreach (var listener in _loadPercentListeners)
                {
                    listener.OnPercentTick(currentLoad);
                }
                tickingListener?.OnLoadSceneTick(currentLoad);
            }

            void CallLastEvents()
            {
                handler.OnFinishLoad(type);
                hiddenListener?.OnLoadingFinish();
                foreach (var listener in _percentListeners)
                {
                    listener.OnHideLoadScreen(type);
                }
            }
        }
        private readonly struct AsyncActions
        {
            public readonly Action OnHideSceneFeedback;
            public readonly Func<bool> IsFinishFunc;
            public readonly Func<float> CurrentPercent;

            public AsyncActions(Action onHideAction, Func<bool> isFinishFunc, Func<float> currentPercentFunc)
            {
                OnHideSceneFeedback = onHideAction;
                IsFinishFunc = isFinishFunc;
                CurrentPercent = currentPercentFunc;
            }
        }

        #endregion
        #region Transition Tweens
        private const float FillOutThreshold = 1 - FillThreshold;
        private const float FillThreshold = .98f;
        private IEnumerator<float> _FadeInLoad(ILoadPercentListener handler, float deltaModifier = 1)
        {
            yield return Timing.WaitForOneFrame;


            float percent = 0;
            float deltaSpeed = showLoadScreenDeltaSpeed * deltaModifier;
            while (percent < FillThreshold)
            {
                yield return Timing.WaitForOneFrame;

                float deltaStep = Time.deltaTime * deltaSpeed;
                percent += deltaStep;
                handler.OnPercentTick(percent);

                foreach (var listener in _percentListeners)
                {
                    listener.OnFillLoadScreenPercent(percent);
                }

            }

            handler.OnPercentTick(1);
        }

        private IEnumerator<float> _FadeOutLoad(ILoadPercentListener handler, float deltaModifier = 1)
        {
            yield return Timing.WaitForOneFrame;
            float percent = 1;
            float deltaSpeed = hideLoadScreenDeltaSpeed * deltaModifier;
            while (percent > FillOutThreshold)
            {
                float deltaStep = Time.deltaTime * deltaSpeed;
                percent -= deltaStep;
                handler.OnPercentTick(percent);

                foreach (var listener in _percentListeners)
                {
                    listener.OnFillOutLoadScreenPercent(percent);
                }
                yield return Timing.WaitForOneFrame;
            }

            handler.OnPercentTick(0);
        }

        private IEnumerator<float> _FadeInMainLoadScreen(float deltaModifier = 1)
        {
            return _FadeInLoad(mainLoadScreenHandler, deltaModifier);
        }



        private IEnumerator<float> _FadeOutMainLoadScreen(float deltaModifier = 1)
        {
            return _FadeOutLoad(mainLoadScreenHandler, deltaModifier);
        }
        #endregion
        




        public interface ILoadScenePercentHandler : ILoadPercentListener
        {
            void OnAwake();
            void OnStartTransition(LoadSceneParameters.LoadType type);
            void OnFinishLoad(LoadSceneParameters.LoadType type);
        }
    }


}
