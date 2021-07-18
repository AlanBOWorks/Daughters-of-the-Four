using System;
using System.Collections.Generic;
using Characters;
using _CombatSystem;
using UnityEngine;

namespace Skills
{
    public static class UtilsSkill
    {
        public static List<CombatSkill> GetSkillsByStance(CombatingEntity entity)
        {
            var state = entity.AreasDataTracker.GetCurrentPositionState();
            var skills = entity.UniqueSkills;
            if (skills == null) return null;
            switch (state)
            {
                case TeamCombatData.Stance.Attacking:
                    return skills.AttackingSkills;
                case TeamCombatData.Stance.Defending:
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
