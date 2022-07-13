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
        private const string DamageValuePrefix = "u.";
        public override string GetEffectTooltip(CombatStats performerStats, float effectValue)
        {
            effectValue *= UtilsStatsFormula.CalculateAttackPower(performerStats);
            return LocalizeEffects.LocalizeEffectDigitValue(effectValue) + DamageValuePrefix;
        }

        public override string EffectTag => DamageEffectTag;
        public override string EffectSmallPrefix => DamageSmallPrefix;
        public override EnumsEffect.ConcreteType EffectType => EnumsEffect.ConcreteType.DamageType;

        public override void DoEffect(EntityPairInteraction entities, ref float effectValue)
        {
            entities.Extract(out var performer, out var target);
            var performerStats = performer.Stats;
            var targetStats = target.Stats;

            var performerAttackPower = UtilsStatsFormula.CalculateAttackPower(performerStats);
            var targetDamageReduction = UtilsStatsFormula.CalculateDamageReduction(targetStats);
            float damage = UtilsStatsEffects.CalculateFinalDamage(effectValue, performerAttackPower, targetDamageReduction);

            if (damage <= 0)
            {
                //todo call for DamageZeroEvent
            }
            else
            {
                UtilsCombatEffect.DoDamageTo(target, performer, damage);
            }
        }

    }

}
