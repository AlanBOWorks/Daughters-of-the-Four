using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace CombatSystem.Player
{
    [CreateAssetMenu(fileName = "N" + AssetDetailName,
        menuName = "Combat/Team/Player Preset")]
    public class SPlayerPresetTeam : ScriptableObject, ICombatTeamProvider
    {
        private const string AssetDetailName = " [Player Predefined Team]";
        [SerializeField] private string teamName;

        [Title("Characters")]
        [SerializeField]
        private SPlayerPreparationEntity[] characters = new SPlayerPreparationEntity[EnumTeam.RoleTypesCount];
        [Title("Skills")]
        [SerializeField]
        private STeamSkill[] teamSkills = new STeamSkill[0];

        internal SPlayerPreparationEntity[] GetPresetCharacters() => characters;

        public IEnumerable<ICombatEntityProvider> GetSelectedCharacters() => characters;
        public int MembersCount => characters.Length;
        public IEnumerable<ITeamSkillPreset> GetTeamSkills() => teamSkills;


        [Button]
        private void UpdateWithID()
        {
            UtilsAssets.UpdateAssetNameWithID(this, teamName + AssetDetailName);
        }
    }
}
