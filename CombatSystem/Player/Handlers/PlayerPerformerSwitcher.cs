using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Team;

namespace CombatSystem.Player.Handlers
{
    public sealed class PlayerPerformerSwitcher : 
        ICombatPreparationListener, 
        ITempoEntityStatesListener, 
        ITempoTeamStatesListener, ITempoTeamStatesExtraListener


    {
        private IReadOnlyList<CombatEntity> _activeEntities;
        private void DoPerformNextEntity()
        {
            if (!_isActive) return;
            if (_activeEntities.Count == 0) return;


            CombatEntity nextControl = _activeEntities[0];
            PlayerCombatSingleton.PlayerCombatEvents.OnPerformerSwitch(nextControl);
        }


        // ----- EVENTS -----
        public void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            _activeEntities = playerTeam.GetControllingMembers();
        }


        private bool _isActive;


        public void OnTempoStartControl(CombatTeamControllerBase controller)
        {
            
        }

        public void OnAllActorsNoActions(CombatEntity lastActor)
        {
            _isActive = false;
        }

        public void OnTempoFinishControl(CombatTeamControllerBase controller)
        {
            _isActive = false;
        }

        public void OnEntityRequestSequence(CombatEntity entity, bool canControl)
        {
        }

        public void OnEntityRequestAction(CombatEntity entity)
        {
        }

        public void OnEntityBeforeSkill(CombatEntity entity)
        {
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
        }

        public void OnEntityEmptyActions(CombatEntity entity)
        {
            DoPerformNextEntity();
        }

        public void OnEntityFinishSequence(CombatEntity entity, bool isForcedByController)
        {
        }

        public void OnTempoPreStartControl(CombatTeamControllerBase controller)
        {
            _isActive = true;
            DoPerformNextEntity();
        }

        public void OnTempoFinishLastCall(CombatTeamControllerBase controller)
        {
        }
    }
}
