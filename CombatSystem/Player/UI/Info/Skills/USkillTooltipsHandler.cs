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
        private SkillInfoHandler skillInfo = new SkillInfoHandler();
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
            skillInfo.HandleSkillName(skill);
            IEnumerable<PerformEffectValues> skillEffects;
            if (skill.Preset is IVanguardSkill vanguardSkill)
                skillEffects = vanguardSkill.GetPerformVanguardEffects();
            else
                skillEffects = skill.GetEffects();

            tooltipWindow.HandleEffects(skillEffects);
            tooltipWindow.Show();

        }

        public void OnSkillButtonExit(ICombatSkill skill)
        {
            tooltipWindow.Hide();
        }



        [Serializable]
        private sealed class SkillInfoHandler
        {
            [SerializeField] private TextMeshProUGUI nameHolder;
            [SerializeField] private Image roleIconHolder;

            public void HandleSkillName(ICombatSkill skill)
            {
                var skillName = LocalizeSkills.LocalizeSkill(skill);
                nameHolder.text = skillName;

                var archetype = skill.TeamTargeting;
                var roleThemesHolder = CombatThemeSingleton.SkillsThemeHolder;
                var roleTheme = UtilsSkill.GetElement(archetype, roleThemesHolder);
                var roleColor = roleTheme.GetThemeColor();
                var roleIcon = roleTheme.GetThemeIcon();

                roleIconHolder.sprite = roleIcon;
                roleIconHolder.color = roleColor;
            }
        }
    }
}
