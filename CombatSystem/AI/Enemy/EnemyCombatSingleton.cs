using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace CombatSystem.AI
{
    public sealed class EnemyCombatSingleton 
    {
        private static readonly EnemyCombatSingleton Instance = new EnemyCombatSingleton();

        static EnemyCombatSingleton()
        {
            EnemyEventsHolder = new EnemyCombatEventsHolder();
        }

        [ShowInInspector]
        public static EnemyCombatEventsHolder EnemyEventsHolder { get; private set; }

        public static EnemyCombatSingleton GetInstance() => Instance;




        // ----- EDITOR WINDOW -----
        private sealed class EnemyCombatSingletonEditorWindow : OdinEditorWindow
        {
            [ShowInInspector] private EnemyCombatSingleton _singleton = GetInstance();


            [MenuItem("Combat/Debug/Enemy Singleton", priority = -4)]
            private static void OpenWindow()
            {
                var windowElement = GetWindow<EnemyCombatSingletonEditorWindow>();
                windowElement.Show();
            }

        }
    }

}
