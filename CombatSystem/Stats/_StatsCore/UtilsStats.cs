using System;

namespace CombatSystem.Stats
{
    public static class UtilsStats 
    {

        public static void DoCopyMaster<T>(IMainStatsInject<T> inject, IMainStatsRead<T> copyFrom)
        {
            inject.OffensiveStatType = copyFrom.OffensiveStatType;
            inject.SupportStatType = copyFrom.SupportStatType;
            inject.VitalityStatType = copyFrom.VitalityStatType;
            inject.ConcentrationStatType = copyFrom.ConcentrationStatType;
        }

        /// <summary>
        /// Copies all values except those which correspond to [<see cref="IMainStats{T}"/>];<br></br>
        /// For that use [<seealso cref="DoCopyFull{T}"/>] instead which copies all values
        /// </summary>
        public static void DoCopyBasics<T>(IBasicStatsInject<T> inject, IBasicStatsRead<T> copyFrom)
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

        public static T GetElement<T>(EnumStats.MasterStatType type, IMainStatsRead<T> structure)
        {
            return type switch
            {
                EnumStats.MasterStatType.Offensive => structure.OffensiveStatType,
                EnumStats.MasterStatType.Support => structure.SupportStatType,
                EnumStats.MasterStatType.Vitality => structure.VitalityStatType,
                EnumStats.MasterStatType.Concentration => structure.ConcentrationStatType,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
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

        public static void GetElements<T>(EnumStats.StatType type,
            IStatsTypesRead<IBasicStatsRead<T>> structure,
            out T baseElement, out T buffElement, out T burstElement)
        {
            switch (type)
            {
                case EnumStats.StatType.Attack:
                    baseElement = structure.BaseType.AttackType;
                    buffElement = structure.BuffType.AttackType;
                    burstElement = structure.BurstType.AttackType;
                    break;
                case EnumStats.StatType.OverTime:
                    baseElement = structure.BaseType.OverTimeType;
                    buffElement = structure.BuffType.OverTimeType;
                    burstElement = structure.BurstType.OverTimeType;
                    break;
                case EnumStats.StatType.DeBuff:
                    baseElement = structure.BaseType.DeBuffType;
                    buffElement = structure.BuffType.DeBuffType;
                    burstElement = structure.BurstType.DeBuffType;
                    break;
                case EnumStats.StatType.FollowUp:
                    baseElement = structure.BaseType.FollowUpType;
                    buffElement = structure.BuffType.FollowUpType;
                    burstElement = structure.BurstType.FollowUpType;
                    break;
                case EnumStats.StatType.Heal:
                    baseElement = structure.BaseType.HealType;
                    buffElement = structure.BuffType.HealType;
                    burstElement = structure.BurstType.HealType;
                    break;
                case EnumStats.StatType.Shielding:
                    baseElement = structure.BaseType.ShieldingType;
                    buffElement = structure.BuffType.ShieldingType;
                    burstElement = structure.BurstType.ShieldingType;
                    break;
                case EnumStats.StatType.Buff:
                    baseElement = structure.BaseType.BuffType;
                    buffElement = structure.BuffType.BuffType;
                    burstElement = structure.BurstType.BuffType;
                    break;
                case EnumStats.StatType.ReceiveBuff:
                    baseElement = structure.BaseType.ReceiveBuffType;
                    buffElement = structure.BuffType.ReceiveBuffType;
                    burstElement = structure.BurstType.ReceiveBuffType;
                    break;
                case EnumStats.StatType.Health:
                    baseElement = structure.BaseType.HealthType;
                    buffElement = structure.BuffType.HealthType;
                    burstElement = structure.BurstType.HealthType;
                    break;
                case EnumStats.StatType.Mortality:
                    baseElement = structure.BaseType.MortalityType;
                    buffElement = structure.BuffType.MortalityType;
                    burstElement = structure.BurstType.MortalityType;
                    break;
                case EnumStats.StatType.DamageReduction:
                    baseElement = structure.BaseType.DamageReductionType;
                    buffElement = structure.BuffType.DamageReductionType;
                    burstElement = structure.BurstType.DamageReductionType;
                    break;
                case EnumStats.StatType.DebuffResistance:
                    baseElement = structure.BaseType.DeBuffResistanceType;
                    buffElement = structure.BuffType.DeBuffResistanceType;
                    burstElement = structure.BurstType.DeBuffResistanceType;
                    break;
                case EnumStats.StatType.Speed:
                    baseElement = structure.BaseType.SpeedType;
                    buffElement = structure.BuffType.SpeedType;
                    burstElement = structure.BurstType.SpeedType;
                    break;
                case EnumStats.StatType.Actions:
                    baseElement = structure.BaseType.ActionsType;
                    buffElement = structure.BuffType.ActionsType;
                    burstElement = structure.BurstType.ActionsType;
                    break;
                case EnumStats.StatType.Control:
                    baseElement = structure.BaseType.ControlType;
                    buffElement = structure.BuffType.ControlType;
                    burstElement = structure.BurstType.ControlType;
                    break;
                case EnumStats.StatType.Critical:
                    baseElement = structure.BaseType.CriticalType;
                    buffElement = structure.BuffType.CriticalType;
                    burstElement = structure.BurstType.CriticalType;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }



    public static class UtilsBuffStats
    {
        public static void MasterBuffOffensive(IOffensiveStats<float> stats, float percentValue)
        {
            stats.AttackType += percentValue;
            stats.OverTimeType += percentValue;
            stats.DeBuffType += percentValue;
            stats.FollowUpType += percentValue;
        }

        public static void MasterBuffSupport(ISupportStats<float> stats, float percentValue)
        {
            stats.HealType += percentValue;
            stats.ShieldingType += percentValue;
            stats.BuffType += percentValue;
            stats.ReceiveBuffType += percentValue;
        }

        public static void MasterBuffVitality(IVitalityStats<float> stats, float percentValue)
        {
            stats.DamageReductionType += percentValue;
            stats.DeBuffResistanceType += percentValue;
            stats.HealthType *= (1 + percentValue);
        }

        private const float ActionAmountModifier = 10;
        private const float SpeedAmountModifier = 10;
        public static void MasterBuffConcentration(IConcentrationStats<float> stats, float percentValue)
        {
            stats.ControlType += percentValue;
            stats.CriticalType += percentValue;

            stats.SpeedType += percentValue * SpeedAmountModifier;
            stats.ActionsType += percentValue * ActionAmountModifier;
        }

    }
}
