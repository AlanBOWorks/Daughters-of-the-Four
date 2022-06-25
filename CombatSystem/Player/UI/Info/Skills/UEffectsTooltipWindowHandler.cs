using System;
using System.Collections.Generic;
using CombatSystem.Localization;
using CombatSystem.Skills;
using CombatSystem.Skills.Effects;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;
using Utils.Utils;

namespace CombatSystem.Player.UI
{
    public class UEffectsTooltipWindowHandler : UEffectTooltipWindowHandlerBasic<UEffectTooltipHolder>
    {
        [Title("Pool References")]
        [SerializeField, PropertyOrder(10)] private ToolTipWindowPool pool;

        [Serializable]
        private sealed class ToolTipWindowPool : TrackedMonoObjectPool<UEffectTooltipHolder>
        {}

        protected override IObjectPoolBasic GetPool() => pool;
        protected override IObjectPoolTracked<UEffectTooltipHolder> GetTrackedPool() => pool;

        public void HandleEffect(in PerformEffectValues values)
        {
            var textElement = pool.Pop();
            HandleText(textElement, in values);
            HandleIcon(textElement, values.Effect);
        }
    }

    public abstract class UEffectTooltipWindowHandlerBasic<T> : MonoBehaviour where T : UEffectTooltipHolder
    {
        [Title("Mono References")]
        [SerializeField]
        private RectTransform parentHolder;


        [Title("Params")]
        [SerializeField, SuffixLabel("px")] private float topMargin = 12;
        [SerializeField, SuffixLabel("px")] private float bottomMargin = 12;

        protected abstract IObjectPoolBasic GetPool();
        protected abstract IObjectPoolTracked<T> GetTrackedPool();

        private void Awake()
        {
            GetPool().Awake();
            gameObject.SetActive(false);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            GetTrackedPool().ReturnToElementsToPool();
        }

        public void Show()
        {
            if (gameObject.activeSelf) return;
            gameObject.SetActive(true);

            //todo animation?
        }


        protected static void HandleText(T holder, in PerformEffectValues values)
        {
            var text = holder.GetTextHolder();
            var effectText = LocalizeEffects.LocalizeEffectTooltip(in values);
            text.text = effectText;
        }

        protected static void HandleIcon(T holder, IEffectBasicInfo effect)
        {
            var icon = UtilsVisual.GetEffectSprite(effect);
            var iconHolder = holder.GetIconHolder();
            iconHolder.sprite = icon;
        }


        private float _accumulatedHeight;
        public void OnFinisHandlingEffects() => OnFinisHandlingEffects(GetTrackedPool());

        private void OnFinisHandlingEffects(IObjectPoolTracked<T> elementsPool)
        {
            bool isActive = elementsPool.IsActive();

            if (isActive)
            {
                HeightResize(elementsPool);
                Show();
            }
            else
            {
                Hide();
            }
        }

        private void HeightResize(IObjectPoolTracked<T> elementsPool)
        {
            _accumulatedHeight = topMargin;
            var activeElements = elementsPool.GetActiveElements();
            foreach (var element in activeElements)
            {
                HandleHeight(element);
            }

            _accumulatedHeight += bottomMargin;
            parentHolder.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _accumulatedHeight);
        }
        private void HandleHeight(T holder)
        {
            var text = holder.GetTextHolder();
            var textTransform = text.rectTransform;
            float textHeight = text.preferredHeight;

            float pivotHeight = _accumulatedHeight;
            UtilsRectTransform.SetPivotVertical(in textTransform, -pivotHeight);

            _accumulatedHeight += textHeight;
        }
    }
}
