using System;
using System.Collections.Generic;
using Characters;
using _CombatSystem;
using UnityEngine;

namespace Skills
{
    public static class UtilsSkill
    {
        public static List<CombatSkill> GetUniqueByStance(CombatingEntity entity)
        {
            var state = entity.AreasDataTracker.GetCurrentPositionState();
            var skills = entity.UniqueSkills;

            return skills == null 
                ? null 
                : TeamCombatData.GetStance(skills, state);
        }
        public static List<CombatSkill> GetSkillsByStance(CombatingEntity entity)
        {
            var state = entity.AreasDataTracker.GetCurrentPositionState();
            var skills = entity.CombatSkills;

            return skills == null
                ? null
                : TeamCombatData.GetStance((IStanceArchetype<List<CombatSkill>>)skills, state);
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
            action(skills.WaitSkill);
        }


        public static void DoParse<T,TParse>(ISkillShared<T> skills, ISkillShared<TParse> parsing,
            Action<T, TParse> action)
        {
            action(skills.UltimateSkill, parsing.UltimateSkill);
            action(skills.CommonSkillFirst, parsing.CommonSkillFirst);
            action(skills.CommonSkillSecondary, parsing.CommonSkillSecondary);
            action(skills.WaitSkill, parsing.WaitSkill);
        }
    }
}
