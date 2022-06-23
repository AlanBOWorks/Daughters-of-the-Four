using System;
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
    public class USkillTooltipsHandler : MonoBehaviour, ISkillTooltipListener, ISkillPointerListener
    {

        [SerializeField]
        private SkillInfoHandler skillInfo = new SkillInfoHandler();
        [SerializeField] private UEffectsTooltipWindowHandler tooltipWindow;

        private void Awake()
        {
            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);
        }

        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.UnSubscribe(this);
        }

        public void OnTooltipEffect(in PerformEffectValues values)
        {
            tooltipWindow.HandleEffect(in values);
        }

        public void OnToolTipOffensiveEffect(in PerformEffectValues values)
        {
        }

        public void OnTooltipSupportEffect(in PerformEffectValues values)
        {
        }

        public void OnTooltipTeamEffect(in PerformEffectValues values)
        {
        }

        public void OnFinishPoolEffects()
        {
            tooltipWindow.OnFinisHandlingEffects();
        }

        public void OnSkillButtonHover(ICombatSkill skill)
        {
            skillInfo.HandleSkillName(skill);
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
