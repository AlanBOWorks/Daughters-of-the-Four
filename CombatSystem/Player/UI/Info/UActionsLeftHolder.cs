using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Skills;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UActionsLeftHolder : MonoBehaviour, ITempoEntityStatesListener,
        IPlayerEntityListener,
        ITempoTeamStatesListener,
        ISkillSelectionListener, ISkillPointerListener
    {
        [SerializeField] private TextMeshProUGUI actionsLimitText;
        [SerializeField] private TextMeshProUGUI actionsLimitTextBackground;
        [SerializeField] private TextMeshProUGUI actionsUsedFirstDigit;
        [SerializeField] private TextMeshProUGUI actionsUsedSecondDigit;
        [SerializeField] private TextMeshProUGUI actionsTooltipText;

        [ShowInInspector]
        private CombatEntity _currentEntity;
        private void Awake()
        {
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerEvents.SubscribeAsPlayerEvent(this);
            playerEvents.DiscriminationEventsHolder.Subscribe(this);
            HideUIInstant();
        }

        private void ShowUI()
        {
            // todo animate
            ToggleActiveToolTip(false);
            gameObject.SetActive(true);
        }
        private void HideUI()
        {
            // todo animate
            gameObject.SetActive(false);
        }
        private void HideUIInstant()
        {
            gameObject.SetActive(false);
        }


        private void UpdateInfoToCurrent()
        {
            if(_currentEntity == null) return;

            UpdateLimitText();
            UpdateUsedActionsText();
        }

        private void UpdateLimitText()
        {
            var actionsLimitValue = UtilsStatsFormula.CalculateActionsAmount(_currentEntity.Stats);
            string limitText = actionsLimitValue.ToString("00");
            actionsLimitText.text = limitText;
            actionsLimitTextBackground.text = limitText;
        }

        private float _usedActions;
        private void UpdateUsedActionsText()
        {
            _usedActions = _currentEntity.Stats.UsedActions;
            string fullText = _usedActions.ToString("00");
            string firstDigit = fullText[^1..];
            string secondDigit = fullText.Substring(0,1);

            actionsUsedFirstDigit.text = firstDigit;
            actionsUsedSecondDigit.text = secondDigit;
        }

        private void ToggleActiveToolTip(bool enabledGO)
        {
            var parent = actionsTooltipText.transform.parent;
            parent.gameObject.SetActive(enabledGO);
        }

        private const string OverflowText = "XX";
        private void UpdateActionsToolTip(in CombatSkill skill)
        {
            float cost = skill.SkillCost + _usedActions;
            var costText = cost > 99 
                ? OverflowText 
                : cost.ToString("00");

            actionsTooltipText.text = costText;
        } public void OnPerformerSwitch(in CombatEntity performer)
        {
            _currentEntity = performer;
            UpdateInfoToCurrent();
        }

        private CombatSkill _selectedSkill;



        public void OnSkillSelect(in CombatSkill skill)
        {

        }

        public void OnSkillSelectFromNull(in CombatSkill skill)
        {
        }

        public void OnSkillSwitch(in CombatSkill skill,in CombatSkill previousSelection)
        {
            _selectedSkill = skill;
            UpdateActionsToolTip(in skill);
            ToggleActiveToolTip(true);
        }

        public void OnSkillDeselect(in CombatSkill skill)
        {
            _selectedSkill = null;
            ToggleActiveToolTip(false);
        }

        public void OnSkillCancel(in CombatSkill skill)
        {
            //todo reset all to the current values of the entity as fresh
        }

        public void OnSkillSubmit(in CombatSkill skill)
        {
            _selectedSkill = null;
            ToggleActiveToolTip(false);

            if(_currentEntity == null) return;
            UpdateUsedActionsText();
        }

        public void OnSkillButtonHover(in CombatSkill skill)
        {
            if(_selectedSkill != null) return;
            UpdateActionsToolTip(in skill);
            ToggleActiveToolTip(true);
        }

        public void OnSkillButtonExit(in CombatSkill skill)
        {
            if(_selectedSkill != null) return;
            ToggleActiveToolTip(false);
        }


        public void OnTempoPreStartControl(in CombatTeamControllerBase controller)
        {
            ShowUI();
        }

        public void OnAllActorsNoActions(in CombatEntity lastActor)
        {
            _currentEntity = null;
        }

        public void OnControlFinishAllActors(in CombatEntity lastActor)
        {
           
        }

        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
            HideUI();
        }

        public void OnTempoFinishLastCall(in CombatTeamControllerBase controller)
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
            UpdateInfoToCurrent();
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
        }

        public void OnEntityEmptyActions(CombatEntity entity)
        {
        }

        public void OnEntityFinishSequence(CombatEntity entity, in bool isForcedByController)
        {
            if(isForcedByController) return;

            UpdateInfoToCurrent();
        }
    }
}
