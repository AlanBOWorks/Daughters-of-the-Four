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
        public override EnumStats.StatType EffectType => EnumStats.StatType.Shielding;

        public override void DoEffect(in CombatEntity performer, in CombatEntity target, in float effectValue)
        {
            float addingShields = effectValue;
            var targetStats = target.Stats;

            UtilsStatsEffects.CalculateShieldsAmount(in performer.Stats, ref addingShields);
            UtilsStatsEffects.ClampShieldsAmount(in targetStats, ref addingShields);

            DoShieldAddition(targetStats,in addingShields);

            // EVENTS
            performer.ProtectionDoneTracker.DoShields(in target, in addingShields);
            target.ProtectionReceiveTracker.DoShields(in performer, in addingShields);
        }


        private static void DoShieldAddition(IDamageableStats<float> target, in float addingShields)
        {
            if (addingShields <= 0)
            {
                //todo event of zero shields
            }
            else
            {
                target.CurrentShields += addingShields;
            }
        }
    }
}
