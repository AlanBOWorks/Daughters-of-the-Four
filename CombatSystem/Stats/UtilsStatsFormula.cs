using UnityEngine;

namespace CombatSystem.Stats
{

    /**
     * Offensive = (Base + Buff) * (1 * Burst)
     * Support = (Base + Buff) * (1 * Burst)
     * Vitality = (Base + Buff) * (1 * Burst) [Mortality Exclude > Mortality = Base]
     *
     * Concentration = Base + Buff + Burst)
     */
    public static class UtilsStatsFormula
    {
        private static float CalculateStatAsMultiplicative(CombatStats stats, EnumStats.StatType type)
        {
            UtilsStats.GetElements(type, stats, out var baseStats, out var buffStats, out var burstStats);
            return (1 + buffStats) * (baseStats + burstStats);
        }
        private static float CalculateStatAsAdditive(CombatStats stats, EnumStats.StatType type)
        {
            UtilsStats.GetElements(type, stats, out var baseStats, out var buffStats, out var burstStats);
            return baseStats + buffStats + burstStats;
        }

        // ---- OFFENSIVE
        public static float CalculateAttackPower(CombatStats stats) 
            => CalculateStatAsMultiplicative(stats, EnumStats.StatType.Attack);
        public static float CalculateOverTimePower(CombatStats stats)
            => CalculateStatAsMultiplicative(stats, EnumStats.StatType.OverTime);
        public static float CalculateDeBuffPower(CombatStats stats)
            => CalculateStatAsMultiplicative(stats, EnumStats.StatType.DeBuff);
        public static float CalculateFollowUpPower(CombatStats stats)
            => CalculateStatAsMultiplicative(stats, EnumStats.StatType.FollowUp);


        // ---- SUPPORT
        public static float CalculateHealPower(CombatStats stats)
            => CalculateStatAsMultiplicative(stats, EnumStats.StatType.Heal);
        public static float CalculateShieldingPower(CombatStats stats)
            => CalculateStatAsMultiplicative(stats, EnumStats.StatType.Shielding);
        public static float CalculateBuffPower(CombatStats stats)
            => CalculateStatAsMultiplicative(stats, EnumStats.StatType.Buff);
        public static float CalculateReceiveBuffPower(CombatStats stats)
            => CalculateStatAsMultiplicative(stats, EnumStats.StatType.ReceiveBuff);


        // ---- Vitality
        public static float CalculateMaxHealth(CombatStats stats) 
            => CalculateStatAsMultiplicative(stats, EnumStats.StatType.Health);
        public static float CalculateMaxMortality(CombatStats stats) 
            => stats.BaseStats.MortalityType; // Mortality shouldn't be modified
        public static float CalculateDamageReduction(CombatStats stats) 
            => CalculateStatAsMultiplicative(stats, EnumStats.StatType.DamageReduction);
        public static float CalculateDeBuffResistance(CombatStats stats)
            => CalculateStatAsMultiplicative(stats, EnumStats.StatType.DebuffResistance);



        // ---- CONCENTRATION
        /*
         * Concentration stats are calculated more frequently, so calculations are handled more manually
         */

        private const float ZeroSpeedInitiativeAmount = .5f;
        public static float CalculateInitiativeSpeed(CombatStats stats)
        {
            float speedAmount = stats.BaseStats.SpeedType
                                + stats.BuffStats.SpeedType
                                + stats.BurstStats.SpeedType;

            // by design, 0 speed will be set by an skill > forcing the enemy reaching this stats.
            // the problem: the entity never will reach 100% initiative, so this should be fixed with small increments
            // NOTE: inmovil entities are the ones with speedAmount < 0
            if (speedAmount == 0) return ZeroSpeedInitiativeAmount;

            return Mathf.Round(speedAmount * 10) * .1f;
        }

        
        public static float CalculateLuckAmount(CombatStats stats)
        {
            return stats.BaseStats.CriticalType
                   + stats.BuffStats.CriticalType
                   + stats.BurstType.CriticalType;
        }

        public static float CalculateControlGain(CombatStats stats)
        {
            return stats.BaseStats.ControlType 
                   + stats.BuffStats.ControlType
                   + stats.BurstStats.ControlType;
        }
    }

}
