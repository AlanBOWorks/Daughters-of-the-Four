using System.Collections;
using System.Collections.Generic;
using CombatSystem.Animations;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Stats;
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
            SkillTargetingHandler 
                = new SkillTargetingHandler();

            var skillsQueue = new SkillQueuePerformer();

            KnockOutHandler = new KnockOutHandler();
            CombatAnimationHandler = new CombatAnimationHandler();


            PrefabInstantiationHandler = new AssetPrefabInstantiationHandler();
            EntityPrefabsPoolHandler = new EntityPrefabsPoolHandler();


            systemEventsHolder.Subscribe(KnockOutHandler);
            systemEventsHolder.Subscribe(skillsQueue);
            systemEventsHolder.Subscribe(CombatAnimationHandler);
            systemEventsHolder.Subscribe(TeamControllers);
            systemEventsHolder.Subscribe(EntityPrefabsPoolHandler);
        }
        private CombatSystemSingleton() { }
        public static CombatSystemSingleton GetInstance() => Instance;

        public static bool GetIsCombatActive() => AliveGameObjectReference;


        internal static AssetPrefabInstantiationHandler PrefabInstantiationHandler;
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
        [ShowInInspector]
        internal static UTeamFullGroupStructure<Transform> PlayerPositionTransformReferences { get; set; }
        [ShowInInspector]
        internal static UTeamFullGroupStructure<Transform> EnemyPositionTransformReferences { get; set; }

        // ------- TEAM ------
        [Title("Team")]
        [ShowInInspector,DisableInEditorMode]
        internal static IReadOnlyCollection<CombatEntity> AllMembersCollection { private get; set; }

        public static CombatTeamsHolder TeamsHolder;

        [ShowInInspector, HorizontalGroup("Teams"), DisableInEditorMode]
        public static CombatTeam PlayerTeam => TeamsHolder.PlayerTeamType;
        [ShowInInspector, HorizontalGroup("Teams"), DisableInEditorMode]
        public static CombatTeam OppositionTeam => TeamsHolder.EnemyTeamType;


        [Title("Controllers")]
        [ShowInInspector]
        public static CombatTeamControllersHandler TeamControllers { get; private set; }

        // ------- TARGETING ------
        [Title("Combat Behaviors")]
        [ShowInInspector]
        public static readonly SkillTargetingHandler SkillTargetingHandler;

        [ShowInInspector] 
        public static readonly KnockOutHandler KnockOutHandler;

        // ------- TEMPO ------
        [Title("Tempo")]
        [ShowInInspector, DisableInEditorMode]
        public static TempoTicker TempoTicker { get; internal set; }


        // ------- ANIMATIONS ------
        [Title("Animator")] 
        public static readonly CombatAnimationHandler CombatAnimationHandler;



        public const int CombatCoroutineLayer = 4;
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


        public static void Clear()
        {
            Timing.KillCoroutines(CombatCoroutineLayer);
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
