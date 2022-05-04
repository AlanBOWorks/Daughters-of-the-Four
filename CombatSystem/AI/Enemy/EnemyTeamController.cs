using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.AI
{
    public class EnemyTeamControllerRandom : CombatTeamControllerBase, ITempoTeamStatesListener,
        ITempoEntityStatesExtraListener
    {
        private IReadOnlyList<CombatEntity> _activeEntities;
        public void OnTempoStartControl(in CombatTeamControllerBase controller, in CombatEntity firstEntity)
        {
            _activeEntities = controller.GetAllActiveMembers();
            HandleControl(in firstEntity);
        }

        public void OnControlFinishAllActors(in CombatEntity lastActor)
        {
        }

        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
        }

        public void OnTempoForceFinish(in CombatTeamControllerBase controller,
            in IReadOnlyList<CombatEntity> remainingMembers)
        {
            
        }

        public void OnAfterEntityRequestSequence(in CombatEntity entity)
        {
        }
        public void OnAfterEntitySequenceFinish(in CombatEntity entity)
        {
            if (_activeEntities.Count > 0) return;

            var nextEntity = _activeEntities[0];
            HandleControl(in nextEntity);
        }

        public void OnNoActionsForcedFinish(in CombatEntity entity)
        {
        }


        private void HandleControl(in CombatEntity onEntity)
        {
            var entitySkills = onEntity.GetCurrentSkills();
        }

        private void HandleEntity(in CombatEntity entity)
        {

        }
    }
}
