using System;
using CombatSystem.Entity;
using CombatSystem.Skills;
using UnityEngine;

namespace CombatSystem.Stats
{
    public static class UtilsStats 
    {

        public static float GetCharacterControlAmount(CombatStats stats)
        {
            return stats.ConcentrationType * (
                stats.BaseStats.ControlType +
                stats.BuffStats.ControlType +
                stats.BurstStats.ControlType);
        }

        public static void DoCopyMaster<T>(IMainStatsInject<T> inject, IMainStatsRead<T> copyFrom)
        {
            inject.OffensiveType = copyFrom.OffensiveType;
            inject.SupportType = copyFrom.SupportType;
            inject.VitalityType = copyFrom.VitalityType;
            inject.ConcentrationType = copyFrom.ConcentrationType;
        }

        /// <summary>
        /// Copies all values except those which correspond to [<see cref="IMainStats{T}"/>];<br></br>
        /// For that use [<seealso cref="DoCopyFull{T}"/>] instead which copies all values
        /// </summary>
        public static void DoCopyBasics<T>(IStatsInject<T> inject, IStatsRead<T> copyFrom)
        {
            inject.AttackType = copyFrom.AttackType;
            inject.OverTimeType = copyFrom.OverTimeType;
            inject.DeBuffType = copyFrom.DeBuffType;
            inject.FollowUpType = copyFrom.FollowUpType;


            inject.HealType = copyFrom.HealType;
            inject.ShieldingType = copyFrom.ShieldingType;
            inject.BuffType = copyFrom.BuffType;
            inject.ReceiveBuffType = copyFrom.ReceiveBuffType;


            inject.HealthType = copyFrom.HealthType;
            inject.MortalityType = copyFrom.MortalityType;
            inject.DamageReductionType = copyFrom.DamageReductionType;
            inject.DeBuffResistanceType = copyFrom.DeBuffResistanceType;


            inject.ActionsType = copyFrom.ActionsType;
            inject.SpeedType = copyFrom.SpeedType;
            inject.ControlType = copyFrom.ControlType;
            inject.CriticalType = copyFrom.CriticalType;
        }

        public static void DoCopyFull<T>(IStatsInject<T> inject, IStatsRead<T> copyFrom)
        {
            DoCopyBasics(inject,copyFrom);
            DoCopyMaster(inject,copyFrom);
        }


        public static void OverrideValuesOffensive<T>(IOffensiveStatsInject<T> data, T value)
        {
            data.AttackType = value;
            data.OverTimeType = value;
            data.DeBuffType = value;
            data.FollowUpType = value;
        }
        public static void OverrideValuesSupport<T>(ISupportStats<T> data, T value)
        {
            data.HealType = value;
            data.BuffType = value;
            data.ReceiveBuffType = value;
            data.ShieldingType = value;
        }

        public static void OverrideValuesVitality<T>(IVitalityStatsInject<T> data, T value)
        {
            data.HealthType = value;
            data.MortalityType = value;
            data.DamageReductionType = value;
            data.DeBuffResistanceType = value;
        }

        public static void OverrideValuesConcentration<T>(IConcentrationStatsInject<T> data, T value)
        {
            data.ActionsType = value;
            data.SpeedType = value;
            data.ControlType = value;
            data.CriticalType = value;
        }


