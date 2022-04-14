using System;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Skills;
using CombatSystem.Stats;
using TMPro;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UActionsLeftHolder : MonoBehaviour, ITempoEntityStatesListener,
        ISkillSelectionListener, ISkillPointerListener
    {
        [SerializeField] private TextMeshProUGUI actionsLimitText;
        [SerializeField] private TextMeshProUGUI actionsUsedText;
        [SerializeField] private TextMeshProUGUI actionsTooltipText;

        private CombatEntity _currentEntity;
        private float _actionsLimitValue;
        private int _virtualUsedActionsValue;
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

        private void UpdateLimitText(in CombatEntity entity)
        {
            _actionsLimitValue = UtilsStatsFormula.CalculateActionsAmount(entity.Stats);
            actionsLimitText.text = _actionsLimitValue.ToString("00");
        }

        private void VariateUsedActionsText(in int increment)
        {
            _virtualUsedActionsValue += increment;
            UpdateUsedActionsText();
        }

        private void UpdateUsedActionsText()
        {
            actionsUsedText.text = _virtualUsedActionsValue.ToString("00");
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


        public void OnEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            _currentEntity = entity;

            _virtualUsedActionsValue = 0;
            UpdateUsedActionsText();
            UpdateLimitText(in entity);
            ShowUI();
        }

        public void OnEntityRequestAction(CombatEntity entity)
        {
            
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
            
        }

        public void OnEntityFinishSequence(CombatEntity entity)
        {
            _currentEntity = null;
            HideUI();
        }

        public void OnEntityWaitSequence(CombatEntity entity)
        {
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

            int cost = skill.SkillCost;
            VariateUsedActionsText(in cost);
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

    }
}
