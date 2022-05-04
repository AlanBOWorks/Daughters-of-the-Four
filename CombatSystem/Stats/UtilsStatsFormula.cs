using UnityEngine;

namespace CombatSystem.Stats
{
    public static class UtilsStatsFormula
    {
        private static void ExtractStats(in CombatStats stats, out StatsValues values, in EnumStats.StatType type)
        {
            ExtractStats(in stats, out float baseStats, out float buffStats, out float burstStats, in type);
            values = new StatsValues(in baseStats,in buffStats,in burstStats);
        }

        public static void ExtractStats(in CombatStats stats,
            out float baseStats, out float buffStats, out float burstStats,
            in EnumStats.StatType type)
        {
            baseStats = UtilsStats.GetElement(type, stats.BaseStats);
            buffStats = UtilsStats.GetElement(type, stats.BuffStats);
            burstStats = UtilsStats.GetElement(type, stats.BurstStats);
        }
        public static void ExtractStats(in CombatStats stats,
            out float baseStats, out float buffStats, out float burstStats,
            in EnumStats.OffensiveStatType type)
        {
            baseStats = UtilsStats.GetElement(type, stats.BaseStats);
            buffStats = UtilsStats.GetElement(type, stats.BuffStats);
            burstStats = UtilsStats.GetElement(type, stats.BurstStats);
        }
        public static void ExtractStats(in CombatStats stats,
            out float baseStats, out float buffStats, out float burstStats,
            in EnumStats.SupportStatType type)
        {
            baseStats = UtilsStats.GetElement(type, stats.BaseStats);
            buffStats = UtilsStats.GetElement(type, stats.BuffStats);
            burstStats = UtilsStats.GetElement(type, stats.BurstStats);
        }
        public static void ExtractStats(in CombatStats stats,
            out float baseStats, out float buffStats, out float burstStats,
            in EnumStats.VitalityStatType type)
        {
            baseStats = UtilsStats.GetElement(type, stats.BaseStats);
            buffStats = UtilsStats.GetElement(type, stats.BuffStats);
            burstStats = UtilsStats.GetElement(type, stats.BurstStats);
        }


        private static float SumOfValues(in StatsValues values)
        {
            return values.BaseType + values.BuffType + values.BurstType;
        }

        public static void CalculateValue(in CombatStats stats, in EnumStats.OffensiveStatType type,
            ref float modifyValue)
        {
            ExtractStats(in stats, out float baseStats, out float buffStats, out float burstStats, in type);

            modifyValue *= 1 + baseStats + buffStats + burstStats;
        }
        public static void CalculateValue(in CombatStats stats, in EnumStats.SupportStatType type,
            ref float modifyValue)
        {
            ExtractStats(in stats, out float baseStats, out float buffStats, out float burstStats, in type);

            modifyValue *=  1+ baseStats + buffStats + burstStats;
        }
        public static void CalculateValue(in CombatStats stats, in EnumStats.VitalityStatType type,
            ref float modifyValue)
        {
            ExtractStats(in stats, out float baseStats, out float buffStats, out float burstStats, in type);

            modifyValue = (modifyValue + baseStats + buffStats + burstStats);
        }

        private const float ZeroSpeedInitiativeAmount = .5f;
        public static float CalculateInitiativeSpeed(in CombatStats stats)
        {
            float speedAmount = stats.BaseStats.SpeedType
                                + stats.BuffStats.SpeedType
                                + stats.BurstStats.SpeedType;
            speedAmount *= stats.ConcentrationType;

            // by design, 0 speed will be set by an skill > forcing the enemy reaching this stats.
            // the problem: the entity never will reach 100% initiative, so this should be fixed with small increments
            // NOTE: inmovil entities are the ones with speedAmount < 0
            if (speedAmount == 0) return ZeroSpeedInitiativeAmount; 

            return Mathf.Round(speedAmount);
        }

        public static float CalculateLuckAmount(in CombatStats stats)
        {
            float luckAmount =
                stats.BaseStats.CriticalType
                + stats.BuffStats.CriticalType
                + stats.BurstType.CriticalType;

            luckAmount *= stats.ConcentrationType;

            return luckAmount;
        }


        public static float CalculateMaxHealth(CombatStats stats)
        {
            ExtractStats(in stats, out var values, EnumStats.StatType.Health);

            float maxHealth = SumOfValues(values);
            maxHealth *= stats.VitalityType;
            return maxHealth;
        }
        public static float CalculateMaxMortality(CombatStats stats)
        {
            ExtractStats(in stats, out var values, EnumStats.StatType.Mortality);

            float maxMortality = SumOfValues(in values);
            maxMortality *= stats.VitalityType;
            return maxMortality;
        }

        private const float MaxActionsAmount = 12f;
        public static float CalculateActionsAmount(CombatStats stats)
        {
            ExtractStats(in stats, out var values, EnumStats.StatType.Actions);

            float actionsAmount = SumOfValues(values);

            if (actionsAmount > MaxActionsAmount)
                actionsAmount = MaxActionsAmount;
            else if (actionsAmount < 0)
                actionsAmount = 0;
            else
            {
                actionsAmount = Mathf.Round(actionsAmount);
            }

            return actionsAmount;
        }

        private struct StatsValues : IStatsTypesRead<float>
        {
            public StatsValues(in float baseType,in float buffType,in float burstType)
            {
                BaseType = baseType;
                BuffType = buffType;
                BurstType = burstType;
            }

            public float BaseType { get; }
            public float BuffType { get; }
            public float BurstType { get; }
        }
    }

}
