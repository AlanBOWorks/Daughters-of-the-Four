using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;

namespace CombatSystem._Core
{
    internal sealed class TempoSequenceStepper : ITempoEntityStatesListener, ITempoEntityStatesExtraListener
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
                eventHolder.OnEntityFinishSequence(entity);
            else
                eventHolder.OnEntityRequestAction(entity);
        }

        /// <summary>
        /// Checks if the [<seealso cref="CombatEntity"/>] is trinity or OffMember
        /// </summary>
        public void OnEntityFinishSequence(CombatEntity entity)
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
    }
}
