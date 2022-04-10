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

        [SerializeField]
        private SPlayerPreparationEntity[] characters = new SPlayerPreparationEntity[EnumTeam.RoleTypesAmount];

        public IReadOnlyCollection<ICombatEntityProvider> GetSelectedCharacters() => characters;

        [Button]
        private void UpdateWithID()
        {
            UtilsAssets.UpdateAssetNameWithID(this, teamName + AssetDetailName);
        }
    }
}
