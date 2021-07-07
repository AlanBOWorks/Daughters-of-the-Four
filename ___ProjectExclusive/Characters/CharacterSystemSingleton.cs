using System;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace Characters
{
    public sealed class CharacterSystemSingleton
    {
        static CharacterSystemSingleton() { }
        public static CharacterSystemSingleton Instance { get; } = new CharacterSystemSingleton();

        private CharacterSystemSingleton()
        {
            Spawner = new EntityHolderSpawner();
        }

        [ShowInInspector]
        public static EntityHolderSpawner Spawner;

    }


    public class CharacterSystemMenu : OdinEditorWindow
    {
        [MenuItem("Game Menus/Character System")]
        private static void OpenWindow()
        {
            CharacterSystemMenu menu = GetWindow<CharacterSystemMenu>();
            menu.Singleton = CharacterSystemSingleton.Instance;
            menu.Show();
        }

        private void OnFocus()
        {
            OpenWindow();
        }
        [NonSerialized, ShowInInspector]
        public CharacterSystemSingleton Singleton = null;
    }
}
