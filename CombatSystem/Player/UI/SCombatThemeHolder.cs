using System;
using CombatSystem.Skills.Effects;
using CombatSystem.Stats;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Utils;

namespace CombatSystem.Player.UI
{
    [CreateAssetMenu(fileName = AssetName, menuName = "Combat/Holders/Combat Theme")]
    public class SCombatThemeHolder : ScriptableObject
    {
        private const string Path = "Assets/ScriptableObjects/Globals/Theme/";
        private const string AssetName = "MainTheme [Theme Holder]";
        private const string AssetPath = Path + AssetName + ".asset";


        [SerializeField, InlineEditor()] private SCombatRolesThemesHolder rolesThemesHolder;
        [SerializeField, InlineEditor()] private SStatsThemeHolder statsThemeHolder;
        [SerializeField, InlineEditor()] private SEffectsThemeHolder effectsTheme;

        [Button]
        private void UpdateAssetName()
        {
            UtilsAssets.UpdateAssetName(this, AssetName);
        }

        private void Awake()
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
            var themeHolder = AssetDatabase.LoadAssetAtPath<SCombatThemeHolder>(AssetPath);
            themeHolder.Awake();
        }
    }
}
