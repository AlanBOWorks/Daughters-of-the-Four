using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        public const string NullReferencePath_EarlyLevel = "(E) NULL Zero Zone [ExplorationScene Data].asset";
        public const string NullReferencePath_MidLevel = "(C) NULL Zero Zone [ExplorationScene Data].asset";
        public const string NullReferencePath_LateLevel = "(A) NULL Zero Zone [ExplorationScene Data].asset";



        [Title("Info")] [SerializeField] private string sceneName = "NULL";

        [SerializeField, PreviewField, GUIColor(.4f, .4f, .4f)]
        private Sprite sceneIcon;

        [Title("Scenes")] [SerializeField] private SceneAsset backgroundScene;
        [SerializeField] private SceneAsset fightScene;
        [SerializeField, EnumToggleButtons, GUIColor(.6f,.6f,.7f)] 
        private EnumExploration.ExplorationTier sceneTier;

        [Title("Entities")] 
        [SerializeReference]
        private ExplorationSceneEntitiesHolder entitiesHolder = new ExplorationSceneEntitiesHolder();

        [SerializeReference, Tooltip("This entities will be used only after the Half Map is explored " +
                                     "(this is for difficulty curve reason")]
        private IExplorationSceneEntitiesHolder halfMapEntitiesHolder;



        // Getters
        public string GetSceneName() => sceneName;
        public Sprite GetSceneIcon() => sceneIcon;
        public EnumExploration.ExplorationTier GetTier() => sceneTier;

        public SceneAsset GetBackgroundSceneAsset() => backgroundScene;
        public SceneAsset GetFightSceneAsset() => fightScene;


        public IExplorationSceneEntitiesHolder GetEntities() => entitiesHolder;
        public IExplorationSceneEntitiesHolder GetHalfMapEntities() => halfMapEntitiesHolder ?? entitiesHolder;



        [Button]
        private void UpdateAssetName()
        {
            string targetName;
            if (backgroundScene)
                targetName = $"{sceneName.ToUpper()} - {backgroundScene.name}";
            else
                targetName = sceneName;

            UtilsAssets.UpdateAssetName(this, $"({sceneTier.ToString()}) {targetName} {AssetPrefix}");
        }

        [Serializable]
        private sealed class ExplorationSceneEntitiesHolder : IExplorationSceneEntitiesHolder
        {
            [TabGroup("Basics")] 
            [SerializeReference] private ISceneEntitiesHolder basicEntities;
            [TabGroup("Basics")] 
            [SerializeReference] private ISceneOffEntitiesHolder basicOffMembers;
            [TabGroup("Basics")] 
            [SerializeReference] private ISceneAdditionEntitiesHolder additionMembers;

            [TabGroup("Elites")] 
            [SerializeReference, Tooltip("The stronger teams are elected from here;\n" +
                                         "On Null will use the basic entities instead")]
            private ISceneEntitiesHolder eliteEntities;
            [TabGroup("Elites")] 
            [SerializeReference] private ISceneOffEntitiesHolder offEliteMembers;
            [ShowIf("UseAdditionMembers")]
            [TabGroup("Elites")] 
            [SerializeField] private bool onNullUseAdditionMembers = true;
            [TabGroup("Elites")] 
            [SerializeReference] private ISceneAdditionEntitiesHolder additionEliteMembers;
            private bool UseAdditionMembers() => additionEliteMembers == null && additionMembers != null;


            public IEnumerable<ICombatEntityProvider> GetBasicEntities()
            {
                var team = GetTeam(basicEntities, basicOffMembers);
                team = DoAdditions(team, additionMembers);
                return team;
            }

            public IEnumerable<ICombatEntityProvider> GetEliteEntities()
            {
                var team = GetTeam(eliteEntities, offEliteMembers);

                if (eliteEntities != null)
                    team = DoAdditions(team, additionEliteMembers);
                else if (onNullUseAdditionMembers) 
                    team = DoAdditions(team, additionMembers);

                return team;
            }

            private IEnumerable<ICombatEntityProvider> GetTeam(
                ISceneEntitiesHolder entitiesHolder, ISceneOffEntitiesHolder offMembers)
            {
                var mainEntities = entitiesHolder.GetTeam();
                if (offMembers == null) return mainEntities.GetEnumerable();

                var offMembersSelection = offMembers.GetOffMembers(in mainEntities);
                return mainEntities.GenerateConcatenation(offMembersSelection);
            }

            private IEnumerable<ICombatEntityProvider> DoAdditions(
                IEnumerable<ICombatEntityProvider> baseTeam, ISceneAdditionEntitiesHolder additions)
            {
                if (additions == null) return baseTeam;
                return baseTeam.Concat(additions.GetAdditionalMembers());
            }
        }

    }

    public interface IExplorationSceneDataHolder :
        IExplorationInfoHolder,
        IExplorationSceneAssetsHolder
    {

        IExplorationSceneEntitiesHolder GetEntities();

        IExplorationSceneEntitiesHolder GetHalfMapEntities();
    }

    public interface IExplorationInfoHolder
    {
        string GetSceneName();
        Sprite GetSceneIcon();
        EnumExploration.ExplorationTier GetTier();
    }

    public interface IExplorationSceneAssetsHolder
    {
        SceneAsset GetBackgroundSceneAsset();
        SceneAsset GetFightSceneAsset();
    }
}
