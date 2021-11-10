using System.Collections.Generic;
using CombatEffects;
using CombatEntity;
using CombatSkills;
using CombatSystem.Animator;
using CombatSystem.Events;
using CombatSystem.PositionHandlers;
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
            EntitiesFixedEvents = new CombatEntityFixedEvents();
            DamageReceiveEvents = new DamageReceiveEventsInvoker();

            EntityDeathHandler = new CombatEntityDeathHandler();

            EntityActionRequestHandler = new EntityActionRequestHandler();
            TempoTicker = new TempoTicker(EntityActionRequestHandler);

            SceneTracker = new CombatSceneTracker();

            // ---->>>> Secondaries
            var teamEventsDiscriminator = new TeamEventsDiscriminator();
            var actionEventDiscriminator = new CombatingActionEventDiscriminator();
            var combatPositionSpawner = new CombatPositionSpawner();
            var initialAnimationRequester = new InitialAnimationsRequester();
            var animationEventsHelper = new AnimationEventsHelper();
            AllEntities = new List<CombatingEntity>();

            // ---->>>> PREPARATION Subscriptions
            CombatPreparationHandler.Subscribe((ICombatPreparationListener) 
                TempoTicker);
            CombatPreparationHandler.Subscribe((ICombatDisruptionListener) 
                TempoTicker);
            CombatPreparationHandler.Subscribe(
                teamEventsDiscriminator);
            CombatPreparationHandler.Subscribe((ICombatPreparationListener) 
                combatPositionSpawner);
            CombatPreparationHandler.Subscribe((ICombatDisruptionListener) 
                combatPositionSpawner);
            CombatPreparationHandler.Subscribe(
                initialAnimationRequester);

            // ---->>>> EVENTS Subscriptions
            EventsHolder.Subscribe((ITempoListener<CombatingEntity>) 
                EntitiesFixedEvents);
            EventsHolder.Subscribe((IRoundListener<CombatingEntity>) 
                EntitiesFixedEvents);
            EventsHolder.Subscribe((IOffensiveActionReceiverListener<CombatEntityPairAction,CombatingSkill,SkillComponentResolution>) 
                EntitiesFixedEvents);
            EventsHolder.Subscribe((ISupportActionReceiverListener<CombatEntityPairAction, CombatingSkill, SkillComponentResolution>) 
                EntitiesFixedEvents);
            EventsHolder.Subscribe((ISkillEventListener)
                EntitiesFixedEvents);
            EventsHolder.Subscribe(
                teamEventsDiscriminator);
            EventsHolder.Subscribe(
                actionEventDiscriminator);
            EventsHolder.Subscribe(
                animationEventsHelper);

            // Second because the Characters could have an event that changes the value of some event's Invoker (suck reducing damage)
            // OnReceiveOffensive, so this can represent the real damage afterwards
            EventsHolder.Subscribe(DamageReceiveEvents);


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
        public static readonly DamageReceiveEventsInvoker DamageReceiveEvents;
        public static readonly CombatEntityDeathHandler EntityDeathHandler;

        [Title("Tempo")]
        [TabGroup("ReadOnly"), ShowInInspector]
        public static readonly TempoTicker TempoTicker;

        [Title("Skills")]
        [TabGroup("ReadOnly"), ShowInInspector]
        internal static readonly EntityActionRequestHandler EntityActionRequestHandler;


        // Temporal can remain more than couple combats (or until close app) but could be modified by external reasons
        [TabGroup("Temporal"), ShowInInspector, DisableInEditorMode]
        public static UPositionProviderBase PositionProvider;
       

        [TabGroup("Combat Only"), ShowInInspector, DisableInEditorMode] 
        public static List<CombatingEntity> AllEntities;
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
