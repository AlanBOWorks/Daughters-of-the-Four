using System;
using System.Globalization;
using CombatSystem.Entity;
using CombatSystem.Stats;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Utils;

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
        [Title("KnockOut")]
        [SerializeField] private VitalityInfoHolder knockOutInfoHolder = new VitalityInfoHolder();
        [SerializeField] private GameObject knockOutHolder;


        private void Awake()
        {
            healthInfoHolder.Awake();
            mortalityInfoHolder.Awake();
            knockOutInfoHolder.Awake();
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
            HideKnockOut();
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
            UpdateShields(shields);

            float currentHealth = stats.CurrentHealth;
            float maxHealth = UtilsStatsFormula.CalculateMaxHealth(stats);
            UpdateHealth(currentHealth, maxHealth);


            float currentMortality = stats.CurrentMortality;
            float maxMortality = UtilsStatsFormula.CalculateMaxMortality(stats);
            UpdateMortality(currentMortality, maxMortality);
        }

        public void ResetDisplayedValues()
        {
            UpdateShields(0);
            UpdateHealth(0,0);
            UpdateMortality(0,0);
        }

        public void UpdateShields(float amount)
        {
            shieldsText.text = amount.ToString(CultureInfo.InvariantCulture);
        }

        public void UpdateHealth(float amount, float max)
        {
            healthInfoHolder.UpdateHealth(amount, max);
        }

        public void UpdateMortality(float amount, float max)
        {
            mortalityInfoHolder.UpdateHealth(amount, max);
        }


        public void ShowKnockOut()
        {
            if(!knockOutHolder) return;
            knockOutHolder.SetActive(true);

        }

        public void HideKnockOut()
        {
            if(!knockOutHolder) return;
            knockOutHolder.SetActive(false);
        }
        private const float KnockOutThreshold = KnockOutHandler.ReviveThreshold +1;
        public void UpdateKnockOut(int currentTick)
        {
            if(!knockOutHolder) return;

            float tick = KnockOutThreshold - currentTick;
            knockOutInfoHolder.UpdateHealth(tick, KnockOutThreshold);
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
                if(!percentBar) return;
                _barMaxWidth = percentBar.sizeDelta.x;
            }

            public void UpdateHealth(float customAmountText, float amount, float max)
            {
                float percent = amount / max;
                UpdatePercentBar(percent);

                if (currentValueText)
                    currentValueText.text = customAmountText.ToString("####");
                if (maxValueText)
                    maxValueText.text = max.ToString("####");
            }

            public void UpdateHealth(float amount, float max)
            {
                UpdateHealth(amount,amount,max);
            }
            private void UpdatePercentBar(float percent)
            {
                if(!percentBar) return;

                var sizeDelta = percentBar.sizeDelta;
                sizeDelta.x = _barMaxWidth * percent;
                percentBar.sizeDelta = sizeDelta;
            }

        }

    }
}
