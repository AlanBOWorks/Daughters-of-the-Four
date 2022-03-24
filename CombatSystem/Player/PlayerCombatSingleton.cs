using System.Collections.Generic;
using CombatSystem.Entity;
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
            [ShowInInspector]
            private PlayerCombatSingleton _singleton = PlayerCombatSingleton.GetInstance();


            [MenuItem("Combat/Debug/Player Singleton", priority = -4)]
            private static void OpenWindow()
            {
                var windowElement = GetWindow<PlayerCombatSingletonEditorWindow>();
                windowElement.Show();
            }

        }
    }

}
