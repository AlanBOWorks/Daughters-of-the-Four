using UnityEngine;

namespace Utils.Maths
{
    public static class UtilsMath 
    {
        public static float UnSafeCalculateDeltaStep(float inTimeAmount)
            => inTimeAmount / Time.deltaTime;
        public static float CalculateDeltaSteps(float inTimeAmount)
        {
            if (inTimeAmount < 0) return 1;
            return UnSafeCalculateDeltaStep(inTimeAmount);
        }

        public static float UnsafeCalculateDeltaUnitIncrement(float inTimeAmount)
        {
            var steps = (UnSafeCalculateDeltaStep(inTimeAmount));
            steps = Mathf.Round(steps);
            return 1 / steps;
        }

        public static float CalculateDeltaUnitIncrement(float inTimeAmount)
        {
            if (inTimeAmount <= 0) return 1;
            var increment = UnsafeCalculateDeltaUnitIncrement(inTimeAmount);
            if (increment > 1) return 1;
            return increment;
        }

    }


    public readonly struct PercentValue
    {
        public readonly float MaxValue;
        public readonly float CurrentValue;
        public readonly float UnitPercentValue;

        public PercentValue(float currentValue, float maxValue)
        {
            MaxValue = maxValue;
            CurrentValue = currentValue;

            if (maxValue != 0)
                UnitPercentValue = currentValue / maxValue;
            else
                UnitPercentValue = 0;
        }
    }
}
