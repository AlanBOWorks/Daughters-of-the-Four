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
        [SerializeField]
        private TooltipWrapper wrapper = new TooltipWrapper();
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
        }

        public void OnToolTipOffensiveEffect(in PerformEffectValues values)
        {
            HandleEffect(in values, wrapper.OffensiveEffectType);
        }

        public void OnTooltipSupportEffect(in PerformEffectValues values)
        {
            HandleEffect(in values, wrapper.SupportEffectType);
        }

        public void OnTooltipTeamEffect(in PerformEffectValues values)
        {
            HandleEffect(in values, wrapper.TeamEffectType);
        }

        public void OnFinishPoolEffects()
        {
            wrapper.OnFinishHandlingEffects();
        }

        private static void HandleEffect(in PerformEffectValues values, USkillTooltipWindow window)
        {
            window.HandleEffect(in values);
        }

        public void OnSkillButtonHover(in CombatSkill skill)
        {
        }

        public void OnSkillButtonExit(in CombatSkill skill)
        {
            wrapper.HideTooltipWindow();
        }

        [Serializable]
        private sealed class TooltipWrapper : IEffectTypeStructureRead<USkillTooltipWindow>
        {
            [Title("Windows")] 
            [SerializeField] private USkillTooltipWindow offensiveWindow;
            [SerializeField] private USkillTooltipWindow supportWindow;
            [SerializeField] private USkillTooltipWindow teamWindow;

            public void ShowToolTipWindow()
            {
                offensiveWindow.Show();
                supportWindow.Show();
                teamWindow.Show();
            }

            public void HideTooltipWindow()
            {
                offensiveWindow.Hide();
                supportWindow.Hide();
                teamWindow.Hide();
            }

            public void OnFinishHandlingEffects()
            {
                offensiveWindow.OnFinisHandlingEffects();
                supportWindow.OnFinisHandlingEffects();
                teamWindow.OnFinisHandlingEffects();
            }

            public USkillTooltipWindow OffensiveEffectType => offensiveWindow;
            public USkillTooltipWindow SupportEffectType => supportWindow;
            public USkillTooltipWindow TeamEffectType => teamWindow;
        }
    }
}
