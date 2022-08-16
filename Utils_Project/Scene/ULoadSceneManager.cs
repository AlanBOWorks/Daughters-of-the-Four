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
        public void DoSceneTransition(LoadSceneParameters parameters,
            Action onFinishLoad = null, Action onAfterFinishLoadScreen = null)
        {
            if(IsLoadingScene()) return;

            _fillingCoroutineHandle =
                Timing.RunCoroutine(_SceneSwapSequence(parameters, onFinishLoad, onAfterFinishLoadScreen));
        }



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

        private IEnumerator<float> _SceneSwapSequence(LoadSceneParameters loadParameters,
            Action onFinishLoad = null, Action onAfterFinishLoadScreen = null)
        {
            loadParameters.ExtractValues(
                out var sceneName,
                out var fillFromLeft, 
                out var isAdditive,out var deltaModifier);

            gameObject.SetActive(true);

            yield return Timing.WaitForOneFrame;
            yield return Timing.WaitUntilDone(_FadeInLoadScreen(fillFromLeft,deltaModifier));

            var loadSceneMode = isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single;
            var sceneAsync = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            while (!sceneAsync.isDone)
            {
                yield return Timing.WaitForOneFrame;
                var currentLoad = sceneAsync.progress;
                foreach (var listener in _loadPercentListeners)
                {
                    listener.OnPercentTick(currentLoad);
                }
            }
            onFinishLoad?.Invoke();
            yield return Timing.WaitUntilDone(_FadeOutLoadScreen(fillFromLeft,deltaModifier));
            onAfterFinishLoadScreen?.Invoke();
            gameObject.SetActive(false);
        }

        public void DoJustScreenTransition(float waitUntilHide, bool fromLeft,
            Action<bool> screenShowsFeedback = null, float deltaModifier = 1)
        {
            Timing.KillCoroutines(_fillingCoroutineHandle);
            _fillingCoroutineHandle = Timing.RunCoroutine(_DoScreenTransition(), Segment.RealtimeUpdate);

            IEnumerator<float> _DoScreenTransition()
            {
                gameObject.SetActive(true);
                yield return Timing.WaitUntilDone(_FadeInLoadScreen(fromLeft, deltaModifier));
                yield return Timing.WaitForOneFrame;
                float timer = 0;
                if (waitUntilHide > 0)
                {
                    while (timer < waitUntilHide)
                    {
                        yield return Timing.DeltaTime;
                        timer += Timing.DeltaTime;
                        float currentLoad = timer / waitUntilHide;

                        CallEvents(currentLoad);
                    }
                }
                else CallEvents(1);


                screenShowsFeedback?.Invoke(true);
                yield return Timing.WaitForOneFrame;
                yield return Timing.WaitUntilDone(_FadeOutLoadScreen(fromLeft, deltaModifier));
                screenShowsFeedback?.Invoke(false);
                gameObject.SetActive(false);
            }

            void CallEvents(float currentLoad)
            {
                foreach (var listener in _loadPercentListeners)
                {
                    listener.OnPercentTick(currentLoad);
                }
            }
        }

        public void DoJustScreenTransition(Func<bool> isFinishCheck, bool fromLeft,
            Action<bool> screenShowsFeedback = null, float deltaModifier = 1)
        {
            Timing.KillCoroutines(_fillingCoroutineHandle);
            _fillingCoroutineHandle = Timing.RunCoroutine(_DoScreenTransition(), Segment.RealtimeUpdate);

            IEnumerator<float> _DoScreenTransition()
            {
                gameObject.SetActive(true);
                yield return Timing.WaitUntilDone(_FadeInLoadScreen(fromLeft, deltaModifier));
                
                do
                {
                    yield return Timing.WaitForOneFrame;
                } while (!isFinishCheck());

                screenShowsFeedback?.Invoke(true);
                yield return Timing.WaitForOneFrame;
                yield return Timing.WaitUntilDone(_FadeOutLoadScreen(fromLeft, deltaModifier));
                screenShowsFeedback?.Invoke(false);
                gameObject.SetActive(false);
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
