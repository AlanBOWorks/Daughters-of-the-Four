using CombatEntity;
using Sirenix.OdinInspector;
using Stats;
using TMPro;
using UnityEngine;

namespace __ProjectExclusive.Player.UI
{
    public class UHealthStateUIHolder : MonoBehaviour
    {
        [SerializeField, HorizontalGroup("Health")]
        private TextMeshProUGUI currentHealth;

        [SerializeField, HorizontalGroup("Health")]
        private TextMeshProUGUI maxHealth;

        private ICombatHealthRead<float> _stats;

        public void Inject(CombatingEntity entity)
        {
            _stats = entity.CombatStats;
            UpdateHealth();
        }

        public void UpdateHealth()
        {
            currentHealth.text = UtilsText.ConstructMaxFourDigit(_stats.CurrentHealth);
            maxHealth.text = "/" + UtilsText.ConstructMaxFourDigit(_stats.MaxHealth);
        }
    }
}
