using System;
using ___ProjectExclusive;
using Characters;
using CombatEffects;
using DG.Tweening;
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
        IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {

        // TODO Icon
        // TODO animations by states
        [Title("Skill")]
        [SerializeField] private Image skillIcon = null;
        [SerializeField] private TextMeshProUGUI skillName = null;
        [Title("Tooltips")] 
        [SerializeField] private Image selectedIcon = null;
        [SerializeField] private TextMeshProUGUI cooldownHolder = null;
        [Title("Aesthetics")]
        [SerializeField] private Image border = null;

        public SkillButtonBehaviour Behaviour { set; private get; }

        public CombatingEntity CurrentEntity { get; private set; }
        [ShowInInspector,DisableInEditorMode]
        public CombatSkill CurrentSkill { get; private set; }
        
        private bool IsInvalid()
        {
            return CurrentSkill == null || CurrentEntity == null;

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
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }


        private void UpdateSkillTooltips()
        {
            var skillPreset = CurrentSkill.Preset;
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


            if (skillName != null)
            {
                skillName.text = CurrentSkill.SkillName;
                skillName.color = skillColor;
            }
            if (skillIcon != null)
            {
                skillIcon.sprite = skillSprite;
                skillIcon.color = skillColor;
            }
            if (border != null)
                border.color = skillColor;
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

        public void UpdateSkill(CombatingEntity entity, CombatSkill skill)
        {
            CurrentEntity = entity;
            CurrentSkill = skill;
            UpdateSkillTooltips();
        }

        public void ToggleSelectedIcon(bool selected)
        {
            if(selectedIcon == null) return;

            selectedIcon.gameObject.SetActive(selected);
            selectedIcon.rectTransform.DOPunchPosition(Vector3.right, .2f, 4);
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
                    PlayerEntitySingleton.SkillSelectorHandler.OnSkillSelect(CurrentSkill);
                    break;
                case PointerEventData.InputButton.Right:
                    break;
            }
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
