using System;
using CombatSkills;
using UnityEngine;

namespace __ProjectExclusive.Localizations
{
    public static class Translation
    {
        public static string GetText(SSkill preset)
        {
            var currentLanguage = LocalizationSingleton.GameLanguage;
            switch (currentLanguage)
            {
                case LocalizationsEnum.Language.en_en:
                    throw new NotImplementedException("Skill language is not implemented");
                case LocalizationsEnum.Language.dev_en:
                default:
                    return preset.GetSkillName();
            }
        }
    }
}
