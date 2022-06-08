using System;
using CombatSystem.Skills.Effects;
using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UCombatThemeInjector : MonoBehaviour
    {
        [SerializeField] private SCombatRolesThemesHolder rolesThemesHolder;
        [SerializeField] private SStatsThemeHolder statsThemeHolder;
        [SerializeField] private SEffectsThemeHolder effectsTheme;

        private void Awake()
        {
            var rolesHolder = rolesThemesHolder.GetHolder();
            CombatThemeSingleton.RolesThemeHolder = rolesHolder;
            CombatThemeSingleton.SkillsThemeHolder = rolesHolder;
            CombatThemeSingleton.StatsThemeHolder = statsThemeHolder.GetHolder();

            CombatThemeSingleton.EffectsNameTagsHolder = effectsTheme.GetNamesTagsHolder();
            CombatThemeSingleton.EffectsIconsHolder = effectsTheme.GetIcons();
            CombatThemeSingleton.EffectsColorsHolder = effectsTheme.GetColorsHolder();
        }
    }
}
