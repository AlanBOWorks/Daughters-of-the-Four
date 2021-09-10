using System;
using Characters;
using Stats;
using TMPro;
using UnityEngine;

namespace _Player
{
    public class UCombatDataTooltip : UPersistentElementInjector
    {
        [SerializeField] private HealthListener healthListener = new HealthListener();
        [SerializeField] private TemporalStatsListener temporalStatsListener = new TemporalStatsListener();

        protected override void DoInjection(EntityPersistentElements persistentElements)
        {
            persistentElements.CombatEvents.Subscribe(healthListener);
            persistentElements.CombatEvents.Subscribe(temporalStatsListener);
        }

        

        [Serializable]
        private class HealthListener : ICombatHealthChangeListener
        {
            [SerializeField] private TextMeshProUGUI healthValue;
            public void OnTemporalStatsChange(ICombatHealthStatsData<float> currentStats)
            {
                float healthAmount = currentStats.HealthPoints;
                healthValue.text = $"{healthAmount:0000}";
            }
        }

        [Serializable]
        private class TemporalStatsListener : ITemporalStatChangeListener
        {
            [SerializeField] private TextMeshProUGUI harmonyValue;
            [SerializeField] private TextMeshProUGUI actionsValue;
            public void OnConcentrationChange(ITemporalStatsData<float> currentStats)
            {
                float harmonyAmount = currentStats.HarmonyAmount;
                float actionsAmount = currentStats.ActionsPerInitiative;

                harmonyValue.text = $"{harmonyAmount:000}%";
                actionsValue.text = $"{actionsAmount:0}";
            }
        }
    }
}
