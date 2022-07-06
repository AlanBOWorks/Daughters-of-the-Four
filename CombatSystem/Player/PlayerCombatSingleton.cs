using CombatSystem._Core;
using CombatSystem.Player.Handlers;
using CombatSystem.Player.UI;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace CombatSystem.Player
{
    public sealed class PlayerCombatSingleton
    {
        public static readonly PlayerCombatSingleton Instance = new PlayerCombatSingleton();

        static PlayerCombatSingleton()
        {

            PlayerCombatEvents = new PlayerCombatEventsHolder();
            PlayerTeamController = new PlayerTeamController();

            SelectedCharactersHolder = new PlayerSelectedCharactersHolder();

            var playerEvents =
                PlayerCombatEvents;
            var discriminationEvents =
                playerEvents.DiscriminationEventsHolder;

            var teamController = PlayerTeamController;
            discriminationEvents.Subscribe(teamController);
            playerEvents.SubscribeAsPlayerEvent(teamController);


            PerformerSwitcher = new PlayerPerformerSwitcher();
            playerEvents.ManualSubscribe(PerformerSwitcher);
            discriminationEvents.Subscribe(PerformerSwitcher);

            StanceSwitcher = new PlayerTeamStanceSwitcher();
            playerEvents.SubscribeForCombatPreparation(StanceSwitcher);
            discriminationEvents.Subscribe(StanceSwitcher);
            
            CombatEscapeButtonHandler = new CombatEscapeButtonHandler();
            playerEvents.ManualSubscribe((ICombatStartListener) CombatEscapeButtonHandler);
            playerEvents.SubscribeAsPlayerEvent(CombatEscapeButtonHandler);
            playerEvents.DiscriminationEventsHolder.Subscribe(CombatEscapeButtonHandler);

            var playerSkillInfoHandler = new SkillInfoHandler();
            playerEvents.Subscribe(playerSkillInfoHandler);
        }


        [Title("Events")]
        [ShowInInspector]
        public static PlayerCombatEventsHolder PlayerCombatEvents { get; private set; }

        public static IEscapeButtonHandler GetCombatEscapeButtonHandler() => CombatEscapeButtonHandler;

        [ShowInInspector] 
        internal static readonly CombatEscapeButtonHandler CombatEscapeButtonHandler;

        [Title("Controller")]
        [ShowInInspector]
        public static PlayerTeamController PlayerTeamController { get; private set; }

        public static readonly PlayerPerformerSwitcher PerformerSwitcher;
        public static readonly PlayerTeamStanceSwitcher StanceSwitcher;

        [ShowInInspector]
        public static readonly UHoverSkillTargetingHandler HoverTargetingHelper;

        [Title("Characters")]
        [ShowInInspector]
        internal static readonly PlayerSelectedCharactersHolder SelectedCharactersHolder;

        [Title("Mono References")]
        [ShowInInspector]
        public static IPlayerCameraStructureRead<Camera> CamerasHolder { get; private set; }
        public static UUIHoverEntitiesHandler HoverEntitiesHandler { get; private set; }


        public static bool IsInPauseMenu => CombatEscapeButtonHandler.IsInPause();

        public static void Injection(IPlayerCameraStructureRead<Camera> holder) => CamerasHolder = holder;
       
        public static void Injection(UUIHoverEntitiesHandler hoverEntitiesHandler) => HoverEntitiesHandler = hoverEntitiesHandler;



        // ----- EDITOR WINDOW -----
        private sealed class PlayerCombatSingletonEditorWindow : OdinEditorWindow
        {
            [ShowInInspector, TabGroup("Functionality")]
            private PlayerCombatSingleton _singleton;

            [ShowInInspector, TabGroup("Visuals")]
            private CombatThemeSingleton _themeSingleton;


            [MenuItem("Combat/Debug/Player Singleton", priority = -4)]
            private static void OpenWindow()
            {
                var windowElement = GetWindow<PlayerCombatSingletonEditorWindow>();

                windowElement.LoadReferences();
                windowElement.Show();
            }

            [Button("Refresh References")]
            private void LoadReferences()
            {
                _singleton = Instance;
                _themeSingleton = CombatThemeSingleton.Instance;
            }
        }

    }

}
