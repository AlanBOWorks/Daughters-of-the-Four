using CombatSystem._Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Stats
{
    public sealed class BurstStats : IBasicStatsRead<float>
    {
        public BurstStats()
        {
            AlliesBuffs = new Stats<float>(0);
            SelfBuffs = new Stats<float>(0);
        }

        // Problem: buff done to self are reset the same moment that ally's ones (finish sequence). Some self buff only
        // are useful if are extended beyond the sequence (eg: burst resistance is ended in end sequence and therefore
        // useless in the end).
        //
        // Solution: divide the buff in two depending who's the buffer and reset in their respective moments
        [HorizontalGroup()]
        public readonly StatsBase<float> AlliesBuffs;
        [HorizontalGroup()]
        public readonly StatsBase<float> SelfBuffs;


        public float AttackType => AlliesBuffs.AttackType + SelfBuffs.AttackType + 1;
        public float OverTimeType => AlliesBuffs.OverTimeType + SelfBuffs.OverTimeType + 1;
        public float DeBuffType => AlliesBuffs.DeBuffType + SelfBuffs.DeBuffType + 1;
        public float FollowUpType => AlliesBuffs.FollowUpType + SelfBuffs.FollowUpType + 1;
        public float HealType => AlliesBuffs.HealType + SelfBuffs.HealType + 1;
        public float ShieldingType => AlliesBuffs.ShieldingType + SelfBuffs.ShieldingType + 1;
        public float BuffType => AlliesBuffs.BuffType + SelfBuffs.BuffType + 1;
        public float ReceiveBuffType => AlliesBuffs.ReceiveBuffType + SelfBuffs.ReceiveBuffType + 1;
        public float HealthType => AlliesBuffs.HealthType + SelfBuffs.HealthType + 1;
        public float MortalityType => AlliesBuffs.MortalityType + SelfBuffs.MortalityType + 1;
        public float DamageReductionType => AlliesBuffs.DamageReductionType + SelfBuffs.DamageReductionType + 1;
        public float DeBuffResistanceType => AlliesBuffs.DeBuffResistanceType + SelfBuffs.DeBuffResistanceType + 1;
        public float ActionsType => AlliesBuffs.ActionsType + SelfBuffs.ActionsType + 1;
        public float SpeedType => AlliesBuffs.SpeedType + SelfBuffs.SpeedType + 1;
        public float ControlType => AlliesBuffs.ControlType + SelfBuffs.ControlType + 1;
        public float CriticalType => AlliesBuffs.CriticalType + SelfBuffs.CriticalType + 1;

        public void OnEntityStartSequence()
        {
            SelfBuffs.OverrideByValue(0);
        }

        public void OnEntityFinishSequence()
        {
            AlliesBuffs.OverrideByValue(0);
        }
    }
}
