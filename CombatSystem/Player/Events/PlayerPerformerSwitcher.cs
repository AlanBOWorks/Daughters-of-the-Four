using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Player.Events
{
    public sealed class PlayerPerformerSwitcher : ICombatPreparationListener, 
        ITempoEntityStatesExtraListener
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
            if (_activeEntities.Count <= 0) return;

            var nextCall = _activeEntities[0];
            PlayerCombatSingleton.PlayerCombatEvents.OnPerformerSwitch(in nextCall);
        }
    }
}
