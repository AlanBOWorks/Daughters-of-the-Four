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
            CombatElementsPools = new PlayerCombatElementsPools();
            var targetsHandler = new PlayerTargetsHandler();

            //Injections
            PlayerCombatEvents.Subscribe(targetsHandler);
        }
        public static PlayerEntitySingleton Instance { get; } = new PlayerEntitySingleton();

        public static Dictionary<CombatingEntity, PlayerCombatElement> CombatDictionary
            => CombatElementsPools.EntitiesDictionary;

        [ShowInInspector] 
        public static PlayerCombatEvents PlayerCombatEvents;
        [ShowInInspector]
        public static PlayerCombatElementsPools CombatElementsPools;

        [ShowInInspector] 
        public static PredefinedUIHolderDictionary PredefinedUIDictionary;

        [ShowInInspector]
        public static USkillButtonsHandler SkillButtonsHandler = null;
        [ShowInInspector]
        public static IPlayerArchetypesData<ICharacterCombatProvider> SelectedCharacters = null;

        [ShowInInspector] 
        public static ITeamCombatControlHolder TeamControlStats;

        public static void DoSubscriptionsToCombatSystem()
        {
            var invoker = CombatSystemSingleton.Invoker;
            invoker.SubscribeListener(CombatElementsPools);

            var tempoHandler = CombatSystemSingleton.TempoHandler;
            tempoHandler.InjectPlayerEvents(PlayerCombatEvents);
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
