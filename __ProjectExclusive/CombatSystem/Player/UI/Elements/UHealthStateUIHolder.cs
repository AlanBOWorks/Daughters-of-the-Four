using CombatEntity;
using DG.Tweening;
using Sirenix.OdinInspector;
using Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace __ProjectExclusive.Player.UI
{
    public class UHealthStateUIHolder : MonoBehaviour
    {
        [SerializeField, HorizontalGroup("Health")]
        private TextMeshProUGUI currentHealth;

        [SerializeField, HorizontalGroup("Health")]
        private TextMeshProUGUI maxHealth;

        [SerializeField] 
        private Image percentBar;

        private ICombatHealthRead<float> _stats;

        public void Inject(CombatingEntity entity)
        {
            _stats = entity.CombatStats;
            UpdateHealth();
        }

        [Button, HideInEditorMode]
        public void UpdateHealth()
        {
            float statsHealth= _stats.CurrentHealth;
            float statsMaxHealth = _stats.MaxHealth;
            currentHealth.text = UtilsText.ConstructMaxFourDigit(statsHealth);
            maxHealth.text = "/" + UtilsText.ConstructMaxFourDigit(statsMaxHealth);

            float percentage = statsHealth / statsMaxHealth;

            UpdateHealthBar(percentage);
        }

        private void UpdateHealthBar(float percentage)
        {
            var barTransform = percentBar.rectTransform;
            Vector2 barPosition = barTransform.anchoredPosition;
            barPosition.x = Mathf.LerpUnclamped(100, 0, percentage); //Zero means the bar is out of canvas showing the color
            barTransform.anchoredPosition = barPosition;
        }

        public void DoDamageToHealth()
        {
            float statsHealth = _stats.CurrentHealth;
            float statsMaxHealth = _stats.MaxHealth;
            currentHealth.text = UtilsText.ConstructMaxFourDigit(statsHealth);
            float percentage = statsHealth / statsMaxHealth;

            UpdateHealthBar(percentage);

            var healthTextTransform = currentHealth.transform;
            healthTextTransform.DOKill();
            healthTextTransform.DOPunchPosition(Vector3.right * 4, .2f, 3);
        }

        public void UpdateMaxHealth()
        {
            float statsHealth = _stats.CurrentHealth;
            float statsMaxHealth = _stats.MaxHealth;
            maxHealth.text = "/" + UtilsText.ConstructMaxFourDigit(statsMaxHealth);

            float percentage = statsHealth / statsMaxHealth;

            UpdateHealthBar(percentage);
        }
    }
}
