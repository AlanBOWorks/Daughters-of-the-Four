using System.Collections.Generic;
using CombatSystem._Core;
using ExplorationSystem;
using MEC;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils_Project;
using LoadSceneParameters = Utils_Project.LoadSceneParameters;

namespace _Injectors.ExplorationCombat
{
    public class ExplorationCombatScenesTransitionHandler : IExplorationSubmitListener, 
        IWorldSceneChangeListener,
        IExplorationOnCombatListener,

        ICombatTerminationListener
    {
        private Scene _worldSelectionScene;
        private IExplorationSceneDataHolder _sceneDataHolder;
        [Title("Exploration Scene")]
        private Scene _currentExplorationScene;

        [Title("Combat Scenes")] 
        private Scene _currentCombatScene;

        public void OnWorldSceneEnters(IExplorationSceneDataHolder lastMap)
        {
            _worldSelectionScene = SceneManager.GetActiveScene();
        }

        public void OnWorldSceneSubmit(IExplorationSceneDataHolder targetMap)
        {
        }

        public void OnWorldSelectSceneLoad(IExplorationSceneDataHolder sceneData)
        {
            _sceneDataHolder = sceneData;

            var explorationMap = sceneData.GetBackgroundSceneAsset();
            _currentExplorationScene = SceneManager.GetSceneByName(explorationMap.name);
        }

        private EnumExploration.ExplorationType _requestedType;
        public void OnExplorationRequest(EnumExploration.ExplorationType type)
        {
            switch (type)
            {
                case EnumExploration.ExplorationType.BasicThreat:
                case EnumExploration.ExplorationType.EliteThreat:
                    LoadBasicCombatBackground();
                    break;
                case EnumExploration.ExplorationType.BossThreat:
                    LoadBossCombatBackground();
                    break;
            }

            _requestedType = type;
        }


        private void LoadBasicCombatBackground()
        {
            var targetScene = _sceneDataHolder.GetFightSceneAsset();
            GoToCombatMap(targetScene);
        }

        private void LoadBossCombatBackground()
        {
            var targetScene = _sceneDataHolder.GetFightSceneAsset(); //todo getBossFightScene
            GoToCombatMap(targetScene);
        }

        private const string OnNullLoadCombatScene = AssetPaths.InGameScenesCombatFolder + "[CombatBackground]OnNullSceneBackUp.unity";
        private SceneAsset _currentCombatAsset;
        private void GoToCombatMap(SceneAsset targetScene)
        {
            if (targetScene == null)
            {
                targetScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(OnNullLoadCombatScene);
            }

            _currentCombatAsset = targetScene;

            //OnStartLoading(Combat) && OnLoadingFinish(Combat)
            var loadCallbacks = new LoadCallBacks(OnStartLoading,OnLoadingFinish);
            UtilsScene.LoadBattleScene(targetScene.name, true, loadCallbacks);
        }


        private static void ToggleSceneObjects(Scene targetScene, bool active)
        {
            if (!targetScene.isLoaded) return;

            foreach (var element in targetScene.GetRootGameObjects())
            {
                element.SetActive(active);
            }
        }

        public void OnStartLoading() //OnStartLoading(Combat)
        {
            var explorationScene = SceneManager.GetSceneByName(_currentExplorationScene.name);
            ToggleSceneObjects(explorationScene, false);
        }


        public void OnLoadingFinish() //OnLoadingFinish(Combat)
        {
            ToggleSceneObjects(_worldSelectionScene, false);
            _currentCombatScene = SceneManager.GetSceneByName(_currentCombatAsset.name);
            SceneManager.SetActiveScene(_currentCombatScene);
            ExplorationSingleton.EventsHolder.OnExplorationCombatLoadFinish(_requestedType);
        }


        public void OnExplorationCombatLoadFinish(EnumExploration.ExplorationType type)
        {
        }

        public void OnExplorationReturnFromCombat(EnumExploration.ExplorationType fromCombatType)
        {
            ToggleSceneObjects(_worldSelectionScene, false);
            ToggleSceneObjects(_currentExplorationScene, false);
        }



        // VVVVVVVVV from COMBAT
        public void OnCombatFinish(UtilsCombatFinish.FinishType finishType)
        {
            switch (finishType)
            {
                case UtilsCombatFinish.FinishType.PlayerWon:
                default:
                var callBacks = new LoadCallBacks(CallOnCombatFinishEvent, null);
                UtilsScene.DoTransition(LoadSceneParameters.LoadType.CombatLoad,.2f, 0, callBacks);
                break;
            }

            void CallOnCombatFinishEvent()
            {
                CombatSystemSingleton.EventsHolder.OnCombatFinishHide(finishType);
            }
        }

        public void OnCombatFinishHide(UtilsCombatFinish.FinishType finishType)
        {
            SceneManager.UnloadSceneAsync(_currentCombatScene);
            ToggleSceneObjects(_currentExplorationScene,true);
            ToggleSceneObjects(_worldSelectionScene, true);
            ExplorationSingleton.EventsHolder.OnExplorationReturnFromCombat(_requestedType);
        }
    }
}
