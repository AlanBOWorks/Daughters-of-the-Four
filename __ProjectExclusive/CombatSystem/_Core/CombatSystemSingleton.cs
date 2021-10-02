using System;
using CombatEntity;
using CombatSystem.Events;
using CombatTeam;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

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
            TempoTicker = new TempoTicker();

            EntitiesFixedEvents = new CombatEntityFixedEvents();

            EntityActionRequestHandler = new EntityActionRequestHandler();

            // PREPARATION Subscriptions
            CombatPreparationHandler.Subscribe(TempoTicker);

            // EVENTS Subscriptions
            EventsHolder.Subscribe(EntitiesFixedEvents);

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
        [ShowInInspector]
        public static readonly CombatPreparationHandler CombatPreparationHandler;
        [Title("Events")]
        [ShowInInspector]
        public static readonly SystemEventsHolder EventsHolder;

        [ShowInInspector] 
        public static readonly CombatEntityFixedEvents EntitiesFixedEvents;

        [Title("Tempo")]
        [ShowInInspector]
        public static readonly TempoTicker TempoTicker;

        internal static readonly EntityActionRequestHandler EntityActionRequestHandler;

        [Title("Volatile")] 
        [ShowInInspector, DisableInEditorMode] public static CombatingTeam VolatilePlayerTeam;
        [ShowInInspector, DisableInEditorMode] public static CombatingTeam VolatileEnemyTeam;

        public static UCombatSingletonInjectionHandler InjectionHandler;
    }

    internal class CombatSystemWindow : OdinEditorWindow
    {
        [ShowInInspector]
        private CombatSystemSingleton _system = CombatSystemSingleton.GetInstance();

        [MenuItem("Debug/Combat System")]
        private static void OpenWindow()
        {
            var window = GetWindow<CombatSystemWindow>();
            window.Show();
        }

    }
}
