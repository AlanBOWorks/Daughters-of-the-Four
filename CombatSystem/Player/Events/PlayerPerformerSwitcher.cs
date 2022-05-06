using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Player.Events
{
    public sealed class PlayerPerformerSwitcher : ICombatPreparationListener, 
        ITempoEntityStatesExtraListener, 
        ITempoTeamStatesListener,

        IPlayerEntityListener
    {
        private IReadOnlyList<CombatEntity> _activeEntities;
        public void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            _activeEntities = playerTeam.GetActiveMembers();
        }

        public void OnAfterEntityRequestSequence(in CombatEntity entity)
        {
        }

        public void OnAfterEntitySequenceFinish(in CombatEntity entity)
        {
            if (!_isActive || _activeEntities.Count <= 0) return;

            var nextCall = _activeEntities[0];
            PlayerCombatSingleton.PlayerCombatEvents.OnPerformerSwitch(in nextCall);
        }

        public void OnNoActionsForcedFinish(in CombatEntity entity)
        {
        }

        private bool _isActive;
        public void OnTempoStartControl(in CombatTeamControllerBase controller, in CombatEntity firstEntity)
        {
            _isActive = true;
        }

        public void OnControlFinishAllActors(in CombatEntity lastActor)
        {
            ResetState();
        }
        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
            ResetState();
        }

        public void OnTempoFinishLastCall(in CombatTeamControllerBase controller)
        {
        }

        private void ResetState()
        {
            _isActive = false;
        }

        public void OnPerformerSwitch(in CombatEntity performer)
        {
            if(!_isActive) return;

            PlayerCombatSingleton.PlayerCombatEvents.OnPerformerSwitch(in performer);
        }
    }
}
