using System;
using CombatSystem.Entity;
using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    [CreateAssetMenu(menuName = "Combat/Effect/Heal",
        fileName = "Heal [Effect]")]
    public class SHealEffect : SEffect, ISupportEffect
    {
        public const string HealEffectTag = "Heal_Effect";
        public override string EffectTag => HealEffectTag;

        public override EnumStats.StatType EffectType => EnumStats.StatType.Heal;

        public override void DoEffect(in CombatEntity performer, in CombatEntity target, in float effectValue)
        {
            float healAmount = effectValue;
            var performerStats = performer.Stats;
            var targetStats = target.Stats;

            UtilsStatsEffects.CalculateHealAmount(in performerStats, ref healAmount);
            UtilsStatsEffects.CalculateReceiveHealAmount(in targetStats, ref healAmount);
            DoHeal(in targetStats, in healAmount);

            // EVENTS
            performer.ProtectionDoneTracker.DoHealth(in target, in healAmount);
            target.ProtectionReceiveTracker.DoHealth(in performer, in healAmount);
        }

        private static void DoHeal(in CombatStats target, in float healAmount)
        {
           UtilsEffect.DoHealTo(in target, in healAmount);
        }
    }
}
