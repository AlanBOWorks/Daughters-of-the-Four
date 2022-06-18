using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Entity
{
    [CreateAssetMenu(menuName = "Combat/Entities/[Enemy] Preparation",
        fileName = "N " + AssetPrefixName)]
    public class SEnemyPreparationEntity : SPreparationEntity
    {
        private const string AssetPrefixName = "[ENEMY Preparation Entity]";
        private const string NullName = "NULL";
        [Title("Prefabs")]
        [SerializeField, AssetsOnly, PreviewField(ObjectFieldAlignment.Left),
         AssetSelector(Paths = AssetPathFolderRoot + "/Enemies/")]
        private GameObject instantiationObject;

        [Title("Names")]
        [SerializeField] private string entityName = NullName;
        [SerializeField] private string entityFullName = NullName;
        [SerializeField] private string shorterName = NullName;
        public override string GetProviderEntityName() => entityName;
        public override string GetProviderEntityFullName() => entityFullName;
        public override string GetProviderShorterName() => shorterName;
        public override GameObject GetVisualPrefab() => instantiationObject;
    }
}
