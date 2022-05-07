using System.Collections.Generic;
using System.Linq;
using CombatSystem._Core;
using CombatSystem.Stats;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Entity
{
    public sealed class EntityTempoStepper :
        ITempoEntityStatesListener, ITempoEntityStatesExtraListener
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
    }
}
