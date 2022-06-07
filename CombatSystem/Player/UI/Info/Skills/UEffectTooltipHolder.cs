using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CombatSystem.Player.UI
{
    public class UEffectTooltipHolder : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI effectTextHolder;
        [SerializeField] private Image iconHolder;

        public TextMeshProUGUI GetTextHolder() => effectTextHolder;
        public Image GetIconHolder() => iconHolder;
    }
}
