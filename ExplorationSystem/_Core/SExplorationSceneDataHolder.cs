using System;
using System.Collections.Generic;
using System.Data;
using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Utils;
using Utils_Project;
using Random = UnityEngine.Random;

namespace ExplorationSystem
{
    [CreateAssetMenu(fileName = "N " + AssetPrefix, menuName = "Editor/Scenes/Exploration Data Holder")]
    public class SExplorationSceneDataHolder : ScriptableObject, 
        IExplorationSceneDataHolder
    {
        private const string AssetPrefix = "[ExplorationScene Data]";


        [Title("Info")] 
        [SerializeField] private string sceneName = "NULL";
        [SerializeField] private Sprite sceneIcon;

        [Title("Scenes")]
        [SerializeField] private SceneAsset backgroundScene;
        [SerializeField] private SceneAsset fightScene;

        [Title("Basic Entities")] 
        [SerializeField, Range(1,12)] private int maxEntitiesAllow = 4;

        [SerializeField] private SRange basicEntitiesProportion = new SRange(2, 3);
        [SerializeField] private SRange weakEntitiesProportion = new SRange(2, 3);
        [SerializeReference]
        private ISceneEntitiesHolder basicEntities;

        [Title("Elite Entities")] 
        [SerializeField, Range(1, 12)] private int minEliteAmount = 4;

        [SerializeField, Tooltip("How many default entities are added to normal entities")] 
        private SRange eliteEntitiesAddition = new SRange(0, 1);
        [SerializeField, Tooltip("How many weak entities are added to normal entities")]
        private SRange eliteWeakEntitiesAddition = new SRange(0, 1);

        public string GetSceneName() => sceneName;
        public Sprite GetSceneIcon() => sceneIcon;
        public SceneAsset GetBackgroundSceneAsset() => backgroundScene;
        public SceneAsset GetFightSceneAsset() => fightScene;
        public ISceneEntitiesHolder GetBasicEntities() => basicEntities;
        public SRange BasicCombatEntitiesProportion => basicEntitiesProportion;
        public SRange WeakCombatEntitiesProportion => weakEntitiesProportion;
        public float MaxBasicEntitiesAmount => maxEntitiesAllow;
        public SRange EliteEntitiesAddition => eliteEntitiesAddition;
        public SRange EliteWeakEntitiesAddition => eliteWeakEntitiesAddition;
        public float MinEliteEntitiesAmount => minEliteAmount;


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

        [Serializable]
        private sealed class EntitiesHolder : ISceneEntitiesHolder
        {
            [SerializeField,HorizontalGroup("FrontLine")] 
            private SceneEntitiesData[] vanguardType = new SceneEntitiesData[0];
            [SerializeField,HorizontalGroup("FrontLine")]
            private SceneEntitiesData[] attackerType = new SceneEntitiesData[0];
            [SerializeField,HorizontalGroup("BackLine")]
            private SceneEntitiesData[] supportType = new SceneEntitiesData[0];
            [SerializeField,HorizontalGroup("BackLine")] 
            private SceneEntitiesData[] flexType = new SceneEntitiesData[0];


            public IEnumerable<SceneEntitiesData> VanguardType => vanguardType;
            public IEnumerable<SceneEntitiesData> AttackerType => attackerType;
            public IEnumerable<SceneEntitiesData> SupportType => supportType;
            public IEnumerable<SceneEntitiesData> FlexType => flexType;
            public int TotalCount => vanguardType.Length + attackerType.Length + supportType.Length + flexType.Length;
            public SceneEntitiesData GetAnyEntity(bool includeFlexType, int maxRandomTries = 2)
            {
                SceneEntitiesData[] collection;
                bool isValid;

                var maxRandom = includeFlexType ? 3 : 2;
                for (int i = 0; i < maxRandomTries; i++)
                {
                    int random = Random.Range(0, maxRandom);
                    HandleValues(random);
                    if (isValid)
                        return GetEntity();
                }
                
                // if maxRandom surpassed > then iterate until has one entity
                for (int i = 0; i < 3; i++)
                {
                    HandleValues(i);
                    if (isValid)
                        return GetEntity();
                }


                throw new NoNullAllowedException("There aren't any entities in this EntitiesHolder");

                void HandleValues(int targetCollectionIndex)
                {
                    collection = GetCollection(targetCollectionIndex);
                    isValid = collection != null && collection.Length > 0;
                }

                SceneEntitiesData GetEntity()
                {
                    return collection[Random.Range(0, collection.Length)];
                }
            }



            private SceneEntitiesData[] GetCollection(int index)
            {
                return index switch
                {
                    // order is the spawn preference
                    0 => attackerType,
                    1 => supportType,
                    2 => vanguardType,
                    3 => flexType,
                    _ => null
                };
            }
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

    public interface IExplorationSceneDataHolder : IExplorationEntitiesSpawnRateHolder
    {
        string GetSceneName();
        Sprite GetSceneIcon();
        SceneAsset GetBackgroundSceneAsset();
        SceneAsset GetFightSceneAsset();
        ISceneEntitiesHolder GetBasicEntities();

        
    }

    public interface IExplorationEntitiesSpawnRateHolder
    {
        SRange BasicCombatEntitiesProportion { get; }
        SRange WeakCombatEntitiesProportion { get; }
        float MaxBasicEntitiesAmount { get; }

        SRange EliteEntitiesAddition { get; }
        SRange EliteWeakEntitiesAddition { get; }
        float MinEliteEntitiesAmount { get; }
    }

    public interface ISceneEntitiesHolder : ITeamFlexStructureRead<IEnumerable<SceneEntitiesData>>
    {
        public int TotalCount { get; }
        SceneEntitiesData GetAnyEntity(bool includeFlexType, int maxRandomTries = 2);
    }
}
