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
        [SerializeField] private ThemeHolder themeHolder = new ThemeHolder();

        public IEffectStructureRead<CombatThemeHolder> GetHolder() => themeHolder;

        [Serializable]
        private sealed class ThemeHolder : ClassEffectStructure<CombatThemeHolder>
        { }

    }
}
