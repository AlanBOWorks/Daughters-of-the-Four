using System.Collections;
using System.Collections.Generic;
using CombatSystem.Animations;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Skills.VanguardEffects;
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
            CombatFinishHandler 
                = new CombatFinishHandler();
            TeamsHolder
                = new CombatTeamsHolder();
            TeamControllers 
                = new CombatTeamControllersHandler();
            SkillQueuePerformer = new SkillQueuePerformer();
            VanguardEffectsHandler = new VanguardEffectsHandler();

            KnockOutHandler = new KnockOutHandler();
            CombatControllerAnimationHandler = new CombatControllerAnimationHandler();


            PrefabInstantiationHandler = new AssetPrefabInstantiationHandler();
            EntityPrefabsPoolHandler = new EntityPrefabsPoolHandler();

            var controlGainHandler = new CombatTeamControlGainHandler();

            systemEventsHolder.Subscribe(CombatFinishHandler);
            systemEventsHolder.Subscribe(KnockOutHandler);
            systemEventsHolder.Subscribe(SkillQueuePerformer);
            systemEventsHolder.Subscribe(VanguardEffectsHandler);
            systemEventsHolder.Subscribe(CombatControllerAnimationHandler);
            systemEventsHolder.Subscribe(TeamControllers);
            systemEventsHolder.Subscribe(EntityPrefabsPoolHandler);
            systemEventsHolder.Subscribe(controlGainHandler);
        }
        private CombatSystemSingleton() { }
        public static CombatSystemSingleton GetInstance() => Instance;



        internal static AssetPrefabInstantiationHandler PrefabInstantiationHandler;
        [ShowInInspector]
        internal static EntityPrefabsPoolHandler EntityPrefabsPoolHandler;

        [ShowInInspector, DisableInEditorMode, DisableInPlayMode]
        internal static GameObject CombatHolderNotDestroyReference;



        public static CombatPreparationStatesHandler CombatPreparationStatesHandler { get; }
        // ------- EVENTS ------
        [ShowInInspector]
        public static SystemCombatEventsHolder EventsHolder { get; private set; }
        public static ICombatFinishHandler CombatFinishHandler { get; }

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

        // ------- SKILLS ------
        public static readonly SkillQueuePerformer SkillQueuePerformer;
        public static readonly VanguardEffectsHandler VanguardEffectsHandler;


        // ------- TARGETING ------
        [Title("Combat Behaviors")]
        [ShowInInspector] 
        public static readonly KnockOutHandler KnockOutHandler;

        // ------- TEMPO ------
        [Title("Tempo")]
        [ShowInInspector, DisableInEditorMode]
        public static TempoTicker TempoTicker { get; internal set; }


        // ------- ANIMATIONS ------
        [Title("Animator")] 
        public static readonly CombatControllerAnimationHandler CombatControllerAnimationHandler;


        public static bool GetIsCombatActive() => TempoTicker.IsTicking();
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
