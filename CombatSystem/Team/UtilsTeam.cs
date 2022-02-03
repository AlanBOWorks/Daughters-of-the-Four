using System;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Team
{
    internal static class UtilsTeam 
    {
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

        public static T GetElement<T>(IStanceDataRead stanceProvider, IFullStanceStructureRead<T> structure)
        {
            if (stanceProvider == null) return structure.DisruptionStance;
            return GetElement(stanceProvider.CurrentStance, structure);
        }

        public static T GetElement<T>(CombatEntity member, IOppositionTeamStructureRead<T> structure)
        {
            var playerTeam = CombatSystemSingleton.PlayerTeam;
            return playerTeam.Contains(member) 
                ? structure.PlayerTeamType 
                : structure.EnemyTeamType;
        }
    }
}
