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
        public const string NullReferencePath_EarlyLevel = "[BG_Level] -0.0 NULL BACKUP [ExplorationScene Data].asset";
        public const string NullReferencePath_MidLevel = NullReferencePath_EarlyLevel; //todo new Asset
        public const string NullReferencePath_LateLevel = NullReferencePath_MidLevel; //todo new Asset



        [Title("Info")] 
        [SerializeField] private string sceneName = "NULL";
        [SerializeField, PreviewField, GUIColor(.4f,.4f,.4f)] private Sprite sceneIcon;

        [Title("Scenes")]
        [SerializeField] private SceneAsset backgroundScene;
        [SerializeField] private SceneAsset fightScene;

        [Title("Entities")] 
        [SerializeReference]
        private IExplorationSceneEntitiesHolder entitiesHolder;
        [SerializeReference, Tooltip("This entities will be used only after the Half Map is explored " +
                                     "(this is for difficulty curve reason")]
        private IExplorationSceneEntitiesHolder halfMapEntitiesHolder;

        [TitleGroup("Rates")]
        [SerializeField, Range(0, 1), SuffixLabel("00%"), ShowIf("halfMapEntitiesHolder"),
         Tooltip("The probability of spawning entities from this collection;\n" +
                 "Note: 1 means only this group will be spawned")]
        private float halfMapEntitiesSpawnChance = 1;
        [TitleGroup("Rates")]
        [SerializeReference]
        private IExplorationEntitiesSpawnRateHolder spawnRates;




        // Getters
        public string GetSceneName() => sceneName;
        public Sprite GetSceneIcon() => sceneIcon;
        public SceneAsset GetBackgroundSceneAsset() => backgroundScene;
        public SceneAsset GetFightSceneAsset() => fightScene;


        public IExplorationSceneEntitiesHolder GetEntities() => entitiesHolder;
        public IExplorationSceneEntitiesHolder GetHalfMapEntities() => halfMapEntitiesHolder;
        public IExplorationEntitiesSpawnRateHolder GetSpawnRates() => spawnRates;

        public float HalfMapEntitiesChance => halfMapEntitiesSpawnChance;


        [Button]
        private void UpdateAssetName()
        {
            if (!backgroundScene)
            {
                Debug.LogError("NULL Reference: There's no scene Asset to update the name to");
                return;
            }

            var targetName = backgroundScene.name + $" - {sceneName}";
            UtilsAssets.UpdateAssetName(this, targetName + $" {AssetPrefix}");
        }



    }


    public interface IExplorationSceneDataHolder :
        IExplorationInfoHolder,
        IExplorationSceneAssetsHolder
    {

        IExplorationSceneEntitiesHolder GetEntities();

        /// <summary>
        /// The chance of [Half Map Entities] spawning instead of the initial ones (in Unit[0,1]); <br></br>
        /// 1 == Override type
        /// </summary>
        float HalfMapEntitiesChance { get; }
        IExplorationSceneEntitiesHolder GetHalfMapEntities();
        IExplorationEntitiesSpawnRateHolder GetSpawnRates();
    }

    public interface IExplorationInfoHolder
    {
        string GetSceneName();
        Sprite GetSceneIcon();
    }

    public interface IExplorationSceneAssetsHolder
    {
        SceneAsset GetBackgroundSceneAsset();
        SceneAsset GetFightSceneAsset();
    }

    public interface IExplorationSceneEntitiesHolder
    {
        ISceneEntitiesHolder GetBasicEntities();
        ISceneEntitiesHolder GetWeakBasicEntities();
        ISceneEntitiesHolder GetEliteEntities();
    }

    public interface IExplorationEntitiesSpawnRateHolder
    {
        SRange BasicCombatWeakEntitiesAddition { get; }

        SRange EliteEntitiesAddition { get; }
    }

    public interface ISceneEntitiesHolder : ICombatTeamProvider
    {
        bool IsValid();
    }
}
