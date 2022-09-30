using CombatSystem.Localization;
using CombatSystem.Skills;
using CombatSystem.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils_Project;

namespace CombatSystem.Player.UI
{
    public class UEffectTooltipHolder : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI effectTextHolder;
        [SerializeField] private Image iconHolder;


        public TextMeshProUGUI GetTextHolder() => effectTextHolder;
        public Image GetIconHolder() => iconHolder;


        public void HandleText(in PerformEffectValues values, CombatStats stats = null, ICombatSkill skill = null)
        {
            LocalizeEffects.LocalizeEffectTooltip(in values, out var effectText);

            var effect = values.Effect;
            bool isPercentSuffix = effect.IsPercentTooltip();
            float effectValue = values.EffectValue;

            string digitText;
            HandleTextWithDigits(); void HandleTextWithDigits()
            {
                if (skill == null || stats == null)
                {
                    digitText = LocalizeMath.LocalizeMathfValue(effectValue, isPercentSuffix);
                }
                else
                {
                    effectValue = effect.CalculateEffectByStatValue(stats, effectValue);

                    float skillLuck = skill.LuckModifier;
                    float statsLuck = UtilsStatsFormula.CalculateLuckAmount(stats);
                    float highValue = effectValue * (1 + skillLuck * statsLuck);
                    digitText = LocalizeMath.LocalizeMathfValue(effectValue, highValue, isPercentSuffix);
                }
            }

            UpdateEffectTextHolder(effectText, digitText);
        }


        protected void UpdateEffectTextHolder(string effectText, string digitText)
        {
            effectTextHolder.text = effectText + ":\n <b>" + digitText + "</b>";
        }

        public void HandleIcon(IEffectBasicInfo effect)
        {
            var icon = UtilsVisual.GetEffectSprite(effect);
            iconHolder.sprite = icon;
        }

        public void HandleTextHeight(ref float accumulatedHeight)
        {
            var textTransform = effectTextHolder.rectTransform;
            float textHeight = effectTextHolder.preferredHeight;

            float pivotHeight = accumulatedHeight;
            UtilsRectTransform.SetPivotVertical(textTransform, -pivotHeight);

            accumulatedHeight += textHeight;
        }

    }
}
