using System;
using Characters;
using MPUIKIT;
using Sirenix.OdinInspector;
using Skills;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Player
{
    public class USkillButton : MonoBehaviour, IPointerClickHandler,
        IPointerEnterHandler, IPointerExitHandler
    {

        // TODO Icon
        // TODO animations by states

        [SerializeField] private Image selectedIcon = null;
        [SerializeField] private TextMeshProUGUI cooldownHolder = null;

        public SkillButtonBehaviour Behaviour { set; private get; }

        public CombatingEntity CurrentEntity { get; private set; }
        [ShowInInspector]
        public CombatSkill CurrentSkill { get; private set; }

        public CombatSkill.State SkillState
        {
            get => CurrentSkill.SkillState;
            private set => CurrentSkill.SkillState = value;
        }

        private bool IsInvalid()
        {
            return CurrentSkill == null || CurrentEntity == null;

        }

        public void Injection(CombatingEntity entity, CombatSkill skill)
        {
            CurrentEntity = entity;
            CurrentSkill = skill;

        }

        public void Show()
        {
            gameObject.SetActive(true);
            UpdateCooldown();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void UpdateCooldown()
        {
            if (CurrentSkill.IsInCooldown())
            {
                cooldownHolder.transform.parent.gameObject.SetActive(true);
                cooldownHolder.text = CurrentSkill.CurrentCooldown.ToString();
            }
            else
            {
                cooldownHolder.transform.parent.gameObject.SetActive(false);
            }
        }

        public void ToggleSelectedIcon(bool set)
        {
            selectedIcon.gameObject.SetActive(set);
        }

        public void HandleOnUse()
        {
            ToggleSelectedIcon(false);
            //TODO show cooldownOnUse
        }

        private void InjectInHandler()
        {
            var handler = Behaviour.Handler;
            var handlerButton = handler.CurrentSelectedButton;

            if (handlerButton != null && handlerButton.SkillState != CombatSkill.State.Cooldown)
            {
                handlerButton.OnDeselectSkill();
            }

            handler.CurrentSelectedButton = this;


        }

        //TODO do mouse handling
        // TODO Click
        // TODO Hover
        // TODO OnSelect > Show targets
        // TODO OnSubmit > DoSkill + Hide UI

        public void OnPointerClick(PointerEventData eventData)
        {
            if(IsInvalid()) return;
            PointerEventData.InputButton input = eventData.button;
            switch (input)
            {
                case PointerEventData.InputButton.Left:
                    HandleLeftClick();
                    break;
                case PointerEventData.InputButton.Right:
                    break;
            }
        }

        private void OnSelectSkill()
        {
            SkillState = CombatSkill.State.Selected;
            ToggleSelectedIcon(true);

        }

        private void OnDeselectSkill()
        {
            SkillState = CombatSkill.State.Idle;
            ToggleSelectedIcon(false);
        }

        private void HandleLeftClick()
        {
            switch (SkillState)
            {
                case CombatSkill.State.Idle:
                    InjectInHandler();
                    OnSelectSkill();
                    break;
                case CombatSkill.State.Selected:
                    Behaviour.Handler.CurrentSelectedButton = null;
                    OnDeselectSkill();
                    break;
                case CombatSkill.State.Cooldown:
                    break;
                default:
                    throw new NotImplementedException("Skill Button Click not Implemented");
            }
        }



        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsInvalid()) return;

            USkillTooltipHandler handler = Behaviour.SkillTooltip;
            handler.HandleButton(this);
            handler.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsInvalid()) return;

            USkillTooltipHandler handler = Behaviour.SkillTooltip;
            handler.gameObject.SetActive(false);
        }
    }

    [Serializable]
    public class SkillButtonBehaviour
    {
        [NonSerialized] 
        public USkillButtonsHandler Handler;

        [SerializeField] private USkillTooltipHandler skillTooltip = null;
        public USkillTooltipHandler SkillTooltip => skillTooltip;
    }
}
