using System;
using UnityEngine;

namespace Stats
{
    public static class UtilStats
    {
        public static float StatFormulaStackingBuffs(float baseStat, float buffStat, float burstStat)
        {
            return baseStat * (1+ buffStat) * (1 + burstStat); 
        }

        public static float StatFormulaPercentageAddition(float baseStat, float buffStat, float burstStat)
        {
            return baseStat + buffStat + burstStat;
        }

        public static void FuncOn<T>(Func<T,T> func, IBaseStats<T> variate)
        {
            FuncOn(func,variate,variate);
        }

        public static float CriticalBuffAddition = .25f;
        public static float CalculateBuffPower(CombatStatsHolder stats, float buffValue, bool isCriticalBuff)
        {
            buffValue *= stats.Buff;
            if (isCriticalBuff) buffValue += CriticalBuffAddition;
            return buffValue;
        }

        public static void FuncOn<T>(Func<T,T> func, IBaseStatsInject<T> injectIn, IBaseStatsRead<T> readFrom)
        {
            injectIn.Attack = func(readFrom.Attack);
            injectIn.Persistent = func(readFrom.Persistent);
            injectIn.Debuff = func(readFrom.Debuff);
            injectIn.FollowUp = func(readFrom.FollowUp);

            injectIn.Heal = func(readFrom.Heal);
            injectIn.Buff = func(readFrom.Buff);
            injectIn.ReceiveBuff = func(readFrom.ReceiveBuff);
            injectIn.Shielding = func(readFrom.Shielding);

            injectIn.MaxHealth = func(readFrom.MaxHealth);
            injectIn.MaxMortality = func(readFrom.MaxMortality);
            injectIn.DebuffResistance = func(readFrom.DebuffResistance);
            injectIn.DamageResistance = func(readFrom.DamageResistance);

            injectIn.InitiativeSpeed = func(readFrom.InitiativeSpeed);
            injectIn.Critical = func(readFrom.Critical);
            injectIn.InitialInitiative = func(readFrom.InitialInitiative);
            injectIn.ActionsPerSequence = func(readFrom.ActionsPerSequence);
        }

        public static void FuncOn<T>(Func<T, T, T> func, IBaseStats<T> injectIn, IOffensiveStatsRead<T> readFrom)
        {
            injectIn.Attack = func(injectIn.Attack, readFrom.Attack);
            injectIn.Persistent = func(injectIn.Persistent, readFrom.Persistent);
            injectIn.Debuff = func(injectIn.Debuff, readFrom.Debuff);
            injectIn.FollowUp = func(injectIn.FollowUp, readFrom.FollowUp);
        }

        public static void FuncOn<T>(Func<T, T, T> func, IBaseStats<T> injectIn, ISupportStatsRead<T> readFrom)
        {
            injectIn.Heal = func(injectIn.Heal, readFrom.Heal);
            injectIn.Buff = func(injectIn.Buff, readFrom.Buff);
            injectIn.ReceiveBuff = func(injectIn.ReceiveBuff, readFrom.ReceiveBuff);
            injectIn.Shielding = func(injectIn.Shielding, readFrom.Shielding);
        }

        public static void FuncOn<T>(Func<T, T, T> func, IBaseStats<T> injectIn, IVitalityStatsRead<T> readFrom)
        {
            injectIn.MaxHealth = func(injectIn.MaxHealth, readFrom.MaxHealth);
            injectIn.MaxMortality = func(injectIn.MaxMortality, readFrom.MaxMortality);
            injectIn.DebuffResistance = func(injectIn.DebuffResistance, readFrom.DebuffResistance);
            injectIn.DamageResistance = func(injectIn.DamageResistance, readFrom.DamageResistance);
        }

        public static void FuncOn<T>(Func<T, T, T> func, IBaseStats<T> injectIn, IConcentrationStatsRead<T> readFrom)
        {
            injectIn.InitiativeSpeed = func(injectIn.InitiativeSpeed, readFrom.InitiativeSpeed);
            injectIn.Critical = func(injectIn.Critical, readFrom.Critical);
            injectIn.InitialInitiative = func(injectIn.InitialInitiative, readFrom.InitialInitiative);
            injectIn.ActionsPerSequence = func(injectIn.ActionsPerSequence, readFrom.ActionsPerSequence);
        }


