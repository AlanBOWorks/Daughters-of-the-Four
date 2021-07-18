﻿using System;
using _CombatSystem;
using Skills;
using UnityEngine;

namespace Characters
{
    public static class UtilsCharacter
    {
        public const int PredictedAmountOfCharactersInBattle = CharacterArchetypes.AmountOfArchetypes * 2;

        public static bool IsAPlayerEntity(CombatingEntity entity)
        {
            CombatingTeam playerCharacters = CombatSystemSingleton.Characters.PlayerFaction;
            return playerCharacters.Contains(entity);
        }

    }


    public static class UtilsStats
    {
        public static CharacterCombatStatsBasic ZeroValuesBasic = new CharacterCombatStatsBasic(0);
        public static CharacterCombatStatsFull ZeroValuesFull = new CharacterCombatStatsFull(0);

        public static float StatsFormula(float baseStat, float buffStat, float burstStat)
        {
            return (baseStat + buffStat) * (1 + burstStat);
        }

        public static float GrowFormula(float baseStat, float growStat, float upgradeAmount)
        {
            return baseStat + growStat * upgradeAmount;
        }


        public static void CopyStats(ICharacterBasicStats injection, ICharacterBasicStats copyFrom)
        {
            injection.AttackPower = copyFrom.AttackPower;
            injection.DeBuffPower = copyFrom.DeBuffPower;
            injection.HealPower = copyFrom.HealPower;

            injection.BuffPower = copyFrom.BuffPower;
            injection.MaxHealth = copyFrom.MaxHealth;
            injection.MaxMortalityPoints = copyFrom.MaxMortalityPoints;
            injection.DamageReduction = copyFrom.DamageReduction;

            injection.Enlightenment = copyFrom.Enlightenment;
            injection.CriticalChance = copyFrom.CriticalChance;
            injection.SpeedAmount = copyFrom.SpeedAmount;
        }

        public static void CopyStats(ICharacterFullStats injection, ICharacterFullStats copyFrom)
        {
            CopyStats(injection as ICharacterBasicStats, copyFrom);
            injection.HealthPoints = copyFrom.HealthPoints;
            injection.ShieldAmount = copyFrom.ShieldAmount;

            injection.MortalityPoints = copyFrom.MortalityPoints;
            injection.HarmonyAmount = copyFrom.HarmonyAmount;
            injection.InitiativePercentage = copyFrom.InitiativePercentage;
            injection.ActionsPerInitiative = copyFrom.ActionsPerInitiative;
        }
    }

    public static class UtilsCombatStats
    {
        public const int PredictedAmountOfSkillsPerState = 4 + 2 + 1; // 4 Unique + 2 common + 1 Ultimate
        public const int PredictedTotalOfSkills = PredictedAmountOfSkillsPerState * 3; // *3 types of states

        public static void DoDamageTo(ICharacterFullStats stats, float damage, bool canDamageMortality = false)
        {
            damage = CalculateNormalizationDamage();

            stats.HealthPoints = CalculateHealth(stats.HealthPoints);
            if(canDamageMortality)
                stats.MortalityPoints = CalculateHealth(stats.MortalityPoints);


            float CalculateNormalizationDamage()
            {
                float normalizedReduction = 1 - (stats.DamageReduction);
                if (normalizedReduction < 0) normalizedReduction = 0;
                return damage * normalizedReduction;
            }
            float CalculateHealth(float health)
            {
                health -= damage;
                if (health >= 0) damage = 0;
                else
                {
                    damage = -health;
                    health = 0;
                }

                return health;
            }
        }

        public static void DoHealTo(ICharacterFullStats stats, float heal)
        {
            if (heal < 0) heal = 0;

            stats.HealthPoints += heal;
            if (stats.HealthPoints > stats.MaxHealth)
                stats.HealthPoints = stats.MaxHealth;
        }

        public static void SetInitiative(ICombatTemporalStats stats, float targetValue = 0)
        {
            const float lowerCap = 0;
            const float maxCap = GlobalCombatParams.InitiativeCheck;

            targetValue = Mathf.Clamp(targetValue, lowerCap,maxCap);
            stats.InitiativePercentage = targetValue;
        }

        public static void AddInitiative(ICombatTemporalStats stats, float addition)
        {
            SetInitiative(stats,stats.InitiativePercentage + addition);
        }

        public static void SetActionAmount(CharacterCombatData stats, int targetValue = 0)
        {
            const int lowerCap = GlobalCombatParams.ActionsLowerCap;
            const int maxCap = GlobalCombatParams.ActionsPerInitiativeCap;

            targetValue = Mathf.Clamp(targetValue, lowerCap, maxCap);
            stats.ActionsLefts = targetValue;
        }

        public static void AddActionAmount(CharacterCombatData stats, int addition = 1)
        {
            SetActionAmount(stats,stats.ActionsLefts + addition);
        }

        public static bool IsCriticalPerformance(ICharacterBasicStats stats, CombatSkill skill, float criticalCheck)
        {
            if (!skill.CanCrit) return false;
            return criticalCheck < stats.CriticalChance + skill.CriticalAddition;
        }

        public const float RandomLow = .8f;
        public const float RandomHigh = 1.2f;

        public static float CalculateRandomModifier(float randomCheck)
        {
            return Mathf.Lerp(RandomLow, RandomHigh, randomCheck);
        }
    }

    public static class UtilsArea
    {

        public static TeamCombatData.Stance ParseStance(float stanceEquivalent)
        {
            TeamCombatData.Stance stance;
            if (stanceEquivalent == 0) stance = TeamCombatData.Stance.Neutral;
            else if (stanceEquivalent > 0) stance = TeamCombatData.Stance.Attacking;
            else stance = TeamCombatData.Stance.Defending;

            return stance;
        }

        public static void ToggleStance(CombatingEntity entity, TeamCombatData.Stance targetStance)
        {
            var areaData = entity.AreasDataTracker;
            if (areaData.IsForceStance)
            {
                areaData.ForceStateFinish();
            }
            else
            {
                areaData.ForceState(targetStance);
            }
        }
    }
}
