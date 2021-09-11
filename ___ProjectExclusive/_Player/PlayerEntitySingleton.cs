using System;
using System.Collections.Generic;
using _CombatSystem;
using _Team;
using Characters;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Skills;
using UnityEditor;
using UnityEngine;

namespace _Player
{
    public sealed class PlayerEntitySingleton
    {
        static PlayerEntitySingleton() { }

        private PlayerEntitySingleton()
        {
            SkillsTracker = new PlayerSkillsTracker();
            PlayerCombatEvents = new PlayerCombatEvents();
            UIDictionary = new PlayerUIDictionary();
            var targetsHandler = new PlayerTargetsHandler();

            //Injections
            PlayerCombatEvents.Subscribe(SkillsTracker);
            PlayerCombatEvents.Subscribe(targetsHandler);
        }
        public static PlayerEntitySingleton Instance { get; } = new PlayerEntitySingleton();

        [ShowInInspector] 
        public static PlayerSkillsTracker SkillsTracker;

        [ShowInInspector] 
        public static PlayerCombatEvents PlayerCombatEvents;
        [ShowInInspector]
        public static PlayerUIDictionary UIDictionary;


        [ShowInInspector]
        public static USkillButtonsHandler SkillButtonsHandler = null;
        [ShowInInspector]
        public static IPlayerArchetypesData<ICharacterCombatProvider> SelectedCharacters = null;

        [ShowInInspector] 
        public static ITeamCombatControlHolder TeamControlStats;

        public static void DoSubscriptionsToCombatSystem()
        {
            var invoker = CombatSystemSingleton.Invoker;
            invoker.SubscribeListener(UIDictionary);
            invoker.SubscribeListener(UIDictionary);

            var controllersHandler = CombatSystemSingleton.ControllersHandler;
            controllersHandler.InjectPlayerEvents(PlayerCombatEvents);

        }
    }


    public class PlayerEntityMenu : OdinEditorWindow
    {
        [MenuItem("Game Menus/Player Singleton")]
        private static void OpenWindow()
        {
            PlayerEntityMenu menu = GetWindow<PlayerEntityMenu>();
            menu.Singleton = PlayerEntitySingleton.Instance;
            menu.Show();
        }

        private void OnFocus()
        {
            OpenWindow();
        }
        [NonSerialized, ShowInInspector]
        public PlayerEntitySingleton Singleton = null;
    }
}
