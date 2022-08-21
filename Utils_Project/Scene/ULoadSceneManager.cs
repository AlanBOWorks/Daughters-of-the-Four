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
    public class ULoadSceneManager : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private MPUIKIT.MPImage fillerImageMask;
        [Title("Params")]
        [SerializeField, SuffixLabel("deltas"), Range(.5f, 12f)] 
        private float showLoadScreenDeltaSpeed = 4f;
        [SerializeField, SuffixLabel("deltas"), Range(.5f, 12f)] 
        private float hideLoadScreenDeltaSpeed = 1f;

        [Title("Stylize")]
        [SerializeField]
        private AnimationCurve fillingCurve 
            = new AnimationCurve(new Keyframe(0,0), new Keyframe(1,1));

        [Title("Listeners"), ShowIf("_percentListeners")] 
        private List<IScreenLoadListener> _percentListeners;

        private List<ILoadPercentListener> _loadPercentListeners;

        private void Awake()
        {
            gameObject.SetActive(false);
            HandleFillFromLeft(true);
            fillerImageMask.fillAmount = 0;
            LoadSceneManagerSingleton.Injection(this);

            DoAsyncFuncInitializations();
            DoSimpleTransitionInitializations();
        }

        public void SubscribeListener(IScreenLoadListener listener)
        {
            if(_percentListeners == null) 
                _percentListeners = new List<IScreenLoadListener>(4);
            _percentListeners.Add(listener);
        }

        public void UnSubscribeListener(IScreenLoadListener listener)
        {
            _percentListeners?.Remove(listener);
        }

        public void SubscribeListener(ILoadPercentListener listener)
        {
            if (_percentListeners == null)
                _loadPercentListeners= new List<ILoadPercentListener>(4);
            _loadPercentListeners.Add(listener);
        }

        public void UnSubscribeListener(ILoadPercentListener listener)
        {
            _loadPercentListeners?.Remove(listener);
        }

        private void HandleFillFromLeft(bool isLeftFill)
        {
            var fillIndex = isLeftFill ? 0 : 1;
            fillerImageMask.fillOrigin = fillIndex;
        }


        public bool IsLoadingScene() => _fillingCoroutineHandle.IsRunning;
        private CoroutineHandle _fillingCoroutineHandle;
        public void DoSceneTransition(
            LoadSceneParameters parameters,
            LoadSceneParameters.ISceneLoadListener listener)
        {
            if(IsLoadingScene()) return;

            _fillingCoroutineHandle =
                Timing.RunCoroutine(_SceneSwapSequence(parameters, listener));
        }


        #region Transition Tweens
        private const float FillThreshold = .98f;
        private IEnumerator<float> _FadeInLoadScreen(bool isLeftFill, float deltaModifier = 1)
        {
            float lerpAmount = 0;

            HandleFillFromLeft(isLeftFill);
            foreach (var listener in _percentListeners)
            {
                listener.OnShowLoadScreen(isLeftFill);
            }
            yield return Timing.WaitForOneFrame;

            float deltaSpeed = showLoadScreenDeltaSpeed * deltaModifier;
            while (lerpAmount < FillThreshold)
            {
                yield return Timing.WaitForOneFrame;

                float deltaStep = Time.deltaTime * deltaSpeed;
                lerpAmount += deltaStep;
                fillerImageMask.fillAmount = fillingCurve.Evaluate(lerpAmount);

                foreach (var listener in _percentListeners)
                {
                    listener.OnFillLoadScreenPercent(lerpAmount);
                }

            }

            fillerImageMask.fillAmount = 1;
        }

        private const float FillOutThreshold = 1 - FillThreshold;
        private IEnumerator<float> _FadeOutLoadScreen(bool isLeftFill, float deltaModifier = 1)
        {
            float lerpAmount = 1;
            HandleFillFromLeft(!isLeftFill);
            yield return Timing.WaitForOneFrame;


            foreach (var listener in _percentListeners)
            {
                listener.OnHideLoadScreen(isLeftFill);
            }

            float deltaSpeed = hideLoadScreenDeltaSpeed * deltaModifier;
            while (lerpAmount > FillOutThreshold)
            {
                float deltaStep = Time.deltaTime * deltaSpeed;
                lerpAmount -= deltaStep;
                fillerImageMask.fillAmount = fillingCurve.Evaluate(lerpAmount);

                foreach (var listener in _percentListeners)
                {
                    listener.OnFillOutLoadScreenPercent(lerpAmount);
                }
                yield return Timing.WaitForOneFrame;
            }

            fillerImageMask.fillAmount = 0;

        }
        #endregion
        #region Transition Operation

        private static void ExtractValues(LoadSceneParameters.ISceneLoadListener listener,
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

        private IEnumerator<float> _TransitionSequence(
            bool fillFromLeft, float deltaModifier,
            AsyncActions feedback,
            LoadSceneParameters.ISceneLoadListener feedbackListener
            )
        {
            ExtractValues(feedbackListener,
                out var firstLastListener,
                out var hiddenListener,
                out var tickingListener);
            var onHideAction = feedback.OnHideSceneFeedback;
            var isFinishFunc = feedback.IsFinishFunc;
            var currentPercentFunc = feedback.CurrentPercent;


            gameObject.SetActive(true);
            yield return Timing.WaitForOneFrame;
            firstLastListener?.OnStartTransition();
            yield return Timing.WaitUntilDone(_FadeInLoadScreen(fillFromLeft, deltaModifier));
            hiddenListener?.OnStartLoading();
            onHideAction?.Invoke();

            while (!isFinishFunc())
            {
                yield return Timing.WaitForOneFrame;
                var currentLoad = currentPercentFunc.Invoke();
                CallEvents(currentLoad);
            }
            hiddenListener?.OnLoadingFinish();
            yield return Timing.WaitUntilDone(_FadeOutLoadScreen(fillFromLeft, deltaModifier));
            firstLastListener?.OnFinishTransition();
            gameObject.SetActive(false);


            void CallEvents(float currentLoad)
            {
                foreach (var listener in _loadPercentListeners)
                {
                    listener.OnPercentTick(currentLoad);
                }
                tickingListener?.OnLoadSceneTick(currentLoad);
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



        //// LOAD SCENE SEGMENT
        #region LOAD SCENE
        private AsyncOperation _currentLoadingAsyncOperation;
        private Func<bool> _isFinishAsyncOperation;
        private Func<float> _asyncOperationPercent;

        private IEnumerator<float> _SceneSwapSequence(
            LoadSceneParameters loadParameters,
            LoadSceneParameters.ISceneLoadListener feedbackListener)
        {
            loadParameters.ExtractValues(
                out var sceneName,
                out var fillFromLeft,
                out var isAdditive, out var deltaModifier);

            var actions = new AsyncActions(DoLoadAsync, _isFinishAsyncOperation, _asyncOperationPercent);
            return _TransitionSequence(fillFromLeft, deltaModifier, actions, feedbackListener);

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
            LoadSceneParameters.ISceneLoadListener feedbackListener = null)
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
                var actions = new AsyncActions(null, _isSimpleTransitionFinish, _simpleTransitionPercent);
                return _TransitionSequence(fromLeft, deltaModifier, actions, feedbackListener);
            }
        }

    }

    public interface IScreenLoadListener
    {
        void OnShowLoadScreen(bool isFillFromLeft);
        void OnHideLoadScreen(bool wasFillFromLeft);
        void OnFillLoadScreenPercent(float fillPercent);
        void OnFillOutLoadScreenPercent(float fillPercent);
    }

    public interface ILoadPercentListener
    {
        void OnPercentTick(float loadPercent);
    }

    internal static class LoadSceneManagerSingleton
    {
        public static ULoadSceneManager ManagerInstance { get; private set; }

        public static void Injection(ULoadSceneManager manager)
        {
            var currentInstance = ManagerInstance;
            if(currentInstance) Object.Destroy(currentInstance);

            ManagerInstance = manager;
        }
    }

    
}
