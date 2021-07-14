using System;
using System.Collections.Generic;
using Characters;
using _CombatSystem;
using UnityEngine;

namespace Skills
{
    public static class UtilsSkill
    {

        public const int PredictedAmountOfSkillsPerState = 4 + 2 + 1; // 4 Unique + 2 common + 1 Ultimate
        public const int PredictedTotalOfSkills = PredictedAmountOfSkillsPerState * 3; // *3 types of states
        public static void DoDamage(float damage, ICharacterFullStats stats)
        {
            float normalizedReduction = 1 - (stats.DamageReduction);
            if (normalizedReduction < 0) normalizedReduction = 0;
            damage *= normalizedReduction;
            stats.HealthPoints = CalculateHealth(stats.HealthPoints);
            stats.MortalityPoints = CalculateHealth(stats.MortalityPoints);

            float CalculateHealth(float health)
            {
                health -= damage;
                if (health >= 0)
                    damage = 0;
                else
                {
                    damage = -health;
                    health = 0;
                }

                return health;
            }
        }

        public static void DoHeal(float heal, ICharacterFullStats stats)
        {
            stats.HealthPoints += heal;
            if (stats.HealthPoints > stats.MaxHealth)
                stats.HealthPoints = stats.MaxHealth;
        }

        public static void SetInitiative(float targetValue, ICombatTemporalStats stats)
        {
            targetValue = Mathf.Clamp(targetValue, 0, 100);
            stats.InitiativePercentage = targetValue;
        }

        public static List<CombatSkill> GetSkillsByTeamState(CombatingEntity entity)
        {
            var state = entity.CharacterGroup.Team.Data.State;
            var skills = entity.UniqueSkills;
            if (skills == null) return null;
            switch (state)
            {
                case TeamCombatData.States.Attacking:
                    return skills.AttackingSkills;
                case TeamCombatData.States.Defending:
                    return skills.DefendingSkills;
                default:
                    return skills.NeutralSkills;
            }
        }

        public static SEffectBase.EffectType GetType(CombatSkill skill)
        {
            return skill.Preset.MainEffectType;
        }

        public static void DoParse<T>(ISkillPositions<T> skills, Action<T> action)
        {
            action(skills.AttackingSkills);
            action(skills.NeutralSkills);
            action(skills.DefendingSkills);
        }

        public static void DoParse<T>(ISkillShared<T> skills, Action<T> action)
        {
            action(skills.UltimateSkill);
            action(skills.CommonSkillFirst);
            action(skills.CommonSkillSecondary);
        }
    }
}
