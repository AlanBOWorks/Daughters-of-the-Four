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
        public void DoSceneTransition(string sceneName, bool showLoadScreenFromLeft, bool keepAliveFromLoad = false, float deltaModifier = 1)
        {
            if(IsLoadingScene()) return;

            var parameters = new LoadSceneParameters(sceneName,showLoadScreenFromLeft, keepAliveFromLoad, deltaModifier);
            _fillingCoroutineHandle =
                Timing.RunCoroutine(_SceneSwapSequence(parameters));
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

        private IEnumerator<float> _SceneSwapSequence(LoadSceneParameters loadParameters)
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
            yield return Timing.WaitUntilDone(_FadeOutLoadScreen(fillFromLeft,deltaModifier));
            gameObject.SetActive(false);
        }

        [Button,DisableInEditorMode]
        private void DebugAnimation(float timeWaitSimulation = 1, bool fillFromLeft = true)
        {
            Timing.KillCoroutines(_fillingCoroutineHandle);
            fillerImageMask.fillAmount = 0;
            _fillingCoroutineHandle = Timing.RunCoroutine(_Steps());

            IEnumerator<float> _Steps()
            {
                gameObject.SetActive(true);
                yield return Timing.WaitForOneFrame;
                yield return Timing.WaitUntilDone(_FadeInLoadScreen(fillFromLeft));
                yield return Timing.WaitForSeconds(timeWaitSimulation);
                yield return Timing.WaitUntilDone(_FadeOutLoadScreen(fillFromLeft));
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

    public readonly struct LoadSceneParameters
    {
        public readonly string SceneName;
        public readonly bool ShowLoadScreenFromLeft;
        public readonly bool IsAdditive;
        public readonly float DeltaModifier;

        public LoadSceneParameters(string sceneName, bool showLoadScreenFromLeft, bool isAdditive = false, float deltaModifier = 1)
        {
            SceneName = sceneName;
            ShowLoadScreenFromLeft = showLoadScreenFromLeft;
            IsAdditive = isAdditive;
            DeltaModifier = deltaModifier;
        }

        public void ExtractValues(
            out string sceneName, 
            out bool showLoadFromLeft, 
            out bool isAdditive,
            out float deltaModifier)
        {
            sceneName = SceneName;
            showLoadFromLeft = ShowLoadScreenFromLeft;
            isAdditive = IsAdditive;
            deltaModifier = DeltaModifier;
        }
    }
}
