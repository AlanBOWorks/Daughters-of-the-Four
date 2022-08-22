using System;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace ExplorationSystem
{
    public sealed class ExplorationEventsDebugLogs : IExplorationEventsHolder
    {
        [Serializable]
        private sealed class ExplorationSceneChangeLogs
        {
            public bool onSceneChange = true;
        }

        [ShowInInspector] private bool _showOnSceneChangeLogs = false;
        [ShowInInspector, ShowIf("_showOnSceneChangeLogs")] 
        private ExplorationSceneChangeLogs _explorationSceneChangeLogs = new ExplorationSceneChangeLogs();

        public void OnSceneChange(IExplorationSceneDataHolder sceneData)
        {
            if(!_showOnSceneChangeLogs || !_explorationSceneChangeLogs.onSceneChange) return;
            Debug.Log("Scene changes towards: " +sceneData.GetSceneName());
        }

        [Serializable]
        private sealed class WorldSceneLogs
        {
            public bool onWorldSceneOpen = true;
            public bool onWorldSceneClose = true;
        }

        [ShowInInspector] private bool _showOnWorldSceneLogs = false;
        [ShowInInspector,ShowIf("_showOnWorldSceneLogs")]
        private WorldSceneLogs _worldSceneLogs = new WorldSceneLogs();

        public void OnWorldSceneOpen(IExplorationSceneDataHolder lastMap)
        {
            if(!_showOnWorldSceneLogs || !_worldSceneLogs.onWorldSceneOpen) return;

            string targetLog;
            if (lastMap != null)
                targetLog = "Returns to World map from: " + lastMap.GetSceneName();
            else
                targetLog = "Opened the World Scene Map from Main Menu/Load Scene";

            Debug.Log(targetLog);
        }

        public void OnWorldMapClose(IExplorationSceneDataHolder targetMap)
        {
            if (!_showOnWorldSceneLogs || !_worldSceneLogs.onWorldSceneClose) return;

            string targetLog;
            if (targetMap != null)
                targetLog = "Going to Exploration map from World Map >>> " + targetMap.GetSceneName();
            else
                targetLog = "Closing the World Map and returning towards Main Menu";

            Debug.Log(targetLog);
        }


        private sealed class ExplorationEventsDebugWindow : OdinEditorWindow
        {
            [ShowInInspector]
            private ExplorationEventsDebugLogs _debugLogs;

            [MenuItem("Game/Debug/Player Exploration [EVENTS]", priority =  2)]
            private static void OpenWindow()
            {
                var window = GetWindow<ExplorationEventsDebugWindow>();
                window._debugLogs = PlayerExplorationSingleton.ExplorationEventsDebugLogs;
            }
        }
    }
}
