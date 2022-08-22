using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils_Project;
using Utils_Project.Scene;

namespace ExplorationSystem
{
    public sealed class WorldExplorationHandler : ISceneHiddenListener
    {
        [Title("Current")]
        [ShowInInspector]
        private IExplorationSceneDataHolder _currentSceneHolder;
        [ShowInInspector]
        private string _currentSceneName;
        [Title("Last")]
        [ShowInInspector]
        private string _lastSceneName;


        public ref IExplorationSceneDataHolder GetDataHolder() => ref _currentSceneHolder;


        public void LoadExplorationScene(IExplorationSceneDataHolder targetScene)
        {
            _lastSceneName = _currentSceneName;
            _currentSceneHolder = targetScene;

            string targetSceneName = null;
            bool loadFromBackUp;
            if (targetScene == null)
            {
                loadFromBackUp = true;
                var backUpSceneAsset = AssetDatabase.LoadAssetAtPath<SExplorationSceneDataHolder>(
                    NullReferenceAssetPaths.ExplorationSceneAssetPath);
                targetScene = backUpSceneAsset;
            }
            else
            {
                targetSceneName = targetScene.GetBackgroundSceneAsset().name;
                bool isInvalidScenePath = SceneUtility.GetBuildIndexByScenePath(targetSceneName) < 0;
                loadFromBackUp = isInvalidScenePath;
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

            // Event Call in here
            PlayerExplorationSingleton.EventsHolder.OnWorldMapClose(targetScene);
            // Event Call in here

            if (IsTheSameSceneLoad()) return;
            UtilsScene.DoTransitionExplorationScene(targetSceneName, true, this, 2);
        }

        private bool IsTheSameSceneLoad() => _lastSceneName == _currentSceneName;
        public void OnStartLoading()
        {
            if(IsTheSameSceneLoad() || _lastSceneName == null) return;

            SceneManager.UnloadSceneAsync(_lastSceneName);
        }

        public void OnLoadingFinish()
        {
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
