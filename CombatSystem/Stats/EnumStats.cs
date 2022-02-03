using UnityEngine;

namespace CombatSystem.Stats
{
    public static class EnumStats 
    {
        public enum BuffType
        {
            Base,
            Buff,
            Burst
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


        public enum MasterStatType
        {
            Offensive,
            Support,
            Vitality,
            Concentration
        }


        public const int AttackIndex = 0;
        public const int OverTimeIndex = AttackIndex + 1;
        public const int DeBuffIndex = OverTimeIndex + 1;
        public const int FollowUpIndex = DeBuffIndex + 1;
        public enum OffensiveStatType
        {
            Attack = AttackIndex,
            OverTime = OverTimeIndex,
            DeBuff = DeBuffIndex,
            FollowUp = FollowUpIndex
        }

        public const int HealIndex = 10;
        public const int ShieldingIndex = HealIndex + 1;
        public const int BuffIndex = ShieldingIndex +1;
        public const int ReceiveBuffIndex = BuffIndex +1;
        public enum SupportStatType
        {
            Heal = HealIndex,
            Shielding = ShieldingIndex,
            Buff = BuffIndex,
            ReceiveBuff = ReceiveBuffIndex,
        }

        public const int HealthIndex = 100;
        public const int MortalityIndex = HealthIndex + 1;
        public const int DamageResistanceIndex = MortalityIndex + 1;
        public const int DebuffResistanceIndex = DamageResistanceIndex + 1;
        public enum VitalityStatType
        {
            Health = HealthIndex,
            Mortality = MortalityIndex,
            DamageReduction = DamageResistanceIndex,
            DebuffResistance = DebuffResistanceIndex,
        }

        public const int SpeedIndex = 1000;
        public const int ControlIndex = SpeedIndex + 1;
        public const int ActionsIndex = ControlIndex + 1;
        public const int CriticalIndex = ActionsIndex + 1;
        public enum ConcentrationStatType
        {
            Actions = ActionsIndex,
            Speed = SpeedIndex,
            Control = ControlIndex,
            Critical = CriticalIndex,
        }

        public enum StatType
        {
            Attack = OffensiveStatType.Attack,
            OverTime = OffensiveStatType.OverTime,
            DeBuff = OffensiveStatType.DeBuff,
            FollowUp = OffensiveStatType.FollowUp,

            Heal = SupportStatType.Heal,
            Shielding = SupportStatType.Shielding,
            Buff = SupportStatType.Buff,
            ReceiveBuff = SupportStatType.ReceiveBuff,

            Health = VitalityStatType.Health,
            Mortality = VitalityStatType.Mortality,
            DamageReduction = VitalityStatType.DamageReduction,
            DebuffResistance = VitalityStatType.DebuffResistance,

            Speed = ConcentrationStatType.Speed,
            Actions = ConcentrationStatType.Actions,
            Control = ConcentrationStatType.Control,
            Critical = ConcentrationStatType.Critical,
        }
    }
}
