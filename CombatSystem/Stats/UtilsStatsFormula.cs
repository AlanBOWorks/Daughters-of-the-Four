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

            modifyValue = (modifyValue + baseStats + buffStats) * (1 + burstStats);
        }
        public static void CalculateValue(in CombatStats stats, in EnumStats.SupportStatType type,
            ref float modifyValue)
        {
            ExtractStats(in stats, out float baseStats, out float buffStats, out float burstStats, in type);

            modifyValue = (modifyValue + baseStats + buffStats) * (1 + burstStats);
        }
        public static void CalculateValue(in CombatStats stats, in EnumStats.VitalityStatType type,
            ref float modifyValue)
        {
            ExtractStats(in stats, out float baseStats, out float buffStats, out float burstStats, in type);

            modifyValue = (modifyValue + baseStats + buffStats + burstStats);
        }


        public static float CalculateInitiativeSpeed(in CombatStats stats)
        {
            float speedAmount = stats.BaseStats.SpeedType
                                + stats.BuffStats.SpeedType
                                + stats.BurstStats.SpeedType;
            speedAmount *= stats.ConcentrationType;

            return Mathf.Round(speedAmount);
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

    public static class UtilsStatsEffects
    {

        public static float CalculateOffensiveStatBuffed(in float buffPower, in float receivePower,
            in float effectValue)
        {
            return effectValue * (buffPower + receivePower);
        }
        public static float CalculateSupportStatBuffed(in float buffPower, in float receivePower,
            in float effectValue)
        {
            return effectValue * (buffPower + receivePower);
        }

        public static void CalculateDamageFromAttackAttribute(in CombatStats stats, ref float baseDamage)
        {
            UtilsStatsFormula.CalculateValue(in stats, EnumStats.OffensiveStatType.Attack, ref baseDamage);
            baseDamage *= stats.OffensiveType;

            if (baseDamage < 0) baseDamage = 0;
        }

        public static void CalculateDamageReduction(in CombatStats stats, ref float currentDamage)
        {
            UtilsStatsFormula.ExtractStats(in stats, 
                out float baseStats, out float buffStats, out float burstStats, 
                EnumStats.VitalityStatType.DamageReduction);

            float reduction = baseStats + buffStats + burstStats;
            if(reduction <= 0) return;

            currentDamage *= (1 - reduction);
            if (currentDamage < 0) currentDamage = 0;
        }

        public static void CalculateHealAmount(in CombatStats performerStats, ref float baseHeal)
        {
            UtilsStatsFormula.CalculateValue(in performerStats, EnumStats.SupportStatType.Heal, ref baseHeal);
            baseHeal *= performerStats.SupportType;

            if (baseHeal < 0) baseHeal = 0;
        }

        public static void CalculateReceiveHealAmount(in CombatStats receiverStats, ref float currentHeal)
        {
            CalculateHealAmount(in receiverStats, ref currentHeal);
        }

        public static void CalculateShieldsAmount(in CombatStats performerStats, ref float desiredShields)
        {
            UtilsStatsFormula.ExtractStats(in performerStats,
                out float baseStats, out float buffStats, out float burstStats,
               EnumStats.SupportStatType.Shielding);
            float statsModifier = performerStats.SupportType * (baseStats + buffStats + burstStats);

            desiredShields *= statsModifier;
        }
        private const float VanillaMaxShieldAmount = 2;

        public static void ClampShieldsAmount(in CombatStats targetStats, 
            ref float addingShields)
        {
            UtilsStatsFormula.ExtractStats(in targetStats,
                out float baseStats, out float buffStats, out float burstStats,
                EnumStats.SupportStatType.Shielding);
            float statsModifier = targetStats.SupportType * (baseStats + buffStats + burstStats);
            float maxShields = VanillaMaxShieldAmount * statsModifier;

            float currentShields = targetStats.CurrentShields;
            float desiredShields = currentShields + addingShields;

            if (desiredShields > maxShields) desiredShields = maxShields;

            addingShields = desiredShields - currentShields;
        }

        public static float CalculateBuffPower(in CombatStats performerStats)
        {
            return performerStats.SupportType * (
                performerStats.BaseStats.BuffType +
                performerStats.BuffStats.BuffType)
                * (performerStats.BurstType.BuffType);
        }

        public static float CalculateBuffReceivePower(in CombatStats targetStats)
        {
            return targetStats.SupportType * (
                   targetStats.BaseStats.ReceiveBuffType +
                   targetStats.BuffStats.ReceiveBuffType) 
                    * (targetStats.BurstType.ReceiveBuffType);
        }
    }
}
