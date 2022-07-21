using System.Globalization;
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

        public override float CalculateEffectByStatValue(CombatStats performerStats, float effectValue)
        {
            return effectValue * UtilsStatsFormula.CalculateAttackPower(performerStats);
        }
        public override bool IsPercentSuffix() => false;


        public override string EffectTag => DamageEffectTag;
        public override string EffectSmallPrefix => DamageSmallPrefix;
        public override EnumsEffect.ConcreteType EffectType => EnumsEffect.ConcreteType.DamageType;

        public override void DoEffect(EntityPairInteraction entities, ref float effectValue, ref float luckModifier)
        {
            entities.Extract(out var performer, out var target);
            var performerStats = performer.Stats;
            var targetStats = target.Stats;

            var performerAttackPower = UtilsStatsFormula.CalculateAttackPower(performerStats);
            var targetDamageReduction = UtilsStatsFormula.CalculateDamageReduction(targetStats);
            float damage = UtilsStatsEffects.CalculateFinalDamage(effectValue, performerAttackPower, targetDamageReduction);
            damage *= luckModifier;
            if (damage <= 0)
            {
                //todo call for DamageZeroEvent
            }
            else
            {
                UtilsCombatEffect.DoDamageTo(target, performer, damage);
            }

            effectValue = damage;
        }

    }

}
