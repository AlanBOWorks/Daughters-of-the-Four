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
            bool blockedByShields = DoDamageToShields(vitality);
            if(blockedByShields) return;

            bool blockedByHealth = DoDamageToHealth(vitality,damage);
            if(blockedByHealth) return;

            DoDamageToMortality(vitality, damage);
        }

        public static void AddPersistentDamage(CombatStats<float, int> stats, float damagePerSequence)
        {
            DoDamageToShields(stats);
            // By design PersistentDamage pass through shields while damaging it
            stats.CurrentPersistentDamage += damagePerSequence;
        }

        public static void DoCurrentPersistentDamage(CombatStats<float, int> stats)
        {
            float currentAccumulatedDamage = stats.CurrentPersistentDamage;
            if(currentAccumulatedDamage <= 0) return;

            DoDamageTo(stats,currentAccumulatedDamage);
        }

        /// <returns>If damages was blocked</returns>
        public static bool DoDamageToShields(ICombatPercentStats<float> vitality)
        {
            // Shields can only block if there's at least 1 shield; 0.5f will be not considered enough shields
            if(vitality.CurrentShields < 1) return false; //See the DoDamageToHealth's comment about the return's reason being there

            vitality.CurrentShields -= 1; // by design shields are lost in units
            if (vitality.CurrentShields > 0) return true;

            vitality.CurrentShields = 0;
            CombatSystemSingleton.DamageReceiveEvents.OnShieldLost();
            return true;
        }

        /// <returns>If damage was blocked</returns>
        public static bool DoDamageToHealth(ICombatPercentStats<float> vitality, float damage)
        {
            /*  Returns was added because by design damage only happens in block of stats (to simply
                chucks of damage information that the player needs to process).
                If there's enough shields, then shields it's the only thing the player will take on account;
                If there's not enough shields but health, then health it's the only thing the player will take on account;
                If there's nothing protecting but Mortality, then only mortality will be taken part on the calculations
                
            */
            if (vitality.CurrentHealth <= 0) return false;

            vitality.CurrentHealth -= damage;
            if(vitality.CurrentHealth > 0) return true;

            vitality.CurrentHealth = 0;
            CombatSystemSingleton.DamageReceiveEvents.OnHealthLost();
            return true;
        }
        // Doesn't have return (bool) because it's the last stats in the damage calculations and the returns (bool) were
        // for avoid the next damage calculation
        public static void DoDamageToMortality(ICombatPercentStats<float> vitality, float damage)
        {
            vitality.CurrentMortality -= damage;
            if(vitality.CurrentMortality > 0) return;

            vitality.CurrentMortality = 0;
            CombatSystemSingleton.DamageReceiveEvents.OnMortalityLost();
        }



        //Heals are done in percent
        public const float CriticalHealPercentageAddition = .25f;

        public static void ModifyByOverHeal(ref float currentHeal, float maxHealth)
        {
            currentHeal += maxHealth * CriticalHealPercentageAddition;
        }

        public static void DoOverHealTo(CombatStats<float, int> stats, float healPercent)
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

        public static void DoHealTo(CombatStats<float, int> stats, float healPercent)
        {
            float maxHealth = stats.MaxHealth;
            float healthMaxCap = maxHealth;
            float currentHealth = stats.CurrentHealth;
            float healAmount = maxHealth;


            healAmount *= healPercent;

            float targetHealth = currentHealth + healAmount;
            if (targetHealth > healthMaxCap) targetHealth = healthMaxCap;


            stats.CurrentHealth = targetHealth;
            stats.CurrentPersistentDamage = 0; //By design heals remove all persistentDamage
        }

        public static void DoShielding(CombatStats<float, int> user, CombatStats<float, int> target, float shieldingVariation)
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


        public static void VariateActions(CombatStats<float, int> statsHolder, int addition)
        {
            statsHolder.CurrentActions += addition;
        }

        public static void OverrideActionsAmount(CombatStats<float, int> statsHolder, int targetAmount)
        {
            statsHolder.CurrentActions = targetAmount;
        }

        public static void RefillActions(CombatStats<float, int> statsHolder)
        {
            statsHolder.CurrentActions = Mathf.RoundToInt(statsHolder.ActionsPerSequence);
        }

        public static void DecreaseActions(CombatStats<float, int> statsHolder, int amount = 1)
        {
            statsHolder.CurrentActions-= amount;
        }

        public static void ResetActions(CombatStats<float, int> statsHolder)
        {
            statsHolder.CurrentActions = 0;
        }

        private const float MaxInitiativeRefillRandom = .1f;
        public static void InitiativeResetOnTrigger(CombatStats<float, int> statsHolder)
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
