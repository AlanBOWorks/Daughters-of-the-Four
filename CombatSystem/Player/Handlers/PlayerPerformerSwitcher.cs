using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;

namespace CombatSystem.Player.Handlers
{
    public sealed class PlayerPerformerSwitcher : 
        ICombatPreparationListener, 
        ITempoEntityMainStatesListener, ITempoEntityActionStatesListener ,
        ITempoControlStatesListener, ITempoControlStatesExtraListener


    {
        private IReadOnlyList<CombatEntity> _activeEntities;
        private void DoPerformNextEntity()
        {
            if (!_isActive) return;
            if (_activeEntities.Count == 0) return;


            CombatEntity nextControl = _activeEntities[0];
            DoPerformEntity(nextControl);
        }

        private static void DoPerformEntity(CombatEntity entity)
        {
            PlayerCombatSingleton.PlayerCombatEvents.OnPerformerSwitch(entity);
        }

        // ----- EVENTS -----
        public void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            _activeEntities = playerTeam.GetControllingMembers();
        }


        private bool _isActive;


        public void OnTempoStartControl(CombatTeamControllerBase controller, CombatEntity firstControl)
        {
            _isActive = true;
            DoPerformEntity(firstControl);
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

        public void OnTempoPreStartControl(CombatTeamControllerBase controller, CombatEntity firstEntity)
        {
            
        }

        public void OnTempoFinishLastCall(CombatTeamControllerBase controller)
        {
        }
    }
}
