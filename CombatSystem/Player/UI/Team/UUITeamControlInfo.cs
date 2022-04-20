using CombatSystem.Stats;
using TMPro;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UUITeamControlInfo : MonoBehaviour
    {
        [SerializeField] private RectTransform percentageBar;
        [SerializeField] private TextMeshProUGUI burstAmountText;
        internal TeamDataValues TeamData;

        private const float MinWidth = 10;
        private const float MaxWidth = 50;

        internal void UpdateControl()
        {
            float currentBurstControl = TeamData.BurstControl;
            UpdateAmountText(currentBurstControl);
            UpdatePercentageBar(in currentBurstControl);
        }

        private void UpdateAmountText(float currentBurstControl)
        {
            currentBurstControl *= 100;
            burstAmountText.text = currentBurstControl.ToString("F0");
        }

        private void UpdatePercentageBar(in float currentBurstPercent)
        {
            var barRect = percentageBar.sizeDelta;
            barRect.x = Mathf.Lerp(MinWidth, MaxWidth, currentBurstPercent);
            percentageBar.sizeDelta = barRect;
        }
    }
}
