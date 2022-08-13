using CombatSystem.Entity;
using CombatSystem.Stats;
using UnityEngine;
using Utils.Maths;

namespace CombatSystem.Player.UI
{
    public class UHealthInfo : MonoBehaviour
    {
        [SerializeField] private UPercentBarInfo healthType;
        [SerializeField] private UPercentBarInfo mortalityType;

        public void UpdateHealth(IDamageableStats<float> currentHealthHolder, IVitalityStatsRead<float> maxHealthHolder)
        {
            if(currentHealthHolder == null) return;

            UtilsVitality.ExtractVitalityValues(
                currentHealthHolder,maxHealthHolder, 
                out var health, out var mortality);

            UpdateHealth(health,mortality);
        }
        public void UpdateHealth(CombatEntity entity)
        {
            var stats = entity.Stats;
            UtilsVitality.ExtractVitalityValues(stats, 
                out var health, out var mortality);

            UpdateHealth(health, mortality);
        }
        public void FirstInjectionHealth(CombatEntity entity)
        {
            var stats = entity.Stats;
            UtilsVitality.ExtractVitalityValues(stats,
                out var health, out var mortality);

            healthType.FirstInjectInfo(health);
            mortalityType.FirstInjectInfo(mortality);
        }

        public void UpdateHealth(PercentValue health, PercentValue mortality)
        {
            healthType.UpdateInfo(health);
            mortalityType.UpdateInfo(mortality);
        }

        public void DisableHealthValues()
        {
            healthType.enabled = false;
        }

    }
}
