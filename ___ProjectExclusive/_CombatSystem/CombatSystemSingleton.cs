using System;
using _Player;
using _Team;
using Characters;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Skills;
using Stats;
using UnityEditor;

namespace _CombatSystem
{
    public sealed class CombatSystemSingleton
    {
        static CombatSystemSingleton() { }
        public static CombatSystemSingleton Instance { get; } = new CombatSystemSingleton();

        private CombatSystemSingleton()
        {
            Invoker = new SystemInvoker();
            ControllersHandler = new CombatControllersHandler();
            TeamsPersistentElements = new PersistentElementsDictionary();
            var combatControlDeclaration = new CombatControlDeclaration();

            Characters = new CombatCharactersHolder();
            CharacterEventsTracker = new CharacterEventsTracker();
            GlobalCharacterChangesEvent = new CombatEvents();

            StaticDamageHandler = new StaticDamageHandler();

            PerformSkillHandler = new PerformSkillHandler();
            SkillUsagesEvent = new SkillUsagesEvent();
            OnDeathBackUpSkillInjector = new OnDeathSkillInjector();

            TempoHandler = new TempoHandler();
            TeamStateTicker = new TeamTempoTicker();
            RoundCheckHandler = new RoundCheckHandler();

            CombatEventsInvoker = new CombatEventsInvoker(GlobalCharacterChangesEvent,TeamsPersistentElements);


            //---- Injections ----
            Invoker.SubscribeListener(Characters);
            Invoker.SubscribeListener(TempoHandler);
            Invoker.SubscribeListener(combatControlDeclaration);
            Invoker.SubscribeListener(RoundCheckHandler);
            Invoker.SubscribeListener(TeamStateTicker);
            Invoker.SubscribeListener(TeamsPersistentElements);
            Invoker.SubscribeListener(CombatEventsInvoker);

            // Round check first since is better to finish the combat before all other unnecessary task
            TempoHandler.Subscribe(StaticDamageHandler);

            TempoHandler.Subscribe(TeamStateTicker);

            TempoHandler.Subscribe((ITempoListener)RoundCheckHandler);
            TempoHandler.Subscribe((ISkippedTempoListener)RoundCheckHandler);

            //---- Events
            GlobalCharacterChangesEvent.Subscribe(OnDeathBackUpSkillInjector);

        }


        [Title("Instantiated","General")]
        [ShowInInspector]
        public static SystemInvoker Invoker { get; private set; }
        public static CombatEventsInvoker CombatEventsInvoker { get; private set; }
        [ShowInInspector]
        public static CombatControllersHandler ControllersHandler { get; private set; }

        [ShowInInspector]
        public static PersistentElementsDictionary TeamsPersistentElements { get; private set; }

        [Title("Instantiated","Characters")]
        [ShowInInspector]
        public static CombatCharactersHolder Characters { get; private set; }
        [ShowInInspector]
        public static CharacterEventsTracker CharacterEventsTracker { get; private set; }
        [ShowInInspector] 
        public static CombatEvents GlobalCharacterChangesEvent { get; private set; }


        [Title("Instantiated", "Stats")]
        [ShowInInspector]
        public static StaticDamageHandler StaticDamageHandler { get; private set; }


        [Title("Instantiated","Skills")]
        [ShowInInspector]
        public static PerformSkillHandler PerformSkillHandler { get; private set; }
        [ShowInInspector] 
        public static SkillUsagesEvent SkillUsagesEvent { get; private set; }
        [ShowInInspector]
        public static OnDeathSkillInjector OnDeathBackUpSkillInjector { get; private set; }


        [Title("Instantiated","Tempo")]
        [ShowInInspector]
        public static TempoHandler TempoHandler { get; private set; }
        [ShowInInspector]
        public static TeamTempoTicker TeamStateTicker { get; private set; }
        [ShowInInspector]
        public static RoundCheckHandler RoundCheckHandler { get; private set; }



        [Title("Injected")]
        [ShowInInspector]
        public static CombatTeamControlsHandler CombatTeamControlHandler;
        [ShowInInspector]
        public static CombatConditionsChecker CombatConditionChecker;
        [ShowInInspector]
        public static SCombatParams ParamsVariable = null;
    }

    public static class GlobalCombatParams
    {
        /// <summary>
        /// A modifier that alters how much the velocity affects each Tempo Tick
        /// </summary>
        public const float TempoVelocityModifier = .01f;


        public const float InitiativeCheck = 1f;
        /// <summary>
        /// Max of actions per initiative once the characters is in [<see cref="CombatCharacterEventsBase.OnInitiativeTrigger"/>]
        /// </summary>
        public const int ActionsPerInitiativeCap = 8;
        public const int ActionsLowerCap = -2;


        public const int PredictedAmountOfCharacters = UtilsCharacter.PredictedAmountOfCharactersInBattle;
        public const int PredictedAmountOfTeamCharacters = UtilsCharacterArchetypes.AmountOfArchetypesAmount;
        public const int PredictedAmountOfTeams = 2;
    }


    public class CombatSystemMenu : OdinEditorWindow
    {
        [MenuItem("Game Menus/Combat System")]
        private static void OpenWindow()
        {
            CombatSystemMenu menu = GetWindow<CombatSystemMenu>();
            menu.Singleton = CombatSystemSingleton.Instance;
            menu.Invoker = CombatSystemSingleton.Invoker;
            menu.Show();
        }

        private void OnFocus()
        {
            OpenWindow();
        }
        [NonSerialized, ShowInInspector]
        public CombatSystemSingleton Singleton = null;

        [Title("Invoker")] 
        [NonSerialized, ShowInInspector]
        public SystemInvoker Invoker = null;
    }
}
