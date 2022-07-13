using System;
using System.Globalization;
using CombatSystem.Skills;
using CombatSystem.Skills.Effects;
using UnityEngine;

namespace CombatSystem.Localization
{
    public static class LocalizeEffects 
    {
        public static void LocalizeEffectTooltip(in PerformEffectValues values, out string localizedEffect)
        {
            var effect = values.Effect;
            var targeting = values.TargetType;
            string localizationTag = effect.EffectTag;
            string localizedName = CombatLocalizations.LocalizeEffectName(localizationTag);
            string localizeTargeting = LocalizeLineTargeting(targeting);

            localizedEffect = "[" + localizeTargeting + "]\n" + localizedName;
        }


        private const string SmallValueSuffix = "";
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
        public const string UnitSuffix = "u";
        public const string MixUnitSuffix = "u/%";


        public static string GetEffectValueSuffix(IEffect effect)
        {
            return effect switch
            {
                IOffensiveEffect o => UnitSuffix,
                ISupportEffect s => PercentSuffix,
                _ => MixUnitSuffix
            };
        }

        public static string LocalizeMathfValue(float value, bool isPercentValue)
        {
            return (isPercentValue) ? LocalizePercentValue(value) : LocalizeArithmeticValue(value);
        }

        /// <summary>
        /// Check if the value is higher than 1000 and converts it to [K,M,...]
        /// </summary>
        public static string LocalizeArithmeticValue(float value)
        {
            value = GetSimplifiedValue(value, out var valueSuffix);
            return value.ToString("####.#") + valueSuffix + UnitSuffix;
        }

        public static string LocalizePercentValue(float value)
        {
            value *= 100f;
            value = GetSimplifiedValue(value, out var valueSuffix);
            return value.ToString("####") + valueSuffix + PercentSuffix;
        }

        public static float GetSimplifiedValue(float value, out string valueSuffix)
        {
            if (value < ThousandThreshold)
            {
                valueSuffix = SmallValueSuffix;
                return value;
            }

            if (value < MillionThreshold)
            {
                valueSuffix = ThousandValueSuffix;
                return (value * ThousandSimplificationModifier);
            }

            if (value < BillionThreshold)
            {
                valueSuffix = MillionsValueSuffix;
                return (value * MillionSimplificationModifier);
            }

            valueSuffix = BillionValueSuffix;
            return (value * BillionSimplificationModifier);
        }

        public static string LocalizeLineTargeting(EnumsEffect.TargetType targetType)
        {
            return targetType.ToString().ToUpper();


            /*
            string GetLineTag()
            {
                return targetType switch
                {
                    EnumsEffect.TargetType.TargetTeam => EnumsEffect.TargetTeamTypeTag,
                    EnumsEffect.TargetType.Performer => EnumsEffect.PerformerTypeTag,
                    EnumsEffect.TargetType.PerformerTeam => EnumsEffect.PerformerTeamTypeTag,
                    EnumsEffect.TargetType.TargetLine => EnumsEffect.TargetLineTypeTag,
                    EnumsEffect.TargetType.PerformerLine => EnumsEffect.PerformerLineTypeTag,
                    EnumsEffect.TargetType.All => EnumsEffect.AllTypeTag,
                    _ => EnumsEffect.TargetLineTypeTag
                };
            }*/
        }


    }
}
