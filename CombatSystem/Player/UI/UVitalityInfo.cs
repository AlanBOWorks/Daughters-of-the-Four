using System;
using CombatSystem.Entity;
using CombatSystem.Stats;
using TMPro;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UVitalityInfo : MonoBehaviour, IEntityExistenceElement<UVitalityInfo>
    {
        [SerializeField] private TextMeshProUGUI shieldsText;
        [SerializeField] private VitalityInfoHolder healthInfoHolder = new VitalityInfoHolder();
        [SerializeField] private VitalityInfoHolder mortalityInfoHolder = new VitalityInfoHolder();


        private void Awake()
        {
            healthInfoHolder.Awake();
            mortalityInfoHolder.Awake();
        }


        private CombatStats _currentStats;

        public void EntityInjection(in CombatEntity entity, int index)
        {
            Injection(entity.Stats);
        }

        public void OnPreStartCombat()
        {
            
        }


        public void Injection(in CombatStats injection)
        {
            _currentStats = injection;
            UpdateToCurrentStats();
        }

        public void UpdateToCurrentStats()
        {
            var stats = _currentStats;
            float shields = stats.CurrentShields;

            float currentHealth = stats.CurrentHealth;
            float maxHealth = UtilsStatsFormula.CalculateMaxHealth(stats);

            float currentMortality = stats.CurrentMortality;
            float maxMortality = UtilsStatsFormula.CalculateMaxMortality(stats);

            UpdateShields(in shields);
            UpdateHealth(in currentHealth, in maxHealth);
            UpdateMortality(in currentMortality, in maxMortality);
        }

        public void UpdateShields(in float amount)
        {
            shieldsText.text = amount.ToString("#");
        }

        public void UpdateHealth(in float amount, in float max)
        {
            healthInfoHolder.UpdateHealth(in amount, in max);
        }

        public void UpdateMortality(in float amount, in float max)
        {
            mortalityInfoHolder.UpdateHealth(in amount, in max);
        }

        [Serializable]
        private sealed class VitalityInfoHolder
        {
            [SerializeField] private TextMeshProUGUI currentValueText;
            [SerializeField] private TextMeshProUGUI maxValueText;
            [SerializeField] private RectTransform percentBar;
            private float _barMaxWidth;

            public void Awake()
            {
                _barMaxWidth = percentBar.sizeDelta.x;
            }

            public void UpdateHealth(in float amount, in float max)
            {
                float percent = amount / max;
                UpdatePercentBar(in percent);
                currentValueText.text = amount.ToString("####");
                maxValueText.text = max.ToString("####");

            }
            private void UpdatePercentBar(in float percent)
            {
                var sizeDelta = percentBar.sizeDelta;
                sizeDelta.x = _barMaxWidth * percent;
                percentBar.sizeDelta = sizeDelta;
            }

        }

    }
}
