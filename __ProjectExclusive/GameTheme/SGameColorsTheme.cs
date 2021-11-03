using System;
using CombatSkills;
using CombatTeam;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameTheme
{
    [CreateAssetMenu(fileName = "N [Color Theme]",
        menuName = "Game/Theme/Colors")]
    public class SGameColorsTheme : ScriptableObject
    {
        [SerializeField] 
        private CombatStatsColorsHolder combatStatsColors = new CombatStatsColorsHolder();
        [SerializeField,HorizontalGroup("TriColor")]
        private CombatRoleColorsHolder combatRoleColors = new CombatRoleColorsHolder();
        [SerializeField, HorizontalGroup("TriColor")]
        private CombatSkillTypeColorsHolder skillTypeColors = new CombatSkillTypeColorsHolder();
        public IMasterStats<Color> GetMasterStructureColors() => combatStatsColors;
        public IBaseStats<Color> GetBaseStatStructureColors() => combatStatsColors;
        public ITeamRoleStructure<Color> GetRoleStructureColors() => combatRoleColors;
        public ISkillTypes<Color> GetSkillTypesStructureColors() => skillTypeColors;

        [Serializable]
        private class CombatStatsColorsHolder : CondensedMasterStructure<Color,Color>
        { }
        [Serializable]
        private class CombatRoleColorsHolder : RoleArchetypeStructure<Color>
        { }
        [Serializable]
        private class CombatSkillTypeColorsHolder : SkillTypeStructure<Color>
        { }
    }
}
