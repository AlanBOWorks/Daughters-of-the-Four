using System;
using CombatSystem.Skills;
using CombatSystem.Skills.Effects;
using Localization.Combat;
using UnityEngine;

namespace CombatSystem.Localization
{
    public static class LocalizeEffects 
    {
        public static string LocalizeEffectTooltip(in PerformEffectValues values)
        {
            var effect = values.Effect;
            var targeting = values.TargetType;
            string localizationTag = effect.EffectTag;
            string localizedName = CombatLocalizations.LocalizeEffectName(in localizationTag);
            string localizeTargeting = LocalizeLineTargeting(targeting);

            string localizedDigits = LocalizeEffectDigitValue(in values);
            return "[" + localizeTargeting + "]\n" + localizedName + ": " + localizedDigits;
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
    }
}
