using __ProjectExclusive.Player;
using CombatSystem;
using GameTheme;
using UnityEngine;

namespace __ProjectExclusive.GameTheme
{
    public class UThemeInjector : MonoBehaviour
    {
        [SerializeField] private SGameColorsTheme colorsTheme;
        [SerializeField] private SGameIconTheme iconsTheme;
        private void Awake()
        {
            PlayerCombatSingleton.CombatMasterStatColors 
                = colorsTheme.GetMasterStructureColors();
            PlayerCombatSingleton.CombatBaseStatsColors
                = colorsTheme.GetBaseStatStructureColors();
            PlayerCombatSingleton.CombatRoleStructureColors
                = colorsTheme.GetRoleStructureColors();
            PlayerCombatSingleton.CombatSkillTypesColors
                = colorsTheme.GetSkillTypesStructureColors();

            PlayerCombatSingleton.CombatMasterStatsIcons
                = iconsTheme.GetMasterStructureIcons();
            PlayerCombatSingleton.CombatBaseStatsIcons
                = iconsTheme.GetBaseStatsStructureIcons();
            PlayerCombatSingleton.CombatRolesIcons
                = iconsTheme.GetRoleStructureIcons();
            PlayerCombatSingleton.CombatSkillTypesIcons
                = iconsTheme.GetSkillTypesIcons();

            Destroy(this);
        }
    }
}
