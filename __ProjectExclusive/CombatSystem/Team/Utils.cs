
using System;
using CombatEffects;
using CombatEntity;

namespace CombatTeam
{
    public static class UtilsTeam
    {
        public static T GetElement<T>(ITeamRoleStructureRead<T> team, EnumTeam.Role role)
        {
            return role switch
            {
                EnumTeam.Role.Vanguard => team.Vanguard,
                EnumTeam.Role.Attacker => team.Attacker,
                EnumTeam.Role.Support => team.Support,
                _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
            };
        }
        public static T GetElement<T>(ITeamRoleStructureRead<T> team, EnumTeam.TeamPosition teamPosition)
            => GetElement(team, (EnumTeam.Role)teamPosition);

        public static T GetElement<T>(ITeamStanceStructureRead<T> stanceStructure, EnumTeam.TeamStance stance)
        {
            return stance switch
            {
                EnumTeam.TeamStance.Neutral => stanceStructure.OnNeutralStance,
                EnumTeam.TeamStance.Attacking => stanceStructure.OnAttackStance,
                EnumTeam.TeamStance.Defending => stanceStructure.OnDefenseStance,
                _ => throw new ArgumentOutOfRangeException(nameof(stance), stance, null)
            };
        }



        public static void InjectElement<T>(ITeamRoleStructureInject<T> team, EnumTeam.Role role, T element)
        {
            switch (role)
            {
                case EnumTeam.Role.Vanguard:
                    team.Vanguard = element;
                    break;
                case EnumTeam.Role.Attacker:
                    team.Attacker = element;
                    break;
                case EnumTeam.Role.Support:
                    team.Support = element;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(role), role, null);
            }
        }
        public static void InjectElement<T>(ITeamRoleStructureInject<T> team, EnumTeam.TeamPosition teamPosition, T element)
            => InjectElement(team, (EnumTeam.Role)teamPosition, element);


        public static void InjectElements<T>(ITeamRoleStructureInject<T> team, ITeamRoleStructureRead<T> injection)
        {
            team.Vanguard = injection.Vanguard;
            team.Attacker = injection.Attacker;
            team.Support = injection.Support;
        }

        public static void InjectElements<T, TParse>(ITeamStanceStructure<T> team,
            ITeamStanceStructureRead<TParse> injection,
            Func<TParse, T> parseFunc)
        {
            team.OnAttackStance = parseFunc(injection.OnAttackStance);
            team.OnNeutralStance = parseFunc(injection.OnNeutralStance);
            team.OnDefenseStance = parseFunc(injection.OnDefenseStance);
        }
        public static void InjectElements<T, TParse, TParse2>(ITeamStanceStructure<T> team,
            ITeamStanceStructureRead<TParse> injection,
            ITeamStanceStructureRead<TParse2> injectionSecondary,
            Func<TParse,TParse2, T> parseFunc)
        {
            team.OnAttackStance = parseFunc(injection.OnAttackStance, injectionSecondary.OnAttackStance);
            team.OnNeutralStance = parseFunc(injection.OnNeutralStance, injectionSecondary.OnNeutralStance);
            team.OnDefenseStance = parseFunc(injection.OnDefenseStance, injectionSecondary.OnDefenseStance);
        }


        public static void InjectElements<T, TParse>(ITeamRoleStructureInject<T> team, ITeamRoleStructureRead<TParse> injection,
            Func<TParse, T> parseFunc)
        {
            team.Vanguard = parseFunc(injection.Vanguard);
            team.Attacker = parseFunc(injection.Attacker);
            team.Support = parseFunc(injection.Support);
        }



        public static void DoActionOnTeam<T>(ITeamRoleStructureRead<T> team, Action<T> action)
        {
            action(team.Vanguard);
            action(team.Attacker);
            action(team.Support);
        }
        public static void DoActionOnTeam<T, T2>(ITeamRoleStructureRead<T> team,
            ITeamRoleStructureRead<T2> injectAction,
            Action<T, T2> action)
        {
            action(team.Vanguard, injectAction.Vanguard);
            action(team.Attacker, injectAction.Attacker);
            action(team.Support, injectAction.Support);
        }

        public static void DoActionOnTeam<T>(ITeamStanceStructureRead<T> team, Action<T> action)
        {
            action(team.OnAttackStance);
            action(team.OnNeutralStance);
            action(team.OnDefenseStance);
        }

        public static void DoActionOnTeam<T, T2>(ITeamStanceStructureRead<T> team,
            ITeamStanceStructureRead<T2> injectAction,
            Action<T, T2> action)
        {
            action(team.OnAttackStance, injectAction.OnAttackStance);
            action(team.OnNeutralStance, injectAction.OnNeutralStance);
            action(team.OnDefenseStance, injectAction.OnDefenseStance);
        }
    }


    public static class EnumTeam
    {
        public enum Role
        {
            Vanguard,
            Attacker,
            Support
        }

        /// <summary>
        /// Alternative to [<see cref="Role"/>];<br>
        /// Is preferred to use [<see cref="Role"/>]
        /// unless the logic dictates positioning (instead of role)
        /// </summary>
        public enum TeamPosition
        {
            FrontLine = Role.Vanguard,
            MidLine = Role.Attacker,
            BackLine = Role.Support
        }

        public enum FieldPosition
        {
            InTeam,
            DivingEnemy,
            OutPosition
        }

        public enum TeamStance
        {
            Neutral,
            Attacking,
            Defending
        }

        public static TeamPosition ParseEnum(Role role) => (TeamPosition)role;
    }
}
