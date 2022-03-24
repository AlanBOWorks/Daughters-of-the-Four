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


        public float AttackType => AlliesBuffs.AttackType + SelfBuffs.AttackType;
        public float OverTimeType => AlliesBuffs.OverTimeType + SelfBuffs.OverTimeType;
        public float DeBuffType => AlliesBuffs.DeBuffType + SelfBuffs.DeBuffType;
        public float FollowUpType => AlliesBuffs.FollowUpType + SelfBuffs.FollowUpType;


        public float HealType => AlliesBuffs.HealType + SelfBuffs.HealType;
        public float ShieldingType => AlliesBuffs.ShieldingType + SelfBuffs.ShieldingType;
        public float BuffType => AlliesBuffs.BuffType + SelfBuffs.BuffType;
        public float ReceiveBuffType => AlliesBuffs.ReceiveBuffType + SelfBuffs.ReceiveBuffType;


        public float HealthType => AlliesBuffs.HealthType + SelfBuffs.HealthType;
        public float MortalityType => AlliesBuffs.MortalityType + SelfBuffs.MortalityType;
        public float DamageReductionType => AlliesBuffs.DamageReductionType + SelfBuffs.DamageReductionType;
        public float DeBuffResistanceType => AlliesBuffs.DeBuffResistanceType + SelfBuffs.DeBuffResistanceType;


        public float ActionsType => AlliesBuffs.ActionsType + SelfBuffs.ActionsType;
        public float SpeedType => AlliesBuffs.SpeedType + SelfBuffs.SpeedType;
        public float ControlType => AlliesBuffs.ControlType + SelfBuffs.ControlType;
        public float CriticalType => AlliesBuffs.CriticalType + SelfBuffs.CriticalType;

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
