using System;
using System.Collections.Generic;
using Characters;
using CombatEffects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatConditions
{
    public abstract class SCombatCondition : ScriptableObject, ICombatCondition
    {
        public abstract bool CanApply(ref EffectArguments arguments, float conditionalCheck);
    }


    [Serializable]
    public struct ConditionParam
    {
        [SerializeField]
        private SCombatCondition condition;
        [SuffixLabel("00%"),Range(-10,10),ShowIf("condition")]
        public float conditionalValue;

        public bool HasCondition() => condition != null;
        public bool CanApplyCondition(ref EffectArguments arguments)
        {
            return condition.CanApply(ref arguments, conditionalValue);
        }
    }

    public interface ICombatCondition
    {
        bool CanApply(ref EffectArguments arguments, float conditionalCheck);
    }
}
