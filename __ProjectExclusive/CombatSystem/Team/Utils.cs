
using System;

namespace CombatTeam
{
    public static class UtilsTeam
    {
        public static T GetElement<T>(ITeamStructureRead<T> team, EnumTeam.Role role)
        {
            return role switch
            {
                EnumTeam.Role.Vanguard => team.Vanguard,
                EnumTeam.Role.Attacker => team.Attacker,
                EnumTeam.Role.Support => team.Support,
                _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
            };
        }
        public static T GetElement<T>(ITeamStructureRead<T> team, EnumTeam.TeamPosition teamPosition)
            => GetElement(team, (EnumTeam.Role)teamPosition);

        public static void InjectElement<T>(ITeamStructureInject<T> team, EnumTeam.Role role, T element)
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
        public static void InjectElement<T>(ITeamStructureInject<T> team, EnumTeam.TeamPosition teamPosition, T element)
            => InjectElement(team, (EnumTeam.Role)teamPosition, element);


        public static void InjectElements<T>(ITeamStructureInject<T> team, ITeamStructureRead<T> injection)
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

        public static void InjectElements<T, TParse>(ITeamStructureInject<T> team, ITeamStructureRead<TParse> injection,
            Func<TParse, T> parseFunc)
        {
            team.Vanguard = parseFunc(injection.Vanguard);
            team.Attacker = parseFunc(injection.Attacker);
            team.Support = parseFunc(injection.Support);
        }



        public static void DoActionOnTeam<T>(ITeamStructureRead<T> team, Action<T> action)
        {
            action(team.Vanguard);
            action(team.Attacker);
            action(team.Support);
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
