using System.Collections;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Team;
using MEC;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace CombatSystem._Core
{
    public sealed class CombatSystemSingleton
    {
        private static readonly CombatSystemSingleton Instance = new CombatSystemSingleton();

        static CombatSystemSingleton()
        {
            var systemEventsHolder = new SystemCombatEventsHolder();

            EventsHolder 
                = systemEventsHolder;
            CombatPreparationStatesHandler 
                = new CombatPreparationStatesHandler(systemEventsHolder);
            TeamsHolder 
                = new CombatTeamsHolder();
            TeamControllers 
                = new CombatTeamControllersHandler();

            EventsHolder.Subscribe(TeamControllers);

            PrefabInstantiationHandler = new PrefabInstantiationHandler();
            EntityPrefabsPoolHandler = new EntityPrefabsPoolHandler();

            systemEventsHolder.Subscribe(EntityPrefabsPoolHandler);
        }
        private CombatSystemSingleton() { }
        public static CombatSystemSingleton GetInstance() => Instance;

        internal static PrefabInstantiationHandler PrefabInstantiationHandler;
        [ShowInInspector]
        internal static EntityPrefabsPoolHandler EntityPrefabsPoolHandler;

        [ShowInInspector, DisableInEditorMode, DisableInPlayMode]
        internal static GameObject CombatHolderNotDestroyReference;

        [ShowInInspector, DisableInEditorMode, DisableInPlayMode]
        public static GameObject AliveGameObjectReference { get; internal set; }


        public static CombatPreparationStatesHandler CombatPreparationStatesHandler { get; }
        // ------- EVENTS ------
        [ShowInInspector]
        public static SystemCombatEventsHolder EventsHolder { get; private set; }

        // ------- ESSENTIALS ------
        internal static UTeamFullGroupStructure<Transform> PlayerPositionTransformReferences { get; set; }
        internal static UTeamFullGroupStructure<Transform> EnemyPositionTransformReferences { get; set; }

        // ------- TEAM ------
        public static IReadOnlyList<CombatEntity> AllMembers => AllMembersCollection;
        [Title("Team")]
        [ShowInInspector,DisableInEditorMode]
        internal static List<CombatEntity> AllMembersCollection { private get; set; }

        public static CombatTeamsHolder TeamsHolder;

        [ShowInInspector, HorizontalGroup("Teams"), DisableInEditorMode]
        public static CombatTeam PlayerTeam => TeamsHolder.PlayerTeamType;
        [ShowInInspector, HorizontalGroup("Teams"), DisableInEditorMode]
        public static CombatTeam OppositionTeam => TeamsHolder.EnemyTeamType;


        [Title("Controllers")]
        [ShowInInspector]
        public static CombatTeamControllersHandler TeamControllers { get; private set; }


        // ------- TEMPO ------
        [Title("Tempo")]
        [ShowInInspector, DisableInEditorMode]
        public static TempoTicker TempoTicker { get; internal set; }

        public static CoroutineHandle MasterCoroutineHandle;

        public static CoroutineHandle LinkCoroutineToMaster(
            in IEnumerator<float> coroutineIterator, in MEC.Segment tickSegment = Segment.RealtimeUpdate)
        {
            CoroutineHandle handle = Timing.RunCoroutine(coroutineIterator, tickSegment, AliveGameObjectReference);
            LinkCoroutineToMaster(in handle);
            return handle;
        }
        public static void LinkCoroutineToMaster(in CoroutineHandle handle)
        {
            Timing.LinkCoroutines(MasterCoroutineHandle, handle);
        }

    }


    public class CombatSingletonEditorWindow : OdinEditorWindow
    {
        [ShowInInspector]
        private CombatSystemSingleton _singleton = CombatSystemSingleton.GetInstance();

        [MenuItem("Combat/Debug/Combat (Core) Singleton", priority = -10)]
        private static void OpenWindow()
        {
            GetWindow<CombatSingletonEditorWindow>().Show();
        }
    }

    public class CombatTeamDebugWindow : OdinEditorWindow
    {
        [ShowInInspector] 
        private static CombatTeamsHolder Teams { get; set; } = CombatSystemSingleton.TeamsHolder;

        [MenuItem("Combat/Debug/Teams [WINDOW]", priority = -1)]
        private static void OpenWindow()
        {
            GetWindow<CombatTeamDebugWindow>().Show();
        }
    }
}
