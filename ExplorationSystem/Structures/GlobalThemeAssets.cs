using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace ExplorationSystem
{
    public static class GlobalThemeAssets 
    {
        public const string AssetFolder = "Assets/ScriptableObjects/Globals/Theme/Exploration/";
        public const string ExplorationThemeAsset = SExplorationThemeHolder.AssetPath;

#if UNITY_EDITOR
        private sealed class ExplorationThemeEditorWindow : OdinEditorWindow
        {
            [ShowInInspector, InlineEditor()] private SExplorationThemeHolder _themeHolder;

            [MenuItem("Game/Editor/Exploration The Holder [WINDOW]", priority = -1)]
            private static void OpenWindow()
            {
                var window = GetWindow<ExplorationThemeEditorWindow>();
                window._themeHolder = 
                    AssetDatabase.LoadAssetAtPath<SExplorationThemeHolder>(ExplorationThemeAsset);
            }
        }
#endif

    }
}
