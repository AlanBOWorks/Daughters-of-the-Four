using System;
using Characters;
using MPUIKIT;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Skills
{
    public class USkillButton : MonoBehaviour, IPointerClickHandler,
        IPointerEnterHandler, IPointerExitHandler
    {

        // TODO Icon
        // TODO animations by states

        [SerializeField] private Image selectedIcon = null;

        public SkillButtonBehaviour Behaviour { set; private get; }

        public CombatingEntity CurrentEntity { get; private set; }
        [ShowInInspector]
        public CombatSkill CurrentSkill { get; private set; }

        public CombatSkill.State SkillState
        {
            get => CurrentSkill.SkillState;
            private set => CurrentSkill.SkillState = value;
        }


        public void Injection(CombatingEntity entity, CombatSkill skill)
        {
            CurrentEntity = entity;
            CurrentSkill = skill;
        }

        public void ToggleSelectedIcon(bool set)
        {
            selectedIcon.gameObject.SetActive(set);
        }

        private void InjectInHandler(ref USkillButton handlerButton)
        {
            if (handlerButton == null)
            {
                handlerButton = this;
            }
            else
            {
                handlerButton.OnDeselectSkill();
                handlerButton = this;
            }

        }

        //TODO do mouse handling
        // TODO Click
        // TODO Hover
        // TODO OnSelect > Show targets
        // TODO OnSubmit > DoSkill + Hide UI

        public void OnPointerClick(PointerEventData eventData)
        {
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
            //TODO call targetsHandler
            SkillState = CombatSkill.State.Selected;
            ToggleSelectedIcon(true);

        }

        private void OnDeselectSkill()
        {
            //TODO cancel targetsHandler
            SkillState = CombatSkill.State.Idle;
            ToggleSelectedIcon(false);
        }

        private void HandleLeftClick()
        {
            switch (SkillState)
            {
                case CombatSkill.State.Idle:
                    InjectInHandler(ref Behaviour.Handler.currentSelectedButton);
                    OnSelectSkill();
                    break;
                case CombatSkill.State.Selected:
                    Behaviour.Handler.currentSelectedButton = null;
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
            USkillTooltipHandler handler = Behaviour.SkillTooltip;
            handler.HandleButton(this);
            handler.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
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
