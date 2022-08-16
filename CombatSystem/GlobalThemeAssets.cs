using CombatSystem.Player.UI;
using CombatSystem.Stats;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace CombatSystem
{
    public static class GlobalThemeAssets 
    {
        public const string AssetFolderPath = "Assets/ScriptableObjects/Globals/Theme/Combat/";
        public const string ThemeAssetPath = AssetFolderPath + SCombatThemeHolder.EditorsName;
        public const string ParticlesPrefabAssetPath = AssetFolderPath + SStatsPrefabsHolder.EditorsName;



#if UNITY_EDITOR
        private sealed class CombatThemeEditorWindow : OdinEditorWindow
        {
            [ShowInInspector, InlineEditor()]
            private SCombatThemeHolder _themeHolder;

            [ShowInInspector, InlineEditor()]
            private SStatsPrefabsHolder _prefabsHolder;

            [MenuItem("Game/Editor/Combat Theme Holder [WINDOW]", priority = -1)]
            public static void OpenWindow()
            {
                var window = GetWindow<CombatThemeEditorWindow>();
                window._themeHolder 
                    = AssetDatabase.LoadAssetAtPath<SCombatThemeHolder>(ThemeAssetPath);
                window._prefabsHolder 
                    = AssetDatabase.LoadAssetAtPath<SStatsPrefabsHolder>(ParticlesPrefabAssetPath);
            }
        }
#endif
    }


}
