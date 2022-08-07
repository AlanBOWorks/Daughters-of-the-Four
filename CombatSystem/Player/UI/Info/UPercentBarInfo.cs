using System;
using CombatSystem.Localization;
using CombatSystem.Stats;
using MPUIKIT;
using TMPro;
using UnityEngine;
using Utils.Maths;
using Utils_Project;

namespace CombatSystem.Player.UI
{
    public class UPercentBarInfo : MonoBehaviour
    {
        [SerializeField] private MPImage percentHolder;
        [SerializeField] private TextMeshProUGUI currentValueText;

        public void UpdateInfo(float currentValue, float maxValue)
        {
            float targetPercent = currentValue / maxValue;
            UpdateInfoAsPercent(targetPercent, currentValue);
        }

        public void UpdateInfoAsPercent(float targetPercent, float currentValue)
        {
            string healthText = LocalizeMath.LocalizeArithmeticValue(currentValue);
            UpdateInfo(targetPercent, healthText);
        }

        public void UpdateInfo(float targetPercent, string valueText)
        {
            percentHolder.fillAmount = targetPercent;
            if(currentValueText)
                currentValueText.text = valueText;
        }

        public void UpdateInfo(PercentValue values) =>
            UpdateInfoAsPercent(values.UnitPercentValue, values.CurrentValue);
    }
}
