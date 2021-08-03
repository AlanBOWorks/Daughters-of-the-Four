using System;
using System.Collections.Generic;
using Characters;
using CombatEffects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatConditions
{
    public abstract class SEffectCondition : ScriptableObject
    {
        public abstract bool CanApply(ref EffectArguments arguments, float conditionalCheck);
    }


    [Serializable]
    public struct ConditionParam
    {
        [SerializeField]
        private SEffectCondition condition;
        [SuffixLabel("00%"),Range(-10,10),ShowIf("condition")]
        public float conditionalValue;
        public bool inverseCondition;

        public bool HasCondition() => condition != null;
        public bool CanApplyCondition(ref EffectArguments arguments)
        {
            bool canBeApply = condition.CanApply(ref arguments, conditionalValue);
            return canBeApply ^ inverseCondition;
        }
    }

}
