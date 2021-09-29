using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace __ProjectExclusive.Player
{
    //It's a Singleton instead of a static class for debugging reasons
    public sealed class PlayerCombatSingleton
    {
        private static readonly PlayerCombatSingleton Instance = new PlayerCombatSingleton();

        static PlayerCombatSingleton()
        {
            CharactersHolder = new PlayerCharactersHolder();
        }

        private PlayerCombatSingleton()
        { }

        public static PlayerCombatSingleton GetInstance() => Instance;

        [ShowInInspector] 
        public static readonly PlayerCharactersHolder CharactersHolder;
    }

    internal class PlayerCombatSystemWindow : OdinEditorWindow
    {
        [ShowInInspector]
        private PlayerCombatSingleton _system = PlayerCombatSingleton.GetInstance();

        [MenuItem("Debug/Combat System (Player)")]
        private static void OpenWindow()
        {
            var window = GetWindow<PlayerCombatSystemWindow>();
            window.Show();
        }

    }
}
