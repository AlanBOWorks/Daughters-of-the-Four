using System;
using CombatSystem.Skills;
using CombatSystem.Skills.Effects;

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

        public static string LocalizeEffectDigitValue(float effectValue)
        {
            return " <b>" + effectValue.ToString("F1") + "</b>";
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
