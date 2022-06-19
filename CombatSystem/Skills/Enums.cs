using CombatSystem.Team;

namespace CombatSystem.Skills
{
    public static class EnumsSkill 
    {
        public enum TargetType
        {
            Direct,
            Area,
            /// <summary>
            /// For possibles targets it will behave as a [<see cref="Direct"/>] type, but on DoEffects
            /// will behave as an [<see cref="Area"/>] type (these could affect one or various targets at once)
            /// </summary>
            Hybrid
        }

        public enum TeamTargeting
        {
            Self,
            Offensive,
            Support
        }

        public enum RoleArchetype
        {
            Flex = EnumTeam.FlexIndex,
            Vanguard = EnumTeam.VanguardIndex,
            Attacker = EnumTeam.AttackerIndex,
            Support = EnumTeam.SupportIndex
        }
    }

}
