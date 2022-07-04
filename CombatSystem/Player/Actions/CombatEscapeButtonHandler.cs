using System.Collections;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Skills;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;


namespace CombatSystem.Player
{
    
    internal sealed class CombatEscapeButtonHandler : 
        IEscapeButtonHandler,
        ICombatStatesListener, 
        ITempoControlStatesListener,
        ISkillSelectionListener
    {
        public CombatEscapeButtonHandler()
        {
            _onTopPauseElements = new Stack<IOverridePauseElement>();
        }
        [Title("Events")]
        [ShowInInspector]
        private IOverridePauseElement _lastOverridingElement;
        [ShowInInspector]
        private readonly Stack<IOverridePauseElement> _onTopPauseElements;
        [Title("Data")]
        [ShowInInspector]
        private bool _isPauseActive;

        public IOverridePauseElement GetLastOverridePauseElement() => _lastOverridingElement;
        public IReadOnlyCollection<IOverridePauseElement> GetOverridePauseElements() => _onTopPauseElements;

        public bool IsInPause() => _isPauseActive;


        private void OnCombatPause()
        {
            _isPauseActive = true;
            PlayerCombatSingleton.PlayerCombatEvents.OnCombatPause();
        }

        private void OnCombatResume()
        {
            _isPauseActive = false;
            PlayerCombatSingleton.PlayerCombatEvents.OnCombatResume();
        }


        private void ResetState()
        {
            _isPauseActive = false;
            _onTopPauseElements.Clear();
        }

        public void PushOverridingAction(IOverridePauseElement listener)
        {
            _onTopPauseElements.Push(listener);
        }

        public void ClearStack(IOverridePauseElement lastElementCheck)
        {
            if(_onTopPauseElements.Count == 0) return;
            if (_onTopPauseElements.Peek() != lastElementCheck) return;
            ClearStack();
        }

        public void RemoveIfLast(IOverridePauseElement lastElementCheck)
        {
            if(_onTopPauseElements.Count == 0) return;

            var lastElement = _onTopPauseElements.Peek();
            bool shouldRemove = lastElement == lastElementCheck;
            if(shouldRemove) 
                _onTopPauseElements.Pop();
        }

        private void ClearStack()
        {
            _onTopPauseElements.Clear();
        }

        public void InvokeEscapeButtonAction()
        {
            if (_onTopPauseElements.Count > 0)
            {
                HandleStack();
                return;
            }


            _lastOverridingElement = null;

            if (_isPauseActive)
                OnCombatResume();
            else
                OnCombatPause();
        }
        private void HandleStack()
        {
            var overridingPauseAction = _onTopPauseElements.Pop();
            overridingPauseAction.OnPauseInputReturnState(_lastOverridingElement);

            _lastOverridingElement = overridingPauseAction;
        }


        public void OnCombatEnd()
        {
            ResetState();
        }

        public void OnCombatFinish(bool isPlayerWin)
        {
        }

        public void OnCombatQuit()
        {
        }

        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
        }

        public void OnCombatStart()
        {
        }
      
        public void OnTempoStartControl(CombatTeamControllerBase controller, CombatEntity firstControl)
        {
            ResetState();
        }

        public void OnAllActorsNoActions(CombatEntity lastActor)
        {
        }

        public void OnTempoFinishControl(CombatTeamControllerBase controller)
        {
            ClearStack();
        }

      

        public void OnSkillSelect(CombatSkill skill)
        {
        }

        public void OnSkillSelectFromNull(CombatSkill skill)
        {
        }

        public void OnSkillSwitch(CombatSkill skill, CombatSkill previousSelection)
        {
        }

        public void OnSkillDeselect(CombatSkill skill)
        {
        }

        public void OnSkillCancel(CombatSkill skill)
        {
            ClearStack();
        }

        public void OnSkillSubmit(CombatSkill skill)
        {
            ClearStack();
        }
    }

    public interface IEscapeButtonHandler
    {
        void PushOverridingAction(IOverridePauseElement listener);
        void ClearStack(IOverridePauseElement lastElementCheck);
        void RemoveIfLast(IOverridePauseElement lastElementCheck);
    }

    public interface IOverridePauseElement
    {
        void OnPauseInputReturnState(IOverridePauseElement lastElement);
    }
}
