using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Lore.Character
{
    [CreateAssetMenu(fileName = "N" + AssetPrefix,
        menuName = "Lore/Character/LoreHolder")]
    public class SCharacterLoreHolder : ScriptableObject, ICharacterNameStructureRead
    {
        private const string NullName = "NULL";
        private const string AssetPrefix = "[Lore]";
        [Title("Names")]
        [SerializeField] private string entityName = NullName;
        [SerializeField] private string entityFullName = NullName;
        [SerializeField] private string shorterName = NullName;

        public string CharacterNameType => entityName;
        public string CharacterFullNameType => entityFullName;
        public string CharacterShorterNameType => shorterName;

        [Button]
        private void UpdateAssetName()
        {
            string assetName = entityName + " " + AssetPrefix;
            UtilsAssets.UpdateAssetNameWithID(this, assetName);
        }
    }
}
