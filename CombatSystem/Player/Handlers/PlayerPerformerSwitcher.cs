using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Team;

namespace CombatSystem.Player.Handlers
{
    public sealed class PlayerPerformerSwitcher : ICombatPreparationListener, 
        ITempoEntityStatesListener,
        ITempoEntityStatesExtraListener, 
        ITempoTeamStatesListener,

        ISkillUsageListener

    {
        private IReadOnlyList<CombatEntity> _activeEntities;
        private CombatEntity _currentEntity;

        private void DoSwitchPerformer(CombatEntity entity)
        {
            if(entity == _currentEntity) return;
           DoSwitchPerformerDirect(entity);
        }

        private void DoSwitchPerformerDirect(CombatEntity entity)
        {
            _currentEntity = entity;
            PlayerCombatSingleton.PlayerCombatEvents.OnPerformerSwitch(entity);
        }
        private void DoPerformNextEntity()
        {
            if (!_isActive) return;
            if (_activeEntities.Count == 0) return;


            CombatEntity nextControl = _activeEntities[0];
            DoSwitchPerformer(nextControl);
        }


        // ----- EVENTS -----
        public void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            _activeEntities = playerTeam.GetControllingMembers();
        }

        public void OnAfterEntityRequestSequence(CombatEntity entity)
        {
            if(_currentEntity == null)
                DoPerformNextEntity();
        }

        public void OnAfterEntitySequenceFinish(CombatEntity entity)
        {
            DoPerformNextEntity();
        }

        public void OnNoActionsForcedFinish(CombatEntity entity)
        {
        }

        private bool _isActive;


        public void OnTempoPreStartControl(CombatTeamControllerBase controller)
        {
            _isActive = true;
        }

        public void OnAllActorsNoActions(CombatEntity lastActor)
        {
        }

        public void OnControlFinishAllActors(CombatEntity lastActor)
        {
        }
        public void OnTempoFinishControl(CombatTeamControllerBase controller)
        {
            ResetState();
        }

        public void OnTempoFinishLastCall(CombatTeamControllerBase controller)
        {
        }

        private void ResetState()
        {
            _isActive = false;
            _currentEntity = null;
        }


        public void OnCombatSkillSubmit(in SkillUsageValues values)
        {
            DoPerformNextEntity();
        }

        public void OnCombatSkillPerform(in SkillUsageValues values)
        {
        }

        public void OnCombatSkillFinish(CombatEntity performer)
        {
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
            if(entity == _currentEntity)
                DoPerformNextEntity();
        }

        public void OnEntityFinishSequence(CombatEntity entity, bool isForcedByController)
        {
        }
    }
}
