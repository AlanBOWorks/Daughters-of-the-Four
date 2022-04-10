using System.Collections.Generic;
using CombatSystem.Entity;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace CombatSystem.Team
{
    [CreateAssetMenu(menuName = "Combat/Team/Preset",
        fileName = "N" + AssetDetailName)]
    public class SPredefinedTeam : ScriptableObject, ICombatTeamProvider
    {
        private const string AssetDetailName = " [Preset Team]";
        [SerializeField] private string teamName;

        [SerializeField]
        private SPreparationEntity[] characters = new SPreparationEntity[EnumTeam.RoleTypesAmount];

        public IReadOnlyCollection<ICombatEntityProvider> GetSelectedCharacters() => characters;

        [Button]
        private void UpdateWithID()
        {
            UtilsAssets.UpdateAssetNameWithID(this,teamName + AssetDetailName);
        }
    }
}
