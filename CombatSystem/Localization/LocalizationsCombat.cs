using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Localization
{
    public static class LocalizationsCombat 
    {
        public static string LocalizeStatName(EnumStats.StatType statType)
        {
            var elementName = UtilsStats.GetElement(statType, StatsTags.StatsNames);
            return LocalizeStatName(elementName);
        }

        public static string LocalizeStatName(string statName)
        {
            return statName;
        }

        public static string LocalizeSkillName(string skillTag)
        {
            return skillTag;
        }

        public static string LocalizeEffectName(string effectTag)
        {
            return effectTag;
        }

        public static string LocalizeStance(string stanceString)
        {
            return stanceString;
        }


    }
}
