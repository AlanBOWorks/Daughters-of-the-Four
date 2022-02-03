using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;

namespace CombatSystem.Skills
{
    public static class UtilsTarget
    {
        private static readonly List<CombatEntity> TargetingHelper = new List<CombatEntity>();

        private static IReadOnlyList<CombatEntity> GetOnlyTargetAsCollection(in CombatEntity target)
        {
            TargetingHelper.Clear();
            TargetingHelper.Add(target);

            return TargetingHelper;
        }

        private static IReadOnlyList<CombatEntity> GetSingleTargetGroup(in CombatEntity target)
        {
            var team = GetTeam(target);
            return team.GetMemberGroup(in target);
        }

        private static IReadOnlyList<CombatEntity> GetTeamAsExcluded(in CombatEntity target)
        {
            TargetingHelper.Clear();
            foreach (var member in target.Team)
            {
                TargetingHelper.Add(member);
            }

            TargetingHelper.Remove(target);
            return TargetingHelper;
        }

        private static CombatTeam GetTeam(in CombatEntity entity) => entity.Team;
        private static CombatTeam GetEnemyTeam(in CombatEntity entity) => GetTeam(in entity).EnemyTeam;

        public static IReadOnlyList<CombatEntity> GetPossibleTargets(ISkill skill, CombatEntity performer)
        {
            var archetype = skill.Archetype;
            switch (archetype)
            {
                case EnumsSkill.Archetype.Self:
                    return GetOnlyTargetAsCollection(in performer);
                case EnumsSkill.Archetype.Offensive:
                    return GetOffensiveTargets();
                case EnumsSkill.Archetype.Support:
                    return TargetingHelper;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            IReadOnlyList<CombatEntity> GetOffensiveTargets()
            {
                var targetType = skill.TargetType;
                var enemyTeam = GetEnemyTeam(performer);
                return targetType == EnumsSkill.TargetType.Area 
                    ? enemyTeam.FrontLineType 
                    : enemyTeam;
            }
        }

        public static IReadOnlyList<CombatEntity> GetEffectTargets(
            in CombatEntity performer,
            in CombatEntity selectedTarget,
            in EnumsEffect.TargetType type)
        {
            switch (type)
            {
                case EnumsEffect.TargetType.Target:
                    return GetSingleTargetGroup(in selectedTarget);
                case EnumsEffect.TargetType.TargetTeam:
                    return GetTeam(in selectedTarget);
                case EnumsEffect.TargetType.TargetTeamExcluded:
                    return GetTeamAsExcluded(in selectedTarget);
                case EnumsEffect.TargetType.Performer:
                    return GetSingleTargetGroup(in performer);
                case EnumsEffect.TargetType.PerformerTeam:
                    return GetTeam(in performer);
                case EnumsEffect.TargetType.PerformerTeamExcluded:
                    return GetTeam(in performer);
                case EnumsEffect.TargetType.All:
                    return CombatSystemSingleton.AllMembers;
                default:
                    throw new NotImplementedException($"Effect Targeting for [{type}] not implemented");
            }
        }


        public static CombatEntity GetFrontMostMember(CombatTeam team)
        {
            foreach (var entity in team.FrontLineType)
            {
                if (UtilsCombatStats.IsAlive(in entity))
                    return entity;
            }

            foreach (var entity in team.MidLineType)
            {
                if (UtilsCombatStats.IsAlive(in entity))
                    return entity;
            }

            foreach (var entity in team.BackLineType)
            {
                if (UtilsCombatStats.IsAlive(in entity))
                    return entity;
            }

            return null;
        }
    }
}
