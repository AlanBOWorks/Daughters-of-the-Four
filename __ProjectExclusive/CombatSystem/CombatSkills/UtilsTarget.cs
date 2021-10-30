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
        public static List<CombatingEntity> GetPossibleTargets(CombatingEntity user, EnumSkills.TargetType targetingType)
        {
            return targetingType switch
            {
                EnumSkills.TargetType.Self => user.TargetingHolder.SelfElement,
                EnumSkills.TargetType.Support => user.TargetingHolder.SelfAlliesElement,
                EnumSkills.TargetType.Offensive => user.Team.EnemyTeam.LivingEntitiesTracker,
                _ => throw new ArgumentOutOfRangeException(nameof(targetingType), targetingType, null)
            };
        }

        public static List<CombatingEntity> GetPossibleTargets(CombatingEntity user, CombatingSkill skill)
            => GetPossibleTargets(user, skill.GetTargetType());

        public static List<CombatingEntity> GetPossibleTargets(
        CombatingEntity user,
        CombatingEntity target, 
        EnumEffects.TargetType targetType)
        {
            switch (targetType)
            {
                case EnumEffects.TargetType.Self:
                    return user.TargetingHolder.SelfElement;
                case EnumEffects.TargetType.SelfAllies:
                    return user.TargetingHolder.SelfAlliesElement;
                case EnumEffects.TargetType.SelfTeam:
                    return user.TargetingHolder.SelfTeamElement;
                case EnumEffects.TargetType.Target:
                    return target.TargetingHolder.SelfElement;
                case EnumEffects.TargetType.TargetAllies:
                    return target.TargetingHolder.SelfAlliesElement;
                case EnumEffects.TargetType.TargetTeam:
                    return target.TargetingHolder.SelfTeamElement;
                case EnumEffects.TargetType.All:
                    return CombatSystemSingleton.AllEntities;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetType), targetType, null);
            }
        }

        public static List<CombatingEntity> GetPossibleTargets(CombatingEntity user, CombatingEntity target, EffectParameter effect) 
            => GetPossibleTargets(user, target, effect.targetType);
    }
}
