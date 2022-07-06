using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Skills;
using CombatSystem.Team;

namespace CombatSystem.Player.Handlers
{
    public sealed class PlayerTeamStanceSwitcher : ICombatPreparationListener, ISkillUsageListener
    {
        private CombatTeam _team;
        public void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            _team = playerTeam;
        }
        public void DoSaveStance(EnumTeam.StanceFull switchedStance)
        {
            _selectedStance = switchedStance;
            PlayerCombatSingleton.PlayerCombatEvents.OnTeamStancePreviewSwitch(switchedStance);
        }

        private EnumTeam.StanceFull _selectedStance;

        public void OnCombatSkillSubmit(in SkillUsageValues values)
        {
            var teamValues = _team.DataValues;
            if(teamValues.CurrentControl < 1) return;

            _team.DataValues.CurrentStance = _selectedStance;
            CombatSystemSingleton.EventsHolder.OnStanceChange(_team, _selectedStance);
            teamValues.CurrentControl = 0;
        }

        public void OnCombatSkillPerform(in SkillUsageValues values)
        {
        }

        public void OnCombatSkillFinish(CombatEntity performer)
        {
        }
    }
}
