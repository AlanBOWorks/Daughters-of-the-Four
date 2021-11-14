using System;
using System.Collections.Generic;
using CombatEffects;
using CombatEntity;
using CombatSkills;
using CombatTeam;
using UnityEngine;

namespace CombatSystem.CombatSkills
{
    public sealed class UtilsTarget
    {
        public static List<CombatingEntity> GetPossibleTargets(EnumSkills.TargetType targetingType,
            CombatingEntity user)
        {
            return targetingType switch
            {
                EnumSkills.TargetType.Self => user.TargetingHolder.SelfElement,
                EnumSkills.TargetType.Support => user.TargetingHolder.SelfAlliesElement,
                EnumSkills.TargetType.Offensive => user.Team.EnemyTeam.LivingEntitiesTracker,
                _ => throw new ArgumentOutOfRangeException(nameof(targetingType), targetingType, null)
            };
        }

        public static List<CombatingEntity> GetPossibleTargets(CombatingSkill skill, CombatingEntity user)
            => GetPossibleTargets(skill.GetTargetType(), user);

        public static List<CombatingEntity> GetPossibleTargets(EnumEffects.TargetType targetType, CombatingEntity user,
            CombatingEntity target)
        {
            var userTargeting = user.TargetingHolder;
            var targetTargeting = target.TargetingHolder;
            return UtilsEffects.GetElement(targetType, userTargeting, targetTargeting);
        }

        public static List<CombatingEntity> GetPossibleTargets(EffectParameter effect, CombatingEntity user,
            CombatingEntity target) 
            => GetPossibleTargets(effect.targetType, user, target);

    }
}
