using UnityEngine;

namespace CombatTeam
{
    public class TeamStats
    {
        public float CompetingControl;
        public float BurstControl;
        public EnumTeam.TeamStance CurrentStance;

        public void DoResetBurst() => BurstControl = 0;
    }
}
