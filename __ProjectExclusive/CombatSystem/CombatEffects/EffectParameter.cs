using System;
using CombatSkills;
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

        private const float CriticalModifier = .5f;
        public void DoEffect(SkillValuesHolders values)
        {
            float finalEffectValue = effectModifier;
            bool isEffectCrit = canCrit && values.IsCritical;
            effectPreset.DoEffect(values, targetType, finalEffectValue, isEffectCrit);
        }
    }

    /// <summary>
    /// Used for events when an effect does its calculations and sends its resolution
    /// </summary>
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
