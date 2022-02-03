using CombatSystem.Entity;
using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    [CreateAssetMenu(menuName = "Combat/Effect/Heal",
        fileName = "Heal [Effect]")]
    public class SHealEffect : SEffect
    {
        public override void DoEffect(in CombatEntity performer, in CombatEntity target, in float effectValue)
        {
            float healAmount = effectValue;
            var performerStats = performer.Stats;
            var targetStats = target.Stats;

            UtilsStatsEffects.CalculateHealAmount(in performerStats, ref healAmount);
            UtilsStatsEffects.CalculateReceiveHealAmount(in targetStats, ref healAmount);
            DoHeal(in targetStats, in healAmount);
        }

        private static void DoHeal(in CombatStats target, in float healAmount)
        {
            float targetHealth = target.CurrentHealth + healAmount;
            float maxHealth = UtilsStatsFormula.CalculateMaxHealth(target);
            if (targetHealth >= maxHealth)
            {
                targetHealth = maxHealth;

                //todo call MaxHealth event
            }

            target.CurrentHealth = targetHealth;
        }
    }
}