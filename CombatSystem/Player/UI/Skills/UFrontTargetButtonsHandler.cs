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
    public class UFrontTargetButtonsHandler : MonoBehaviour, IUIHoverListener, 
        IPlayerEntityListener,ITempoTeamStatesListener,
        ISkillSelectionListener,
        ITargetSelectionListener, ITargetPointerListener
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
        }

        public void OnElementCreated(in UUIHoverEntityHolder element, in CombatEntity entity)
        {
            var targetButton = element.GetTargetButton();
            _buttonsDictionary.Add(entity,targetButton);
            targetButton.Inject(entity);
            targetButton.Inject(this);
            targetButton.HideInstantly();
        }

        public void ClearEntities()
        {
            _buttonsDictionary.Clear();
        }

        [ShowInInspector, DisableInEditorMode]
        private CombatEntity _currentControl;


        public void OnPerformerSwitch(in CombatEntity performer)
        {
            _currentControl = performer;
        }


        public void OnSkillSelect(in CombatSkill skill)
        {

        }
        public void OnSkillSwitch(in CombatSkill skill,in CombatSkill previousSelection)
        {
            HideTargets();
            ShowTargets(in skill);
        }

        public void OnSkillDeselect(in CombatSkill skill)
        {
            HideTargets();
        }

        public void OnSkillCancel(in CombatSkill skill)
        {
            
        }

        public void OnSkillSubmit(in CombatSkill skill)
        {
            
        }

        private void ShowTargets(in CombatSkill skill)
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
        }

        public void OnTargetSelect(in CombatEntity target)
        {
            PlayerCombatSingleton.PlayerCombatEvents.
                OnTargetSelect(in target);
            HideTargets();
        }

        public void OnTargetCancel(in CombatEntity target)
        {
            
        }

        public void OnTargetSubmit(in CombatEntity target)
        {
            _currentControl = null;
        }

        public void OnTargetButtonHover(in CombatEntity target)
        {
            PlayerCombatSingleton.PlayerCombatEvents.
               OnTargetButtonHover(in target);
        }

        public void OnTargetButtonExit(in CombatEntity target)
        {
            PlayerCombatSingleton.PlayerCombatEvents.
                OnTargetButtonExit(in target);
        }

        public void OnTempoPreStartControl(in CombatTeamControllerBase controller)
        {
        }

        public void OnControlFinishAllActors(in CombatEntity lastActor)
        {
            HideTargets();
        }

        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
            HideTargets();
        }

        public void OnTempoFinishLastCall(in CombatTeamControllerBase controller)
        {
            
        }
    }
}
