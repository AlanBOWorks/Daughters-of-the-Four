using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils_Project;
using Utils_Project.Scene;

namespace ExplorationSystem
{
    public sealed class WorldExplorationHandler : ISceneHiddenListener, IWorldSceneListener
    {
        [Title("Current")]
        [ShowInInspector]
        private IExplorationSceneDataHolder _currentSceneHolder;
        [ShowInInspector]
        private string _currentSceneName;
        [Title("Last")]
        [ShowInInspector]
        private string _lastSceneName;

        public int CurrentWorldIndex;

        public ref IExplorationSceneDataHolder GetDataHolder() => ref _currentSceneHolder;

        private bool IsTheSameSceneLoad() => _lastSceneName == _currentSceneName;
        public void OnStartLoading()
        {
            ExplorationSingleton.EventsHolder.OnWorldMapClose(_currentSceneHolder);
            if (IsTheSameSceneLoad() || _lastSceneName == null) return;

            SceneManager.UnloadSceneAsync(_lastSceneName);
        }

        public void OnLoadingFinish()
        {
        }
        public void OnWorldSceneOpen(IExplorationSceneDataHolder lastMap)
        {
            /*
             * VVVVVVVVVV THIS SHOULDN'T BE DONE VVVVVVVVVVVVV
             *  if (lastMap == null) CurrentWorldIndex = 0;
             * VVVVVVVVVV WARNING: VVVVVVVVVVVVV
             *
             * This should not be done because could do a [value racing] onLoadingPlayState:
             * The load could have another index value but be overriden to 0 instead its intended value  
             */
        }

        public void OnWorldMapClose(IExplorationSceneDataHolder targetMap)
        {
            if (targetMap == null)
                CurrentWorldIndex = 0; // this is a safe value override (in case this object is used outside of its Mono
        }

        public void LoadExplorationScene(IExplorationSceneDataHolder targetScene)
        {
            _lastSceneName = _currentSceneName;
            _currentSceneHolder = targetScene;

            string targetSceneName = null;
            bool loadFromBackUp = false;
            if (targetScene == null)
            {
                loadFromBackUp = true;
                const string folderPath = AssetPaths.ExplorationScenesScriptablesFolderPath;
                var assetPath = folderPath + GetNullReferenceLevelPathByLevel();
                var backUpSceneAsset = AssetDatabase.LoadAssetAtPath<SExplorationSceneDataHolder>(assetPath);
                _currentSceneHolder = backUpSceneAsset;

                string GetNullReferenceLevelPathByLevel()
                {
                    return SExplorationSceneDataHolder.NullReferencePath_EarlyLevel; //todo check currentLevel index
                }
            }
            else
            {
                var backgroundAsset = targetScene.GetBackgroundSceneAsset();
                if (backgroundAsset != null)
                {
                    targetSceneName = backgroundAsset.name;
                    bool isInvalidScenePath = SceneUtility.GetBuildIndexByScenePath(targetSceneName) < 0;
                    loadFromBackUp = isInvalidScenePath;
                }
                else
                    loadFromBackUp = true;
            }

            if (loadFromBackUp)
            {
#if UNITY_EDITOR
                Debug.LogWarning("An empty Level was introduced in the load parameters;\n" +
                                 "Loading BackUpScene");
#endif
                targetSceneName = SceneStructures.NullLevelSceneName;
            }


            _currentSceneName = targetSceneName;

            if (IsTheSameSceneLoad()) return;
            UtilsScene.DoTransitionExplorationScene(targetSceneName, true, this, 2);
        }


    }

    public interface IWorldSceneListener : IExplorationEventListener
    {
        /// <summary>
        /// Event call once the player reach the World Selection Scene;
        /// </summary>
        /// <param name="lastMap">The map which the player came from after defeating the Boss of the level;<br></br>
        /// >Note: In case of [NULL] it means that the previous map was the CharacterSelector or loading screen</param>
        void OnWorldSceneOpen(IExplorationSceneDataHolder lastMap);
        /// <summary>
        /// Event call when there's a change from the World Selection Scene(as main) to another scene(generally
        /// towards an Exploration Scene)
        /// </summary>
        /// <param name="targetMap">The target map towards the player is switching towards to;<br></br>
        /// >Note: in case of NULL it means is leaving towards the Main Menu from the World Map selection Scene</param>
        void OnWorldMapClose(IExplorationSceneDataHolder targetMap);
    }
}
