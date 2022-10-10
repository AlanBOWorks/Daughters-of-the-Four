using System;
using CombatSystem.Localization;
using CombatSystem.Skills;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public sealed class UEffectsTargetingGroupHolder : MonoBehaviour
    {
        [Title("References")] 
        [SerializeField] 
        private TextMeshProUGUI groupNameHolder;

        private RectTransform _rectTransform;
        public RectTransform GetRectTransform() => _rectTransform;

        private void Awake()
        {
            _rectTransform = (RectTransform) transform;
        }


        private float _titleInitialHeight;
        private void RewriteHeight()
        {
            var nameTransform = groupNameHolder.rectTransform;
            _titleInitialHeight = nameTransform.rect.height + nameTransform.anchoredPosition.y;
        }

        public void HandleInjection(EnumsEffect.TargetType targetType)
        {
            var groupName = LocalizeEffects.LocalizeLineTargeting(targetType);
            groupNameHolder.text = groupName;
            RewriteHeight();
        }

        private const float VerticalPadding = 12f;
        public void HandleElement(UEffectTooltipHolder effectHolder)
        {
            var effectsHolderTransform = (RectTransform) effectHolder.transform;
            effectsHolderTransform.SetParent(transform);
        }

        private float _currentWidth;
        public void SetWidth(float width)
        {
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            _currentWidth = width;
        }
        /// <returns>The accumulated Height</returns>
        public float ResizeHeight()
        {
            HandleChildrenTransform(out var accumulatedHeight);
            accumulatedHeight += VerticalPadding;

            var size = _rectTransform.sizeDelta;
            size.y = accumulatedHeight;
            _rectTransform.sizeDelta = size;

            return accumulatedHeight;
        }

        private void HandleChildrenTransform(out float accumulatedHeight)
        {
            accumulatedHeight = _titleInitialHeight;
            bool isFirstElement = true;
            foreach (var childTransform in transform)
            {
                if (isFirstElement)
                {
                    isFirstElement = false;
                    continue;
                }

                var rectTransform = (RectTransform) childTransform;
                var position = rectTransform.anchoredPosition;

                position.x = 0;
                position.y = -accumulatedHeight;

                rectTransform.anchoredPosition = position;
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _currentWidth);

                accumulatedHeight += VerticalPadding + rectTransform.rect.height;

            }
        }
    }
}
