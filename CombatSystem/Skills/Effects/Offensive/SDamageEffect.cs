using CombatSystem.Entity;
using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    [CreateAssetMenu(menuName = "Combat/Effect/Damage",
        fileName = "Damage [Effect]")]
    public class SDamageEffect : SEffect, IOffensiveEffect
    {

        private const string DamageEffectTag = "Damage_Effect";
        public override string EffectTag => DamageEffectTag;
        public override EnumStats.StatType EffectType => EnumStats.StatType.Attack;

        public override void DoEffect(in CombatEntity performer, in CombatEntity target, in float effectValue)
        {
            float damage = effectValue;
            var targetStats = target.Stats;

            UtilsStatsEffects.CalculateDamageFromAttackAttribute(in performer.Stats, ref damage);
            UtilsStatsEffects.CalculateDamageReduction(in targetStats,ref damage);


            if (damage <= 0)
            {
                //todo call for DamageZeroEvent
            }
            else
            {
                UtilsEffect.DoDamageTo(in target, in performer, in damage);
            }
        }
    }

}
