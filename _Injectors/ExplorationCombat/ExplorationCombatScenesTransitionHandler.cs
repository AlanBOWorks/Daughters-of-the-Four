using System.Collections.Generic;
using CombatSystem._Core;
using ExplorationSystem;
using MEC;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils_Project;

namespace _Injectors.ExplorationCombat
{
    public class ExplorationCombatScenesTransitionHandler : IExplorationSubmitListener, 
        IWorldSceneChangeListener,
        IExplorationOnCombatListener,
        ISceneHiddenCallBack,

        ICombatTerminationListener
    {
        private Scene _worldSelectionScene;
        private IExplorationSceneDataHolder _sceneDataHolder;
        [Title("Exploration Scene")]
        private Scene _currentExplorationScene;

        [Title("Combat Scenes")]
        [ShowInInspector, DisableInEditorMode]
        private readonly Dictionary<SceneAsset, Scene> _combatScenesPool;

        public ExplorationCombatScenesTransitionHandler()
        {
            _combatScenesPool = new Dictionary<SceneAsset, Scene>();
        }

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
        private SceneAsset _currentCombatScene;
        private void GoToCombatMap(SceneAsset targetScene)
        {
            if (targetScene == null)
            {
                targetScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(OnNullLoadCombatScene);
            }

            _currentCombatScene = targetScene;

            //OnStartLoading(Combat) && OnLoadingFinish(Combat)
            bool isFirstLoad = !_combatScenesPool.ContainsKey(targetScene);
            UtilsScene.LoadBattleScene(targetScene.name, isFirstLoad, this, 1);
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
            Scene combatScene;
            if (_combatScenesPool.ContainsKey(_currentCombatScene))
            {
                combatScene = _combatScenesPool[_currentCombatScene];
                ToggleSceneObjects(combatScene, true);
            }
            else
            {
                combatScene = SceneManager.GetSceneByName(_currentCombatScene.name);
                _combatScenesPool.Add(_currentCombatScene, combatScene);
            }

            SceneManager.SetActiveScene(combatScene);
            ExplorationSingleton.EventsHolder.OnExplorationCombatLoadFinish(_requestedType);
        }

        private IEnumerator<float> UnLoadAllCombatScenes()
        {
            foreach (var pair in _combatScenesPool)
            {
                var unloadOperation = SceneManager.UnloadSceneAsync(pair.Value);
                yield return Timing.WaitUntilDone(unloadOperation);
            }
            _combatScenesPool.Clear();
            yield return Timing.WaitForOneFrame;
        }

        public void OnExplorationCombatLoadFinish(EnumExploration.ExplorationType type)
        {
        }

        public void OnExplorationReturnFromCombat(EnumExploration.ExplorationType fromCombatType)
        {
            ToggleSceneObjects(_worldSelectionScene, false);
            ToggleSceneObjects(_currentExplorationScene, false);
        }



        // VVVVVVVVV from COMBAT todo make it to individual Class for ISceneLoadCallback
        public void OnCombatFinish(UtilsCombatFinish.FinishType finishType)
        {
            switch (finishType)
            {
                case UtilsCombatFinish.FinishType.PlayerWon:
                default:
                // todo invoke transition to explorationScene

                    break;
            }
        }

        public void OnCombatFinishHide(UtilsCombatFinish.FinishType finishType)
        {
            var combatScene = _combatScenesPool[_currentCombatScene];
            ToggleSceneObjects(combatScene,false);
        }
    }
}
