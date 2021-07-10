using System;
using System.Collections.Generic;
using _CombatSystem;
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
            CombatElementsPools = new PlayerCombatElementsPools();
            TargetsHandler = new PlayerTargetsHandler();

#if UNITY_EDITOR
            Debug.Log("Player Singleton Created");
#endif
        }
        public static PlayerEntitySingleton Instance { get; } = new PlayerEntitySingleton();

        public static Dictionary<CombatingEntity, PlayerCombatElement> CombatDictionary
            => CombatElementsPools.EntitiesDictionary;
        [ShowInInspector]
        public static PlayerCombatElementsPools CombatElementsPools;
        public static PlayerTargetsHandler TargetsHandler;

        [ShowInInspector]
        public static USkillButtonsHandler SkillButtonsHandler = null;
        [ShowInInspector]
        public static IPlayerArchetypesData<SPlayerCharacterEntityVariable> SelectedCharacters = null;

        public static void DoSubscriptionsToCombatSystem()
        {
            var invoker = CombatSystemSingleton.Invoker;
            invoker.SubscribeListener(CombatElementsPools);
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
