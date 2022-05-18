using System.Collections.Generic;
using System.Linq;
using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem._Core
{
    internal sealed class TempoSequenceStepper 
    {
        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
            var remainingEntities = controller.ControllingTeam.GetControllingMembers();
            if (remainingEntities.Count == 0) return;

            OnTempoForceFinish(in controller, in remainingEntities);
        }

        public void OnTempoFinishLastCall(in CombatTeamControllerBase controller)
        {
            controller.ControllingTeam.ClearControllingMembers();
        }

        public void OnTempoForceFinish(in CombatTeamControllerBase controller,
            in IReadOnlyList<CombatEntity> remainingMembers)
        {
            var eventsHolder = CombatSystemSingleton.EventsHolder;

            var allActives = remainingMembers;
            const bool isForced = true;
            foreach (var entity in allActives)
            {
                UtilsCombatStats.FullTickActions(entity);
                eventsHolder.OnEntityEmptyActions(entity);
                eventsHolder.OnEntityFinishSequence(entity, isForced);
            }

            controller.ControllingTeam.OnTempoForceFinish();
        }
    }
}
