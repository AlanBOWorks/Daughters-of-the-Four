using System;
using CombatEffects;
using CombatSkills;
using CombatTeam;
using UnityEngine;

namespace __ProjectExclusive.Localizations
{
    public static class TranslatorSkill
    {
        public static string GetText(ISkill preset)
        {
            var currentLanguage = LocalizationSingleton.GameLanguage;
            switch (currentLanguage)
            {
                case LocalizationsEnum.Language.en_en:
                    throw new NotImplementedException(
                        $"[{typeof(SSkill)}.{currentLanguage}] language is not implemented");
                case LocalizationsEnum.Language.dev_en:
                default:
                    return preset.GetSkillName();
            }
        }


    }

}
