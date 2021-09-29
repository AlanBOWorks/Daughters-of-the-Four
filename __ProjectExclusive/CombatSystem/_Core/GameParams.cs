using UnityEngine;

namespace CombatSystem
{
    public static class GameParams
    {
        public const int DefaultMembersPerTeam = 3;
        public const int DefaultAmountOfTeams = 2;
        public const int DefaultMemberPerCombat = DefaultMembersPerTeam * DefaultAmountOfTeams;
    }
}
