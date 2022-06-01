using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CombatSystem.Stats
{
    public static class UtilsStatsEffects
    {

        public static float CalculateOffensiveStatBuffValue(in float buffPower, in float receivePower,
            in float effectValue)
        {
            return effectValue * (buffPower + receivePower);
        }

        public static float CalculateSupportStatBuffValue(in float buffPower, in float receivePower,
            in float effectValue)
        {
            return effectValue * (buffPower + receivePower);
        }

        public static float CalculateVitalityStatBuffValue(in float buffPower, in float receivePower,
            in float effectValue)
        {
            return effectValue + (buffPower + receivePower);
        }

        public static float CalculateConcentrationStatBuffValue(in float buffPower, in float receivePower,
            in float effectValue)
        {
            return effectValue + (buffPower + receivePower);
        }

        /// <summary>
        /// It calculates the final effect value for Debuffing
        /// </summary>
        public static float CalculateStatsDeBuffValue(in float debuff, in float resistance,
            in float effectValue)
        {
            float debuffDifference = debuff - resistance;
            if (debuffDifference <= 0) return 0;

            return effectValue * (debuffDifference);
        }

        public static void CalculateDamageFromAttackAttribute(in CombatStats stats, ref float baseDamage)
        {
            UtilsStatsFormula.CalculateValue(in stats, EnumStats.OffensiveStatType.Attack, ref baseDamage);

            if (baseDamage < 0) baseDamage = 0;
        }

        public static void CalculateDamageReduction(in CombatStats stats, ref float currentDamage)
        {
            UtilsStatsFormula.ExtractStats(in stats,
                out float baseStats, out float buffStats, out float burstStats,
                EnumStats.VitalityStatType.DamageReduction);

            float reduction = baseStats + buffStats + burstStats;
            if (reduction <= 0) return;

            currentDamage *= (1 - reduction);
            if (currentDamage < 0) currentDamage = 0;
        }

        public static void CalculateHealAmount(in CombatStats performerStats, ref float baseHeal)
        {
            UtilsStatsFormula.CalculateValue(in performerStats, EnumStats.SupportStatType.Heal, ref baseHeal);

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
            float statsModifier = (baseStats + buffStats + burstStats);

            desiredShields *= statsModifier;
        }

        private const float VanillaMaxShieldAmount = 2;

        public static void ClampShieldsAmount(in CombatStats targetStats,
            ref float addingShields)
        {
            float statsModifier = UtilsStatsFormula.CalculateStatsSum(in targetStats, EnumStats.SupportStatType.Shielding);
            float maxShields = VanillaMaxShieldAmount * statsModifier;

            float currentShields = targetStats.CurrentShields;
            float desiredShields = currentShields + addingShields;

            if (desiredShields > maxShields) desiredShields = maxShields;

            addingShields = desiredShields - currentShields;
        }

        public static float CalculateBuffPower(in CombatStats performerStats) =>
            (performerStats.BaseStats.BuffType +
             performerStats.BuffStats.BuffType)
            * (performerStats.BurstType.BuffType);

        public static float CalculateBuffReceivePower(in CombatStats targetStats) =>
            (targetStats.BaseStats.ReceiveBuffType +
             targetStats.BuffStats.ReceiveBuffType)
            * (targetStats.BurstType.ReceiveBuffType);

        public static float CalculateDeBuffPower(in CombatStats performerStats) =>
            performerStats.BaseStats.DeBuffType +
            performerStats.BuffStats.DeBuffType +
            performerStats.BurstStats.DeBuffType;

        public static float CalculateDeBuffResistance(in CombatStats targetStats) =>
            targetStats.BaseStats.DeBuffResistanceType +
            targetStats.BuffStats.DeBuffResistanceType +
            targetStats.BurstStats.DeBuffResistanceType;
    }
}