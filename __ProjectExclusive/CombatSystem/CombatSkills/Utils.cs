using System;
using CombatEntity;
using Stats;
using UnityEngine;

namespace CombatSkills
{
    public static class UtilSkills
    {
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

        public static T GetElement<T>(ISkillTypesRead<T> skillsStructures, EnumSkills.TargetType targetType)
        {
            return targetType switch
            {
                EnumSkills.TargetType.Self => skillsStructures.SelfType,
                EnumSkills.TargetType.Support => skillsStructures.SupportType,
                EnumSkills.TargetType.Offensive => skillsStructures.OffensiveType,
                _ => throw new ArgumentOutOfRangeException(nameof(targetType), targetType, null)
            };
        }
    }

    
}
