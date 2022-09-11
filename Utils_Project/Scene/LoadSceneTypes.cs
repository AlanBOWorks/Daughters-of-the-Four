using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils_Project.Scene
{
    [Serializable]
    internal sealed class MainSceneLoadTransitionWrapper : ULoadSceneManager.ILoadScenePercentHandler
    {
        [Title("References")]
        [SerializeField] private MPUIKIT.MPImage fillerImageMask;

        [SerializeField]
        private AnimationCurve fillingCurve
            = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

        public void OnPercentTick(float loadPercent)
        {
            fillerImageMask.fillAmount = fillingCurve.Evaluate(loadPercent);
        }

        public void OnAwake()
        {
            fillerImageMask.fillAmount = 0;
        }

        public void OnStartTransition(LoadSceneParameters.LoadType type)
        {
            bool isLeftFill = type == LoadSceneParameters.LoadType.MainLoadFromLeft;
            HandleFillFromLeft(isLeftFill);
        }

        public void OnFinishLoad(LoadSceneParameters.LoadType type)
        {
            bool wasLeft = type == LoadSceneParameters.LoadType.MainLoadFromLeft;
            HandleFillFromLeft(!wasLeft);
        }

        public void HandleFillFromLeft(bool isLeftFill)
        {
            var fillIndex = isLeftFill ? 1 : 0;
            fillerImageMask.fillOrigin = fillIndex;
        }
    }
}
