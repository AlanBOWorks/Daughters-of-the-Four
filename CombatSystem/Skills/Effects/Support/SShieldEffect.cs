using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Localization;
using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    [CreateAssetMenu(menuName = "Combat/Effect/Shield",
        fileName = "Shielding [Effect]")]
    public class SShieldEffect : SEffect, ISupportEffect
    {
        private const string ShieldEffectTag = EffectTags.ShieldingEffectName;
        private const string ShieldEffectSmallPrefix = EffectTags.ShieldingEffectPrefix;

        private const string ShieldValuePrefix = "u.";

        public override string GetEffectTooltip(CombatStats performerStats, float effectValue)
        {
            effectValue *= UtilsStatsFormula.CalculateShieldingPower(performerStats);
            return LocalizeEffects.LocalizeEffectDigitValue(effectValue) + ShieldValuePrefix;
        }

        public override string EffectTag => ShieldEffectTag;
        public override string EffectSmallPrefix => ShieldEffectSmallPrefix;
        public override EnumsEffect.ConcreteType EffectType => EnumsEffect.ConcreteType.Shielding;

        public override void DoEffect(EntityPairInteraction entities,ref float effectValue)
        {
            entities.Extract(out var performer, out var target);
            float addingShields = effectValue;
            var targetStats = target.Stats;

            UtilsStatsEffects.CalculateShieldsAmount(performer.Stats, ref addingShields);

            DoShieldAddition(targetStats,addingShields);

            // EVENTS
            performer.ProtectionDoneTracker.DoShields(target, addingShields);
            target.ProtectionReceiveTracker.DoShields(performer, addingShields);
            if (addingShields <= 0) return;


            CombatSystemSingleton.EventsHolder.OnShieldGain(performer, target, addingShields);
            effectValue = addingShields;
        }


        private static void DoShieldAddition(IDamageableStats<float> target, float addingShields)
        {
            var targetShields = target.CurrentShields + addingShields;
            const float maxShields = UtilsStatsEffects.VanillaMaxShieldAmount;
            if (targetShields > maxShields) targetShields = maxShields;
            target.CurrentShields = targetShields;
        }
    }
}
