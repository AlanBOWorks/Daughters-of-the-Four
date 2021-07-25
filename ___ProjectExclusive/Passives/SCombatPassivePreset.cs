using System;
using ___ProjectExclusive;
using CombatConditions;
using CombatEffects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Passives
{

   
    public abstract class SCombatPassivePreset : ScriptableObject, IPassiveEffect
    {
        [SerializeField, Delayed] 
        private string passiveName = "NULL";
        public string PassiveName => passiveName;

        [TitleGroup("Passives")]
        [SerializeField] 
        private PassiveParam[] passives = new PassiveParam[0];

        public void DoPassiveFilter(
            ref EffectArguments arguments,
            ref float currentValue,
            float originalValue)
        {
            foreach (PassiveParam passive in passives)
            {
                passive.DoPassiveFilter(ref arguments,ref currentValue,originalValue);
            }
        }

        protected const string InjectionNamePrefix = " - Combat Passive [Preset]";
        [Button(ButtonSizes.Large)]
        protected virtual void UpdateAssetName()
        {
            name = PassiveName.ToUpper() + InjectionNamePrefix;
            UtilsGame.UpdateAssetName(this);
        }
    }

    [Serializable]
    internal struct PassiveParam : IPassiveEffect
    {
        [Title("Effect")]
        [SerializeField] private SPassiveEffect effect;
        [SerializeField,ShowIf("effect")]
        [SuffixLabel("00%"), Range(0, 10)] private float effectValue;

        [Title("Conditions")]
        [SerializeField] private SCombatCondition condition;
        [SerializeField, ShowIf("condition")]
        [SuffixLabel("00%"), Range(-10, 10)] private float conditionValue;

        public void DoPassiveFilter(ref EffectArguments arguments, ref float currentValue, float originalValue)
        {
            if (condition == null || condition.CanApply(ref arguments, conditionValue))
            {
                if(effect.CanApplyPassive(arguments.Effect))
                    effect.DoPassiveFilter(ref currentValue,originalValue, effectValue);
            }
        }
    }

    public abstract class SPassiveEffect : ScriptableObject
    {
        public abstract void DoPassiveFilter
            (ref float currentValue, float originalValue, float effectValue);

        public abstract bool CanApplyPassive(SEffectBase effect);
    }

}
