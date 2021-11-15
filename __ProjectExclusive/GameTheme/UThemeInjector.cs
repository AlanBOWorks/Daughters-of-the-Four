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
                = colorsTheme.GetMasterTypes();
            PlayerCombatSingleton.CombatRoleStructureColors
                = colorsTheme.GetRoleTypes();
            PlayerCombatSingleton.CombatSkillTypesColors
                = colorsTheme.GetSkillTypes();
            PlayerCombatSingleton.SkillInteractionColors
                = colorsTheme;

            PlayerCombatSingleton.CombatMasterStatsIcons
                = iconsTheme.GetMasterTypes();
            PlayerCombatSingleton.CombatRolesIcons
                = iconsTheme.GetRoleTypes();
            PlayerCombatSingleton.CombatSkillTypesIcons
                = iconsTheme.GetSkillTypes();
            PlayerCombatSingleton.SkillInteractionIcons
                = iconsTheme;

            Destroy(this);
        }
    }
}
