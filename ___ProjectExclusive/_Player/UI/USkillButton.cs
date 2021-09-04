using System;
using ___ProjectExclusive;
using Characters;
using CombatEffects;
using MPUIKIT;
using Sirenix.OdinInspector;
using Skills;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Player
{
    public class USkillButton : MonoBehaviour, 
        IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler,
        IPlayerButtonListener
    {

        // TODO Icon
        // TODO animations by states
        [SerializeField] private Image skillIcon = null;
        [SerializeField] private Image border = null;
        [SerializeField] private Image selectedIcon = null;
        [SerializeField] private TextMeshProUGUI cooldownHolder = null;

        public SkillButtonBehaviour Behaviour { set; private get; }

        public CombatingEntity CurrentEntity { get; private set; }
        [ShowInInspector]
        public CombatSkill CurrentSkill { get; private set; }

        public CombatSkill.State SkillState
        {
            get => CurrentSkill.SkillState;
            private set => CurrentSkill.SwitchState(value);
        }

        private bool IsInvalid()
        {
            return CurrentSkill == null || CurrentEntity == null;

        }

        public void Injection(CombatingEntity entity, CombatSkill skill)
        {
            CurrentEntity = entity;
            CurrentSkill = skill;
            UpdateSkillIcon();
        }

        private void UpdateSkillIcon()
        {
            var skillPreset= CurrentSkill.Preset;
            Sprite skillSprite = skillPreset.GetSkillIcon();

            EnumSkills.TargetingType skillType = skillPreset.GetSkillType();
            if (skillSprite == null) // backup icon
            {
                var spriteThemes = GameThemeSingleton.ThemeVariable.SkillIcons;
                EnumSkills.StatDriven statType = skillPreset.GetStatDriven();
                skillSprite = spriteThemes.GetElement(skillType, statType);

            }

            var colorThemes = GameThemeSingleton.ThemeColors;
            var skillColor = colorThemes.effectColors.GetHolderElement(skillType);

            skillIcon.sprite = skillSprite;
            skillIcon.color = skillColor;
            border.color = skillColor;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsInvalid()) return;

            USkillTooltipHandler skillTooltip = Behaviour.SkillTooltip;
            skillTooltip.HandleButton(this);
            skillTooltip.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsInvalid()) return;

            USkillTooltipHandler skillTooltip = Behaviour.SkillTooltip;
            skillTooltip.gameObject.SetActive(false);
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
            if (!CurrentSkill.CanBeUse(CurrentEntity))
            {
                cooldownHolder.transform.parent.gameObject.SetActive(true);
                int cooldownAmount = CurrentSkill.CurrentCooldown;
                if (cooldownAmount > 0)
                    cooldownHolder.text = CurrentSkill.CurrentCooldown.ToString();
                else
                    cooldownHolder.text = "X";
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


        private void HandleLeftClick()
        {
            if (CurrentSkill.IsInCooldown())
                return;
            

            switch (SkillState)
            {
                case CombatSkill.State.Idle:
                    PlayerEntitySingleton.SkillButtonsHandler.OnSkillSelect(this);
                    break;
                case CombatSkill.State.Selected:
                    PlayerEntitySingleton.SkillButtonsHandler.OnSkillDeselect(this);
                    break;
                case CombatSkill.State.Cooldown:
                    break;
                default:
                    throw new NotImplementedException("Skill Button Click not Implemented");
            }
        }


        public void OnSkillSelect(USkillButton selectedSkill)
        {
            SkillState = CombatSkill.State.Selected;
            ToggleSelectedIcon(true);
        }

        public void OnSkillDeselect(USkillButton deselectSkill)
        {
            SkillState = CombatSkill.State.Idle;
            ToggleSelectedIcon(false);
        }

        public void OnSubmitSkill(USkillButton submitSkill)
        {
            // SkillState = CombatSkill.State.Cooldown; <<<< Don't use this; 
            // The cooldown is call by [PerformSkillHandler] since the AI uses this as well for 
            // its skill usages (and also could bypass the skill cost <= 0 check)
            ToggleSelectedIcon(false);
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
