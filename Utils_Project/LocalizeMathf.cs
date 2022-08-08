namespace Utils_Project
{
    public static class LocalizeMath
    {
        public const string SmallValueSuffix = "";
        public const string ThousandValueSuffix = "K";
        public const float ThousandThreshold = 1000;
        public const float ThousandSimplificationModifier = 0.001f;
        public const string MillionsValueSuffix = "M";
        public const float MillionThreshold = 1000000;
        public const float MillionSimplificationModifier = 0.000001f;
        public const string BillionValueSuffix = "B";
        public const float BillionThreshold = 1000000000;
        public const float BillionSimplificationModifier = 0.000000001f;


        public const string PercentSuffix = "%";
        public const string UnitSuffix = "u.";
        public const string MixUnitSuffix = "u/%";

        /// <summary>
        /// <inheritdoc cref="LocalizeMathfValue(float,bool)"/>
        /// </summary>
        public static string LocalizeMathfValue(float value, float highValue, bool isPercentValue)
        {
            if (highValue - value < .01f)
                return LocalizeMathfValue(value, isPercentValue);

            string minValueText;
            string highValueText;
            if (isPercentValue)
            {
                minValueText = LocalizePercentValueWithDecimals(value);
                highValueText = LocalizePercentValueWithDecimals(highValue);

                return ConcatenateValues(PercentSuffix);
            }

            minValueText = LocalizeArithmeticValue(value);
            highValueText = LocalizeArithmeticValue(highValue);
            return ConcatenateValues(UnitSuffix);


            string ConcatenateValues(string suffix)
            {
                return "[" + minValueText + " - " + highValueText + "]" + suffix;
            }
        }

        /// <summary>
        /// Generic type that checks the [<paramref name="isPercentValue"/>]: <br></br>
        /// - true: returns [<seealso cref="LocalizePercentValueWithDecimals"/>]; <br></br>
        /// - false: returns [<seealso cref="LocalizeArithmeticValue"/>]
        /// </summary>
        public static string LocalizeMathfValue(float value, bool isPercentValue)
        {
            return (isPercentValue)
                ? LocalizePercentValueWithDecimals(value) + PercentSuffix
                : LocalizeArithmeticValue(value) + UnitSuffix;
        }

        /// <summary>
        /// Check if the value is higher than 1000 and converts it to [K,M,...]
        /// </summary>
        public static string LocalizeArithmeticValue(float value)
        {
            value = UtilsMath.GetSimplifiedValue(value, out var valueSuffix);
            return value.ToString("####.#") + valueSuffix;
        }
        /// <summary>
        /// <inheritdoc cref="LocalizeArithmeticValue"/>
        /// </summary>
        public static string LocalizeArithmeticIntegerValue(float value)
        {
            value = UtilsMath.GetSimplifiedValue(value, out var valueSuffix);
            return value.ToString("####") + valueSuffix;
        }

        private const string ZeroPercentText = "0";

        public static string LocalizePercentValue(float value)
        {
            if (value < .005f && value > -.005f) return ZeroPercentText;
            value *= 100f;
            value = UtilsMath.GetSimplifiedValue(value, out var valueSuffix);
            return value.ToString("####") + valueSuffix;
        }

        public static string LocalizePercentValueWithDecimals(float value)
        {
            if (value < .005f && value > -.005f) return ZeroPercentText;
            value *= 100f;
            value = UtilsMath.GetSimplifiedValue(value, out var valueSuffix);
            return value.ToString("###.#") + valueSuffix;
        }
    }

    public static class UtilsMath
    {
        public static float GetSimplifiedValue(float value, out string valueSuffix)
        {
            if (value < LocalizeMath.ThousandThreshold)
            {
                valueSuffix = LocalizeMath.SmallValueSuffix;
                return value;
            }

            if (value < LocalizeMath.MillionThreshold)
            {
                valueSuffix = LocalizeMath.ThousandValueSuffix;
                return (value * LocalizeMath.ThousandSimplificationModifier);
            }

            if (value < LocalizeMath.BillionThreshold)
            {
                valueSuffix = LocalizeMath.MillionsValueSuffix;
                return (value * LocalizeMath.MillionSimplificationModifier);
            }

            valueSuffix = LocalizeMath.BillionValueSuffix;
            return (value * LocalizeMath.BillionSimplificationModifier);
        }
    }
}
