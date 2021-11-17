using System.Collections.Generic;
using CombatEffects;
using CombatSkills;
using CombatTeam;
using UnityEngine;

namespace __ProjectExclusive.Localizations
{
    public static class EffectsLocalizationHandler
    {
        private static IEffectLocalizationHandler _effectLocalizationHolder = new ProvisionalEffectLocalization();


        public static string GetEffectLocalization(ISkillComponent element)
        {
            return _effectLocalizationHolder.GetLocalization(element);
        }
        private class ProvisionalEffectLocalization : IEffectLocalizationHandler
        {
            public string GetLocalization(ISkillComponent element)
            {
                if (element is ScriptableObject scriptableObject)
                {
                    return scriptableObject.name;
                }
                return element.ToString();
            }
        }
    }

    public interface IEffectLocalizationHandler
    {
        string GetLocalization(ISkillComponent element);
    }
}
