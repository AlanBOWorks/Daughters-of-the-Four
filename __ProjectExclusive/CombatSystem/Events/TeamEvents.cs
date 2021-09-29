using System.Collections.Generic;
using CombatEntity;
using CombatTeam;

namespace CombatSystem.Events
{
    public class TeamEvents : List<ITeamStateChangeListener>, ITeamStateChangeListener
    {
        public void OnStanceChange(EnumTeam.TeamStance switchStance)
        {
            foreach (var listener in this)
            {
                listener.OnStanceChange(switchStance);
            }
        }

        public void OnMemberDeath(CombatingEntity member)
        {
            foreach (var listener in this)
            {
                listener.OnMemberDeath(member);
            }
        }
    }
}
