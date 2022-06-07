using CombatSystem.Skills;
using CombatSystem.Skills.Effects;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    /// <summary>
    /// Singleton to extract visual references for COMBAT
    /// </summary>
    public sealed class CombatThemeSingleton
    {
        public static readonly CombatThemeSingleton Instance = new CombatThemeSingleton();

        static CombatThemeSingleton()
        { }

        [ShowInInspector, InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        public static ClassTeamRolesStructure<CombatThemeHolder> RolesThemeHolder { get; internal set; }
        public static ISkillArchetypeStructureRead<CombatThemeHolder> SkillsThemeHolder { get; internal set; }
        public static IStatsRead<CombatThemeHolder> StatsThemeHolder { get; internal set; }
        public static IEffectStructureRead<CombatThemeHolder> EffectThemeHolder { get; internal set; }
    }
}
