using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Localization
{
    public static class LocalizeStats 
    {
        public static string LocalizeStatTag(EnumStats.StatType statType)
        {
            var elementName = UtilsStats.GetElement(statType, StatsTags.HolderStatsTags);
            return LocalizeStatTag(elementName);
        }

        public static string LocalizeStatTag(EnumStats.MasterStatType statType)
        {
            var elementName = UtilsStats.GetElement(statType, StatsTags.HolderStatsTags);
            return LocalizeStatTag(elementName);
        }
        public static string LocalizeStatTag(EnumStats.OffensiveStatType statType)
        {
            var elementName = UtilsStats.GetElement(statType, StatsTags.HolderStatsTags);
            return LocalizeStatTag(elementName);
        }
        public static string LocalizeStatTag(EnumStats.SupportStatType statType)
        {
            var elementName = UtilsStats.GetElement(statType, StatsTags.HolderStatsTags);
            return LocalizeStatTag(elementName);
        }
        public static string LocalizeStatTag(EnumStats.VitalityStatType statType)
        {
            var elementName = UtilsStats.GetElement(statType, StatsTags.HolderStatsTags);
            return LocalizeStatTag(elementName);
        }
        public static string LocalizeStatTag(EnumStats.ConcentrationStatType statType)
        {
            var elementName = UtilsStats.GetElement(statType, StatsTags.HolderStatsTags);
            return LocalizeStatTag(elementName);
        }

        public static string LocalizeStatTag(string statTag)
        {
            return statTag;
        }

        public static string LocalizeStatName(EnumStats.StatType statType)
        {
            var elementName = UtilsStats.GetElement(statType, StatsTags.HolderStatsNames);
            return LocalizeStatName(elementName);
        }

        public static string LocalizeStatName(EnumStats.OffensiveStatType statType)
        {
            var elementName = UtilsStats.GetElement(statType, StatsTags.HolderStatsNames);
            return LocalizeStatName(elementName);
        }
        public static string LocalizeStatName(EnumStats.SupportStatType statType)
        {
            var elementName = UtilsStats.GetElement(statType, StatsTags.HolderStatsNames);
            return LocalizeStatName(elementName);
        }
        public static string LocalizeStatName(EnumStats.VitalityStatType statType)
        {
            var elementName = UtilsStats.GetElement(statType, StatsTags.HolderStatsNames);
            return LocalizeStatName(elementName);
        }
        public static string LocalizeStatName(EnumStats.ConcentrationStatType statType)
        {
            var elementName = UtilsStats.GetElement(statType, StatsTags.HolderStatsNames);
            return LocalizeStatName(elementName);
        }

        public static string LocalizeStatName(string elementName)
        {
            return elementName;
        }

        public static string LocalizeStatPrefix(EnumStats.StatType statType)
        {
            var elementName = UtilsStats.GetElement(statType, StatsTags.HolderStatsPrefix);
            return LocalizeStatPrefix(elementName);
        }

        public static string LocalizeStatPrefix(EnumStats.MasterStatType statType)
        {
            var elementName = UtilsStats.GetElement(statType, StatsTags.HolderStatsPrefix);
            return LocalizeStatPrefix(elementName);
        }
        public static string LocalizeStatPrefix(EnumStats.OffensiveStatType statType)
        {
            var elementName = UtilsStats.GetElement(statType, StatsTags.HolderStatsPrefix);
            return LocalizeStatPrefix(elementName);
        }
        public static string LocalizeStatPrefix(EnumStats.SupportStatType statType)
        {
            var elementName = UtilsStats.GetElement(statType, StatsTags.HolderStatsPrefix);
            return LocalizeStatPrefix(elementName);
        }
        public static string LocalizeStatPrefix(EnumStats.VitalityStatType statType)
        {
            var elementName = UtilsStats.GetElement(statType, StatsTags.HolderStatsPrefix);
            return LocalizeStatPrefix(elementName);
        }
        public static string LocalizeStatPrefix(EnumStats.ConcentrationStatType statType)
        {
            var elementName = UtilsStats.GetElement(statType, StatsTags.HolderStatsPrefix);
            return LocalizeStatPrefix(elementName);
        }

        public static string LocalizeStatPrefix(string statTag)
        {
            return statTag;
        }
    }
}
