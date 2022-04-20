using System;
using CombatSystem.Team;

namespace CombatSystem.Player.UI
{
    public class UUITeamBurstControlHandler : UTeamMonoDiscriminatorListener<UUITeamControlInfo>
    {
        protected override void OnCombatPrepares(in UUITeamControlInfo element, in CombatTeam team)
        {
            element.TeamData = team.DataValues;
            element.UpdateControl();
        }

        protected override void OnStanceChange(in UUITeamControlInfo element, in EnumTeam.StanceFull switchStance)
        {
        }

        protected override void OnControlChange(in UUITeamControlInfo element, in float phasedControl,
            in bool isBurst)
        {
            if(isBurst)
                element.UpdateControl();
        }
    }
}
