using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    /// <summary>
    /// Singleton to extract visual references for COMBAT
    /// </summary>
    public sealed class PlayerCombatVisualsSingleton
    {
        public static readonly PlayerCombatVisualsSingleton Instance = new PlayerCombatVisualsSingleton();
        static PlayerCombatVisualsSingleton()
        {
            
        }

        [ShowInInspector, InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        public static IOppositionTeamStructureRead<CombatPlayerTeamFeedBack> CombatTeamFeedBacks { get; internal set; }

        [ShowInInspector, InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        public static IOppositionTeamStructureRead<IStatsRead<GameObject>> SecondaryParticlesHolder { get; internal set; }

    }
}
