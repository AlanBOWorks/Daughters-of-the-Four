using System;
using CombatSystem.Skills;
using CombatSystem.Skills.Effects;
using UnityEngine;

namespace CombatSystem.Localization
{
    public static class LocalizeEffects 
    {
        public static string LocalizeEffectTooltip(in PerformEffectValues values)
        {
            //string targetType; todo
            string localizedName = values.Effect.ToString();
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
