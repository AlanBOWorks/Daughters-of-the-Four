using System.Collections.Generic;
using System.Linq;
using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem._Core
{
    internal sealed class CombatSubEventsSequence 
    {
        public CombatSubEventsSequence(SystemCombatEventsHolder eventsHolder)
        {
            _eventsHolder = eventsHolder;
        }
        private readonly SystemCombatEventsHolder _eventsHolder;

        public void OnTempoPreStartControl(in CombatTeamControllerBase controller)
        {
            var team = controller.ControllingTeam;
            team.OnControlStart();
        }

        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
            var remainingEntities = controller.ControllingTeam.GetControllingMembers();
            if (remainingEntities.Count == 0) return;

            OnTempoForceFinish(in controller, in remainingEntities);
        }

        public void OnTempoFinishLastCall(in CombatTeamControllerBase controller)
        {
            var controllingTeam = controller.ControllingTeam;
            controllingTeam.OnControlFinnish();

        }

        public void OnTempoForceFinish(in CombatTeamControllerBase controller,
            in IReadOnlyList<CombatEntity> remainingMembers)
        {

            var allActives = remainingMembers;
            const bool isForced = true;
            foreach (var entity in allActives)
            {
                UtilsCombatStats.FullTickActions(entity);
                _eventsHolder.OnEntityEmptyActions(entity);
                _eventsHolder.OnEntityFinishSequence(entity, isForced);
            }

            controller.ControllingTeam.OnTempoForceFinish();
        }

        public void OnStanceChance(CombatTeam team, EnumTeam.StanceFull targetStance, bool isControlChange)
        {
            var teamValues = team.DataValues;
            teamValues.CurrentStance = targetStance;

            if(!isControlChange) return;

            float currentControl = teamValues.CurrentControl;
            teamValues.CurrentControl = 0;
            _eventsHolder.OnControlChange(team, -currentControl);
        }
    }
}
