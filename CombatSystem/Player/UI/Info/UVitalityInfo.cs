using System;
using System.Globalization;
using CombatSystem.Entity;
using CombatSystem.Stats;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UVitalityInfo : MonoBehaviour, IEntityExistenceElement<UVitalityInfo>
    {

        [Title("Holder")] 
        [SerializeField] private CanvasGroup alphaGroup;

        [Title("Elements")]
        [SerializeField] private TextMeshProUGUI entityNameText;
        [SerializeField] private TextMeshProUGUI shieldsText;
        [SerializeField] private VitalityInfoHolder healthInfoHolder = new VitalityInfoHolder();
        [SerializeField] private VitalityInfoHolder mortalityInfoHolder = new VitalityInfoHolder();



        private void Awake()
        {
            healthInfoHolder.Awake();
            mortalityInfoHolder.Awake();
        }


        private CombatStats _currentStats;

        public void EntityInjection(in CombatEntity entity)
        {
            if (entity == null)
            {
                UpdateInfoAsNull();
                ResetDisplayedValues();
                return;
            }

            Injection(entity.Stats);
            var entityName = entity.CombatCharacterName;
            UpdateCharacterName(in entityName);
        }

        public void OnPreStartCombat()
        {
            
        }


        public void ShowElement()
        {
            gameObject.SetActive(true);
            alphaGroup.alpha = 1;
        }

        public void OnInstantiation()
        {
            ShowElement();
        }

        public void OnDestruction()
        {
        }

        private const float DisableAlphaValue = .3f;
        public void DisableElement()
        {
            alphaGroup.alpha = DisableAlphaValue;
        }

        public void HideElement()
        {
            gameObject.SetActive(false);
        }


        private void Injection(in CombatStats injection)
        {
            _currentStats = injection;
            UpdateToCurrentStats();
        }

        private const string OnNullName = "-------";
        private void UpdateInfoAsNull()
        {
            UpdateCharacterName(OnNullName);
        }
        private void UpdateCharacterName(in string entityName)
        {
            if (entityNameText)
                entityNameText.text = entityName;
        }

        public void UpdateToCurrentStats()
        {
            var stats = _currentStats;
            float shields = stats.CurrentShields;
            UpdateShields(in shields);

            float currentHealth = stats.CurrentHealth;
            float maxHealth = UtilsStatsFormula.CalculateMaxHealth(stats);
            UpdateHealth(in currentHealth, in maxHealth);


            float currentMortality = stats.CurrentMortality;
            float maxMortality = UtilsStatsFormula.CalculateMaxMortality(stats);
            UpdateMortality(in currentMortality, in maxMortality);
        }

        public void ResetDisplayedValues()
        {
            UpdateShields(0);
            UpdateHealth(0,0);
            UpdateMortality(0,0);
        }

        public void UpdateShields(in float amount)
        {
            shieldsText.text = amount.ToString(CultureInfo.InvariantCulture);
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

                if(currentValueText)
                    currentValueText.text = amount.ToString("####");
                if(maxValueText)
                    maxValueText.text = max.ToString("####");

            }
            private void UpdatePercentBar(in float percent)
            {
                if(!percentBar) return;

                var sizeDelta = percentBar.sizeDelta;
                sizeDelta.x = _barMaxWidth * percent;
                percentBar.sizeDelta = sizeDelta;
            }

        }

    }
}
