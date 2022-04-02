using TMPro;
using UnityEngine;

namespace CombatSystem.Player.UI
{

    public class ULuckInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI luckAmountText;

        public void UpdateLuck(in string luckAmount)
        {
            luckAmountText.text = luckAmount;
        }
    }
}
