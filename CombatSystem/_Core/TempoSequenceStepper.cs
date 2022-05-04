using System.Collections.Generic;
using System.Linq;
using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;

namespace CombatSystem._Core
{
    internal sealed class TempoSequenceStepper : ITempoEntityStatesListener, ITempoEntityStatesExtraListener, ITempoTeamStatesListener
    {
        public void OnEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            if(!canAct)
            {
                eventsHolder.OnNoActionsForcedFinish(in entity);
                return;
            }
            eventsHolder.OnAfterEntityRequestSequence(in entity);

            eventsHolder.OnEntityRequestAction(entity);
        }

        public void OnEntityRequestAction(CombatEntity entity)
        {
        }

        /// <summary>
        /// Checks if the [<seealso cref="CombatEntity"/>] is finish
        /// /// </summary>
        public void OnEntityFinishAction(CombatEntity entity)
        {
            bool canAct = UtilsCombatStats.CanActRequest(entity);
            var eventHolder = CombatSystemSingleton.EventsHolder;
            if (!canAct)
                eventHolder.OnEntityFinishSequence(entity, false);
            else
                eventHolder.OnEntityRequestAction(entity);
        }

        /// <summary>
        /// Checks if the [<seealso cref="CombatEntity"/>] is trinity or OffMember
        /// </summary>
        public void OnEntityFinishSequence(CombatEntity entity,in bool isForcedByController)
        {
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            var isTrinityRole = UtilsTeam.IsTrinityRole(in entity);
            if (isTrinityRole)
                eventsHolder.OnTrinityEntityFinishSequence(entity);
            else
                eventsHolder.OnOffEntityFinishSequence(entity);

            eventsHolder.OnAfterEntitySequenceFinish(in entity);
        }

        public void OnAfterEntityRequestSequence(in CombatEntity entity)
        {
        }

        public void OnAfterEntitySequenceFinish(in CombatEntity entity)
        {
            var entityTeam = entity.Team;
            if(!entityTeam.IsControlActive) return;

            var activeMembers = entityTeam.GetActiveMembers();
            if (activeMembers.Count > 0) return;

            var eventsHolder = CombatSystemSingleton.EventsHolder;
            eventsHolder.OnControlFinishAllActors(in entity);
        }

        public void OnNoActionsForcedFinish(in CombatEntity entity)
        {
        }

        public void OnTempoStartControl(in CombatTeamControllerBase controller, in CombatEntity firstEntity)
        {
        }

        public void OnControlFinishAllActors(in CombatEntity lastActor)
        {
            
        }

        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
            var remainingEntities = controller.ControllingTeam.GetActiveMembers();
            if(remainingEntities.Count == 0) return;
            
            OnTempoForceFinish(in controller, in remainingEntities);
        }

        public void OnTempoForceFinish(in CombatTeamControllerBase controller,
            in IReadOnlyList<CombatEntity> remainingMembers)
        {
            var eventsHolder = CombatSystemSingleton.EventsHolder;

            var allActives = remainingMembers;
            const bool isForced = true;
            foreach (var entity in allActives)
            {
                eventsHolder.OnEntityFinishSequence(entity, isForced);
            }

            controller.ControllingTeam.OnTempoForceFinish(in controller,in remainingMembers);
        }
    }
}
