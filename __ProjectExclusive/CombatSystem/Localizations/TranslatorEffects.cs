using System;
using CombatEffects;
using CombatTeam;
using UnityEngine;

namespace __ProjectExclusive.Localizations
{
    public static class TranslatorEffects 
    {
        public static string GetText(EnumEffects.TargetType effectType)
        {
            var currentLanguage = LocalizationSingleton.GameLanguage;
            IFullTargetingStructureRead<string> targetingTexts;
            if(currentLanguage != LocalizationsEnum.Language.dev_en)
                throw new NotImplementedException(
                    $"[{typeof(EnumEffects.TargetType)}.{currentLanguage}] language is not implemented");

            targetingTexts = ProvisionalTargetingTexts;
            return UtilsEffects.GetElement(effectType, targetingTexts);
        }

        private static readonly IFullTargetingStructureRead<string> ProvisionalTargetingTexts = new TargetingProvisionalText();
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
