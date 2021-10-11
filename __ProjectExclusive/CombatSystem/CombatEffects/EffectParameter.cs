using System;
using CombatSkills;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [Serializable]
    public struct EffectParameter
    {
        public SEffect effectPreset;
        public EnumEffects.TargetType targetType;
        public float effectValue;
        public bool canCrit;

        private const float CriticalModifier = .5f;
        public void DoEffect(SkillValuesHolders values)
        {
            bool isEffectCrit = canCrit && values.IsCritical;
            effectPreset.DoEffect(values, targetType, effectValue, isEffectCrit);
        }
    }

    /// <summary>
    /// Used for events when an effect does its calculations and sends its resolution
    /// to global/entity events
    /// </summary>
    public struct SkillComponentResolution
    {
        public readonly ISkillComponent UsedSkillComponent;
        public readonly float EffectValue;

        public SkillComponentResolution(ISkillComponent usedEffect, float effectValue)
        {
            UsedSkillComponent = usedEffect;
            EffectValue = effectValue;
        }
    }

    [Serializable]
    public struct BuffParameter
    {
        public SBuff buffPreset;
        public EnumEffects.TargetType targetType;
        public EnumStats.BuffType buffType;
        public float buffValue;
        public bool canCrit;

        public void DoBuff(SkillValuesHolders values)
        {
            bool isEffectCrit = canCrit && values.IsCritical;
            buffPreset.DoBuff(values, buffType, targetType, buffValue, isEffectCrit);
        }
    }

}
