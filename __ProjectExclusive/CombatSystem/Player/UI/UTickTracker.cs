using System;
using System.Collections.Generic;
using MEC;
using TMPro;
using UnityEngine;

namespace __ProjectExclusive.Player
{
    public class UTickTracker : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countText;
        private float _currentAmount;

        public void UpdateTickCount(float amount)
        {
            _currentAmount = amount;
            string amountString = _currentAmount.ToString("00");
            countText.SetText(amountString);
        }

    }
}
