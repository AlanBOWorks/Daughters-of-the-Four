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

        public PlayerCombatSingleton()
        {
            PlayerCombatEvents = new PlayerCombatEventsHolder();
            PlayerTeamController = new PlayerTeamController();

            var playerEvents = 
                PlayerCombatEvents;
            var discriminationEvents =
                playerEvents.DiscriminationEventsHolder;

            var teamController = PlayerTeamController;
            discriminationEvents.Subscribe(teamController);
            playerEvents.SubscribeAsPlayerEvent(teamController);
        }

        static PlayerCombatSingleton()
        {
            SelectedCharactersHolder = new PlayerSelectedCharactersHolder();
        }

        public static PlayerCombatSingleton GetInstance() => Instance;

        [ShowInInspector]
        public static PlayerCombatEventsHolder PlayerCombatEvents { get; private set; }
        [ShowInInspector]
        public static PlayerTeamController PlayerTeamController { get; private set; }


        [ShowInInspector]
        internal static readonly PlayerSelectedCharactersHolder SelectedCharactersHolder;
        public static IReadOnlyCollection<ICombatEntityProvider> SelectedCharacters => SelectedCharactersHolder;

        [ShowInInspector]
        public static Camera InterfaceCombatCamera { get; internal set; }
    }

    public sealed class PlayerCombatSingletonEditorWindow : OdinEditorWindow
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
