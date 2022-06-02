using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utils
{
    [Serializable]
    public struct SRange
    {
        public SRange(float min, float max)
        {
            minValue = min;
            maxValue = max;
        }

        public float minValue;
        public float maxValue;

        public float Clamp(float value) => Mathf.Clamp(value, minValue, maxValue);
        public float CalculateRandom() => Random.Range(minValue, maxValue);
        public float Evaluate(float percent) => Mathf.LerpUnclamped(minValue, maxValue, percent);
        public float EvaluateClamped(float percent) => Mathf.Lerp(minValue, maxValue, percent);

    }

}