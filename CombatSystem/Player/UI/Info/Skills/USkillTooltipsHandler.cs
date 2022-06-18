using System;
using CombatSystem.Player.Events;
using CombatSystem.Skills;
using CombatSystem.Skills.Effects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class USkillTooltipsHandler : MonoBehaviour, ISkillTooltipListener, ISkillPointerListener
    {

        [SerializeField] private USkillTooltipWindow tooltipWindow;

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
            tooltipWindow.Show();
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
            tooltipWindow.HandleSkill(skill);
        }

        public void OnSkillButtonExit(ICombatSkill skill)
        {
            tooltipWindow.Hide();
        }

    }
}
