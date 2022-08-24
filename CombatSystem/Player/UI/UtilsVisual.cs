using CombatSystem.Skills;
using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public static class UtilsVisual 
    {
        public static Sprite GetEffectSprite(IEffectBasicInfo effect)
        {
            var sprite = effect.GetIcon();
            if (sprite) return sprite;
            var type = effect.EffectType;

            var theme = CombatThemeSingleton.EffectsIconsHolder;
            sprite = UtilsStructureEffect.GetElement(type, theme);

            return sprite;
        }
    }
}
