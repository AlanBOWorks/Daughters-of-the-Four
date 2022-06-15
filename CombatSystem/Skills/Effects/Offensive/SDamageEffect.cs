using CombatSystem.Entity;
using CombatSystem.Localization;
using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    [CreateAssetMenu(menuName = "Combat/Effect/Damage",
        fileName = "Damage [Effect]")]
    public class SDamageEffect : SEffect, IOffensiveEffect
    {

        private const string DamageEffectTag = EffectTags.DamageEffectTag;
        private const string DamageSmallPrefix = EffectTags.DamageEffectPrefix;
        public override string EffectTag => DamageEffectTag;
        public override string EffectSmallPrefix => DamageSmallPrefix;
        public override EnumsEffect.ConcreteType EffectType => EnumsEffect.ConcreteType.DamageType;

        public override void DoEffect(CombatEntity performer, CombatEntity target, float effectValue)
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
