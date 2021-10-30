using System;
using CombatEntity;
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
        [ShowIf("IsBuffType",Animate = false)]
        public EnumStats.BuffType buffType;

        [Title("Params")]
        public float effectValue;
        public bool canCrit;

        // Used in ShowIf
        private bool IsBuffType() => !(preset is IEffect);

        public void DoEffect(ISkillValues values)
        {
            bool isEffectCrit = canCrit && values.IsCritical;
            var entities = new CombatEntityPairAction(values.Performer,values.Target);
            DoEffect(entities,isEffectCrit);
        }

        public void DoEffect(CombatEntityPairAction entities,bool isEffectCrit = false)
        {
            if (buffType == EnumStats.BuffType.Provoke)
            {
                entities.Target.ProvokeEffects.Enqueue(this);
                return;
            }

            switch (preset)
            {
                case IEffect effectPreset:
                    effectPreset.DoEffect(entities, targetType, effectValue, isEffectCrit);
                    return;
                case IBuff buffPreset:
                    buffPreset.DoBuff(entities, buffType, targetType, effectValue, isEffectCrit);
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

    public abstract class SSkillComponentEffect : ScriptableObject, ISkillComponent {
    }
}
