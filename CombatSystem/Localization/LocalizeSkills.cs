using System;
using CombatSystem.Skills;
using UnityEngine;

namespace CombatSystem.Localization
{
    public static class LocalizeSkills 
    {
        public static string LocalizeEffect(in IEffect effect)
        {
            throw new NotImplementedException("Localize Effect is not Implemented");
        }

        public static string LocalizeEffect(in PerformEffectValues values)
        {
            //string effectName = LocalizeEffect(values.Effect); todo once finish, concat
            string effectValue = values.EffectValue.ToString("F1");
            return effectValue;
        }
    }
}
