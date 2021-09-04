using System;
using Sirenix.OdinInspector;
using Skills;
using Stats;
using UnityEditor;
using UnityEngine;

namespace ___ProjectExclusive
{
    public sealed class GameThemeSingleton
    {
        static GameThemeSingleton() { }

        private GameThemeSingleton()
        {
        }
        public static GameThemeSingleton Instance { get; } = new GameThemeSingleton();

        [ShowInInspector, NonSerialized] 
        public static SGameTheme ThemeVariable;

        public static GameThemeColors ThemeColors => ThemeVariable.ThemeColors;

        public void Injection(SGameTheme variable)
        {
            ThemeVariable = variable;
        }
    }

    [Serializable]
    public class GameThemeColors
    {
        public TextColors textColors = new TextColors();
        public PlayerColors playerColors = new PlayerColors();
        public CombatEffectColors effectColors = new CombatEffectColors();
    }



    [Serializable]
    public class TextColors
    {
        public Color mainColor = new Color(.4f, .4f, .4f);
        public Color neutral = new Color(.4f, .4f, .4f);
        public Color text = new Color(0.1f,0.1f,0.1f);
    }

    [Serializable]
    public class PlayerColors
    {
        public Color controllableColor = new Color(.2f,.6f,.8f);
        public Color enemyColor = new Color(.9f, .2f, .4f);
        public Color neutralColor = new Color(.4f,.4f,.4f);
    }

    [Serializable]
    public class CombatEffectColors : SerializableFullDrivenData<Color>
    {}

    public static class UtilsGameTheme
    {
        public const float KThousand = 1000;
        public const float KDivisor = 1 / KThousand;
        public static string GetNumericalPrint(float value)
        {
            string generated;

            if (value < 1000)
            {
                generated = $"{value:0000}";
            }
            else
            {
                generated = $"{value * KDivisor:000}K";
            }

            return generated;
        }
    }

    public static class UtilsGame
    {

        public const string IconsPath = "Assets/ART/Icons/Dayed/";

        public static void UpdateAssetName(ScriptableObject asset)
        {
            string assetPath = AssetDatabase.GetAssetPath(asset.GetInstanceID());
            var errorLog = AssetDatabase.RenameAsset(assetPath, asset.name);
            if(errorLog.Length > 0)
                Debug.LogWarning("Name could be updated: " + errorLog);

        }
    }
}
