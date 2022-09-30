using System;
using CombatSystem.Localization;
using CombatSystem.Skills;
using UnityEngine;
using Utils_Project;

namespace CombatSystem.Player.UI
{
    public class UPunishEffectTooltipHandler : UEffectTooltipHolder
    {
        private bool _isPercentSuffix;
        private string _effectText;
        private float _currentEffectValue;
        private void OnDisable()
        {
            _currentEffectValue = 0;
        }

        public void HandleVanguardEffect(PerformEffectValues values, float effectValue)
        {
            LocalizeEffects.LocalizeEffectTooltip(in values, out var effectText, false);
            var effect = values.Effect;

            _isPercentSuffix = effect.IsPercentSuffix();
            _effectText = effectText;
            _currentEffectValue = effectValue;

            UpdateTextToCurrentValues();
        }
        public void IncreaseEffectValue(float increment)
        {
            _currentEffectValue += increment;
            UpdateTextToCurrentValues();
        }

        private void UpdateTextToCurrentValues()
        {
            var digitText = LocalizeMath.LocalizeMathfValue(_currentEffectValue, _isPercentSuffix);
            UpdateEffectTextHolder(_effectText, digitText);
        }

    }
}
