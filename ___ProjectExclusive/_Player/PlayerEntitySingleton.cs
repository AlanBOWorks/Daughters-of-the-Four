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
            PlayerCombatEvents = new PlayerCombatEvents();
            SkillSelectorHandler = new CombatSkillSelector();
            EntitiesUIDictionary = new PlayerUIDictionary();
            var targetsHandler = new PlayerTargetsHandler();

            //Injections
            PlayerCombatEvents.Subscribe(targetsHandler);
            PlayerCombatEvents.Subscribe(SkillSelectorHandler);
        }
        public static PlayerEntitySingleton Instance { get; } = new PlayerEntitySingleton();

       

        [ShowInInspector] 
        public static PlayerCombatEvents PlayerCombatEvents;
        [ShowInInspector]
        public static PlayerUIDictionary EntitiesUIDictionary;

        public static Dictionary<CombatSkill, USkillButton> SkillButtonsDictionary;

        public static CombatSkillSelector SkillSelectorHandler;

        [ShowInInspector]
        public static IPlayerArchetypesData<ICharacterCombatProvider> SelectedCharacters = null;

        [ShowInInspector] 
        public static ITeamCombatControlHolder TeamControlStats;

        // This is for injecting Player's listeners to Invoker and it self to CombatSingleton; 
        // The rest of elements should be injected in the PlayerSingleton instead
        public static void DoSubscriptionsToCombatSystem()
        {
            var invoker = CombatSystemSingleton.Invoker;
            invoker.SubscribeListener(EntitiesUIDictionary);

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
