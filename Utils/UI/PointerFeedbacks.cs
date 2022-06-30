using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utils.UI
{

    [Serializable]
    public class BasicScalePointerFeedback : IButtonPointerFeedbackHandler
    {
        private RectTransform _button;
        [SerializeField, Range(0.05f, 3f), SuffixLabel("sec")]
        private float animationDuration = .2f;

        [SerializeField, Tooltip("How much it will change")]
        private float scaleModifier = -.2f;

        [SerializeField] private int vibration = 10;

        public void Injection(RectTransform button)
        {
            _button = button;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            DoAnimation();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ResetState();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            DoAnimation();
        }

        private void DoAnimation()
        {
            ResetState();
            DOTween.Kill(_button);
            _button.DOPunchScale(ScaleModifier(), animationDuration, vibration);

            Vector3 ScaleModifier()
            {
                return new Vector3(scaleModifier, scaleModifier, scaleModifier);
            }
        }

        private void ResetState()
        {
            _button.localScale = Vector3.one;

        }

    }
}
