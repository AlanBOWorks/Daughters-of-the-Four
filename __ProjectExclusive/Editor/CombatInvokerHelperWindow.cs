using __ProjectExclusive;
using __ProjectExclusive.Enemy;
using __ProjectExclusive.Player;
using CombatEntity;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace CombatSystem.Editor
{
    public class CombatInvokerHelperWindow : OdinEditorWindow
    {
        [MenuItem("Project Tools/Combat invoker (Helper)", false, -100)]
        private static void OpenWindow()
        {
            var window = GetWindow<CombatInvokerHelperWindow>();
            window.Show();
        }

        [SerializeField] private SCombatInvokerHelperPreset preset;
        [SerializeField,HorizontalGroup(), HideIf("preset")]
        private PlayerCharactersSelector playerCharacters = new PlayerCharactersSelector();
        [SerializeField, HorizontalGroup(), HideIf("preset")] 
        private EnemyCombatProvider enemyCombatProvider = new EnemyCombatProvider();
        public bool showCombatDebugWindow = true;

        [Button(ButtonSizes.Large), DisableInEditorMode]
        public void InvokeCombatWithThisTeam()
        {
            GameStateLoader.LoadPrepareInstances();

            if (preset)
            {
                playerCharacters = preset.playerCharacters;
                enemyCombatProvider = preset.enemyCombatProvider;
            }

            playerCharacters.InjectInSingleton();
            enemyCombatProvider.InvokeCombatWithThisTeam();

            if(showCombatDebugWindow)
                GetWindow<CombatSystemWindow>().Show();
        }
    }
}
