using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Localization
{
    public static class StatsTags
    {
        // Name
        public const string OffensiveStatName = "Offensive";
        public const string SupportStatName = "Support";
        public const string VitalityStatName = "Vitality";
        public const string ConcentrationStatName = "Concentration";

        public const string AttackStatName = "Attack";
        public const string OverTimeStatName = "Over Time";
        public const string DeBuffStatName = "DeBuff";
        public const string FollowUpStatName = "Follow Up";

        public const string HealStatName = "Heal";
        public const string ShieldingStatName = "Shielding";
        public const string BuffStatName = "Buff";
        public const string ReceiveBuffStatName = "Receive Buff";

        public const string HealthStatName = "Health";
        public const string MortalityStatName = "Mortality";
        public const string DamageReductionStatName = "Protection";
        public const string DeBuffResistanceStatName = "Resistance";

        public const string ActionsStatName = "Actions";
        public const string SpeedStatName = "Speed";
        public const string ControlStatName = "ControlGain";
        public const string CriticalStatName = "Luck";


        public const string InitiativeStatName = "Initiative";


        public static readonly IStatsRead<string> HolderStatsNames = new StatsNamesHolder();
        private sealed class StatsNamesHolder : IStatsRead<string>
        {
            public string OffensiveStatType { get; } = OffensiveStatName;
            public string SupportStatType { get; } = SupportStatName;
            public string VitalityStatType { get; } = VitalityStatName;
            public string ConcentrationStatType { get; } = ConcentrationStatName;

            public string AttackType { get; } = AttackStatName;
            public string OverTimeType { get; } = OverTimeStatName;
            public string DeBuffType { get; } = DeBuffStatName;
            public string FollowUpType { get; } = FollowUpStatName;
            public string HealType { get; } = HealStatName;
            public string ShieldingType { get; } = ShieldingStatName;
            public string BuffType { get; } = BuffStatName;
            public string ReceiveBuffType { get; } = ReceiveBuffStatName;
            public string HealthType { get; } = HealthStatName;
            public string MortalityType { get; } = MortalityStatName;
            public string DamageReductionType { get; } = DamageReductionStatName;
            public string DeBuffResistanceType { get; } = DeBuffResistanceStatName;
            public string ActionsType { get; } = ActionsStatName;
            public string SpeedType { get; } = SpeedStatName;
            public string ControlType { get; } = ControlStatName;
            public string CriticalType { get; } = CriticalStatName;
        }

        // Tag
        public const string OffensiveStatTag = "Offensive_Stat";
        public const string SupportStatTag = "Support_Stat";
        public const string VitalityStatTag = "Vitality_Stat";
        public const string ConcentrationStatTag = "Concentration_Stat";
        
        public const string AttackStatTag = "Attack_Stat";
        public const string OverTimeStatTag = "Over Time_Stat";
        public const string DeBuffStatTag = "DeBuff_Stat";
        public const string FollowUpStatTag = "Follow Up_Stat";

        public const string HealStatTag = "Heal_Stat";
        public const string ShieldingStatTag = "Shielding_Stat";
        public const string BuffStatTag = "Buff_Stat";
        public const string ReceiveBuffStatTag = "Receive Buff_Stat";

        public const string HealthStatTag = "Health_Stat";
        public const string MortalityStatTag = "Mortality_Stat";
        public const string DamageReductionStatTag = "Protection_Stat";
        public const string DeBuffResistanceStatTag = "Resistance_Stat";

        public const string ActionsStatTag = "Actions_Stat";
        public const string SpeedStatTag = "Speed_Stat";
        public const string ControlStatTag = "Control_Stat";
        public const string CriticalStatTag = "Luck_Stat";


        public const string InitiativeStatTag = "Initiative_Stat";

        public static readonly IStatsRead<string> HolderStatsTags = new StatsTagsHolder();
        private sealed class StatsTagsHolder : IStatsRead<string>
        {
            public string OffensiveStatType { get; } = OffensiveStatTag;
            public string SupportStatType { get; } = SupportStatTag;
            public string VitalityStatType { get; } = VitalityStatTag;
            public string ConcentrationStatType { get; } = ConcentrationStatTag;

            public string AttackType { get; } = AttackStatTag;
            public string OverTimeType { get; } = OverTimeStatTag;
            public string DeBuffType { get; } = DeBuffStatTag;
            public string FollowUpType { get; } = FollowUpStatTag;
            public string HealType { get; } = HealStatTag;
            public string ShieldingType { get; } = ShieldingStatTag;
            public string BuffType { get; } = BuffStatTag;
            public string ReceiveBuffType { get; } = ReceiveBuffStatTag;
            public string HealthType { get; } = HealthStatTag;
            public string MortalityType { get; } = MortalityStatTag;
            public string DamageReductionType { get; } = DamageReductionStatTag;
            public string DeBuffResistanceType { get; } = DeBuffResistanceStatTag;
            public string ActionsType { get; } = ActionsStatTag;
            public string SpeedType { get; } = SpeedStatTag;
            public string ControlType { get; } = ControlStatTag;
            public string CriticalType { get; } = CriticalStatTag;
        }


        // Prefix
        public const string OffensiveStatPrefix = "Off";
        public const string SupportStatPrefix = "Supp";
        public const string VitalityStatPrefix = "Vit";
        public const string ConcentrationStatPrefix = "Conc";

        public const string AttackStatPrefix = "Atk";
        public const string OverTimeStatPrefix = "DoT";
        public const string DeBuffStatPrefix = "DBff";
        public const string FollowUpStatPrefix = "FUp";

        public const string HealStatPrefix = "Heal";
        public const string ShieldingStatPrefix = "Shld";
        public const string BuffStatPrefix = "Buff";
        public const string ReceiveBuffStatPrefix = "RBuf";

        public const string HealthStatPrefix = "HP";
        public const string MortalityStatPrefix = "MP";
        public const string DamageReductionStatPrefix = "Prot";
        public const string DeBuffResistanceStatPrefix = "Rest";

        public const string ActionsStatPrefix = "Acts";
        public const string SpeedStatPrefix = "Spd";
        public const string ControlStatPrefix = "Ctrl";
        public const string CriticalStatPrefix = "Luck";


        public const string InitiativeStatPrefix = "Init";


        public static readonly IStatsRead<string> HolderStatsPrefix = new StatsPrefixHolder();
        private sealed class StatsPrefixHolder : IStatsRead<string>
        {
            public string OffensiveStatType { get; } = OffensiveStatPrefix;
            public string SupportStatType { get; } = SupportStatPrefix;
            public string VitalityStatType { get; } = VitalityStatPrefix;
            public string ConcentrationStatType { get; } = ConcentrationStatPrefix;

            public string AttackType { get; } = AttackStatPrefix;
            public string OverTimeType { get; } = OverTimeStatPrefix;
            public string DeBuffType { get; } = DeBuffStatPrefix;
            public string FollowUpType { get; } = FollowUpStatPrefix;
            public string HealType { get; } = HealStatPrefix;
            public string ShieldingType { get; } = ShieldingStatPrefix;
            public string BuffType { get; } = BuffStatPrefix;
            public string ReceiveBuffType { get; } = ReceiveBuffStatPrefix;
            public string HealthType { get; } = HealthStatPrefix;
            public string MortalityType { get; } = MortalityStatPrefix;
            public string DamageReductionType { get; } = DamageReductionStatPrefix;
            public string DeBuffResistanceType { get; } = DeBuffResistanceStatPrefix;
            public string ActionsType { get; } = ActionsStatPrefix;
            public string SpeedType { get; } = SpeedStatPrefix;
            public string ControlType { get; } = ControlStatPrefix;
            public string CriticalType { get; } = CriticalStatPrefix;
        }
    }
}
