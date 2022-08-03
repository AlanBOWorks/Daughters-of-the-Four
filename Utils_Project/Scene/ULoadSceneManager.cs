using System;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
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

        private void Awake()
        {

            gameObject.SetActive(false);
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



        private void HandleFillFromLeft(bool isLeftFill)
        {
            var fillIndex = isLeftFill ? 0 : 1;
            fillerImageMask.fillOrigin = fillIndex;
        }


        private CoroutineHandle _fillingCoroutineHandle;
        public void DoSceneTransition(bool showLoadScreenFromLeft, float deltaModifier = 1)
        {

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
