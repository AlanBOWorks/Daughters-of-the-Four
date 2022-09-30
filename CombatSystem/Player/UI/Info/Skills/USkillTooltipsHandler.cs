using System;
using System.Collections.Generic;
using CombatSystem.Localization;
using CombatSystem.Player.Events;
using CombatSystem.Skills;
using CombatSystem.Skills.Effects;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CombatSystem.Player.UI
{
    public class USkillTooltipsHandler : MonoBehaviour, ISkillPointerListener,
        ISkillSelectionListener
    {
        [Title("References")]
        [SerializeField]
        private GameObject ignoreSelfTextHolder;

        [SerializeField]
        private SkillInfoHandler skillInfo;
        [SerializeField]
        private LuckInfoHandler luckInfo;
       

        [Title("Holder")]
        [SerializeField] 
        private UEffectsTooltipWindowHandler tooltipWindow;

        [SerializeField] 
        private UEffectsTooltipWindowHandler counterTooltipWindow;
        [SerializeField] 
        private UEffectsTooltipWindowHandler punishTooltipWindow;

        private void Awake()
        {
            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);

            var shortcutAction = 
                CombatShortcutsSingleton.InputActions.SkillInfoShortCut.action;
            shortcutAction.performed += ShortcutShowSkillInfo;
            shortcutAction.canceled += ShortcutHideSkillInfo;

            _vanguardHorizontalPosition = punishTooltipWindow.transform.localPosition.x;
        }

        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.UnSubscribe(this);

            var shortcutAction = 
                CombatShortcutsSingleton.InputActions.SkillInfoShortCut.action;
            shortcutAction.performed -= ShortcutShowSkillInfo;
            shortcutAction.canceled -= ShortcutHideSkillInfo;
        }

        private ICombatSkill _hoverSkill; //todo 
        public void OnSkillButtonHover(ICombatSkill skill)
        {
            if(_shortcutSelectedSkill != null)
                HideSkillInfo(skill);
            ShowSkillInfo(skill);

            _hoverSkill = skill;
        }


        private float _vanguardHorizontalPosition;
        private void ShowSkillInfo(ICombatSkill skill)
        {
            ExtractTheme(skill, out var roleColor, out var roleIcon);
            skillInfo.HandleSkillName(skill, roleColor, roleIcon);
            luckInfo.HandleLuckAmount(skill, roleColor);

            

            var performer = PlayerCombatSingleton.PlayerTeamController.GetPerformer();
            var skillEffects = skill.GetEffects();

            bool ignoreSelfTargetText = skill.IgnoreSelf();
            ignoreSelfTextHolder.SetActive(ignoreSelfTargetText);
            HandleTooltipWindow(tooltipWindow,skillEffects);


            if (skill.Preset is IVanguardSkill vanguardSkill)
            {
                bool hasCounterEffects = vanguardSkill.HasCounterEffects();
                bool hasPunishEffects = vanguardSkill.HasPunishEffects();
                
                HandleCounterWindow(hasCounterEffects);
                HandlePunishEffects(hasPunishEffects, hasCounterEffects);
            }
            else
            {
                counterTooltipWindow.gameObject.SetActive(false);
                punishTooltipWindow.gameObject.SetActive(false);
            }

            void HandleCounterWindow(bool hasEffects)
            {
                var windowGameObject = counterTooltipWindow.gameObject;
                windowGameObject.SetActive(hasEffects);
                if (!hasEffects) return;
                HandleTooltipWindow(counterTooltipWindow, vanguardSkill.GetCounterEffects());
            }

            void HandlePunishEffects(bool hasEffects, bool counterHasEffects)
            {
                var windowGameObject = counterTooltipWindow.gameObject;
                windowGameObject.SetActive(hasEffects);
                if (!hasEffects) return;

                var windowTransform = windowGameObject.transform;
                var windowPosition = windowTransform.localPosition;
                if (!counterHasEffects)
                    windowPosition.x = 0;
                else
                    windowPosition.x = _vanguardHorizontalPosition;
                windowTransform.localPosition = windowPosition;

                HandleTooltipWindow(punishTooltipWindow, vanguardSkill.GetPunishEffects());
            }

            void HandleTooltipWindow(UEffectsTooltipWindowHandler window, IEnumerable<PerformEffectValues> effects)
            {
                window.HandleEffects(skill, effects, performer);
                window.Show();
            }
        }



        public void OnSkillButtonExit(ICombatSkill skill)
        {
            HideSkillInfo(skill);
            if(_shortcutSelectedSkill != null)
                ShowSkillInfo(_shortcutSelectedSkill);

            if (skill == _hoverSkill) _hoverSkill = null;
        }

        private void HideSkillInfo(ICombatSkill skill)
        {
            tooltipWindow.Hide();
            counterTooltipWindow.Hide();
            punishTooltipWindow.Hide();
        }

        private static void ExtractTheme(ISkill skill, out Color roleColor, out Sprite roleIcon)
        {
            var archetype = skill.TeamTargeting;
            var roleThemesHolder = CombatThemeSingleton.SkillsThemeHolder;
            var roleTheme = UtilsSkill.GetElement(archetype, roleThemesHolder);
            roleColor = roleTheme.GetThemeColor();
            roleIcon = roleTheme.GetThemeIcon();
        }

        private ICombatSkill _shortcutSelectedSkill;
        private bool _isShortcutPressed;
        private void ShortcutShowSkillInfo(InputAction.CallbackContext context)
        {
            var skill = PlayerCombatSingleton.PlayerTeamController.GetSkill();
            _isShortcutPressed = true;
            if(skill == null) return;
            _shortcutSelectedSkill = skill;

            if(_hoverSkill != null) return;;
            ShowSkillInfo(skill);

        }

        private void ShortcutHideSkillInfo(InputAction.CallbackContext context)
        {
            var skill = PlayerCombatSingleton.PlayerTeamController.GetSkill();
            _isShortcutPressed = false;
            if(skill == null) return;

            _shortcutSelectedSkill = null;

            if(_hoverSkill != null) return;;
            HideSkillInfo(skill);

        }
        public void OnSkillSelect(CombatSkill skill)
        {
        }

        public void OnSkillSelectFromNull(CombatSkill skill)
        {
        }

        public void OnSkillSwitch(CombatSkill skill, CombatSkill previousSelection)
        {
            if(!_isShortcutPressed) return;
            if(_shortcutSelectedSkill != null)
                HideSkillInfo(_shortcutSelectedSkill);

            _shortcutSelectedSkill = skill;
            ShowSkillInfo(skill);
        }

        public void OnSkillDeselect(CombatSkill skill)
        {
        }

        public void OnSkillCancel(CombatSkill skill)
        {
        }

        public void OnSkillSubmit(CombatSkill skill)
        {
            skillInfo.UpdateCost(skill);
        }



        [Serializable]
        private struct SkillInfoHandler
        {
            [SerializeField] private TextMeshProUGUI nameHolder;
            [SerializeField] private Image roleIconHolder;
            [SerializeField] private TextMeshProUGUI skillCostHolder;

            public void HandleSkillName(ICombatSkill skill, Color roleColor, Sprite roleIcon)
            {
                var skillName = LocalizeSkills.LocalizeSkill(skill);
                nameHolder.text = skillName;

                roleIconHolder.sprite = roleIcon;
                roleIconHolder.color = roleColor;
                skillCostHolder.text = LocalizeSkills.LocalizeSkillCost(skill);
            }

            public void UpdateCost(ISkill skill)
            {
                if(skill == null) return;

                skillCostHolder.text = LocalizeSkills.LocalizeSkillCost(skill);
            }
        }

        [Serializable]
        private struct LuckInfoHandler
        {
            [SerializeField] private TextMeshProUGUI luckAmountHolder;

            public void HandleLuckAmount(ISkill skill, Color color)
            {
                var luckAmount = skill.LuckModifier;
                HandleLuckText(luckAmount);
                HandleLuckColor(color);
            }

            private void HandleLuckText(float luckAmount)
            {
                string luckAmountText = luckAmount.ToString("P0");
                luckAmountHolder.text = luckAmountText;
            }

            private void HandleLuckColor(Color color)
            {
                luckAmountHolder.color = color;
            }
        }
    }
}
