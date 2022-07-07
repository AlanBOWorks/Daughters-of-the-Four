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
        private EnumTeam.StanceFull _selectedStance;

        public void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            _team = playerTeam;
        }
        public void DoSaveStance(EnumTeam.StanceFull switchedStance)
        {
            if(switchedStance == _selectedStance) return;

            _selectedStance = switchedStance;
            PlayerCombatSingleton.PlayerCombatEvents.OnTeamStancePreviewSwitch(switchedStance);
        }


        public void OnCombatSkillSubmit(in SkillUsageValues values)
        {
            var teamValues = _team.DataValues;
            if(teamValues.CurrentControl < 1 || teamValues.CurrentStance == _selectedStance) return;

            CombatSystemSingleton.EventsHolder.OnStanceChange(_team, _selectedStance, true);
        }

        public void OnCombatSkillPerform(in SkillUsageValues values)
        {
        }

        public void OnCombatSkillFinish(CombatEntity performer)
        {
        }
    }
}
