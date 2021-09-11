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
            CombatUiElements = new PlayerCombatUIElements();
            var targetsHandler = new PlayerTargetsHandler();

            //Injections
            PlayerCombatEvents.Subscribe(SkillsTracker);
            PlayerCombatEvents.Subscribe(targetsHandler);
        }
        public static PlayerEntitySingleton Instance { get; } = new PlayerEntitySingleton();

        public static Dictionary<CombatingEntity, PlayerCombatUIElement> CombatDictionary => CombatUiElements.Dictionary;

        [ShowInInspector] 
        public static PlayerSkillsTracker SkillsTracker;

        [ShowInInspector] 
        public static PlayerCombatEvents PlayerCombatEvents;
        [ShowInInspector]
        public static PlayerCombatUIElements CombatUiElements;


        [ShowInInspector]
        public static USkillButtonsHandler SkillButtonsHandler = null;
        [ShowInInspector]
        public static IPlayerArchetypesData<ICharacterCombatProvider> SelectedCharacters = null;

        [ShowInInspector] 
        public static ITeamCombatControlHolder TeamControlStats;

        public static void DoSubscriptionsToCombatSystem()
        {
            var invoker = CombatSystemSingleton.Invoker;
            invoker.SubscribeListener(CombatUiElements);

            var controllersHandler = CombatSystemSingleton.ControllersHandler;
            controllersHandler.InjectPlayerEvents(PlayerCombatEvents);

            var teamPersistentElements = CombatSystemSingleton.TeamsPersistentElements;
            teamPersistentElements.DoInjectionIn(CombatUiElements);
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
