using System;
using System.Globalization;
using CombatSystem.Skills;
using CombatSystem.Skills.Effects;
using UnityEngine;
using Utils_Project;

namespace CombatSystem.Localization
{
    public static class LocalizeEffects 
    {
        public static void LocalizeEffectTooltip(in PerformEffectValues values, 
            out string localizedEffect, 
            bool handleTargeting = false)
        {
            var effect = values.Effect;
            var targeting = values.TargetType;
            string localizationTag = effect.EffectTag;
            string localizedName = LocalizationsCombat.LocalizeEffectTag(localizationTag);

            localizedEffect = localizedName;

            if(!handleTargeting) return;
            string localizeTargeting = LocalizeLineTargeting(targeting);

            localizedEffect = "[" + localizeTargeting + "]\n" + localizedEffect;
        }



        public static string GetEffectValueSuffix(IEffect effect)
        {
            if (effect == null) return LocalizeMath.MixUnitSuffix;
            return effect.IsPercentTooltip() ? LocalizeMath.PercentSuffix : LocalizeMath.UnitSuffix;
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
