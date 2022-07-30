using Lore.Character;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Entity
{
    [CreateAssetMenu(menuName = "Combat/Entities/[Player] Preparation",
        fileName = "N " + PlayerAssetPrefixName)]
    public sealed class SPlayerPreparationEntity : SPreparationEntity
    {
        [Title("Prefabs")]
        [SerializeField, AssetsOnly, PreviewField(ObjectFieldAlignment.Left),
        AssetSelector(Paths = AssetPathFolderRoot + "/Players/")]
        private GameObject instantiationObject;

        [SerializeField, InlineEditor()] 
        private SCharacterLoreHolder loreHolder;

        private const string PlayerAssetPrefixName = "[PLAYER Preparation Entity]";
        public override string GetProviderEntityName() => loreHolder.CharacterNameType;
        public override string GetProviderEntityFullName() => loreHolder.CharacterFullNameType;
        public override string GetProviderShorterName() => loreHolder.CharacterShorterNameType;

        public ICharacterPortraitHolder GetPortraitHolder() => loreHolder.GetPortraitHolder();

        protected override string AssetPrefix()
        {
            return PlayerAssetPrefixName;
        }

        public override GameObject GetVisualPrefab() => instantiationObject;
    }
}
