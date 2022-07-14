using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CombatSystem.Stats
{
    public static class UtilsStatsEffects
    {
        public static float CalculateFinalDamage(float effectDamage, float performerAttackUnit, float targetDamageReductionUnit)
        {
            // Attack Power is normally 1 or higher
            // Damage reduction is normally 0
            if (targetDamageReductionUnit < 0) targetDamageReductionUnit = 0;

            float damageModifier = performerAttackUnit - targetDamageReductionUnit;
            float finalDamage = effectDamage * damageModifier;
            if (finalDamage > 0) return finalDamage;
            return 0;
        }

        public static void CalculateHealAmount(CombatStats performerStats, ref float effectHeal)
        {
            effectHeal *= UtilsStatsFormula.CalculateHealPower(performerStats);
            if (effectHeal < 0) effectHeal = 0;
        }

        public const float VanillaMaxShieldAmount = 4;
        public static void CalculateShieldsAmount(CombatStats performerStats, ref float effectAddingShields)
        {
            var statsModifier = UtilsStatsFormula.CalculateShieldingPower(performerStats);
            effectAddingShields *= statsModifier;
        }

        public static float CalculateStatsDeBuffValue(float effectValue, float debuffPower, float debuffResistance)
        {
            // DeBuff Power is normally 1 or higher
            // DeBuff Resistance is normally 0
            float effectModifier = debuffPower - debuffResistance;
            float finalDebuffValue = effectValue * effectModifier;
            if (finalDebuffValue > 0) return finalDebuffValue;
            return 0;
        }

        public static float CalculateStatsBuffValue(float effectValue, float bufferPower, float receivePower)
        {
            // Both buffPower and receivePower are normally 1
            float effectModifier = bufferPower * receivePower;
            float finalBuffValue = effectValue * effectModifier;
            if (finalBuffValue > 0) return finalBuffValue;
            return 0;
        }
    }
}