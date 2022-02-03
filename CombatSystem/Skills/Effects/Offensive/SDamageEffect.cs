using CombatSystem.Entity;
using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    [CreateAssetMenu(menuName = "Combat/Effect/Damage",
        fileName = "Damage [Effect]")]
    public class SDamageEffect : SEffect
    {
        public override void DoEffect(in CombatEntity performer, in CombatEntity target, in float effectValue)
        {
            float damage = effectValue;
            var targetStats = target.Stats;

            UtilsStatsEffects.CalculateDamageFromAttackAttribute(in performer.Stats, ref damage);
            UtilsStatsEffects.CalculateDamageReduction(in targetStats,ref damage);
            DoDamage(targetStats, in damage);
        }


        private static void DoDamage(IHealthStats<float> target, in float damageAmount)
        {
            if (damageAmount <= 0)
            {
                //todo call for DamageZeroEvent
            }
            else
                UtilsEffect.DoDamageTo(target, in damageAmount);
        }

    }
}
