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

        public override string EffectTag => ShieldEffectTag;
        public override string EffectSmallPrefix => ShieldEffectSmallPrefix;
        public override EnumsEffect.ConcreteType EffectType => EnumsEffect.ConcreteType.Shielding;

        public override void DoEffect(EntityPairInteraction entities,ref float effectValue)
        {
            entities.Extract(out var performer, out var target);
            float addingShields = effectValue;
            var targetStats = target.Stats;

            UtilsStatsEffects.CalculateShieldsAmount(performer.Stats, ref addingShields);
            UtilsStatsEffects.ClampShieldsAmount(targetStats, ref addingShields);

            DoShieldAddition(targetStats,addingShields);

            // EVENTS
            performer.ProtectionDoneTracker.DoShields(target, addingShields);
            target.ProtectionReceiveTracker.DoShields(performer, addingShields);

            effectValue = addingShields;
        }


        private static void DoShieldAddition(IDamageableStats<float> target, float addingShields)
        {
            if (addingShields > 0)
            {
                target.CurrentShields += addingShields;
            }
            else
            {
                //todo event of zero shields
            }
        }
    }
}
