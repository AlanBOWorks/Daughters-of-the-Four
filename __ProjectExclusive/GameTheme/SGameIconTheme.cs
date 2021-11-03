using System;
using CombatSkills;
using CombatTeam;
using Sirenix.OdinInspector;
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
        [SerializeField,HorizontalGroup()]
        private CombatRoleIconsThemeHolder combatRoleIcons = new CombatRoleIconsThemeHolder();
        [SerializeField, HorizontalGroup()]
        private CombatSkillTypesThemeHolder combatSkillTypes = new CombatSkillTypesThemeHolder();

        public IMasterStats<Sprite> GetMasterStructureIcons() => combatIconTheme;
        public IBaseStats<Sprite> GetBaseStatsStructureIcons() => combatIconTheme;
        public ITeamRoleStructure<Sprite> GetRoleStructureIcons() => combatRoleIcons;
        public ISkillTypes<Sprite> GetSkillTypesIcons() => combatSkillTypes;


        [Serializable]
        private class CombatIconThemeHolder : CondensedMasterStructure<Sprite,Sprite>
        { }

        [Serializable]
        private class CombatRoleIconsThemeHolder : RoleArchetypeStructure<Sprite>
        { }

        [Serializable]
        private class CombatSkillTypesThemeHolder : SkillTypeStructure<Sprite>
        { }
    }
}
