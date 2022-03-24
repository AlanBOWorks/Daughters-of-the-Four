using System;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Team
{
    internal static class UtilsTeam 
    {

        public static bool IsAllyEntity(in CombatEntity entity, in CombatEntity control)
        {
            var entityTeam = entity.Team;
            return entityTeam.Contains(control);
        }

        public static bool IsAllyEntity(in CombatEntity entity, in CombatTeam inTeam)
        {
            return inTeam.Contains(entity);
        }

        public static bool IsPlayerTeam(in CombatTeam team)
        {
            return team == CombatSystemSingleton.PlayerTeam;
        }

        public static int GetRoleIndex(ICombatEntityProvider entity)
        {
            var entityRole = entity.GetAreaData().RoleType;
            return (int) entityRole;
        }

        public static T GetElement<T>(EnumTeam.Positioning positioning, ITeamPositionStructureRead<T> structure)
        {
            return positioning switch
            {
                EnumTeam.Positioning.FrontLine => structure.FrontLineType,
                EnumTeam.Positioning.MidLine => structure.MidLineType,
                EnumTeam.Positioning.BackLine => structure.BackLineType,
                _ => throw new ArgumentOutOfRangeException(nameof(positioning), positioning, null)
            };
        }

        public static TeamAreaData GenerateAreaData(EnumTeam.Role fromRole)
        {
            EnumTeam.Positioning positioning = GetEquivalent(fromRole);

            return new TeamAreaData(fromRole, positioning);
        }

        public static EnumTeam.Positioning GetEquivalent(EnumTeam.Role fromRole)
        {
            switch (fromRole)
            {
                case EnumTeam.Role.Vanguard:
                    return EnumTeam.Positioning.FrontLine;
                case EnumTeam.Role.Attacker:
                    return EnumTeam.Positioning.MidLine;
                case EnumTeam.Role.Support:
                    return EnumTeam.Positioning.BackLine;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fromRole), fromRole, null);
            }
        }

        public static T GetElement<T>(EnumTeam.Role role, ITeamRoleStructureRead<T> structure)
        {
            return role switch
            {
                EnumTeam.Role.Vanguard => structure.VanguardType,
                EnumTeam.Role.Attacker => structure.AttackerType,
                EnumTeam.Role.Support => structure.SupportType,
                _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
            };
        }

        public static T GetElement<T>(EnumTeam.Stance stance, IStanceStructureRead<T> structure)
        {
            return stance switch
            {
                EnumTeam.Stance.Neutral => structure.NeutralStance,
                EnumTeam.Stance.Attacking => structure.AttackingStance,
                EnumTeam.Stance.Defending => structure.DefendingStance,
                _ => throw new ArgumentOutOfRangeException(nameof(stance), stance, null)
            };
        }

        public static T GetElement<T>(EnumTeam.StanceFull stance, IFullStanceStructureRead<T> structure)
        {
            return stance switch
            {
                EnumTeam.StanceFull.Neutral => structure.NeutralStance,
                EnumTeam.StanceFull.Attacking => structure.AttackingStance,
                EnumTeam.StanceFull.Defending => structure.DefendingStance,
                _ => structure.DisruptionStance
            };
        }

        public static T GetElement<T>(CombatEntity member, IOppositionTeamStructureRead<T> structure)
        {
            var playerTeam = CombatSystemSingleton.PlayerTeam;
            return playerTeam.Contains(member) 
                ? structure.PlayerTeamType 
                : structure.EnemyTeamType;
        }

        public static EnumTeam.StanceFull ParseStance(EnumTeam.Stance basicStance)
        {
            switch (basicStance)
            {
                case EnumTeam.Stance.Neutral:
                    return EnumTeam.StanceFull.Neutral;
                case EnumTeam.Stance.Attacking:
                    return EnumTeam.StanceFull.Attacking;
                case EnumTeam.Stance.Defending:
                    return EnumTeam.StanceFull.Defending;
                default:
                    return EnumTeam.StanceFull.Disrupted;
            }
        }
    }

    public static class UtilsCombatTeam
    {
        public static void SwitchStance(in CombatTeam team, in EnumTeam.Stance targetStance)
        {
            team.DataValues.CurrentStance = targetStance;
            CombatSystemSingleton.EventsHolder.OnStanceChange(in team, in targetStance);
        }

        public static void GainControl(in CombatTeam team, in float controlVariation)
        {
            var teamData = team.DataValues;
            float controlAmount = teamData.NaturalControl + controlVariation;
            controlAmount = Mathf.Clamp01(controlAmount);
            teamData.NaturalControl = controlAmount;

            var enemyTeam = team.EnemyTeam;
            var enemyTeamData = enemyTeam.DataValues;
            float enemyControl = 1-controlAmount; //By design: team vs enemy control goes in percent.
            enemyTeamData.NaturalControl = enemyControl;

            var eventsHolder = CombatSystemSingleton.EventsHolder;
            eventsHolder.OnControlChange(in team, in controlVariation, false);
            float enemyControlVariation = -controlVariation;
            eventsHolder.OnControlChange(in enemyTeam, in enemyControlVariation, false);
        }

        public static void BurstControl(in CombatTeam team, in float controlVariation)
        {
            team.DataValues.BurstControl += controlVariation;
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            eventsHolder.OnControlChange(in team, in controlVariation, true);
        }
    }
}
