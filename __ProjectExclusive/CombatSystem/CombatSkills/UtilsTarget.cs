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
        static UtilsTarget()
        {
            int listAlloc = GameParams.DefaultMembersPerTeam;

            SkillTargetsHolder = new List<CombatingEntity>(listAlloc);
            EffectTargetsHolder = new List<CombatingEntity>(listAlloc);
        }

        private static readonly List<CombatingEntity> SkillTargetsHolder;
        private static readonly List<CombatingEntity> EffectTargetsHolder;

        public static List<CombatingEntity> GetPossibleTargets(CombatingEntity user, EnumSkills.TargetType targetingType)
        {
            SkillTargetsHolder.Clear();

            switch (targetingType)
            {
                case EnumSkills.TargetType.Self:
                    SkillTargetsHolder.Add(user);
                    break;
                case EnumSkills.TargetType.Support:
                    SkillTargetsHolder.AddRange(user.Team.LivingEntitiesTracker);
                    break;
                case EnumSkills.TargetType.Offensive:
                    HandlePossibleEnemies(user.Team.EnemyTeam);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return SkillTargetsHolder;
        }

        public static List<CombatingEntity> GetPossibleTargets(
            CombatingEntity user,
            CombatingEntity target, 
            EnumEffects.TargetType targetType)
        {
            EffectTargetsHolder.Clear();
            var list = EffectTargetsHolder;

            switch (targetType)
            {
                case EnumEffects.TargetType.Self:
                    list.Add(user);
                    break;
                case EnumEffects.TargetType.SelfExclude:
                    list.AddRange(user.Team);
                    list.Remove(user);
                    break;
                case EnumEffects.TargetType.SelfTeam:
                    list.AddRange(user.Team);
                    break;
                case EnumEffects.TargetType.Target:
                    list.Add(target);
                    break;
                case EnumEffects.TargetType.TargetExclude:
                    list.AddRange(target.Team);
                    list.Remove(target);
                    break;
                case EnumEffects.TargetType.TargetTeam:
                    list.AddRange(target.Team);
                    break;
                case EnumEffects.TargetType.All:
                    list.AddRange(user.Team);
                    list.AddRange(target.Team);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetType), targetType, null);
            }

            return list;
        }

        private static void HandlePossibleEnemies(CombatingTeam team)
        {
            if (team.CurrentStance == EnumTeam.TeamStance.Defending)
            {
                SkillTargetsHolder.Add(team.CollectFrontMostMember());
                return;
            }

            SkillTargetsHolder.AddRange(team.LivingEntitiesTracker);
        }
    }
}
