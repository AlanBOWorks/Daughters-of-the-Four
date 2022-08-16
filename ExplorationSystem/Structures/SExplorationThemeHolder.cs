using System;
using Common;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace ExplorationSystem
{
    [CreateAssetMenu(fileName = "N [ExplorationTheme]", menuName = "Editor/Visuals/Exploration Theme")]
    public class SExplorationThemeHolder : ScriptableObject
    {
        private const string MainThemeAssetName = "MainTheme [ExplorationTheme]";
        public const string AssetFolder = GlobalThemeAssets.AssetFolder;
        public const string AssetPath = AssetFolder + MainThemeAssetName + ".asset";

        [SerializeField] private ExplorationThemeHolder dataHolder = new ExplorationThemeHolder();

        public IExplorationTypesStructureRead<ExplorationThemeData> GetDataHolder() => dataHolder;

        [Serializable]
        private sealed class ExplorationThemeHolder : ExplorationTypeStructure<ExplorationThemeData>
        {
            
        }
    }

    [Serializable]
    public sealed class ExplorationThemeData : IThemeHolder
    {
        [SerializeField] private string themeName = "NULL";
        [SerializeField, PreviewField, GUIColor(.3f,.3f,.3f,.8f)] private Sprite themeIcon;
        [SerializeField] private Color themeColor = new Color(.1f,.1f,.1f);

        public string GetThemeName() => themeName;
        public Sprite GetThemeIcon() => themeIcon;
        public Color GetThemeColor() => themeColor;
    }

    [Serializable]
    public class ExplorationTypeStructure<T> : IExplorationTypesStructureRead<T>
    {
        [SerializeField, HorizontalGroup("Battle Top"), LabelWidth(100)]
        private T basicThreatType;
        [SerializeField, HorizontalGroup("Battle Top"), LabelWidth(100)]
        private T eliteThreatType;
        [SerializeField, HorizontalGroup("Battle Bottom"), LabelWidth(100)]
        private T bossThreatType;

        [SerializeField, HorizontalGroup("Skills"), LabelWidth(100)]
        private T combinationType;
        [SerializeField, HorizontalGroup("Skills"), LabelWidth(100)]
        private T awakeningType;
        [SerializeField, HorizontalGroup("Elements"), LabelWidth(100)]
        private T treasureType;
        [SerializeField, HorizontalGroup("Elements"), LabelWidth(100)]
        private T shopType;

        public T BasicThreatType => basicThreatType;
        public T EliteThreatType => eliteThreatType;
        public T BossThreatType => bossThreatType;
        public T CombinationType => combinationType;
        public T AwakeningType => awakeningType;
        public T TreasureType => treasureType;
        public T ShopType => shopType;
    }

}
