using System;
using Characters;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Skills;
using UnityEditor;

namespace _Player
{
    public sealed class PlayerEntitySingleton
    {
        static PlayerEntitySingleton() { }

        private PlayerEntitySingleton()
        {
            CombatDictionary = new PlayerCombatDictionary(CharacterUtils.PredictedAmountOfCharactersInBattle);
            CombatElementsPools = new PlayerCombatElementsPools(CombatDictionary);
        }
        public static PlayerEntitySingleton Instance { get; } = new PlayerEntitySingleton();

        public static PlayerCombatDictionary CombatDictionary = null;
        [ShowInInspector] 
        public static PlayerCombatElementsPools CombatElementsPools;
        [ShowInInspector]
        public static USkillButtonsHandler SkillButtonsHandler = null;
        [ShowInInspector]
        public static IPlayerArchetypesData<SPlayerCharacterEntityVariable> SelectedCharacters = null;
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
