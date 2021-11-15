using System;
using CombatSkills;
using Stats;

namespace __ProjectExclusive.Localizations
{
    public class TranslatorStats 
    {
        public static string GetText(EnumStats.MasterStatType stat)
        {
            return UtilStats.GetElement(stat, _statsLocalization as IMasterStatsRead<string>);
        }
        public static string GetText(EnumStats.OffensiveStatType stat)
        {
            return UtilStats.GetElement(stat, _statsLocalization);
        }

        public static string GetText(EnumStats.SupportStatType stat)
        {
            return UtilStats.GetElement(stat, _statsLocalization);
        }

        public static string GetText(EnumStats.VitalityStatType stat)
        {
            return UtilStats.GetElement(stat, _statsLocalization as IVitalityStatsRead<string>);
        }

        public static string GetText(EnumStats.ConcentrationStatType stat)
        {
            return UtilStats.GetElement(stat,_statsLocalization as IConcentrationStatsRead<string>);
        }

        public static string GetText(EnumSkills.DominionType dominionType)
        {
            return UtilSkills.GetElement(dominionType, _statsLocalization);
        }


        private static ISkillInteractionStructureRead<string> _statsLocalization = new ProvisionalStatsLocalizations();
        private class ProvisionalStatsLocalizations : IMasterStatsRead<string>,ISkillInteractionStructureRead<string>, IBaseStatsRead<string>
            
        {
            public string Offensive => "Offensive";
            public string Support => "Support";
            public string Vitality => "Vitality";
            public string Concentration => "Concentration";

            public string Attack => "Attack";
            public string Persistent => "Persistent";
            public string Debuff => "Debuff";
            public string FollowUp => "FollowUp";

            public string Heal => "Heal";
            public string Shielding => "Shielding";
            public string Buff => "Buff";
            public string ReceiveBuff => "ReceiveBuff";

            public string Guard => "Guard";
            public string Control => "Control";
            public string Provoke => "Provoke";
            public string Stance => "Stance";

            public string MaxHealth => "MaxHealth";
            public string MaxMortality => "MaxMortality";
            public string DebuffResistance => "DebuffResistance";
            public string DamageResistance => "DamageResistance";

            public string InitiativeSpeed => "InitiativeSpeed";
            public string InitialInitiative => "InitialInitiative";
            public string ActionsPerSequence => "ActionsPerSequence";
            public string Critical => "Critical";
        }

    }
}
