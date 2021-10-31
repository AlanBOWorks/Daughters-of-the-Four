using System;
using Stats;
using UnityEngine;

namespace GameTheme
{

    [CreateAssetMenu(fileName = "N [Icons Theme]",
        menuName = "Game/Theme/Icons")]
    public class SGameIconTheme : ScriptableObject
    {
        [SerializeField] 
        private CombatIconThemeHolder combatIconTheme = new CombatIconThemeHolder();

        public IMasterStats<Sprite> GetMasterStructureIcons() => combatIconTheme;
        public IBaseStats<Sprite> GetBaseStatsStructureIcons() => combatIconTheme;


        [Serializable]
        private class CombatIconThemeHolder : CondensedMasterStructure<Sprite>
        {
            
        }
    }
}
