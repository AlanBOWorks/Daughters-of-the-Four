using System;
using CombatSystem.Skills.Effects;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Stats
{
    [CreateAssetMenu(fileName = "COLORS - N [StatsType Holder]",
        menuName = "Combat/Holders/Stats Type/Colors [Holder]")]
    public class SMainStatsColorsHolder : ScriptableObject
    {
        [SerializeField]
        private ReferencesHolder holder = new ReferencesHolder();

        public TeamRolesStructure<Color> GetHolder() => holder;

        [Serializable]
        private sealed class ReferencesHolder : TeamRolesStructure<Color>
        {
            
        }

    }
}
