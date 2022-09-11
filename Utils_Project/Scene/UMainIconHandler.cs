using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils_Project.Scene
{
    public class UMainIconHandler : MonoBehaviour, IScreenLoadListener
    {
        [Title("Params")]
        [SerializeField] private CanvasGroup groupAlphaCanvas;
        [SerializeField, SuffixLabel("px")] private float lateralOffset = 100;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = (RectTransform)transform;
            _initialPosition = _rectTransform.localPosition;
        }

        private void Start()
        {
            LoadSceneManagerSingleton.ScreenLoadListeners.Add(this);
        }

        private const float ShowAnimationDuration = .4f;
        private Vector3 _initialPosition;
        public void OnShowLoadScreen(LoadSceneParameters.LoadType type)
        {
            bool isLeft = type == LoadSceneParameters.LoadType.MainLoadFromLeft;
            CallPunchAnimation(isLeft);
        }

        public void OnHideLoadScreen(LoadSceneParameters.LoadType type)
        {
            
        }

        private void CallPunchAnimation(bool isLeftAnimation)
        {
            float lateralPunch = isLeftAnimation
                ? lateralOffset
                : -lateralOffset;
            DoAnimation(lateralPunch, ShowAnimationDuration);
        }


        private void DoAnimation(float lateralPunch, float animationDuration)
        {
            if(!enabled) return;

            DOTween.Kill(_rectTransform);
            _rectTransform.localPosition = _initialPosition;
            _rectTransform.DOPunchPosition(new Vector3(lateralPunch, 0), animationDuration, 0);
        }

        public void OnFillLoadScreenPercent(float fillPercent)
        {
            groupAlphaCanvas.alpha = fillPercent;
        }

        public void OnFillOutLoadScreenPercent(float fillPercent)
        {
            groupAlphaCanvas.alpha = fillPercent;
        }
    }
}
