using CombatSystem.Player.UI;
using CombatSystem.Stats;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace CombatSystem
{
    public static class GlobalThemeAssets 
    {
        public const string AssetFolderPath = "Assets/ScriptableObjects/Globals/Theme/";
        public const string ThemeAssetPath = AssetFolderPath + SCombatThemeHolder.EditorsName;
        public const string ParticlesPrefabAssetPath = AssetFolderPath + SStatsPrefabsHolder.EditorsName;
    }


#if UNITY_EDITOR
    public class CombatThemeEditorWindow : OdinEditorWindow
    {
        [ShowInInspector, InlineEditor()]
        private SCombatThemeHolder _themeHolder;

        [ShowInInspector, InlineEditor()]
        private SStatsPrefabsHolder _prefabsHolder;

        [MenuItem("Game/Editor/Theme Holder [WINDOW]", priority = -1)]
        public static void OpenWindow()
        {
            var window = GetWindow<CombatThemeEditorWindow>();
            window._themeHolder = AssetDatabase.LoadAssetAtPath<SCombatThemeHolder>(GlobalThemeAssets.ThemeAssetPath);
            window._prefabsHolder = AssetDatabase.LoadAssetAtPath<SStatsPrefabsHolder>(GlobalThemeAssets.ParticlesPrefabAssetPath);
        }
    }
#endif
}
