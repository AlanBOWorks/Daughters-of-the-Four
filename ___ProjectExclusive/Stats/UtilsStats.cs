using System;
using _CombatSystem;
using Characters;
using Skills;
using SMaths;
using UnityEngine;

namespace Stats 
{
    public static class EnumStats
    {
        public const int AttackIndex = 0;
        public const int DeBuffIndex = AttackIndex + 1;
        public enum Offensive
        {
            Attack = AttackIndex,
            DeBuff = DeBuffIndex
        }

        public const int HealIndex = 10;
        public const int BuffIndex = HealIndex + 1; 
        public enum Support
        {
            Heal = HealIndex,
            Buff = BuffIndex
        }

        public const int MaxHealthIndex = 100;
        public const int MaxMortalityIndex = MaxHealthIndex + 1;
        public const int DamageReductionIndex = MaxMortalityIndex + 1;
        public const int DeBuffReductionIndex = DamageReductionIndex + 1;
        public enum Vitality
        {
            MaxHealth = MaxHealthIndex,
            MaxMortality = MaxMortalityIndex,
            DamageReduction = DamageReductionIndex,
            DeDuffReduction = DeBuffReductionIndex
        }

        public const int EnlightenmentIndex = 1000;
        public const int CriticalIndex = EnlightenmentIndex + 1;
        public const int SpeedIndex = CriticalIndex + 1;
        public enum Special
        {
            Enlightenment = EnlightenmentIndex,
            Critical = CriticalIndex,
            Speed = SpeedIndex
        }

        public const int HealthIndex = 10000;
        public const int ShieldIndex = HealthIndex + 1;
        public const int MortalityIndex = ShieldIndex + 1;
        public const int HarmonyIndex = MortalityIndex + 1;
        public const int InitiativeIndex = HarmonyIndex + 1;
        public const int ActionsIndex = InitiativeIndex + 1;
        public enum Combat
        {
            Health = HealthIndex,
            Shield = ShieldIndex,
            Mortality = MortalityIndex,
            Harmony = HarmonyIndex,
            Initiative = InitiativeIndex,
            Actions = ActionsIndex
        }
    }

    public static class UtilsStats
    {
        public static CharacterCombatStatsBasic ZeroValuesBasic = new CharacterCombatStatsBasic(0);
        public static CharacterCombatStatsFull ZeroValuesFull = new CharacterCombatStatsFull(0);

        public static float StatsFormula(float baseStat, float buffStat, float burstStat)
        {
            return baseStat + baseStat * (buffStat + burstStat);
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

        public static void CopyStats(IStatsUpgradable injection, IStatsUpgradable copyFrom)
        {
            injection.VitalityAmount = copyFrom.VitalityAmount;
            injection.Enlightenment = copyFrom.Enlightenment;
            injection.OffensivePower = copyFrom.OffensivePower;
            injection.SupportPower = copyFrom.SupportPower;
        }

        public static CharacterCombatData GenerateCombatData(ICharacterFullStats stats)
        {
            return new CharacterCombatData(stats);
        }
        public static CharacterCombatData GenerateCombatData(IPlayerCharacterStats playerStats)
        {
            var copyStats = new PlayerCharacterCombatStats(playerStats);
            return new CharacterCombatData(copyStats);
        }
    }

    public static class UtilsCombatStats
    {
        public const int PredictedAmountOfSkillsPerState = 4 + 2 + 1; // 4 Unique + 2 common + 1 Ultimate
        public const int PredictedTotalOfSkills = PredictedAmountOfSkillsPerState * 3; // *3 types of states

        public static float HealthPercentage(CombatingEntity entity)
        {
            var stats = entity.CombatStats;
            return SRange.Percentage(stats.HealthPoints, 0, stats.MaxHealth);
        }

        public static float CalculateFinalDamage(
            CharacterCombatData attacker,
            ICharacterFullStats receiver, float damageModifier)
        {
            float baseDamage = attacker.BaseStats.AttackPower;
            float damageVariation = attacker.BuffStats.AttackPower
                                    + attacker.BurstStats.AttackPower
                                    - receiver.DamageReduction;
            baseDamage += baseDamage * damageVariation;

            float total = baseDamage * damageModifier;
            if (total < 0) total = 0;

            return total;
        }

        /// <returns>The damage left</returns>
        public static float DoDamageTo(ICharacterFullStats stats, float damage, bool canDamageMortality = false)
        {
            stats.ShieldAmount = CalculateDamageOnVitality(stats.ShieldAmount);
            stats.HealthPoints = CalculateDamageOnVitality(stats.HealthPoints);
            if (canDamageMortality)
                stats.MortalityPoints = CalculateDamageOnVitality(stats.MortalityPoints);

            return damage;

            float CalculateDamageOnVitality(float vitalityStat)
            {
                vitalityStat -= damage;
                if (vitalityStat >= 0) damage = 0;
                else
                {
                    damage = -vitalityStat;
                    vitalityStat = 0;
                }

                return vitalityStat;
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

            targetValue = Mathf.Clamp(targetValue, lowerCap, maxCap);
            stats.InitiativePercentage = targetValue;
        }

        public static void AddInitiative(ICombatTemporalStats stats, float addition)
        {
            SetInitiative(stats, stats.InitiativePercentage + addition);
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
            SetActionAmount(stats, stats.ActionsLefts + addition);
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

    public static class UtilityBuffStats
    {
        public const string BuffTooltip = "Buff";
        public const string BurstTooltip = "Burst";
        public const string DeBuffTooltip = "DeBuff";
        public const string DeBurstToolTip = "DeBurst";

        public static string GetBuffTooltip(bool isBuff)
        {
            return (isBuff) ? BurstTooltip : BuffTooltip;
        }

        public static string GetDeBuffTooltip(bool isBuff)
        {
            return (isBuff) ? DeBuffTooltip : DeBurstToolTip;
        }

        public static string GetTooltip(bool isBuff, bool isPositive)
        {
            return (isPositive) ? GetBuffTooltip(isBuff) : GetDeBuffTooltip(isBuff);
        }
    }
}