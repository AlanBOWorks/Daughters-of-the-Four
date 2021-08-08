using System;
using System.Collections.Generic;
using Characters;
using _CombatSystem;
using _Team;
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

        public static SCharacterSharedSkillsPreset GetOnNullSkills(CharacterArchetypes.TeamPosition position)
        {
            var backUpElements 
                = CombatSystemSingleton.ParamsVariable.ArchetypesOnNullSkills;
            return CharacterArchetypes.GetElement(backUpElements, position);
        }

        public static List<CombatSkill> GetUniqueByStance(CombatingEntity entity)
        {
            var state = entity.AreasDataTracker.GetCurrentPositionState();
            var skills = entity.UniqueSkills;

            return skills == null 
                ? null 
                : TeamCombatState.GetStance(skills, state);
        }
        public static List<CombatSkill> GetSkillsByStance(CombatingEntity entity)
        {
            var state = entity.AreasDataTracker.GetCurrentPositionState();
            var skills = entity.CombatSkills;

            return skills == null
                ? null
                : TeamCombatState.GetStance((IStanceArchetype<List<CombatSkill>>)skills, state);
        }

        public static T GetElement<T>(ISkillPositions<T> elements, CombatingEntity entity)
        {
            var state = entity.AreasDataTracker.GetCurrentPositionState();
            return state switch
            {
                TeamCombatState.Stances.Attacking => elements.AttackingSkills,
                TeamCombatState.Stances.Neutral => elements.NeutralSkills,
                TeamCombatState.Stances.Defending => elements.DefendingSkills,
                _ => throw new NotImplementedException("Not implemented stance was invoked" +
                                                       $"in the GetElement for [{typeof(T)}]")
            };
        }

        public static SSkillPreset.SkillType GetType(CombatSkill skill)
        {
            return skill.Preset.GetSkillType();
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

        public static void DoSafeParse<T>(ISkillShared<T> skills, Action<T> action)
        {
            DoAction(skills.UltimateSkill);
            DoAction(skills.CommonSkillFirst);
            DoAction(skills.CommonSkillSecondary);
            DoAction(skills.WaitSkill);

            void DoAction(T element)
            {
                if (element == null) return;
                action(element);
            }
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
