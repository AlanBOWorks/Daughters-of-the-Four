using System;
using System.Collections.Generic;
using System.Linq;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Skills
{
    public static class UtilsSkill
    {
        public static void DoSubmitSkill(in CombatSkill skill, in CombatEntity performer, in CombatEntity mainTarget)
        {

        }
    }

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
            var team = GetTeam(in target);
            return team.AliveTargeting.GetAlivePositionMembers(target.PositioningType);
        }

        private static IReadOnlyList<CombatEntity> GetAliveTeamAsExcluded(in CombatEntity target)
        {
            var team = GetTeam(in target);
            return team.AliveTargeting.GetAllAliveMembers(in target);
        }

        private static CombatTeam GetTeam(in CombatEntity entity) 
            => entity.Team;
        private static CombatTeam GetEnemyTeam(in CombatEntity entity) 
            => GetTeam(in entity).EnemyTeam;

        private static IReadOnlyList<CombatEntity> GetAliveTeam(in CombatEntity entity)
            => GetTeam(in entity).AliveTargeting.GetAllAliveMembers();


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
                    return GetAliveTeamAsExcluded(in performer);
                default:
                    throw new ArgumentOutOfRangeException();
            }

            IReadOnlyList<CombatEntity> GetOffensiveTargets()
            {
                var targetType = skill.TargetType;
                var enemyTeam = GetEnemyTeam(performer);
                return targetType == EnumsSkill.TargetType.Area 
                    ? enemyTeam.AliveTargeting.GetFrontMostAliveLineMembers()
                    : enemyTeam.AliveTargeting.GetAllAliveMembers();
            }
        }

        public static IReadOnlyList<CombatEntity> GetEffectTargets(
            in CombatEntity performer,
            in CombatEntity selectedTarget,
            in EnumsEffect.TargetType type)
        {
            var aliveGroupReadOnly = HandleEffectTargets(in performer, in selectedTarget, in type);

            // Problem: the collection is modified during the effects
            // Solution: create a copy that will be used only for the effects purposes
            return aliveGroupReadOnly.ToArray();
        }


        private static IReadOnlyList<CombatEntity> HandleEffectTargets(
            in CombatEntity performer,
            in CombatEntity selectedTarget,
            in EnumsEffect.TargetType type)
        {
            switch (type)
            {
                case EnumsEffect.TargetType.Target:
                    return GetSingleTargetGroup(in selectedTarget);
                case EnumsEffect.TargetType.Performer:
                    return GetSingleTargetGroup(in performer);



                case EnumsEffect.TargetType.TargetTeam:
                    return GetAliveTeam(in selectedTarget);
                case EnumsEffect.TargetType.PerformerTeam:
                    return GetAliveTeam(in performer);



                case EnumsEffect.TargetType.TargetTeamExcluded:
                    return GetAliveTeamAsExcluded(in selectedTarget);
                case EnumsEffect.TargetType.PerformerTeamExcluded:
                    return GetAliveTeamAsExcluded(in performer);



                default:
                    throw new NotImplementedException($"Effect Targeting for [{type}] not implemented");
            }
        }
    }
}
