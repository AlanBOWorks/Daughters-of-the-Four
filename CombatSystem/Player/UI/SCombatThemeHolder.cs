using System;
using CombatSystem.Skills.Effects;
using CombatSystem.Stats;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Utils;

namespace CombatSystem.Player.UI
{
    [CreateAssetMenu(fileName = AssetName, menuName = "Combat/Holders/Combat Theme")]
    public class SCombatThemeHolder : ScriptableObject
    {
        private const string Path = GlobalThemeAssets.AssetFolderPath + "/Combat/";
        private const string AssetName = "MainTheme [Theme Holder]";
        public const string AssetPath = Path + EditorsName;

        public const string EditorsName = AssetName + ".asset";

        [SerializeField, InlineEditor()] private SCombatRolesThemesHolder rolesThemesHolder;
        [SerializeField, InlineEditor()] private SStatsThemeHolder statsThemeHolder;
        [SerializeField, InlineEditor()] private SEffectsThemeHolder effectsTheme;

        [Button]
        private void UpdateAssetName()
        {
            UtilsAssets.UpdateAssetName(this, AssetName);
        }

        private void DoInjections()
        {
            var rolesHolder = rolesThemesHolder.GetHolder();
            CombatThemeSingleton.RolesThemeHolder = rolesHolder;
            CombatThemeSingleton.SkillsThemeHolder = rolesHolder;
            CombatThemeSingleton.StatsThemeHolder = statsThemeHolder.GetHolder();

            CombatThemeSingleton.EffectsNameTagsHolder = effectsTheme.GetNamesTagsHolder();
            CombatThemeSingleton.EffectsIconsHolder = effectsTheme.GetIcons();
            CombatThemeSingleton.EffectsColorsHolder = effectsTheme.GetColorsHolder();

#if UNITY_EDITOR
            Debug.Log($"Loaded Combat Theme at: {AssetPath}");
#endif        
        }

        public static void LoadAsset()
        {
            var asset = AssetDatabase.LoadAssetAtPath<SCombatThemeHolder>(AssetPath);
            asset.DoInjections();
        }
    }

}
