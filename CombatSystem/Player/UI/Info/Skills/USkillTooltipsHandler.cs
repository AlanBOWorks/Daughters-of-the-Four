using System;
using System.Collections.Generic;
using CombatSystem.Localization;
using CombatSystem.Player.Events;
using CombatSystem.Skills;
using CombatSystem.Skills.Effects;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CombatSystem.Player.UI
{
    public class USkillTooltipsHandler : MonoBehaviour, ISkillPointerListener
    {
        [SerializeField]
        private SkillInfoHandler skillInfo;
        [SerializeField]
        private LuckInfoHandler luckInfo;

        [Title("Holder")]
        [SerializeField] 
        private UEffectsTooltipWindowHandler tooltipWindow;

        private void Awake()
        {
            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);
        }

        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.UnSubscribe(this);
        }

        public void OnSkillButtonHover(ICombatSkill skill)
        {
            ExtractTheme(skill,out var roleColor,out var roleIcon);
            skillInfo.HandleSkillName(skill, roleColor, roleIcon);
            luckInfo.HandleLuckAmount(skill, roleColor);

            IEnumerable<PerformEffectValues> skillEffects;
            if (skill.Preset is IVanguardSkill vanguardSkill)
                skillEffects = vanguardSkill.GetPerformVanguardEffects();
            else
                skillEffects = skill.GetEffects();

            var performer = PlayerCombatSingleton.PlayerTeamController.GetPerformer();
            tooltipWindow.HandleEffects(skill,skillEffects, performer);
            tooltipWindow.Show();

        }

        public void OnSkillButtonExit(ICombatSkill skill)
        {
            tooltipWindow.Hide();
        }

        private static void ExtractTheme(ISkill skill, out Color roleColor, out Sprite roleIcon)
        {
            var archetype = skill.TeamTargeting;
            var roleThemesHolder = CombatThemeSingleton.SkillsThemeHolder;
            var roleTheme = UtilsSkill.GetElement(archetype, roleThemesHolder);
            roleColor = roleTheme.GetThemeColor();
            roleIcon = roleTheme.GetThemeIcon();
        }


        [Serializable]
        private struct SkillInfoHandler
        {
            [SerializeField] private TextMeshProUGUI nameHolder;
            [SerializeField] private Image roleIconHolder;

            public void HandleSkillName(ICombatSkill skill, Color roleColor, Sprite roleIcon)
            {
                var skillName = LocalizeSkills.LocalizeSkill(skill);
                nameHolder.text = skillName;

                roleIconHolder.sprite = roleIcon;
                roleIconHolder.color = roleColor;
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
