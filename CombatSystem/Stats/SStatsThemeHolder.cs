using System;
using CombatSystem.Player.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Stats
{
    [CreateAssetMenu(fileName = "ICONS - N [StatsType Holder]",
        menuName = "Combat/Holders/Stats Type/Icons [Holder]")]
    public class SStatsThemeHolder : ScriptableObject
    {
        [SerializeField]
        private ThemeHolder themeHolder = new ThemeHolder();
        public IStatsRead<CombatThemeHolder> GetHolder() => themeHolder;


        [Serializable]
        private sealed class ThemeHolder : ClassStatsStructure<CombatThemeHolder>
        {
            
        }
    }
}
