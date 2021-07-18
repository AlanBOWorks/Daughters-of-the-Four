using System;
using _Player;
using Characters;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Skills;
using UnityEditor;

namespace _CombatSystem
{
    public sealed class CombatSystemSingleton
    {
        static CombatSystemSingleton() { }
        public static CombatSystemSingleton Instance { get; } = new CombatSystemSingleton();

        private CombatSystemSingleton()
        {
            TempoHandlerBase tempoHandlerBase;
            SafetyBackupSkillsInjection backupSkillsInjection;
            InitializeSystem();

            Invoker = new SystemInvoker();
            Characters = new CombatCharactersHolder();
            TempoHandler = new TempoHandler(tempoHandlerBase);
            PerformSkillHandler = new PerformSkillHandler();

            // Locals declarations that wont be used further than durante the CombatInvoker
            var combatControlDeclaration = new CombatControlDeclaration();
            var skillCooldown = new SkillCooldownHandler();
            var roundCheckHandler = new RoundCheckHandler();
            var specialBuffHandler = new SpecialBuffHandler();

            //---- Injections ----
            Invoker.SubscribeListener(Characters);
            Invoker.SubscribeListener(TempoHandler);
            Invoker.SubscribeListener(combatControlDeclaration);
            Invoker.SubscribeListener(backupSkillsInjection);
            Invoker.SubscribeListener(roundCheckHandler);

            TempoHandler.Subscribe(PerformSkillHandler);
            TempoHandler.Subscribe(skillCooldown);

            TempoHandler.Subscribe((IRoundListener) specialBuffHandler);
            TempoHandler.Subscribe((ITempoListener) specialBuffHandler);

            TempoHandler.Subscribe((ITempoListener) roundCheckHandler);
            TempoHandler.Subscribe((ISkippedTempoListener) roundCheckHandler);



            void InitializeSystem()
            {
#if UNITY_EDITOR
                tempoHandlerBase = new TempoHandlerBase();
#else
                tempoHandler = new CharacterTempoHandler(false);
#endif

                backupSkillsInjection = new SafetyBackupSkillsInjection();

            }

        }

        // Useful objects will remain in the Singleton as well
        public static bool IsCombatActive => Invoker.CombatHandle.IsRunning;
        [ShowInInspector]
        public static PerformSkillHandler PerformSkillHandler;
        [ShowInInspector]
        public static CombatCharactersHolder Characters;
        [ShowInInspector]
        public static CombatTeamsHandler TeamsDataHandler;

        [ShowInInspector] 
        public static CombatCharacterEvents CharacterChangesEvent;

        // This are not invoked that usually
        [ShowInInspector]
        public static SystemInvoker Invoker;
        [ShowInInspector]
        public static TempoHandler TempoHandler;

        [ShowInInspector]
        public static SCombatParams ParamsVariable = null;


    }


    public static class GlobalCombatParams
    {

        public const float InitiativeCheck = 1;
        public const float SpeedStatModifier = InitiativeCheck * 0.01f;
        public const int PredictedAmountOfCharacters = UtilsCharacter.PredictedAmountOfCharactersInBattle;
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
