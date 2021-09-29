using System;
using UnityEngine;

namespace CombatEffects
{
    [Serializable]
    public struct EffectParameter
    {
        public SEffect effectPreset;
        public EnumEffects.TargetType targetType;
        public float effectModifier;
        public bool canCrit;
    }

    public struct EffectResolution
    {
        public readonly IEffect UsedEffect;
        public readonly float EffectValue;

        public EffectResolution(IEffect usedEffect, float effectValue)
        {
            UsedEffect = usedEffect;
            EffectValue = effectValue;
        }
    }
}