        public static T GetElement<T>(EnumStats.OffensiveStatType type, IOffensiveStatsRead<T> structure)
        {
            return type switch
            {
                EnumStats.OffensiveStatType.Attack => structure.AttackType,
                EnumStats.OffensiveStatType.OverTime => structure.OverTimeType,
                EnumStats.OffensiveStatType.DeBuff => structure.DeBuffType,
                EnumStats.OffensiveStatType.FollowUp => structure.FollowUpType,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        public static T GetElement<T>(EnumStats.SupportStatType type, ISupportStatsRead<T> structure)
        {
            return type switch
            {
                EnumStats.SupportStatType.Heal => structure.HealType,
                EnumStats.SupportStatType.Shielding => structure.ShieldingType,
                EnumStats.SupportStatType.Buff => structure.BuffType,
                EnumStats.SupportStatType.ReceiveBuff => structure.ReceiveBuffType,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        public static T GetElement<T>(EnumStats.VitalityStatType type, IVitalityStatsRead<T> structure)
        {
            return type switch
            {
                EnumStats.VitalityStatType.Health => structure.HealthType,
                EnumStats.VitalityStatType.Mortality => structure.MortalityType,
                EnumStats.VitalityStatType.DamageReduction => structure.DamageReductionType,
                EnumStats.VitalityStatType.DebuffResistance => structure.DeBuffResistanceType,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public static T GetElement<T>(EnumStats.ConcentrationStatType type, IConcentrationStatsRead<T> structure)
        {
            return type switch
            {
                EnumStats.ConcentrationStatType.Speed => structure.SpeedType,
                EnumStats.ConcentrationStatType.Actions => structure.ActionsType,
                EnumStats.ConcentrationStatType.Control => structure.ControlType,
                EnumStats.ConcentrationStatType.Critical => structure.CriticalType,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public static T GetElement<T>(EnumStats.StatType type, IBasicStatsRead<T> structure)
        {
            return type switch
            {
                EnumStats.StatType.Attack => structure.AttackType,
                EnumStats.StatType.OverTime => structure.OverTimeType,
                EnumStats.StatType.DeBuff => structure.DeBuffType,
                EnumStats.StatType.FollowUp => structure.FollowUpType,

                EnumStats.StatType.Heal => structure.HealType,
                EnumStats.StatType.Shielding => structure.ShieldingType,
                EnumStats.StatType.Buff => structure.BuffType,
                EnumStats.StatType.ReceiveBuff => structure.ReceiveBuffType,

                EnumStats.StatType.Health => structure.HealthType,
                EnumStats.StatType.Mortality => structure.MortalityType,
                EnumStats.StatType.DamageReduction => structure.DamageReductionType,
                EnumStats.StatType.DebuffResistance => structure.DeBuffResistanceType,

                EnumStats.StatType.Speed => structure.SpeedType,
                EnumStats.StatType.Actions => structure.ActionsType,
                EnumStats.StatType.Control => structure.ControlType,
                EnumStats.StatType.Critical => structure.CriticalType,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }


    public static class UtilsCombatStats
    {
        /// <summary>
        /// Determines if the [<see cref="CombatEntity"/>] has the requirements to be affected by first call
        /// of actions/Tempo.
        /// </summary>
        public static bool CanRequestActing(CombatEntity entity)
        {
            var stats = entity.Stats;
            float actionsAmount = stats.BaseStats.ActionsType;
            // A zero actions is equivalent to a passive Entity (and can be affected on start sequence events/effects)
            // while a negative actions type are in-mobile entities (obstacles like entities) than can't be affected by
            // "time" nor "effects" related to tempo
            if (actionsAmount < 0) return false;

            return entity.AllSkills.Count > 0;
        }

        private const float MaxActionAmount = 12f;
        public static bool CanActRequest(in CombatEntity entity) => CanActRequest(entity.Stats);
        public static bool CanActRequest(in CombatStats stats)
        {
            float actionsLimit =
                stats.BaseStats.ActionsType + stats.BuffStats.ActionsType + stats.BurstStats.ActionsType;
            if (actionsLimit > MaxActionAmount) actionsLimit = MaxActionAmount;

            return stats.UsedActions < actionsLimit;
        }

        public static bool IsAlive(in CombatEntity entity) => IsAlive(entity.Stats);
        public static bool IsAlive(in CombatStats stats)
        {
            return stats.CurrentMortality > 0 || stats.CurrentHealth > 0;
        }

        public static void TickActions(in CombatStats stats, in CombatSkill usedSkill)
        {
            stats.UsedActions += usedSkill.SkillCost;
        }
    }
}
