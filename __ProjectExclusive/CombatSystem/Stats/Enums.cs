using UnityEngine;

namespace Stats
{
    public static class EnumStats
    {
        public enum BuffType
        {
            Base,
            Buff,
            Burst,
            Provoke
        }

        public enum OffensiveType
        {
            Direct,
            Persistent
        }
        public enum RecoveryType
        {
            Heal,
            Shield,
            Guard
        }

        public enum DamageResult
        {
            /// <summary>
            /// Non important damage was dealt
            /// </summary>
            None,
            /// <summary>
            /// Shields were lost
            /// </summary>
            ShieldBreak,
            /// <summary>
            /// Health reached 0
            /// </summary>
            HealthLost,
            /// <summary>
            /// Mortality reached 0
            /// </summary>
            Death
        }

        public enum RangeType
        {
            Melee,
            Ranged,
            /// <summary>
            /// Both Melee and Ranged; But with a preference towards Melee
            /// </summary>
            HybridMelee,
            /// <summary>
            /// Both Melee and Ranged, But with a preference towards Ranged
            /// </summary>
            HybridRanged //Means for common skills; selectable/unique skills can be both Melee and Ranged
        }

        public enum MasterStatType
        {
            Offensive,
            Support,
            Vitality,
            Concentration
        }

        public enum OffensiveStatType
        {
            Attack,
            Persistent,
            DeBuff,
            FollowUp
        }
        public enum SupportStatType
        {
            Heal = 10,
            Buff,
            ReceiveBuff,
            Shielding
        }
        public enum VitalityStatType
        {
            MaxHealth = 100,
            MaxMortality,
            DebuffResistance,
            DamageResistance
        }
        public enum ConcentrationStatType
        {
            InitiativeSpeed = 1000,
            InitialInitiative,
            ActionsPerSequence,
            Critical
        }

        public enum BaseStatType
        {
            Attack = OffensiveStatType.Attack,
            Persistent = OffensiveStatType.Persistent,
            DeBuff = OffensiveStatType.DeBuff,
            FollowUp = OffensiveStatType.FollowUp,

            Heal = SupportStatType.Heal,
            Buff = SupportStatType.Buff,
            ReceiveBuff = SupportStatType.ReceiveBuff,
            Shielding = SupportStatType.Shielding,

            MaxHealth = VitalityStatType.MaxHealth,
            MaxMortality = VitalityStatType.MaxMortality,
            DebuffResistance = VitalityStatType.DebuffResistance,
            DamageResistance = VitalityStatType.DamageResistance,

            InitiativeSpeed = ConcentrationStatType.InitiativeSpeed,
            InitialInitiative = ConcentrationStatType.InitialInitiative,
            ActionsPerSequence = ConcentrationStatType.ActionsPerSequence,
            Critical = ConcentrationStatType.Critical,
        }
    }
}
