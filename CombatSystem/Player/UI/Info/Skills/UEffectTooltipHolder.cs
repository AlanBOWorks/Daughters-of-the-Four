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
    public sealed class UEffectTooltipHolder : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI effectTextHolder;
        [SerializeField] private Image iconHolder;

        private string _effectMainText;
        private float _effectValue;
        private bool _isPercentSuffix;
        public void HandleText(in PerformEffectValues values, CombatStats stats = null, ISkill skill = null)
        {
            LocalizeEffects.LocalizeEffectTooltip(in values, out var effectText);

            var effect = values.Effect;
            _isPercentSuffix = effect.IsPercentTooltip();
            _effectValue = values.EffectValue;

            string digitText;
            HandleTextWithDigits(); void HandleTextWithDigits()
            {
                if (skill == null || stats == null)
                {
                    digitText = LocalizeMath.LocalizeMathfValue(_effectValue, _isPercentSuffix);
                }
                else
                {
                    _effectValue = effect.CalculateEffectByStatValue(stats, _effectValue);

                    float skillLuck = skill.LuckModifier;
                    float statsLuck = UtilsStatsFormula.CalculateLuckAmount(stats);
                    float highValue = _effectValue * (1 + skillLuck * statsLuck);
                    digitText = LocalizeMath.LocalizeMathfValue(_effectValue, highValue, _isPercentSuffix);
                }
            }

            UpdateEffectTextHolder(effectText, digitText);
        }


        private void UpdateEffectTextHolder(string effectText, string digitText)
        {
            _effectMainText = effectText;
            effectTextHolder.text = effectText + ":\n <b>" + digitText + "</b>";
        }

        public void UpdateEffectDigitText(float variation)
        {
            _effectValue += variation;
            var digitText = LocalizeMath.LocalizeMathfValue(_effectValue, _isPercentSuffix);
            UpdateEffectTextHolder(_effectMainText, digitText);
        }

        public void HandleIcon(IEffectBasicInfo effect)
        {
            var icon = UtilsVisual.GetEffectSprite(effect);
            iconHolder.sprite = icon;
        }

    }
}
