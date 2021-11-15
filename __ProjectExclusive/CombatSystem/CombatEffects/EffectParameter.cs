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
        
        [ShowIf("IsBuffType",Animate = false)]
        public EnumStats.BuffType buffType;


        [Title("Params")]
        public float effectValue;
        [HorizontalGroup()]
        public bool canCrit;
        [HorizontalGroup()]
        public bool isProvokeEffect;

#if UNITY_EDITOR
        // ___________ Used in ShowIf [Do not remove]
        private bool IsBuffType() => !(preset is IEffect);
#endif


        // Was needed a way to make the provoke effect implemented but invoking 
        // this method in the provoke will inject itself once more making a recursive loop;
        // Because of that it was separated in two methods, this being the default one using
        // during actions and DoDirectEffect for the actual effect logic
        /// <summary>
        /// Logic for the Action segment; It will perform the effect of the Skill or inject into another system
        /// depending of the parameters
        /// </summary>
        public void DoActionEffect(ISkillParameters parameters)
        {
            var target = parameters.Target;
            if (isProvokeEffect)
            {
                target.ProvokeEffects.Enqueue(this, parameters.UsedSkill);
                return;
            }

            bool isEffectCrit = canCrit && parameters.IsCritical;

            DoDirectEffect(parameters,isEffectCrit);
        }

        /// <summary>
        /// Does directly the effects. For checking states or special conditions that the skill could perform (such provoke)
        /// use [<seealso cref="DoActionEffect"/>] instead
        /// </summary>
        public void DoDirectEffect(ISkillParameters parameters,bool isEffectCrit = false)
        {
            switch (preset)
            {
                case IEffect effectPreset:
                    effectPreset.DoEffect(parameters, effectValue, isEffectCrit);
                    return;
                case IBuff buffPreset:
                    buffPreset.DoBuff(parameters, buffType, effectValue, isEffectCrit);
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
        public abstract EnumSkills.SkillInteractionType GetComponentType();
        public abstract Color GetDescriptiveColor();
        public abstract string GetEffectValueText(float effectValue);
    }
}
