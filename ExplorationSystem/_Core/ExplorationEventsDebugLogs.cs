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

        public void OnWorldSelectSceneLoad(IExplorationSceneDataHolder sceneData)
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

        [Serializable]
        private sealed class ExplorationSubmitLogs
        {
            public bool onExplorationRequest = true;
            public bool onCombatRequest = true;
            public bool onReturnFromCombat = true;
        }

        [ShowInInspector] private bool _showOnExplorationSubmitLogs = false;
        [ShowInInspector, ShowIf("_showOnExplorationSubmitLogs")]
        private ExplorationSubmitLogs _explorationSubmitLogs = new ExplorationSubmitLogs();
        public void OnExplorationRequest(EnumExploration.ExplorationType type)
        {
            if(!_showOnExplorationSubmitLogs || !_explorationSubmitLogs.onExplorationRequest) return;
            Debug.Log($"Exploration Submit: {type}");
        }

        public void OnExplorationCombatLoadFinish(EnumExploration.ExplorationType type)
        {
            if(!_showOnExplorationSubmitLogs || !_explorationSubmitLogs.onCombatRequest) return;
            Debug.Log($"Combat type: {type}");
        }

        public void OnExplorationReturnFromCombat(EnumExploration.ExplorationType fromCombatType)
        {
            if(!_showOnExplorationSubmitLogs || !_explorationSubmitLogs.onReturnFromCombat) return;
            Debug.Log($"From combat type: {fromCombatType}");
        }


        private sealed class ExplorationEventsDebugWindow : OdinEditorWindow
        {
            [ShowInInspector]
            private ExplorationEventsDebugLogs _debugLogs;

            [MenuItem("Game/Debug/Exploration DeLogs [EVENTS]", priority =  2)]
            private static void OpenWindow()
            {
                var window = GetWindow<ExplorationEventsDebugWindow>();
                window._debugLogs = ExplorationSingleton.ExplorationEventsDebugLogs;
            }
        }

    }
}
