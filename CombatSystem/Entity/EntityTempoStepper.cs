using System.Collections.Generic;
using System.Linq;
using CombatSystem._Core;
using CombatSystem.Stats;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Entity
{
    public sealed class EntityTempoStepper : ITempoTeamStatesListener,
        ITempoEntityStatesListener,ITempoDedicatedEntityStatesListener,
        ITempoEntityStatesExtraListener
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

        public void OnEntityFinishSequence(CombatEntity entity,in bool isForcedByController)
        {
            entity.Stats.CurrentInitiative = 0;
            entity.Team.OnEntityFinishSequence(entity,in isForcedByController);
        }

        public void OnAfterEntityRequestSequence(in CombatEntity entity)
        {
        }

        public void OnAfterEntitySequenceFinish(in CombatEntity entity)
        {
        }

        public void OnNoActionsForcedFinish(in CombatEntity entity)
        {
            OnEntityFinishSequence(entity, false);
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

        public void OnTempoStartControl(in CombatTeamControllerBase controller, in CombatEntity firstEntity)
        {
            firstEntity.Team.OnTempoStartControl(in controller,in firstEntity);
        }

        public void OnControlFinishAllActors(in CombatEntity lastActor)
        {
            lastActor.Team.OnControlFinishAllActors(in lastActor);
        }

        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
            
        }

        public void OnTempoFinishLastCall(in CombatTeamControllerBase controller)
        {
            
        }

        public void OnTempoForceFinish(in CombatTeamControllerBase controller,
            in IReadOnlyList<CombatEntity> remainingMembers)
        {
        }
    }
}
