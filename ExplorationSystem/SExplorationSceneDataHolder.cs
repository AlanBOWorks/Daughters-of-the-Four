using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Utils;

namespace ExplorationSystem
{
    [CreateAssetMenu(fileName = "N " + AssetPrefix, menuName = "Editor/Scenes/Exploration Data Holder")]
    public class SExplorationSceneDataHolder : ScriptableObject
    {
        private const string AssetPrefix = "[ExplorationScene Data]";

        [Title("Visuals")]
        [SerializeField] private SceneAsset backgroundScene;

        [Title("Entities")] 
        [SerializeField] private SceneEntitiesData[] basicEntities = new SceneEntitiesData[0];



        public SceneAsset GetBackgroundSceneAsset() => backgroundScene;
        public IReadOnlyList<SceneEntitiesData> GetBasicEntities() => basicEntities;

        [Button]
        private void UpdateAssetName()
        {
            if (!backgroundScene)
            {
                Debug.LogError("NULL Reference: There's no scene Asset to update the name to");
                return;
            }

            var targetName = backgroundScene.name;
            UtilsAssets.UpdateAssetName(this, targetName + $" {AssetPrefix}");
        }
    }

    [Serializable]
    public struct SceneEntitiesData
    {
        [SerializeField] private SEnemyPreparationEntity preset;
        [SerializeField] private bool forceIgnoreVariationEntities;

        public SEnemyPreparationEntity GetEntityPreset() => preset;
        public bool IgnoreVariations() => forceIgnoreVariationEntities;
    }
}
