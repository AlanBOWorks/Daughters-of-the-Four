using System.Collections.Generic;
using CombatSystem.Entity;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace CombatSystem.Team
{
    [CreateAssetMenu(menuName = "Combat/Team/Preset",
        fileName = "N" + AssetDetailName)]
    public class SEnemyPredefinedTeam : ScriptableObject, ICombatTeamProvider
    {
        private const string AssetDetailName = " [Enemy Preset Team]";
        [SerializeField] private string teamName;

        [Title("Characters")]
        [SerializeField]
        private SEnemyPreparationEntity[] characters = new SEnemyPreparationEntity[EnumTeam.RoleTypesCount];
        [SerializeField]
        [Title("Skills")]
        private STeamSkill[] teamSkills = new STeamSkill[0];


        public IEnumerable<ICombatEntityProvider> GetSelectedCharacters() => characters;
        public IEnumerable<ITeamSkillPreset> GetTeamSkills() => teamSkills;

        [Button]
        private void UpdateWithID()
        {
            UtilsAssets.UpdateAssetNameWithID(this,teamName + AssetDetailName);
        }
    }
}
