using CombatSystem.Team;

namespace CombatSystem.Skills
{
    public static class EnumsSkill 
    {
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
