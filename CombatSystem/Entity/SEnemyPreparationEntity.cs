using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Entity
{
    [CreateAssetMenu(menuName = "Combat/Entities/[Enemy] Preparation",
        fileName = "N " + AssetPrefixName)]
    public class SEnemyPreparationEntity : SPreparationEntity
    {
        public const string AssetPrefixName = "[ENEMY Preparation Entity]";
        private const string NullName = "NULL";
        public const string EnemiesAssetPathFolder = AssetPathFolderRoot + "/Enemies/";

        [Title("Prefabs")]
        [SerializeField, AssetsOnly, PreviewField(ObjectFieldAlignment.Left),
         AssetSelector(Paths = EnemiesAssetPathFolder)]
        private GameObject instantiationObject;

        [Title("Names")]
        [SerializeField] private string entityName = NullName;
        public override string GetProviderEntityName() => entityName;
        public override GameObject GetVisualPrefab() => instantiationObject;


    }
}
