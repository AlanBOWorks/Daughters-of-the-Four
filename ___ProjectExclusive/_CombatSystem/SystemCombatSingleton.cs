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
            Characters = new CombatCharactersHolder();
            TempoHandler = new TempoHandler();
            PerformSkillHandler = new PerformSkillHandler();
            CharacterChangesEvent = new CombatCharacterEventsBase();

            // Locals declarations that wont be used further than durante the CombatInvoker
            var combatControlDeclaration = new CombatControlDeclaration();
            var skillCooldown = new SkillCooldownHandler();
            var staticDamageHandler = new StaticDamageHandler();
            var roundCheckHandler = new RoundCheckHandler();
            var burstResetHandler = new CharacterCombatDataResetHandler();
            var teamStateTicker = new TeamTempoTicker();
            var onDeathBackUpSkillInjector = new OnDeathSkillInjector();

            //---- Injections ----
            Invoker.SubscribeListener(Characters);
            Invoker.SubscribeListener(TempoHandler);
            Invoker.SubscribeListener(combatControlDeclaration);
            Invoker.SubscribeListener(roundCheckHandler);
            Invoker.SubscribeListener(teamStateTicker);

            // Round check first since is better to finish the combat before all other unnecessary task
            TempoHandler.Subscribe((ITempoListener)PerformSkillHandler);
            TempoHandler.Subscribe((ISkippedTempoListener)PerformSkillHandler);
            TempoHandler.Subscribe(skillCooldown);

            TempoHandler.Subscribe(staticDamageHandler);
            
            TempoHandler.Subscribe((ITempoListener) burstResetHandler);
            TempoHandler.Subscribe((ISkippedTempoListener)burstResetHandler);

            TempoHandler.Subscribe(teamStateTicker);

            TempoHandler.Subscribe((ITempoListener)roundCheckHandler);
            TempoHandler.Subscribe((ISkippedTempoListener)roundCheckHandler);

            //---- Events
            CharacterChangesEvent.Subscribe(onDeathBackUpSkillInjector);
        }

        [ShowInInspector]
        public static PerformSkillHandler PerformSkillHandler;
        [ShowInInspector]
        public static CombatCharactersHolder Characters;
        [ShowInInspector]
        public static CombatTeamControlsHandler CombatTeamControlHandler;

        [ShowInInspector] 
        public static CombatCharacterEventsBase CharacterChangesEvent;

        // This are not invoked that usually
        [ShowInInspector]
        public static SystemInvoker Invoker;
        [ShowInInspector]
        public static TempoHandler TempoHandler;

        [ShowInInspector] 
        public static CombatConditionsChecker CombatConditionChecker;

        [Title("Global Variables")]
        [ShowInInspector]
        public static SCombatParams ParamsVariable = null;
    }


    public static class GlobalCombatParams
    {

        public const float InitiativeCheck = 1;
        public const float SpeedStatModifier = InitiativeCheck * 0.01f;
        public const int PredictedAmountOfCharacters = UtilsCharacter.PredictedAmountOfCharactersInBattle;
        public const int PredictedAmountOfTeamCharacters = CharacterArchetypes.AmountOfArchetypesAmount;
        public const int ActionsPerInitiativeCap = 8;
        public const int ActionsLowerCap = -2;
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
