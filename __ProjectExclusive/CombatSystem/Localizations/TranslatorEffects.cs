using System;
using CombatEffects;
using CombatTeam;
using Stats;
using UnityEngine;

namespace __ProjectExclusive.Localizations
{
    public static class TranslatorEffects 
    {
        private static IFullTargetingStructureRead<string> _targetingLocalizationHolder = new TargetingProvisionalText();

        public static string GetText(EnumEffects.TargetType effectType)
        {
            return UtilsEffects.GetElement(effectType, _targetingLocalizationHolder);
        }

        public static string GetText(EffectParameter effect)
        {
            var component = effect.preset;
            var effectColor = component.GetDescriptiveColor();
            string effectText 
                = $"<#{ColorUtility.ToHtmlStringRGB(effectColor)}>"
                + EffectsLocalizationHandler.GetEffectLocalization(component) 
                + "</color>: " 
                + component.GetEffectValueText(effect.effectValue);
            return effectText;
        }

        public static void SwitchTargetingTextHolder(IFullTargetingStructureRead<string> holder)
        {
            _targetingLocalizationHolder = holder;
        }
        private class TargetingProvisionalText : IFullTargetingStructureRead<string>
        {
            private const string SelfTypeText = "Self";
            private const string SelfAlliesTypeText = "Allies";
            private const string SelfTeamTypeText = "Team";
            private const string TargetTypeText = "Target";
            private const string TargetAlliesTypeText = "Target(Allies)";
            private const string TargetTeamTypeText = "Target(Team)";

            public string SelfElement => SelfTypeText;
            public string SelfTeamElement => SelfTeamTypeText;
            public string SelfAlliesElement => SelfAlliesTypeText;
            public string TargetElement => TargetTypeText;
            public string TargetTeamElement => TargetTeamTypeText;
            public string TargetAlliesElement => TargetAlliesTypeText;
        }
    }
}
