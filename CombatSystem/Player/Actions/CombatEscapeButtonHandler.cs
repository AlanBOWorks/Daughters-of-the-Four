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
        ICombatStatesListener, ITempoTeamStatesListener,
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
            ResetState();
        }

        public void OnCombatStart()
        {
        }
        public void OnTempoPreStartControl(in CombatTeamControllerBase controller)
        {
        }

        public void OnAllActorsNoActions(in CombatEntity lastActor)
        {
        }

        public void OnControlFinishAllActors(in CombatEntity lastActor)
        {
        }

        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
            ClearStack();
        }

        public void OnTempoFinishLastCall(in CombatTeamControllerBase controller)
        {
        }
      

        public void OnSkillSelect(in CombatSkill skill)
        {
        }

        public void OnSkillSelectFromNull(in CombatSkill skill)
        {
        }

        public void OnSkillSwitch(in CombatSkill skill, in CombatSkill previousSelection)
        {
        }

        public void OnSkillDeselect(in CombatSkill skill)
        {
        }

        public void OnSkillCancel(in CombatSkill skill)
        {
            ClearStack();
        }

        public void OnSkillSubmit(in CombatSkill skill)
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
