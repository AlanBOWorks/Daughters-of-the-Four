using System;
using CombatSystem._Core;
using MPUIKIT;
using TMPro;
using UnityEngine;

namespace CombatSystem.Player.UI.Info
{
    public class URoundCountHandler : MonoBehaviour, ITempoTickListener
    {
        [SerializeField] private TextMeshProUGUI tickText;
        [SerializeField] private MPImage borderRadialImage;

        private const float RoundThreshold = TempoTicker.LoopThresholdAsIntended;
        private float _currentTick;

        private void Awake()
        {
            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);
        }

        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.UnSubscribe(this);
        }

        private void UpdateCountText(float amount)
        {
            tickText.text = amount.ToString("00");
        }



        private void DoRadialFill(float percent)
        {
            borderRadialImage.fillAmount = percent;
        }


        public void OnStartTicking()
        {
            _currentTick = 1;
        }

        public void OnTick()
        {
            _currentTick++;
            float percent = _currentTick / RoundThreshold;
            DoRadialFill(percent);
            UpdateCountText(_currentTick);
        }

        public void OnRoundPassed()
        {
            _currentTick = 0;
        }

        public void OnStopTicking()
        {
        }
    }
}
