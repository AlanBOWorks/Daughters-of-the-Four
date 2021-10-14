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
    }
}
