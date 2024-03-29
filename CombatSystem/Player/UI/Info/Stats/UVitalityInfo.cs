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
        [InfoBox("Shield text parent will be deActivated", InfoMessageType.Warning)]
        [SerializeField] private TextMeshProUGUI shieldsText;
        [SerializeField] private UPercentBarInfo healthInfoHolder;
        [SerializeField] private UPercentBarInfo mortalityInfoHolder;
        [Title("KnockOut")]
        [SerializeField] private UPercentBarInfo knockOutInfoHolder;
        [SerializeField] private GameObject knockOutHolder;
        
        private CombatStats _currentStats;

        public void EntityInjection(CombatEntity entity)
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


        private void Injection(CombatStats injection)
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

        private const string OverFlowShieldsText = "X"; 
        public void UpdateShields(float amount)
        {
            if (amount <= 0)
            {
                ToggleShieldsHolderActive(false);
            }
            else
            {
                ToggleShieldsHolderActive(true);
                if (amount > 9)
                {
                    shieldsText.text = OverFlowShieldsText;

                }
                shieldsText.text = amount.ToString(CultureInfo.InvariantCulture);
            }

            void ToggleShieldsHolderActive(bool active)
            {
                shieldsText.transform.parent.gameObject.SetActive(active);
            }
        }

        public void UpdateHealth(float amount, float max)
        {
            healthInfoHolder.UpdateInfo(amount, max);
        }

        public void UpdateMortality(float amount, float max)
        {
            mortalityInfoHolder.UpdateInfo(amount, max);
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
            knockOutInfoHolder.UpdateInfo(tick, KnockOutThreshold);
        }



    }
}