        public static void FuncOn<T>(Func<T,T,T> func, IBaseStats<T> injectIn, IBaseStatsRead<T> readFrom)
        {
            FuncOn(func,injectIn,readFrom as IOffensiveStatsRead<T>);
            FuncOn(func,injectIn,readFrom as ISupportStatsRead<T>);
            FuncOn(func,injectIn,readFrom as IVitalityStatsRead<T>);
            FuncOn(func,injectIn,readFrom as IConcentrationStatsRead<T>);
        }



        public static void CopyValues<T>(IBaseStatsInject<T> injectIn, IBaseStatsRead<T> copyValues)
        {
            FuncOn(GetElement,injectIn,copyValues);
            T GetElement(T element) => element;
        }

        public static void OverrideByValue<T>(IBaseStatsInject<T> stats, T overrideAllBy)
        {
            stats.Attack = overrideAllBy;
            stats.Persistent = overrideAllBy;
            stats.Debuff = overrideAllBy;
            stats.FollowUp = overrideAllBy;

            stats.Heal = overrideAllBy;
            stats.Buff = overrideAllBy;
            stats.ReceiveBuff = overrideAllBy;
            stats.Shielding = overrideAllBy;

            stats.MaxHealth = overrideAllBy;
            stats.MaxMortality = overrideAllBy;
            stats.DebuffResistance = overrideAllBy;
            stats.DamageResistance = overrideAllBy;

            stats.InitiativeSpeed = overrideAllBy;
            stats.Critical = overrideAllBy;
            stats.InitialInitiative = overrideAllBy;
            stats.ActionsPerSequence = overrideAllBy;
        }

        private static float Addition(float original, float add) => original + add;

        public static void SumStats(IBaseStats<float> into, IBaseStatsRead<float> addition) 
            => FuncOn(Addition,into,addition);
        public static void SumStats(IBaseStats<float> into, IOffensiveStatsRead<float> addition)
            => FuncOn(Addition, into, addition);
        public static void SumStats(IBaseStats<float> into, ISupportStatsRead<float> addition)
            => FuncOn(Addition, into, addition);
        public static void SumStats(IBaseStats<float> into, IVitalityStatsRead<float> addition)
            => FuncOn(Addition, into, addition);
        public static void SumStats(IBaseStats<float> into, IConcentrationStatsRead<float> addition)
            => FuncOn(Addition, into, addition);

        public static void SumStats(IBaseStats<float> stats, float addition)
        {
            FuncOn(Sum,stats);
            float Sum(float original) => original + addition;
        }

        public static void MultiplyStats(IBaseStatsInject<float> injectInto, IBaseStatsRead<float> readFrom,
            IMasterStatsRead<float> multiplies)
        {
            MultiplyStats(injectInto as IOffensiveStatsInject<float>, readFrom, multiplies.Offensive);
            MultiplyStats(injectInto as ISupportStatsInject<float>,readFrom,multiplies.Support);
            MultiplyStats(injectInto as IVitalityStatsInject<float>, readFrom, multiplies.Vitality);
            MultiplyStats(injectInto as IConcentrationStatsInject<float>,readFrom,multiplies.Concentration);
        }

        public static void MultiplyStats(IOffensiveStatsInject<float> injectInto, IOffensiveStatsRead<float> readFrom,
            float multiplier)
        {
            injectInto.Attack = readFrom.Attack * multiplier;
            injectInto.Persistent = readFrom.Persistent * multiplier;
            injectInto.Debuff = readFrom.Debuff * multiplier;
            injectInto.FollowUp = readFrom.FollowUp * multiplier;
        }
        public static void MultiplyStats(ISupportStatsInject<float> injectInto, ISupportStatsRead<float> readFrom,
            float multiplier)
        {
            injectInto.Heal = readFrom.Heal * multiplier;
            injectInto.Buff = readFrom.Buff * multiplier;
            injectInto.ReceiveBuff = readFrom.ReceiveBuff * multiplier;
            injectInto.Shielding = readFrom.Shielding * multiplier;
        }
        public static void MultiplyStats(IVitalityStatsInject<float> injectInto, IVitalityStatsRead<float> readFrom,
            float multiplier)
        {
            injectInto.MaxHealth = readFrom.MaxHealth * multiplier;
            injectInto.MaxMortality = readFrom.MaxMortality * multiplier;
            injectInto.DebuffResistance = readFrom.DebuffResistance * multiplier;
            injectInto.DamageResistance = readFrom.DamageResistance * multiplier;
        }
        public static void MultiplyStats(IConcentrationStatsInject<float> injectInto, IConcentrationStatsRead<float> readFrom,
            float multiplier)
        {
            injectInto.InitiativeSpeed = readFrom.InitiativeSpeed * multiplier;
            injectInto.Critical = readFrom.Critical * multiplier;
            injectInto.InitialInitiative = readFrom.InitialInitiative * multiplier;
            injectInto.ActionsPerSequence = readFrom.ActionsPerSequence * multiplier;
        }

