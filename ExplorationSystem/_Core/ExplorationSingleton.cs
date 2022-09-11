using CombatSystem.Team;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace ExplorationSystem
{
    public sealed class ExplorationSingleton
    {
        public static readonly ExplorationSingleton Instance;


        static ExplorationSingleton()
        {
            Instance = new ExplorationSingleton();

            WorldExplorationHandler = new WorldExplorationHandler();
            EventsHolder = new ExplorationEventsHolder();


#if UNITY_EDITOR
            ExplorationEventsDebugLogs = new ExplorationEventsDebugLogs();
            EventsHolder.Subscribe(ExplorationEventsDebugLogs);
#endif

            EventsHolder.Subscribe(WorldExplorationHandler);
        }

        [Title("Core")]
        [ShowInInspector]
        public static readonly WorldExplorationHandler WorldExplorationHandler;
        [Title("Events")]
        [ShowInInspector]
        public static readonly ExplorationEventsHolder EventsHolder;

        [Title("Enemies")]
        public static IExplorationThreatsStructureRead<ICombatTeamProvider> SceneEnemyTeamsHolder { get; internal set; }

#if UNITY_EDITOR
        internal static readonly ExplorationEventsDebugLogs ExplorationEventsDebugLogs;
#endif



        private sealed class ExplorationSingletonWindow : OdinEditorWindow
        {
            [ShowInInspector] 
            private ExplorationSingleton _explorationSingleton;


            [MenuItem("Game/Debug/Exploration [Singleton]", priority = -110)]
            private static void ShowWindow()
            {
                var window = GetWindow<ExplorationSingletonWindow>();
                window._explorationSingleton = Instance;
            }
        }
    }
}
