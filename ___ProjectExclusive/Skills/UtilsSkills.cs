using System;
using System.Collections.Generic;
using Characters;
using _CombatSystem;
using _Team;
using Stats;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Skills
{
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
            OffensiveHealth = TargetingType.Offensive | StatDriven.Health,
            OffensiveStatic = TargetingType.Offensive | StatDriven.Static,
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
            /// Health (and Mortality)
            /// </summary>
            SupportHealth = TargetingType.Support | StatDriven.Health,
            SupportStatic = TargetingType.Support | StatDriven.Static,
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
            Static = StaticIndex,
            Buff = BuffIndex,
            Harmony = HarmonyIndex,
            Tempo = TempoIndex,
            Control = ControlIndex,
            Stance = StanceIndex,
            Area = AreaIndex
        }


        private const int HealthIndex = 0;
        private const int StaticIndex = HealthIndex + 1;
        private const int BuffIndex = StaticIndex + 1;
        private const int HarmonyIndex = BuffIndex + 1;
        private const int TempoIndex = HarmonyIndex + 1;
        private const int ControlIndex = TempoIndex + 1;
        private const int StanceIndex = ControlIndex + 1;
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
        private const int OffensiveIndex = SelfOnlyIndex * 2;
        private const int SupportIndex = OffensiveIndex * 3;

        public enum HitType
        {
            /// <summary>
            /// Invoke the event directly on hit
            /// </summary>
            DirectHit,
            /// <summary>
            /// Each hit will increase the effect; Once on StartSequence all counts will be invoke
            /// </summary>
            OnHitIncrease,
            /// <summary>
            /// Remove the effect if received hit, else on StartSequence invoke
            /// </summary>
            OnHitCancel
        }
    }


    public static class UtilsSkill
    {

        public static bool CanUseSkills(CombatingEntity entity)
        {
            var skills = entity.CombatSkills;
            var sharedSkills = skills.GetCurrentSharedSkills();

            if (!skills.WaitSkill.IsInCooldown())
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

            if (skills.UltimateSkill != null && !skills.UltimateSkill.IsInCooldown())
            {
                return true;
            }

            return false;
        }

        public static ISharedSkillsInPosition<SkillPreset> GetSharedSkillPreset(
            EnumCharacter.RoleArchetype role, EnumCharacter.RangeType rangeType)
        {
            return CombatSystemSingleton.ParamsVariable.GetSkillPreset(role, rangeType);
        }

        public static List<CombatSkill> GetUniqueByStance(CombatingEntity entity)
        {
            var state = entity.AreasDataTracker.GetCurrentPositionState();
            var skills = entity.CombatSkills.UniqueSkills;

            return skills == null 
                ? null 
                : UtilsSkill.GetElement(skills, state);
        }
        public static List<CombatSkill> GetSkillsByStance(CombatingEntity entity)
        {
            var state = entity.AreasDataTracker.GetCurrentPositionState();
            var skills = entity.CombatSkills;

            return skills == null
                ? null
                : UtilsTeam.GetElement(skills, state);
        }
        public static T GetElement<T>(ISkillPositions<T> elements, CombatingEntity entity)
        {
            var state = entity.AreasDataTracker.GetCurrentPositionState();
            return GetElement(elements, state);
        }
        public static T GetElement<T>(ISkillPositions<T> elements, EnumTeam.Stances state)
        {
            return state switch
            {
                EnumTeam.Stances.Attacking => elements.AttackingSkills,
                EnumTeam.Stances.Neutral => elements.NeutralSkills,
                EnumTeam.Stances.Defending => elements.DefendingSkills,
                _ => throw new NotImplementedException("Not implemented [Stance] was invoked" +
                                                       $"in the GetElement for [{typeof(T)}]")
            };
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
        public static void DoParse<T>(ISharedSkills<T> skills, Action<T> action)
        {
            action(skills.CommonSkillFirst);
            action(skills.CommonSkillSecondary);
        }
        public static void DoParse<T>(ISharedSkillsInPosition<T> sharedSkillsPositions, Action<T> action)
        {
            DoParse(sharedSkillsPositions.AttackingSkills, action);
            DoParse(sharedSkillsPositions.NeutralSkills, action);
            DoParse(sharedSkillsPositions.DefendingSkills, action);
        }


        public static void DoParse<T, TParse>(ISkillPositions<T> skills, ISkillPositions<TParse> parsing,
            Action<T, TParse> action)
        {
            action(skills.AttackingSkills, parsing.AttackingSkills);
            action(skills.NeutralSkills, parsing.NeutralSkills);
            action(skills.DefendingSkills, parsing.DefendingSkills);
        }

        public static void DoParse<T, TParse>(ISharedSkills<T> skills, ISharedSkills<TParse> parsing,
            Action<T, TParse> action)
        {
            action(skills.CommonSkillFirst, parsing.CommonSkillFirst);
            action(skills.CommonSkillSecondary, parsing.CommonSkillSecondary);
        }

        public static void DoParse<T, TParse>(ISharedSkillsInPosition<T> sharedSkillsPositions,
            ISharedSkillsInPosition<TParse> parsing, Action<T, TParse> action)
        {
            DoParse(sharedSkillsPositions.AttackingSkills,parsing.AttackingSkills,action);
            DoParse(sharedSkillsPositions.NeutralSkills,parsing.NeutralSkills,action);
            DoParse(sharedSkillsPositions.DefendingSkills,parsing.DefendingSkills,action);
        }

        public static void DoParse<T>(ISharedSkillsSet<T> skills, Action<T> action)
        {
            action(skills.UltimateSkill);
            action(skills.WaitSkill);
            DoParse(skills as ISharedSkillsInPosition<T>, action);
        }

        public static void DoSafeParse<T>(ISharedSkillsSet<T> skills, Action<T> action)
        {
            DoAction(skills.UltimateSkill);
            DoAction(skills.WaitSkill);

            DoActions(skills.AttackingSkills);
            DoActions(skills.NeutralSkills);
            DoActions(skills.DefendingSkills);

            void DoAction(T element)
            {
                if (element == null) return;
                action(element);
            }

            void DoActions(ISharedSkills<T> elements)
            {
                DoAction(elements.CommonSkillFirst);
                DoAction(elements.CommonSkillSecondary);
            }
        }

        public static void DoParse<T,TParse>(ISharedSkillsSet<T> skills, ISharedSkillsSet<TParse> parsing,
            Action<T, TParse> action)
        {
            action(skills.UltimateSkill, parsing.UltimateSkill);
            action(skills.WaitSkill, parsing.WaitSkill);

            DoParse(skills as ISharedSkillsInPosition<T>,parsing,action);
        }

        public static void AddTo(ISharedSkills<CombatSkill> skills, List<CombatSkill> addTo)
        {
            addTo.Add(skills.CommonSkillFirst);
            addTo.Add(skills.CommonSkillSecondary);
        }

        public static void AddTo(ISharedSkillsSet<CombatSkill> skills, List<CombatSkill> addTo)
        {
            AddTo(skills.AttackingSkills, addTo);
            AddTo(skills.NeutralSkills, addTo);
            AddTo(skills.DefendingSkills, addTo);
        }
    }

}
