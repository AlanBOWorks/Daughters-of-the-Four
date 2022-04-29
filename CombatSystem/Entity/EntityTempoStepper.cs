using CombatSystem._Core;
using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Entity
{
    public sealed class EntityTempoStepper : ITempoEntityStatesListener,ITempoDedicatedEntityStatesListener
    {
        public void OnEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            var entityTeam = entity.Team;
            entityTeam.OnEntityRequestSequence(entity, canAct);

            if (!canAct) return;

            var stats = entity.Stats;
            stats.UsedActions = 0;
        }

        public void OnEntityRequestAction(CombatEntity entity)
        {
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
            
        }

        public void OnEntityFinishSequence(CombatEntity entity)
        {
            entity.Stats.CurrentInitiative = 0;
        }



        public void OnTrinityEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            var entityTeam = entity.Team;
            entityTeam.OnTrinityEntityRequestSequence(entity,canAct);
        }

        public void OnOffEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            entity.Team.OnOffEntityRequestSequence(entity,canAct);
        }



        public void OnTrinityEntityFinishSequence(CombatEntity entity)
        {
            entity.Team.OnTrinityEntityFinishSequence(entity);
        }

        public void OnOffEntityFinishSequence(CombatEntity entity)
        {
            entity.Team.OnOffEntityFinishSequence(entity);
        }
    }
}
