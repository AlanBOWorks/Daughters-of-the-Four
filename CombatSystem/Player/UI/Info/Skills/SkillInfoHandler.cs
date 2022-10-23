using CombatSystem.Player.Events;
using CombatSystem.Skills;
using CombatSystem.Skills.Effects;

namespace CombatSystem.Player.UI
{
    public class SkillInfoHandler : ISkillPointerListener
    {
        public void OnSkillButtonHover(IFullSkill skill)
        {
            var effects = skill.GetEffectsFeedBacks();
            foreach (PerformEffectValues values in effects)
            {
                HandleEffect(in values);
            }
            PlayerCombatSingleton.PlayerCombatEvents.OnFinishPoolEffects();
        }
        public void OnSkillButtonExit(IFullSkill skill)
        {
        }

        private static void HandleEffect(in PerformEffectValues values)
        {
            var archetype = UtilsEffectOrganization.ConvertEffectArchetype(values.Effect);
            switch (archetype)
            {
                case EnumsEffect.Archetype.Others:
                    InvokeOthersListeners(in values);
                    break;
                case EnumsEffect.Archetype.Offensive:
                    InvokeOffensiveListeners(in values);
                    break;
                case EnumsEffect.Archetype.Support:
                    InvokeSupportListeners(in values);
                    break;
                case EnumsEffect.Archetype.Team:
                    InvokeTeamListeners(in values);
                    break;
            }
        }

        private static void InvokeOthersListeners(in PerformEffectValues values)
        {
            PlayerCombatSingleton.PlayerCombatEvents.OnTooltipEffect(in values);
        }

        private static void InvokeOffensiveListeners(in PerformEffectValues values)
        {
            var eventsHolder = PlayerCombatSingleton.PlayerCombatEvents;

            eventsHolder.OnTooltipEffect(in values);
            eventsHolder.OnToolTipOffensiveEffect(in values);
        }

        private static void InvokeSupportListeners(in PerformEffectValues values)
        {
            var eventsHolder = PlayerCombatSingleton.PlayerCombatEvents;

            eventsHolder.OnTooltipEffect(in values);
            eventsHolder.OnTooltipSupportEffect(in values);
        }

        private static void InvokeTeamListeners(in PerformEffectValues values)
        {
            var eventsHolder = PlayerCombatSingleton.PlayerCombatEvents;

            eventsHolder.OnTooltipEffect(in values);
            eventsHolder.OnTooltipTeamEffect(in values);
        }

    }

}
