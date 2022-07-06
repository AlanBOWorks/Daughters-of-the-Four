using System;
using CombatSystem.Skills;
using CombatSystem.Skills.Effects;

namespace CombatSystem.Localization
{
    public static class LocalizeEffects 
    {
        public static void LocalizeEffectTooltip(in PerformEffectValues values, out string localizedEffect, out string effectValueDigits)
        {
            var effect = values.Effect;
            var targeting = values.TargetType;
            string localizationTag = effect.EffectTag;
            string localizedName = CombatLocalizations.LocalizeEffectName(localizationTag);
            string localizeTargeting = LocalizeLineTargeting(targeting);

            localizedEffect = "[" + localizeTargeting + "]\n" + localizedName + ": ";
             effectValueDigits= LocalizeEffectDigitValue(in values);
        }

        public static string LocalizeEffectDigitValue(in PerformEffectValues values)
        {
            string effectValue = values.EffectValue.ToString("F1");
            return effectValue;
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
    }
}
