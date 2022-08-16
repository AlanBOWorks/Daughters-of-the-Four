using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ExplorationSystem.Elements
{
    public class UWorldElementHolder : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameHolder;
        [SerializeField] private Image iconHolder;

        public void Injection(string worldName)
        {
            nameHolder.text = worldName;
        }

        public void Injection(Sprite icon)
        {
            iconHolder.sprite = icon;
        }
    }
}
