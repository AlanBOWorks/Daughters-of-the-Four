using System;
using System.Collections.Generic;
using Characters;
using _CombatSystem;
using UnityEngine;

namespace Skills
{
    public static class UtilsSkill
    {

        public static bool CanUseSkills(CombatingEntity entity)
        {
            var sharedSkills = entity.SharedSkills;
            if (!sharedSkills.WaitSkill.IsInCooldown())
            {
                return true;
            }
            if (!sharedSkills.CommonSkillFirst.IsInCooldown())
            {
                return true;
            }
            if (!sharedSkills.CommonSkillSecondary.IsInCooldown())
            {
                return true;
            }

            var currentSkills = GetSkillsByStance(entity);
            foreach (CombatSkill skill in currentSkills)
            {
                if (!skill.IsInCooldown())
                    return true;
            }

            if (sharedSkills.UltimateSkill != null && !sharedSkills.UltimateSkill.IsInCooldown())
            {
                return true;
            }

            return false;
        }

        public static SCharacterSharedSkillsPreset GetBackUpSkills(CharacterArchetypes.TeamPosition position)
        {
            var backUpElements 
                = CombatSystemSingleton.ParamsVariable.ArchetypesBackupSkills;
            return CharacterArchetypes.GetElement(backUpElements, position);
        }

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
            return skill.Preset.GetMainEffect().GetEffectType();
        }
        
        public static void DoParse<T>(ISkillPositions<T> skills, Action<T> action)
        {
            action(skills.AttackingSkills);
            action(skills.NeutralSkills);
            action(skills.DefendingSkills);
        }

        public static void DoParse<T, TParse>(ISkillPositions<T> skills, ISkillPositions<TParse> parsing,
            Action<T, TParse> action)
        {
            action(skills.AttackingSkills, parsing.AttackingSkills);
            action(skills.NeutralSkills, parsing.NeutralSkills);
            action(skills.DefendingSkills, parsing.DefendingSkills);
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
