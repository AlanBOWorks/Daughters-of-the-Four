using System;
using CombatEntity;
using CombatSystem.Events;
using CombatSystem.PositionHandlers;
using CombatTeam;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CombatSystem
{
    //It's a Singleton instead of a static class for debugging reasons
    public sealed class CombatSystemSingleton
    {
        private static readonly CombatSystemSingleton Instance = new CombatSystemSingleton();

        static CombatSystemSingleton()
        {
            CombatPreparationHandler = new CombatPreparationHandler();
            EventsHolder = new SystemEventsHolder();
            EntitiesFixedEvents = new CombatEntityFixedEvents();

            TempoTicker = new TempoTicker();
            EntityActionRequestHandler = new EntityActionRequestHandler();

            SceneTracker = new CombatSceneTracker();

            // Secondaries
            var combatPositionSpawner = new CombatPositionSpawner();

            // PREPARATION Subscriptions
            CombatPreparationHandler.Subscribe(TempoTicker);
            CombatPreparationHandler.Subscribe((ICombatPreparationListener) combatPositionSpawner);
            CombatPreparationHandler.Subscribe((ICombatFinishListener) combatPositionSpawner);

            // EVENTS Subscriptions
            EventsHolder.Subscribe(EntitiesFixedEvents);

            // TEMPO Subscription
            TempoTicker.Subscribe((ITempoListener<CombatingEntity>) EventsHolder);
            TempoTicker.Subscribe((IRoundListener<CombatingEntity>) EventsHolder);

#if UNITY_EDITOR
            if (_singletonExplanation != null)
                Debug.Log("[Combat Singleton] instantiated");
#endif

        }

        private CombatSystemSingleton()
        {
        }

        public static CombatSystemSingleton GetInstance() => Instance;


#if UNITY_EDITOR
        [Title("Info"),ShowInInspector, DisableIf("_singletonExplanation")]
        private static string _singletonExplanation =
    "The main System of the Combat. Holds all behaviors and events in a static way";
#endif
        [Title("Preparation")]
        [TabGroup("ReadOnly"), ShowInInspector]
        public static readonly CombatPreparationHandler CombatPreparationHandler;
        [TabGroup("ReadOnly"), ShowInInspector]
        public static readonly CombatSceneTracker SceneTracker;

        [Title("Events")]
        [TabGroup("ReadOnly"), ShowInInspector]
        public static readonly SystemEventsHolder EventsHolder;

        [TabGroup("ReadOnly"), ShowInInspector]
        public static readonly CombatEntityFixedEvents EntitiesFixedEvents;

        [Title("Tempo")]
        [TabGroup("ReadOnly"), ShowInInspector]
        public static readonly TempoTicker TempoTicker;

        [Title("Skills")]
        [TabGroup("ReadOnly"), ShowInInspector]
        internal static readonly EntityActionRequestHandler EntityActionRequestHandler;
        
        [TabGroup("Temporal"), ShowInInspector, DisableInEditorMode]
        public static UPositionProviderBase PositionProvider;


        [Title("Volatile")] 
        [TabGroup("Combat Only"), ShowInInspector, DisableInEditorMode] 
        public static CombatingTeam VolatilePlayerTeam;
        [TabGroup("Combat Only"), ShowInInspector, DisableInEditorMode] 
        public static CombatingTeam VolatileEnemyTeam;




    }

    public class CombatSystemWindow : OdinEditorWindow
    {
        [ShowInInspector]
        private CombatSystemSingleton _system = CombatSystemSingleton.GetInstance();

        [MenuItem("Debug/Combat System (MAIN)", false, -100)]
        private static void OpenWindow()
        {
            var window = GetWindow<CombatSystemWindow>();
            window.Show();
        }

    }
}
