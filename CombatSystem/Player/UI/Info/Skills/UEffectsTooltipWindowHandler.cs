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


        public void HandleEffects(ICombatSkill skill, IEnumerable<PerformEffectValues> effects, CombatEntity performer)
        {
            float accumulatedHeight = 0;
            CombatStats stats = performer?.Stats;
            foreach (var value in effects)
            {
                var holder = pool.PopElementSafe();
                UtilsEffectTooltip.HandleText(holder, in value, stats, skill);
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
        public static void HandleText(
            UEffectTooltipHolder holder, 
            in PerformEffectValues values, 
            CombatStats stats = null, 
            ICombatSkill skill = null)
        {
            var textHolder = holder.GetTextHolder();
            LocalizeEffects.LocalizeEffectTooltip(in values, out var effectText);

            var effect = values.Effect;
            bool isPercentSuffix = effect.IsPercentTooltip();
            float effectValue = values.EffectValue;

            string digitText;
            HandleTextWithDigits(); void HandleTextWithDigits()
            {
                if(skill == null || stats == null)
                {
                    digitText = LocalizeEffects.LocalizeMathfValue(effectValue,isPercentSuffix);
                }
                else
                {
                    effectValue = effect.CalculateEffectByStatValue(stats, effectValue);

                    float skillLuck = skill.LuckModifier;
                    float statsLuck = UtilsStatsFormula.CalculateLuckAmount(stats);
                    float highValue = effectValue * (1+ skillLuck * statsLuck);
                    digitText = LocalizeEffects.LocalizeMathfValue(effectValue,highValue,isPercentSuffix);
                }
            }

            textHolder.text = effectText + ":\n <b>" + digitText + "</b>";

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
