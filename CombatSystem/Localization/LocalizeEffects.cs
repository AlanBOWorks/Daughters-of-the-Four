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
            string localizationTag = values.Effect.EffectTag;
            string localizedName = CombatLocalizations.LocalizeEffectName(in localizationTag);


            string localizedDigits = LocalizeEffectDigitValue(in values);
            return localizedName + ": " + localizedDigits;
        }

        public static string LocalizeEffectDigitValue(in PerformEffectValues values)
        {
            string effectValue = values.EffectValue.ToString("F1");
            return effectValue;
        }
    }
}
