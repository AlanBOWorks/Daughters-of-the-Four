using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Player.UI;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace CombatSystem.Player
{
    public sealed class PlayerCombatSingleton 
    {
        private static readonly PlayerCombatSingleton Instance = new PlayerCombatSingleton();

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

            var performerSwitcherHandler = new PlayerPerformerSwitcher();
            playerEvents.ManualSubscribe(performerSwitcherHandler);
            discriminationEvents.Subscribe(performerSwitcherHandler);
        }

        public static PlayerCombatSingleton GetInstance() => Instance;

        [Title("Events")]
        [ShowInInspector]
        public static PlayerCombatEventsHolder PlayerCombatEvents { get; private set; }
        [Title("Controller")]
        [ShowInInspector]
        public static PlayerTeamController PlayerTeamController { get; private set; }


        [Title("Characters")]
        [ShowInInspector]
        internal static readonly PlayerSelectedCharactersHolder SelectedCharactersHolder;
        public static IReadOnlyCollection<ICombatEntityProvider> SelectedCharacters => SelectedCharactersHolder;

        [Title("Mono References")]
        [ShowInInspector]
        public static Camera InterfaceCombatCamera { get; private set; }

        public static void InjectCombatCamera(in Camera camera)
        {
            InterfaceCombatCamera = camera;
            PlayerCombatEvents.OnSwitchCamera(in camera);
        }




        // ----- EDITOR WINDOW -----
        private sealed class PlayerCombatSingletonEditorWindow : OdinEditorWindow
        {
            [ShowInInspector, TabGroup("Functionality")]
            private PlayerCombatSingleton _singleton;

            [ShowInInspector, TabGroup("Visuals"),InlineEditor(InlineEditorObjectFieldModes.Foldout)]
            private IOppositionTeamStructureRead<CombatPlayerTeamFeedBack> _feedBacks;


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
                _singleton = GetInstance();
                _feedBacks = PlayerCombatUserInterfaceSingleton.CombatTeemFeedBacks;
            }
        }
    }

}
