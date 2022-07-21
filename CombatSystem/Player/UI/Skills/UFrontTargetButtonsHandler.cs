using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Skills;
using CombatSystem.Team;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UFrontTargetButtonsHandler : MonoBehaviour, ITeamElementSpawnListener<UUIHoverEntityHolder>, 
        IPlayerCombatEventListener,ITempoControlStatesListener,
        ISkillSelectionListener, ISkillPointerListener,
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
            playerEventsHolder.SubscribeAsPlayerEvent(this);
        }

        private void DisableTargetHandling()
        {
            HideTargets();
            _currentSkill = null;
            _currentControl = null;
        }


        public void OnElementCreated(in UTeamElementSpawner<UUIHoverEntityHolder>.CreationValues creationValues)
        {
            var entity = creationValues.Entity;
            var element = creationValues.Element;
            var targetButton = element.GetTargetButton();

            _buttonsDictionary.Add(entity, targetButton);
            targetButton.Inject(entity);
            targetButton.Inject(this);
            targetButton.HideInstantly();
        }

        public void OnAfterElementsCreated(UTeamElementSpawner<UUIHoverEntityHolder> holder)
        {
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

        public void OnTeamStancePreviewSwitch(EnumTeam.StanceFull targetStance)
        {
            
        }


        public void OnSkillSelect(CombatSkill skill)
        { }

        public void OnSkillSelectFromNull(CombatSkill skill)
        { }

        private CoroutineHandle _switchHandle;
        public void OnSkillSwitch(CombatSkill skill, CombatSkill previousSelection)
        {
            _currentSkill = skill;

            Timing.KillCoroutines(_switchHandle);
            _switchHandle = Timing.RunCoroutine(_SwitchSkill());
            IEnumerator<float> _SwitchSkill()
            {
                HideTargets();
                yield return Timing.WaitForOneFrame;
                ShowTargets(skill);
            }
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
                PlayerCombatSingleton.PlayerCombatEvents.OnTargetButtonExit(_hoverTarget);
        }

        public void DoTargetSelect(CombatEntity target)
        {
            PlayerCombatSingleton.PlayerCombatEvents.
                OnTargetSelect(target);
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
            _currentControl = firstControl;
        }

        public void OnAllActorsNoActions(CombatEntity lastActor)
        {
            DisableTargetHandling();
        }
        public void OnTempoFinishControl(CombatTeamControllerBase controller)
        {
            DisableTargetHandling();
        }


        public void OnSkillButtonHover(ICombatSkill skill)
        {
            if(_currentControl == null) return;

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

        private CombatEntity _hoverTarget;
        public void OnTargetButtonHover(CombatEntity target)
        {
            _hoverTarget = target;
        }

        public void OnTargetButtonExit(CombatEntity target)
        {
            if (_hoverTarget == target) _hoverTarget = null;
        }
    }
}
