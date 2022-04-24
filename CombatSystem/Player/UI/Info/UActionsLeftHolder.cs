using System;
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
        [SerializeField] private TextMeshProUGUI actionsUsedText;
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

        private void SwitchUser(in CombatEntity entity)
        {
            _currentEntity = entity;
            UpdateInfoToCurrent();
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
            actionsLimitText.text = actionsLimitValue.ToString("00");
        }


        private void UpdateUsedActionsText()
        {
            var usedActions = _currentEntity.Stats.UsedActions;
            actionsUsedText.text = usedActions.ToString("00");
        }

        private void ToggleActiveToolTip(bool enabledGO)
        {
            var parent = actionsTooltipText.transform.parent;
            parent.gameObject.SetActive(enabledGO);
        }

        private void UpdateActionsToolTip(in CombatSkill skill)
        {
            int cost = skill.SkillCost;
            string costText;
            if (cost > 9)
                costText = cost.ToString();
            else
                costText = "+" + cost;

            actionsTooltipText.text = costText;
        }

        private CombatSkill _selectedSkill;

        public void OnSkillSelect(in CombatSkill skill)
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

        public void OnPerformerSwitch(in CombatEntity performer)
        {
            SwitchUser(in performer);
        }

        public void OnTempoStartControl(in CombatTeamControllerBase controller,in CombatEntity firstEntity)
        {
            SwitchUser(in firstEntity);
            ShowUI();
        }

        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
            _currentEntity = null;
            HideUI();
        }

        public void OnEntityRequestSequence(CombatEntity entity, bool canAct)
        {
        }

        public void OnEntityRequestAction(CombatEntity entity)
        {

        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
            UpdateInfoToCurrent();
        }

        public void OnEntityFinishSequence(CombatEntity entity)
        {
            UpdateInfoToCurrent();
        }
    }
}
