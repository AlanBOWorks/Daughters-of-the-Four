using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Localization;
using CombatSystem.Skills;
using CombatSystem.Skills.Effects;
using CombatSystem.Stats;
using MEC;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Utils;

namespace CombatSystem.Player.UI
{
    public class UEffectsTooltipWindowHandler : MonoBehaviour
    {
        [Title("Mono References")]
        [SerializeField]
        private RectTransform resizeWindow;
        [SerializeField] private float verticalPadding;

        [Title("Pool References")]
        [SerializeField, PropertyOrder(10)] private ToolTipWindowPool pool = new ToolTipWindowPool();

        [Serializable]
        private sealed class ToolTipWindowPool : TrackedMonoObjectPool<UEffectTooltipHolder>
        {}

        private void Awake()
        {
            pool.Awake();
            gameObject.SetActive(false);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            pool.ReturnToElementsToPool();
        }

        public void Show()
        {
            if (gameObject.activeSelf) return;
            gameObject.SetActive(true);

            //todo animation?
        }


        public void HandleEffects(IEnumerable<PerformEffectValues> effects, CombatEntity performer)
        {
            float accumulatedHeight = 0;
            var stats = performer.Stats;
            foreach (var value in effects)
            {
                var holder = pool.GetElementSafe();
                UtilsEffectTooltip.HandleText(holder, in value, stats);
                UtilsEffectTooltip.HandleIcon(holder, value.Effect);

                UtilsEffectTooltip.HandleTextHeight(holder.GetTextHolder(), ref accumulatedHeight);
            }
            HeightResize(accumulatedHeight);
        }


        protected void HeightResize(float accumulatedHeight)
        {
            UtilsEffectTooltip.HandleRootHeight(resizeWindow,accumulatedHeight,verticalPadding);
        }
    }

    public static class UtilsEffectTooltip
    {
        public static void HandleText(UEffectTooltipHolder holder, in PerformEffectValues values, CombatStats stats = null)
        {
            var textHolder = holder.GetTextHolder();
            LocalizeEffects.LocalizeEffectTooltip(in values, out var effectText);

            var effect = values.Effect;
            if (stats == null)
            {
                textHolder.text = effectText;
                return;
            }

            var valueDigits = effect.GetEffectTooltip(stats, values.EffectValue);
            if(valueDigits != null)
                textHolder.text = effectText + ":" + valueDigits;
            else
                textHolder.text = effectText;
        }

        public static void MultiplyEffectText(UEffectTooltipHolder holder, float modifier, CombatStats stats = null)
        {
            var currentValues = holder.EffectValues;
            PerformEffectValues nextValues = currentValues * modifier;
            HandleText(holder, nextValues, stats);
        }


        public static void HandleIcon(UEffectTooltipHolder holder, IEffectBasicInfo effect)
        {
            var icon = UtilsVisual.GetEffectSprite(effect);
            var iconHolder = holder.GetIconHolder();
            iconHolder.sprite = icon;
        }

        public static void HandleTextHeight(TMP_Text text, ref float accumulatedHeight)
        {
            var textTransform = text.rectTransform;
            float textHeight = text.preferredHeight;

            float pivotHeight = accumulatedHeight;
            UtilsRectTransform.SetPivotVertical(textTransform, -pivotHeight);

            accumulatedHeight += textHeight;
        }

        public static void HandleRootHeight(RectTransform resizeWindow, float accumulatedHeight,float verticalPadding)
        {
            accumulatedHeight += verticalPadding;
            resizeWindow.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, accumulatedHeight);
        }
    }
}
