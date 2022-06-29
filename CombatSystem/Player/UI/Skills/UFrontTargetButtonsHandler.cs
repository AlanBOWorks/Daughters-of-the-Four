using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Skills;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UFrontTargetButtonsHandler : MonoBehaviour, ITeamElementSpawnListener<UUIHoverEntity>, 
        IPlayerEntityListener,ITempoControlStatesListener,
        ISkillSelectionListener, ISkillPointerListener,
        ITargetSelectionListener
    {
        [ShowInInspector,ReadOnly]
        private Dictionary<CombatEntity, UTargetButton> _buttonsDictionary;

        public IReadOnlyDictionary<CombatEntity, UTargetButton> GetDictionary() => _buttonsDictionary;

        private void Awake()
        {
            _buttonsDictionary = new Dictionary<CombatEntity, UTargetButton>();
            var playerEventsHolder = PlayerCombatSingleton.PlayerCombatEvents;

            playerEventsHolder.DiscriminationEventsHolder.Subscribe(this);

            // This is an invoker and not a listener of (ITargetSelectionListener && ITargetPointerListener), so
            // when subscribing as a normal listener will subscribe to those events as well and call
            // recursively those events; creating an infinite loop of calls
            // To solve it: manual subscription of ISKillSelection because this is its listening behaviour
            playerEventsHolder.ManualSubscribe(this as ISkillSelectionListener);
            playerEventsHolder.ManualSubscribe(this as IPlayerEntityListener);
            playerEventsHolder.ManualSubscribe(this as ISkillPointerListener);
        }

        public void OnAfterElementsCreated(UTeamElementSpawner<UUIHoverEntity> holder)
        {
        }

        public void OnElementCreated(UUIHoverEntity element, CombatEntity entity,
            int index)
        {
            var targetButton = element.GetTargetButton();
            _buttonsDictionary.Add(entity, targetButton);
            targetButton.Inject(entity);
            targetButton.Inject(this);
            targetButton.HideInstantly();
        }

        public void OnCombatEnd()
        {
        }

        [ShowInInspector, DisableInEditorMode]
        private CombatEntity _currentControl;

        [ShowInInspector, DisableInEditorMode] 
        private CombatSkill _currentSkill;


        public void OnPerformerSwitch(CombatEntity performer)
        {
            _currentControl = performer;
            _currentSkill = null;
        }


        public void OnSkillSelect(CombatSkill skill)
        { }

        public void OnSkillSelectFromNull(CombatSkill skill)
        { }

        public void OnSkillSwitch(CombatSkill skill, CombatSkill previousSelection)
        {
            _currentSkill = skill;
            HideTargets();
            ShowTargets(skill);
        }

        public void OnSkillDeselect(CombatSkill skill)
        {
            if (_currentSkill != skill) return;

            _currentSkill = null;
            HideTargets();
        }

        public void OnSkillCancel(CombatSkill skill)
        { }

        public void OnSkillSubmit(CombatSkill skill)
        { }

        private void ShowTargets(ICombatSkill skill)
        {
            var possibleTargets = UtilsTarget.GetPossibleTargets(skill, _currentControl);
            foreach (var target in possibleTargets)
            {
                var buttonHolder = _buttonsDictionary[target];
                buttonHolder.enabled = true;
                buttonHolder.ShowButton();
            }
        }

        private void HideTargets()
        {
            foreach (var button in _buttonsDictionary)
            {
                var buttonHolder = button.Value;
                buttonHolder.enabled = false;
                buttonHolder.Hide();
            }
            if (_hoverTarget != null)
                DoTargetButtonExit(_hoverTarget);
        }

        public void DoTargetSelect(CombatEntity target)
        {
            PlayerCombatSingleton.PlayerCombatEvents.
                OnTargetSelect(target);
        }

        private CombatEntity _hoverTarget;
        public void DoTargetButtonHover(CombatEntity target)
        {
            _hoverTarget = target;
            PlayerCombatSingleton.PlayerCombatEvents.
                OnTargetButtonHover(target);
        }

        public void DoTargetButtonExit(CombatEntity target)
        {
            if (_hoverTarget == target) _hoverTarget = null;

            PlayerCombatSingleton.PlayerCombatEvents.
                OnTargetButtonExit(target);
        }

        public void OnTargetSelect(CombatEntity target)
        {
        }

        public void OnTargetCancel(CombatEntity target)
        {
            HideTargets();
        }

        public void OnTargetSubmit(CombatEntity target)
        {
        }


        public void OnTempoStartControl(CombatTeamControllerBase controller, CombatEntity firstControl)
        {
            
        }

        public void OnAllActorsNoActions(CombatEntity lastActor)
        {
            HideTargets();
        }


        public void OnTempoFinishControl(CombatTeamControllerBase controller)
        {
            HideTargets();
        }


        public void OnSkillButtonHover(ICombatSkill skill)
        {
            if(_currentSkill != null)
                HideTargets();
            ShowTargets(skill);
        }

        public void OnSkillButtonExit(ICombatSkill skill)
        {
            HideTargets();
            if (_currentSkill != null)
                ShowTargets(_currentSkill);

        }

    }
}
