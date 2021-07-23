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
        public abstract bool CanApply(CombatConditionArguments arguments, float conditionalCheck);
    }

    [Serializable]
    public struct ConditionParam
    {
        [SerializeField]
        private SCombatCondition condition;
        [SuffixLabel("00%"),Range(-10,10)]
        public float conditionalValue;

        public bool CanApplyCondition(CombatConditionArguments arguments)
        {
            return condition.CanApply(arguments, conditionalValue);
        }
    }

    public static class UtilsConditions
    {
        public static bool CanApplyConditions(ConditionParam[] conditions, CombatConditionArguments arguments)
        {
            for (int i = 0; i < conditions.Length; i++)
            {
                if (!conditions[i].CanApplyCondition(arguments))
                    return false;
            }

            return true;
        }
    }

    public struct CombatConditionArguments
    {
        public readonly CombatingEntity User;
        public readonly CombatingEntity Target;
        public readonly SEffectBase Effect;

        public CombatConditionArguments(CombatingEntity user, CombatingEntity target, SEffectBase effect)
        {
            User = user;
            Target = target;
            Effect = effect;
        }
    }

    public interface ICombatCondition
    {
        bool CanApply(CombatConditionArguments arguments, float conditionalCheck);
    }
}
