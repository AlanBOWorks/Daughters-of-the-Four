using System;
using Characters;
using Sirenix.OdinInspector;
using Skills;
using UnityEngine;

namespace CombatConditions
{
    public abstract class SCondition : ScriptableObject
    {
        public abstract bool CanApply(CombatingEntity target, float checkValue);
    }


    [Serializable]
    public struct ConditionParam
    {
        [SerializeField]
        private SCondition condition;
        [SuffixLabel("u|%%"), ShowIf("condition"),
         Tooltip("Checks value is equals or below")]
        public float conditionalValue;
        public bool inverseCondition;
        [Tooltip("If is the user that needs to be checked or the target")]
        public bool isUserCheck;

        public bool HasCondition() => condition != null;
        public readonly bool CanApply(CombatingEntity user, CombatingEntity target)
        {
            var entityCheck = (isUserCheck) ? user : target;
            return CanApply(entityCheck);
        }

        public readonly bool CanApply(CombatingEntity target)
        {
            bool canBeApply = condition.CanApply(target, conditionalValue);

            return canBeApply ^ inverseCondition;
        }
    }


    [Serializable]
    public struct ConditionUserParams
    {
        public SCondition useCondition;
        public float conditionCheck;
        public bool inverseCondition;

        public bool CanBeUse(CombatingEntity user)
        {
            if (useCondition == null) return true;

            bool canBeUse = useCondition.CanApply(user, conditionCheck);
            return canBeUse ^ inverseCondition;
        }
    }
}
