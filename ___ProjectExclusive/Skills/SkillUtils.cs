﻿using System;
using System.Collections.Generic;
using Characters;
using _CombatSystem;
using UnityEngine;

namespace Skills
{
    public static class UtilsSkill
    {
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
            var skills = entity.Skills;
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
    }
}
