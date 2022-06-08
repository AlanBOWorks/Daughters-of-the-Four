using System;
using CombatSystem.Player.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    [CreateAssetMenu(fileName = "ICONS - N [EffectsType Holder]",
        menuName = "Combat/Holders/Effect Type/Icons [Holder]")]
    public class SEffectsThemeHolder : ScriptableObject
    {
        [SerializeField] private NamesHolder namesHolder = new NamesHolder();
        [SerializeField] private ColorsHolder colorsHolder = new ColorsHolder();
        [SerializeField] private IconsHolder iconsHolder = new IconsHolder();


        public IFullEffectStructureRead<string> GetNamesTagsHolder() => namesHolder;
        public IFullEffectStructureRead<Color> GetColorsHolder() => colorsHolder;
        public IFullEffectStructureRead<Sprite> GetIcons() => iconsHolder;

        [Serializable]
        private sealed class NamesHolder : EffectStructure<string>
        { }

        [Serializable]
        private sealed class ColorsHolder : EffectStructure<Color>
        { }

        [Serializable]
        private sealed class IconsHolder : PreviewMonoEffectStructure<Sprite>
        { }
    }
}
