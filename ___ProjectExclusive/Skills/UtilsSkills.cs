using System;
using System.Collections.Generic;
using Characters;
using _CombatSystem;
using _Team;
using UnityEngine;
using Object = UnityEngine.Object;

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
                _ => throw new NotImplementedException("Not implemented [Stance] was invoked" +
                                                       $"in the GetElement for [{typeof(T)}]")
            };
        }

        public static T GetElement<T>(IStatDrivenData<T> elements, EnumSkills.StatDriven type)
        {
            return type switch
            {
                EnumSkills.StatDriven.Health => elements.Health,
                EnumSkills.StatDriven.Buff => elements.Buff,
                EnumSkills.StatDriven.Harmony => elements.Harmony,
                EnumSkills.StatDriven.Tempo => elements.Tempo,
                EnumSkills.StatDriven.Control => elements.Control,
                EnumSkills.StatDriven.Stance => elements.Stance,
                EnumSkills.StatDriven.Area => elements.Area,
                _ => throw new NotImplementedException("Not implemented [Stat] was invoked" +
                                                       $"in the GetElement for [{typeof(T)}]")
            };
        }

        public static T GetElement<T>(ITargetDrivenData<T> elements, EnumSkills.TargetingType type)
        {
            return type switch
            {
                EnumSkills.TargetingType.SelfOnly => elements.SelfOnly,
                EnumSkills.TargetingType.Offensive => elements.Offensive,
                EnumSkills.TargetingType.Support => elements.Support,
                _ => throw new NotImplementedException("Not implemented [Targeting] was invoked" +
                                                       $"in the GetElement for [{typeof(T)}]")
            };
        }

        public static T GetElement<T>(IStatDrivenEntity<T> elements,
            EnumSkills.TargetingType targetingType, EnumSkills.StatDriven statType)
        {
            IStatDriven<T> targetingElement = GetElement(elements, targetingType);
            T element = GetElement(targetingElement, statType);


            switch (element)
            {
                case Object unityElement:
                {
                    if(!unityElement) 
                        InjectBackUp();
                    break;
                }
                case null:
                    InjectBackUp();
                    break;
            }

            return element;

            void InjectBackUp()
            {
                targetingElement = elements.BackUpElement;
                element = GetElement(targetingElement, statType);
            }
        }

        public static EnumSkills.TargetingType GetType(CombatSkill skill)
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

    public static class EnumSkills
    {
        /// <summary>
        /// Mainly for AI to have a more concise way of differentiate
        /// the skill archetype and make better decision. <br></br>
        /// (Can also be used to determinate with kind of animation should
        /// be playing)
        /// </summary>
        public enum Archetype
        {
            Undefined = -1,
            /// <summary>
            /// Damages, Static damage
            /// </summary>
            OffensiveHealth = TargetingType.Offensive | StatDriven.Health,
            /// <summary>
            /// DeBuff, Mutes
            /// </summary>
            OffensiveBuff = TargetingType.Offensive | StatDriven.Buff,
            /// <summary>
            /// Disruption Damage
            /// </summary>
            OffensiveHarmony = TargetingType.Offensive | StatDriven.Harmony,
            /// <summary>
            /// Slow downs, stuns, freezes
            /// </summary>
            OffensiveTempo = TargetingType.Offensive | StatDriven.Tempo,
            /// <summary>
            /// Team Health/Control damage
            /// </summary>
            OffensiveControl = TargetingType.Offensive | StatDriven.Control,
            /// <summary>
            /// Alters the Stance
            /// </summary>
            OffensiveStance = TargetingType.Offensive | StatDriven.Stance,
            OffensiveArea = TargetingType.Offensive | StatDriven.Area,

            /// <summary>
            /// Health, Shields (and Mortality)
            /// </summary>
            SupportHealth = TargetingType.Support | StatDriven.Health,
            SupportBuff = TargetingType.Support | StatDriven.Buff,
            SupportHarmony = TargetingType.Support | StatDriven.Harmony,
            SupportTempo = TargetingType.Support | StatDriven.Tempo,
            SupportControl = TargetingType.Support | StatDriven.Control,
            SupportStance = TargetingType.Support | StatDriven.Stance,
            SupportArea = TargetingType.Support | StatDriven.Area,
        }

        public enum StatDriven
        {
            Health = HealthIndex,
            Buff = BuffIndex,
            Harmony = HarmonyIndex,
            Tempo = TempoIndex,
            Control = ControlIndex,
            Stance = StanceIndex,
            Area = AreaIndex
        }


        private const int HealthIndex = 0;
        private const int BuffIndex = HealthIndex+1;
        private const int HarmonyIndex = BuffIndex + 1;
        private const int TempoIndex = HarmonyIndex+1;
        private const int ControlIndex = TempoIndex+1;
        private const int StanceIndex = ControlIndex+1;
        private const int AreaIndex = StanceIndex + 1;

        /// <summary>
        /// Types that defines what kind of usage the skill has
        /// </summary>
        public enum TargetingType
        {
            SelfOnly = SelfOnlyIndex,
            Offensive = OffensiveIndex,
            Support = SupportIndex,
            Other = SelfOnly
        }

        private const int SelfOnlyIndex = 1000;
        private const int OffensiveIndex = SelfOnlyIndex *2;
        private const int SupportIndex = OffensiveIndex *3;
    }
}
