using System;
using CombatEntity;
using Stats;
using UnityEngine;

namespace CombatSkills
{
    public static class UtilSkills
    {

        public static EnumSkills.SkillInteractionType GetInteractionType(ISkill skill)
        {
            return skill.GetMainEffect().preset.GetComponentType();
        }
        public static EnumSkills.SkillInteractionType GetInteractionType(SkillValuesHolders skill)
            => GetInteractionType(skill.UsedSkill);

        public static void DoActionOn<T>(ISkillGroupTypesRead<T> group, Action<T> action)
        {
            action(group.SharedSkillTypes);
            action(group.MainSkillTypes);
        }

        public static void DoActionOn<T, TSecondary>(ISkillGroupTypesRead<T> primary,
            ISkillGroupTypesRead<TSecondary> secondary,
            Action<T, TSecondary> action)
        {
            action(primary.SharedSkillTypes, secondary.SharedSkillTypes);
            action(primary.MainSkillTypes, secondary.MainSkillTypes);
        }

        public static T GetElement<T>(EnumSkills.TargetType targetType, ISkillTypesRead<T> skillsStructures)
        {
            return targetType switch
            {
                EnumSkills.TargetType.Self => skillsStructures.SelfType,
                EnumSkills.TargetType.Support => skillsStructures.SupportType,
                EnumSkills.TargetType.Offensive => skillsStructures.OffensiveType,
                _ => throw new ArgumentOutOfRangeException(nameof(targetType), targetType, null)
            };
        }

        public static T GetElement<T>(EnumSkills.DominionType type, IDominionStructureRead<T> dominionStructure)
        {
            return type switch
            {
                EnumSkills.DominionType.Guard => dominionStructure.Guard,
                EnumSkills.DominionType.Control => dominionStructure.Control,
                EnumSkills.DominionType.Provoke => dominionStructure.Provoke,
                EnumSkills.DominionType.Stance => dominionStructure.Stance,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public static T GetElement<T>(EnumSkills.SkillInteractionType type, ISkillInteractionStructureRead<T> structure)
        {
            return type switch
            {
                EnumSkills.SkillInteractionType.Attack => structure.Attack,
                EnumSkills.SkillInteractionType.Persistent => structure.Persistent,
                EnumSkills.SkillInteractionType.DeBuff => structure.Debuff,
                EnumSkills.SkillInteractionType.FollowUp => structure.FollowUp,

                EnumSkills.SkillInteractionType.Heal => structure.Heal,
                EnumSkills.SkillInteractionType.Buff => structure.Buff,
                EnumSkills.SkillInteractionType.ReceiveBuff => structure.ReceiveBuff,
                EnumSkills.SkillInteractionType.Shielding => structure.Shielding,

                EnumSkills.SkillInteractionType.Guard => structure.Guard,
                EnumSkills.SkillInteractionType.Control => structure.Control,
                EnumSkills.SkillInteractionType.Provoke => structure.Provoke,
                EnumSkills.SkillInteractionType.Stance => structure.Stance,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        public static T GetElementSafe<T>(EnumSkills.SkillInteractionType type,
            ISkillInteractionStructureRead<T> structure)
        where T : class
        {
            return type switch
            {
                EnumSkills.SkillInteractionType.Attack => structure.Attack,
                EnumSkills.SkillInteractionType.Persistent => structure.Persistent,
                EnumSkills.SkillInteractionType.DeBuff => structure.Debuff,
                EnumSkills.SkillInteractionType.FollowUp => structure.FollowUp,

                EnumSkills.SkillInteractionType.Heal => structure.Heal,
                EnumSkills.SkillInteractionType.Buff => structure.Buff,
                EnumSkills.SkillInteractionType.ReceiveBuff => structure.ReceiveBuff,
                EnumSkills.SkillInteractionType.Shielding => structure.Shielding,

                EnumSkills.SkillInteractionType.Guard => structure.Guard,
                EnumSkills.SkillInteractionType.Control => structure.Control,
                EnumSkills.SkillInteractionType.Provoke => structure.Provoke,
                EnumSkills.SkillInteractionType.Stance => structure.Stance,
                _ => null
            };
        }

        public static T GetElementMaster<T>(EnumSkills.SkillInteractionType type,
            ICondensedSkillInteractionStructure<T, T> structure)
        {
            return type switch
            {
                EnumSkills.SkillInteractionType.Attack => structure.Attack,
                EnumSkills.SkillInteractionType.Persistent => structure.Attack,
                EnumSkills.SkillInteractionType.DeBuff => structure.Attack,
                EnumSkills.SkillInteractionType.FollowUp => structure.Attack,

                EnumSkills.SkillInteractionType.Heal => structure.Support,
                EnumSkills.SkillInteractionType.Buff => structure.Support,
                EnumSkills.SkillInteractionType.ReceiveBuff => structure.Support,
                EnumSkills.SkillInteractionType.Shielding => structure.Support,

                EnumSkills.SkillInteractionType.Guard => structure.Dominion,
                EnumSkills.SkillInteractionType.Control => structure.Dominion,
                EnumSkills.SkillInteractionType.Provoke => structure.Dominion,
                EnumSkills.SkillInteractionType.Stance => structure.Dominion,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        public static T GetElementMasterSafe<T>(EnumSkills.SkillInteractionType type,
            ICondensedSkillInteractionStructure<T, T> structure)
        where T : class
        {
            return type switch
            {
                EnumSkills.SkillInteractionType.Attack => structure.Offensive,
                EnumSkills.SkillInteractionType.Persistent => structure.Offensive,
                EnumSkills.SkillInteractionType.DeBuff => structure.Offensive,
                EnumSkills.SkillInteractionType.FollowUp => structure.Offensive,
                EnumSkills.SkillInteractionType.Heal => structure.Support,
                EnumSkills.SkillInteractionType.Buff => structure.Support,
                EnumSkills.SkillInteractionType.ReceiveBuff => structure.Support,
                EnumSkills.SkillInteractionType.Shielding => structure.Support,
                EnumSkills.SkillInteractionType.Guard => structure.Dominion,
                EnumSkills.SkillInteractionType.Control => structure.Dominion,
                EnumSkills.SkillInteractionType.Provoke => structure.Dominion,
                EnumSkills.SkillInteractionType.Stance => structure.Dominion,
                _ => null
            };
        }

        public static T GetElementSafe<T>(SkillValuesHolders skillValues,
            ICondensedSkillInteractionStructure<T, T> structure)
        where T : class
        {
            var skill = skillValues.UsedSkill;
            var mainType = skill.GetMainEffect().preset.GetComponentType();

            var element = GetElementSafe(mainType, structure);
            if (!element.Equals(null)) return element;
            element = GetElementMasterSafe(mainType, structure);
            if (!element.Equals(null)) return element;

            // Problem: The mainType could be a non returning switch(value) [eg: MaxHealth] and can't return a valid element
            // Solution: Use the targetType and use a generic element [masterElement] to return at least something
            // Exception: Maybe the master also are null, in that case there's nothing that can be done in this part
            var targetType = skill.GetTargetType();
            return targetType switch
            {
                EnumSkills.TargetType.Self => structure.Dominion,
                EnumSkills.TargetType.Support => structure.Support,
                EnumSkills.TargetType.Offensive => structure.Offensive,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    
}
