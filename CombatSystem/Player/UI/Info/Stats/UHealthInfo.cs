using CombatSystem.Entity;
using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UHealthInfo : MonoBehaviour
    {
        [SerializeField] private UPercentBarInfo healthType;
        [SerializeField] private UPercentBarInfo mortalityType;


        public void UpdateHealth(CombatEntity entity)
        {
            var stats = entity.Stats;
            UtilsVitality.ExtractVitalityValues(stats, 
                out var health, out var mortality);

            healthType.UpdateInfo(health);
            mortalityType.UpdateInfo(mortality);
        }
        public void FirstInjectionHealth(CombatEntity entity)
        {
            var stats = entity.Stats;
            UtilsVitality.ExtractVitalityValues(stats,
                out var health, out var mortality);

            healthType.FirstInjectInfo(health);
            mortalityType.FirstInjectInfo(mortality);
        }

    }
}
