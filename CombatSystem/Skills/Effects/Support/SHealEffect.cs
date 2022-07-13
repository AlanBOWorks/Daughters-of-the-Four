using System;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Localization;
using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    [CreateAssetMenu(menuName = "Combat/Effect/Heal",
        fileName = "Heal [Effect]")]
    public class SHealEffect : SEffect, ISupportEffect
    {
        private const string HealEffectTag = EffectTags.HealEffectTag;
        private const string HealEffectSmallPrefix = EffectTags.DamageEffectPrefix;
        public override string EffectTag => HealEffectTag;
        public override string EffectSmallPrefix => HealEffectSmallPrefix;
        public override EnumsEffect.ConcreteType EffectType => EnumsEffect.ConcreteType.Heal;

        private const string HealValuePrefix = "%";

        public override string GetEffectTooltip(CombatStats performerStats, float effectValue)
        {
            effectValue *= UtilsStatsFormula.CalculateHealPower(performerStats);
            return LocalizeEffects.LocalizeEffectDigitValue(effectValue) + HealValuePrefix;
        }
        public override void DoEffect(EntityPairInteraction entities, ref float effectValue)
        {
            entities.Extract(out var performer, out var target);
            float healAmount = effectValue;
            var performerStats = performer.Stats;
            var targetStats = target.Stats;

            UtilsStatsEffects.CalculateHealAmount(performerStats, ref healAmount);
            UtilsCombatEffect.DoHealTo(targetStats, healAmount);

            // EVENTS
            performer.ProtectionDoneTracker.DoHealth(target, healAmount);
            target.ProtectionReceiveTracker.DoHealth(performer, healAmount);

            CombatSystemSingleton.EventsHolder.OnHealthGain(performer,target, healAmount);

            effectValue = healAmount;
        }
    }
}
