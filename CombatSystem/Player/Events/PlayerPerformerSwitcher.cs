using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Player.Events
{
    public sealed class PlayerPerformerSwitcher : ICombatPreparationListener, 
        ITempoEntityStatesExtraListener, 
        ITempoTeamStatesListener,

        ISkillUsageListener

    {
        private IReadOnlyList<CombatEntity> _activeEntities;
        public void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            _activeEntities = playerTeam.GetControllingMembers();
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
        public void OnTempoPreStartControl(in CombatTeamControllerBase controller)
        {
            _isActive = true;
            var firstEntity = _activeEntities[0];
            OnPerformerSwitch(in firstEntity);
        }

        public void OnAllActorsNoActions(in CombatEntity lastActor)
        {
            ResetState();
        }

        public void OnControlFinishAllActors(in CombatEntity lastActor)
        {
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
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerEvents.OnPerformerSwitch(in performer);
        }

        public void OnAllPlayerEntitiesFinish()
        {

        }


        public void OnCombatSkillSubmit(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
            if(!_isActive) return;

            CombatEntity nextControl = null;
            if (_activeEntities.Count > 0) nextControl = _activeEntities[0];

            OnPerformerSwitch(in nextControl);
        }

        public void OnCombatSkillPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
        }

        public void OnCombatEffectPerform(in CombatEntity performer, in CombatEntity target, in PerformEffectValues values)
        {
        }

        public void OnCombatSkillFinish(in CombatEntity performer)
        {
        }
    }
}
