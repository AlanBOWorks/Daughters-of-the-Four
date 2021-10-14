using System;
using CombatSkills;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [Serializable]
    public struct EffectParameter
    {
        [Title("Preset")]
        public SSkillComponentEffect preset;

        [HorizontalGroup("Type",Title = "_____ Types _________")]
        public EnumEffects.TargetType targetType;
        [HorizontalGroup("Type")]
        [ShowIf("IsBuffEffect",Animate = false)]
        public EnumStats.BuffType buffType;

        [Title("Params")]
        public float effectValue;
        public bool canCrit;

        private bool IsBuffEffect() => preset is IBuff;

        public void DoEffect(SkillValuesHolders values)
        {
            bool isEffectCrit = canCrit && values.IsCritical;
            switch (preset)
            {
                case IEffect effectPreset:
                    effectPreset.DoEffect(values, targetType, effectValue, isEffectCrit);
                    return;
                case IBuff buffPreset:
                    buffPreset.DoBuff(values,buffType,targetType,effectValue,isEffectCrit);
                    return;
            }
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

    public class SSkillComponentEffect : ScriptableObject, ISkillComponent { }
}
