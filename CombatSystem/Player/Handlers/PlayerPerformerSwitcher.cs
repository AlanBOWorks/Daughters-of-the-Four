using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Stats;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Player.Handlers
{
    public sealed class PlayerPerformerSwitcher : 
        ICombatPreparationListener, 
        ITempoEntityMainStatesListener, ITempoEntityActionStatesListener ,
        ITempoControlStatesListener, ITempoControlStatesExtraListener
    {
        private IReadOnlyList<CombatEntity> _allEntities;
        private IReadOnlyList<CombatEntity> _activeEntities;
        private CombatEntity _lastPerformer;

        private int _currentPerformerIndex;
        public void DoPerformNextEntity()
        {
            CombatEntity nextControl = (_isActive) 
                ? GetNextActiveEntity()
                : GetNextInactiveEntity();


            if(nextControl == _lastPerformer) return;
            
            DoPerformEntityEvent(nextControl);

            CombatEntity GetNextActiveEntity()
            {
                int entitiesCount = _allEntities.Count;
                for (int i = 0; i < entitiesCount -1; i++) // -1 because we gonna ignore current;
                {
                    StepIndexIndex();
                    var nextEntity = _allEntities[_currentPerformerIndex];
                    if (nextEntity.IsActive()) return nextEntity;
                }

                return _lastPerformer;
            }

            CombatEntity GetNextInactiveEntity()
            {
                StepIndexIndex();
                return _allEntities[_currentPerformerIndex];
            }

            void StepIndexIndex()
            {
                _currentPerformerIndex++;
                if (_currentPerformerIndex >= _allEntities.Count) _currentPerformerIndex = 0;
            }
        }

        private void DoPerformEntityEvent(CombatEntity entity)
        {
            PlayerCombatSingleton.PlayerCombatEvents.OnPerformerSwitch(entity);
            _lastPerformer = entity;
        }

        public void DoPerformEntityWithIndex(CombatEntity entity)
        {
            DoPerformEntityEvent(entity);
            UpdatePerformerIndex(entity);
        }

        private void UpdatePerformerIndex(CombatEntity entity)
        {
            for (var i = 0; i < _allEntities.Count; i++)
            {
                var checkEntity = _allEntities[i];
                if (checkEntity != entity) continue;

                _currentPerformerIndex = i;
                return;
            }
        }


        // ----- EVENTS -----
        public void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            _activeEntities = playerTeam.GetControllingMembers();
            _allEntities = playerTeam.GetAllMembers();
            _currentPerformerIndex = 0;
        }


        private bool _isActive;
        public void OnTempoStartControl(CombatTeamControllerBase controller, CombatEntity firstControl)
        {
            _isActive = true;
            if(_lastPerformer != null && _lastPerformer.IsActive())
            {
                DoPerformEntityEvent(_lastPerformer);
                return;
            }

            DoPerformEntityWithIndex(firstControl);
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
            if(!_isActive) return;

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
