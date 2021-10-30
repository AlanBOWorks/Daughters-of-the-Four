using System;
using CombatEntity;
using CombatSkills;
using CombatSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Stats
{
    public static class UtilsCombatStats
    {

        public static bool IsAlive(CombatStatsHolder stats) => stats.CurrentMortality > 0 || stats.CurrentHealth > 0;
        public static bool CanAct(CombatStatsHolder stats) => stats.CurrentActions > 0 && IsAlive(stats);
        public static bool IsTickingValid(CombatStatsHolder stats) => IsAlive(stats);


        // By design shield breaks and health lost stops the over damage
        public static void DoDamageTo(ICombatPercentStats<float> vitality, float damage)
        {
            if (damage <= 0)
            {
                return;
            }

            if (vitality.CurrentShields >= 1)
            {
                vitality.CurrentShields -= 1; // by design shields are lost in units
                if (vitality.CurrentShields <= 0)
                {
                    vitality.CurrentShields = 0;
                    CombatSystemSingleton.DamageReceiveEvents.OnShieldLost();
                }
            }

            if (vitality.CurrentHealth > 0)
            {
                vitality.CurrentHealth -= damage;
                if (!(vitality.CurrentHealth <= 0)) return;

                vitality.CurrentHealth = 0;
                CombatSystemSingleton.DamageReceiveEvents.OnHealthLost();
            }

            vitality.CurrentMortality -= damage;
            if (!(vitality.CurrentMortality < 0)) return;

            vitality.CurrentMortality = 0;
            CombatSystemSingleton.DamageReceiveEvents.OnMortalityDeath();
        }

        //Heals are done in percent
        public const float CriticalHealPercentageAddition = .25f;

        public static void ModifyByOverHeal(ref float currentHeal, float maxHealth)
        {
            currentHeal += maxHealth * CriticalHealPercentageAddition;
        }

        public static void DoOverHealTo(CombatStatsHolder stats, float healPercent)
        {
            float maxHealth = stats.MaxHealth;
            float healthMaxCap = maxHealth * (1 + CriticalHealPercentageAddition);
            float currentHealth = stats.CurrentHealth;
            if(healthMaxCap <= currentHealth) return; //previous overHeal could be higher than new overHeals


            float healAmount = maxHealth * healPercent;
            ModifyByOverHeal(ref healAmount, maxHealth);

            float targetHealth = currentHealth + healAmount;
            if (targetHealth > healthMaxCap) targetHealth = healthMaxCap;


            stats.CurrentHealth = targetHealth;
        }

        public static void DoHealTo(CombatStatsHolder stats, float healPercent)
        {
            float maxHealth = stats.MaxHealth;
            float healthMaxCap = maxHealth;
            float currentHealth = stats.CurrentHealth;
            float healAmount = maxHealth;


            healAmount *= healPercent;

            float targetHealth = currentHealth + healAmount;
            if (targetHealth > healthMaxCap) targetHealth = healthMaxCap;


            stats.CurrentHealth = targetHealth;
        }

        public static void DoShielding(CombatStatsHolder user, CombatStatsHolder target, float shieldingVariation)
        {
            var userShielding = user.Shielding;
            var targetShielding = target.Shielding;

            float shieldIncrement = userShielding * shieldingVariation;
            float finalShields = target.CurrentShields + shieldIncrement;

            if (finalShields > targetShielding) //By design the max shield amount is given by the target's Shielding stat 
                finalShields = targetShielding;

            if(finalShields > target.CurrentShields)
                target.CurrentShields = finalShields;
        }


        public static void VariateActions(CombatStatsHolder statsHolder, int addition)
        {
            statsHolder.CurrentActions += addition;
        }

        public static void OverrideActionsAmount(CombatStatsHolder statsHolder, int targetAmount)
        {
            statsHolder.CurrentActions = targetAmount;
        }

        public static void RefillActions(CombatStatsHolder statsHolder)
        {
            statsHolder.CurrentActions = Mathf.RoundToInt(statsHolder.ActionsPerSequence);
            Debug.Log($"REFILL actions: {statsHolder.CurrentActions}");
        }

        public static void DecreaseActions(CombatStatsHolder statsHolder, int amount = 1)
        {
            Debug.Log($"Decreasing actions: {statsHolder.CurrentActions} > {statsHolder.CurrentActions -amount}");
            statsHolder.CurrentActions-= amount;
        }

        public static void ResetActions(CombatStatsHolder statsHolder)
        {
            statsHolder.CurrentActions = 0;
        }

        private const float MaxInitiativeRefillRandom = .1f;
        public static void InitiativeResetOnTrigger(CombatStatsHolder statsHolder)
        {
            statsHolder.TickingInitiative = 0;
        }
    }


    public static class UtilsRandomStats
    {
        public static bool IsCritical(CombatStatsHolder stats, float percentageVariation = 0)
        {
            float criticalChance = stats.Critical;
            criticalChance += percentageVariation;

            if (criticalChance <= 0) return false; //exclude zeros or below (since it could be random: 0 <= critical: 0)
            return Random.value <= criticalChance; // could be random: 1 <= critical: 1
        }

        public static bool IsCritical(CombatStatsHolder stats, CombatingSkill skill)
            => IsCritical(stats, skill.GetCritVariation());
    }
    
}
