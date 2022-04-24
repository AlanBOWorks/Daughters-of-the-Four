using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;

namespace CombatSystem._Core
{
    internal sealed class TempoSequenceStepper : ITempoEntityStatesListener
    {
        public void OnEntityRequestSequence(CombatEntity entity, bool canAct)
        {

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
            if (!canAct)
                CombatSystemSingleton.EventsHolder.OnEntityFinishSequence(entity);
        }

        /// <summary>
        /// Checks if the [<seealso cref="CombatEntity"/>] is trinity or OffMember
        /// </summary>
        public void OnEntityFinishSequence(CombatEntity entity)
        {
            var isTrinityRole = UtilsTeam.IsTrinityRole(in entity);
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            if (isTrinityRole)
                eventsHolder.OnTrinityEntityFinishSequence(entity);
            else
                eventsHolder.OnOffEntityFinishSequence(entity);

        }
    }
}
