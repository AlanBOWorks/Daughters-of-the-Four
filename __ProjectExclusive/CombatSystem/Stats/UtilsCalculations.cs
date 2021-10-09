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

        public static void ResetBurstStats(CombatStatsHolder statsHolder)
        {
            statsHolder.ResetBurst();
        }

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
                return;
            }

            if (vitality.CurrentHealth > 0)
            {
                vitality.CurrentHealth -= damage;
                if (vitality.CurrentHealth < 0) vitality.CurrentHealth = 0;
                return;
            }

            vitality.CurrentMortality -= damage;
            if (vitality.CurrentMortality < 0) vitality.CurrentMortality = 0;
        }

        //Heals are done in percent
        public const float CriticalHealPercentageAddition = .25f;

        public static void ModifyByOverHeal(ref float currentHeal, float maxHealth)
        {
            currentHeal += maxHealth * CriticalHealPercentageAddition;
        }

        public static void DoHealTo(CombatStatsHolder stats, float healPercent, bool isCriticalHeal)
        {
            float maxHealth = stats.MaxHealth;
            float healthMaxCap = maxHealth;
            float currentHealth = stats.CurrentHealth;

            float healAmount = maxHealth * healPercent;

            if (isCriticalHeal)
            {
                ModifyByOverHeal(ref healAmount,maxHealth);
                healthMaxCap *= 1 + CriticalHealPercentageAddition;
            }

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



        public static void VariateActions(CombatStatsHolder statsHolder, int addition)
        {
            statsHolder.CurrentActions += addition;
        }
        public static void TickCurrentActions(CombatStatsHolder statsHolder) => VariateActions(statsHolder, -1);

        public static void OverrideActionsAmount(CombatStatsHolder statsHolder, int targetAmount)
        {
            statsHolder.CurrentActions = targetAmount;
        }

        public static void RefillActions(CombatStatsHolder statsHolder)
        {
            statsHolder.CurrentActions = Mathf.RoundToInt(statsHolder.ActionsPerSequence);
        }

        public static void DecreaseActions(CombatStatsHolder statsHolder, int amount = 1)
        {
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
        public static bool IsCritical(CombatStatsHolder stats, CombatingSkill skill)
        {
            float criticalChance = stats.Critical;
            float criticalVariation = skill.GetCritVariation();

            criticalChance += criticalVariation;

            if (criticalChance <= 0) return false; //exclude zeros or below (since it could be random: 0 <= critical: 0)
            return Random.value <= criticalChance; // could be random: 1 <= critical: 1
        }
    }
    
}
