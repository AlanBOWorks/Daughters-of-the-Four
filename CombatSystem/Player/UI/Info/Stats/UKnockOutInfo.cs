using CombatSystem.Entity;
using CombatSystem.Stats;
using TMPro;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UKnockOutInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI currentTickText;

        private const int ReviveThreshold = KnockOutHandler.ReviveThreshold;


        public void Tick(int currentTick)
        {
            int targetAmount = ReviveThreshold - currentTick; //is a countdown
            currentTickText.text = targetAmount.ToString();
        }
    }
}