        public static void OverrideStats<T>(IMasterStatsInject<T> onTo, IMasterStatsRead<T> readFrom)
        {
            onTo.Offensive = readFrom.Offensive;
            onTo.Support = readFrom.Support;
            onTo.Vitality = readFrom.Vitality;
            onTo.Concentration = readFrom.Concentration;
        }

        public static T GetElement<T>(IBehaviourStatsRead<T> stats, EnumStats.BuffType type)
        {
            return type switch
            {
                EnumStats.BuffType.Base => stats.BaseStats,
                EnumStats.BuffType.Buff => stats.BuffStats,
                EnumStats.BuffType.Burst => stats.BurstStats,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public static T GetElement<T>(IMasterStatsRead<T> masterStatsStructure, EnumStats.MasterStatType type)
        {
            switch (type)
            {
                case EnumStats.MasterStatType.Offensive:
                    return masterStatsStructure.Offensive;
                case EnumStats.MasterStatType.Support:
                    return masterStatsStructure.Support;
                case EnumStats.MasterStatType.Vitality:
                    return masterStatsStructure.Vitality;
                case EnumStats.MasterStatType.Concentration:
                    return masterStatsStructure.Concentration;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static T GetElement<T>(IOffensiveStatsRead<T> offensiveStructure, EnumStats.OffensiveStatType type)
        {
            switch (type)
            {
                case EnumStats.OffensiveStatType.Attack:
                    return offensiveStructure.Attack;
                case EnumStats.OffensiveStatType.Persistent:
                    return offensiveStructure.Persistent;
                case EnumStats.OffensiveStatType.DeBuff:
                    return offensiveStructure.Debuff;
                case EnumStats.OffensiveStatType.FollowUp:
                    return offensiveStructure.FollowUp;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static T GetElement<T>(ICondensedOffensiveStat<T,T> offensiveStructure, EnumStats.OffensiveStatType type)
        where T : class
        {
            var element = GetElement(offensiveStructure as IOffensiveStatsRead<T>, type);
            return element ?? offensiveStructure.Offensive;
        }

        public static T GetElement<T>(ISupportStatsRead<T> supportStructure, EnumStats.SupportStatType type)
        {
            switch (type)
            {
                case EnumStats.SupportStatType.Heal:
                    return supportStructure.Heal;
                case EnumStats.SupportStatType.Buff:
                    return supportStructure.Buff;
                case EnumStats.SupportStatType.ReceiveBuff:
                    return supportStructure.ReceiveBuff;
                case EnumStats.SupportStatType.Shielding:
                    return supportStructure.Shielding;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static T GetElement<T>(ICondensedSupportStat<T, T> supportStructure, EnumStats.SupportStatType type)
        where T : class
        {
            var element = GetElement(supportStructure as ISupportStatsRead<T>, type);
            return element ?? supportStructure.Support;
        }

        public static T GetElement<T>(IVitalityStatsRead<T> vitalityStructure, EnumStats.VitalityStatType type)
        {
            switch (type)
            {
                case EnumStats.VitalityStatType.MaxHealth:
                    return vitalityStructure.MaxHealth;
                case EnumStats.VitalityStatType.MaxMortality:
                    return vitalityStructure.MaxMortality;
                case EnumStats.VitalityStatType.DebuffResistance:
                    return vitalityStructure.DebuffResistance;
                case EnumStats.VitalityStatType.DamageResistance:
                    return vitalityStructure.DamageResistance;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static T GetElement<T>(IConcentrationStatsRead<T> concentrationStructure,
            EnumStats.ConcentrationStatType type)
        {
            switch (type)
            {
                case EnumStats.ConcentrationStatType.InitiativeSpeed:
                    return concentrationStructure.InitiativeSpeed;
                case EnumStats.ConcentrationStatType.InitialInitiative:
                    return concentrationStructure.InitialInitiative;
                case EnumStats.ConcentrationStatType.ActionsPerSequence:
                    return concentrationStructure.ActionsPerSequence;
                case EnumStats.ConcentrationStatType.Critical:
                    return concentrationStructure.Critical;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }


        public static T GetElement<T>(IBaseStatsRead<T> baseStatsStructure, EnumStats.BaseStatType type)
        {
            switch (type)
            {
                case EnumStats.BaseStatType.Attack:
                    return baseStatsStructure.Attack;
                case EnumStats.BaseStatType.Persistent:
                    return baseStatsStructure.Persistent;
                case EnumStats.BaseStatType.DeBuff:
                    return baseStatsStructure.Debuff;
                case EnumStats.BaseStatType.FollowUp:
                    return baseStatsStructure.FollowUp;

                case EnumStats.BaseStatType.Heal:
                    return baseStatsStructure.Heal;
                case EnumStats.BaseStatType.Buff:
                    return baseStatsStructure.Buff;
                case EnumStats.BaseStatType.ReceiveBuff:
                    return baseStatsStructure.ReceiveBuff;
                case EnumStats.BaseStatType.Shielding:
                    return baseStatsStructure.Shielding;

                case EnumStats.BaseStatType.MaxHealth:
                    return baseStatsStructure.MaxHealth;
                case EnumStats.BaseStatType.MaxMortality:
                    return baseStatsStructure.MaxMortality;
                case EnumStats.BaseStatType.DebuffResistance:
                    return baseStatsStructure.DebuffResistance;
                case EnumStats.BaseStatType.DamageResistance:
                    return baseStatsStructure.DamageResistance;

                case EnumStats.BaseStatType.InitiativeSpeed:
                    return baseStatsStructure.InitiativeSpeed;
                case EnumStats.BaseStatType.InitialInitiative:
                    return baseStatsStructure.InitialInitiative;
                case EnumStats.BaseStatType.ActionsPerSequence:
                    return baseStatsStructure.ActionsPerSequence;
                case EnumStats.BaseStatType.Critical:
                    return baseStatsStructure.Critical;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static T GetElement<T>(IBaseStatsRead<T> baseStatsStructure, IMasterStatsRead<T> backUpElements,
            EnumStats.BaseStatType type)
        where T : class
        {
            var element = GetElement(baseStatsStructure, type);
            if (element != null) return element;

            return type switch
            {
                EnumStats.BaseStatType.Attack => backUpElements.Offensive,
                EnumStats.BaseStatType.Persistent => backUpElements.Offensive,
                EnumStats.BaseStatType.DeBuff => backUpElements.Offensive,
                EnumStats.BaseStatType.FollowUp => backUpElements.Offensive,

                EnumStats.BaseStatType.Heal => backUpElements.Support,
                EnumStats.BaseStatType.Buff => backUpElements.Support,
                EnumStats.BaseStatType.ReceiveBuff => backUpElements.Support,
                EnumStats.BaseStatType.Shielding => backUpElements.Support,

                EnumStats.BaseStatType.MaxHealth => backUpElements.Vitality,
                EnumStats.BaseStatType.MaxMortality => backUpElements.Vitality,
                EnumStats.BaseStatType.DebuffResistance => backUpElements.Vitality,
                EnumStats.BaseStatType.DamageResistance => backUpElements.Vitality,

                EnumStats.BaseStatType.InitiativeSpeed => backUpElements.Concentration,
                EnumStats.BaseStatType.InitialInitiative => backUpElements.Concentration,
                EnumStats.BaseStatType.ActionsPerSequence => backUpElements.Concentration,
                EnumStats.BaseStatType.Critical => backUpElements.Concentration,

                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}
