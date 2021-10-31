using System;
using Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameTheme
{
    [CreateAssetMenu(fileName = "N [Color Theme]",
        menuName = "Game/Theme/Colors")]
    public class SGameColorsTheme : ScriptableObject
    {
        [FormerlySerializedAs("colorsTheme")] 
        [SerializeField] 
        private CombatColorsThemeHolder combatColorsTheme = new CombatColorsThemeHolder();

        public IMasterStats<Color> GetMasterStructureColors() => combatColorsTheme;
        public IBaseStats<Color> GetBaseStatStructureColors() => combatColorsTheme;


        [Serializable]
        private class CombatColorsThemeHolder : CondensedMasterStructure<Color>
        { }
    }
}
