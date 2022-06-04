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

        private void DoSwitchPerformer(in CombatEntity entity)
        {
            if(entity == _currentEntity) return;
            _currentEntity = entity;
            PlayerCombatSingleton.PlayerCombatEvents.OnPerformerSwitch(in entity);

        }

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
            DoSwitchPerformer(in nextCall);
        }

        public void OnNoActionsForcedFinish(in CombatEntity entity)
        {
        }

        private bool _isActive;

        private void DoPerformNextEntity()
        {
            if (!_isActive) return;

            CombatEntity nextControl = null;
            if (_activeEntities.Count > 0) nextControl = _activeEntities[0];

            DoSwitchPerformer(in nextControl);
        }

        public void OnTempoPreStartControl(in CombatTeamControllerBase controller)
        {
            _isActive = true;
            var firstEntity = _activeEntities[0];
            DoSwitchPerformer(in firstEntity);
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

        public void OnEntityFinishSequence(CombatEntity entity, in bool isForcedByController)
        {
        }
    }
}
