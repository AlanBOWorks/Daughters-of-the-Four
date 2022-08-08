using System;
using System.Collections.Generic;
using CombatSystem.Localization;
using CombatSystem.Stats;
using MEC;
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


        public void UpdateInfo(PercentValue values) =>
            UpdateInfoAsPercent(values.UnitPercentValue, values.CurrentValue);
        public void FirstInjectInfo(PercentValue values) =>
            FirstUpdateInfoAsPercent(values.UnitPercentValue, values.CurrentValue);


        public void FirstUpdateInfoAsPercent(float targetPercent, float currentValue)
        {
            percentHolder.fillAmount = 0;
            string healthText = LocalizeMath.LocalizeArithmeticIntegerValue(currentValue);
            UpdateInfo(targetPercent, healthText);
        }
        public void UpdateInfo(float currentValue, float maxValue)
        {
            float targetPercent = currentValue / maxValue;
            UpdateInfoAsPercent(targetPercent, currentValue);
        }

        public void UpdateInfoAsPercent(float targetPercent, float currentValue)
        {
            string healthText = LocalizeMath.LocalizeArithmeticIntegerValue(currentValue);
            UpdateInfo(targetPercent, healthText);
        }

        public void UpdateInfo(float targetPercent, string valueText)
        {
            if(currentValueText)
                currentValueText.text = valueText;
            AnimateBar(targetPercent);
        }


        private void OnEnable()
        {
            Timing.ResumeCoroutines(_animationHandle);
        }

        private void OnDisable()
        {
            Timing.PauseCoroutines(_animationHandle);
        }

        private void OnDestroy()
        {
            Timing.KillCoroutines(_animationHandle);
        }

        private float _targetPercent;
        private const float PercentDifferenceThreshold = .005f;
        private const float DeltaSpeed = 4f;

        private CoroutineHandle _animationHandle;
        private void AnimateBar(float targetPercent)
        {
            _targetPercent = targetPercent;
            if(_animationHandle.IsRunning) return;

            _animationHandle = Timing.RunCoroutine(_Animation());

            IEnumerator<float> _Animation()
            {
                float currentPercent;
                do {
                    currentPercent = percentHolder.fillAmount;
                    percentHolder.fillAmount = Mathf.Lerp(currentPercent, _targetPercent, Time.deltaTime * DeltaSpeed);
                    yield return Timing.WaitForOneFrame;
                }
                while (currentPercent - _targetPercent > PercentDifferenceThreshold) ;

                percentHolder.fillAmount = _targetPercent;
            }
        }



    }
}
