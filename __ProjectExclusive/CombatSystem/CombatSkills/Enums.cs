using Stats;
using UnityEngine;

namespace CombatSkills
{
    public static class EnumSkills
    {
        /// <summary>
        /// [Idle, InCooldown]
        /// </summary>
        public enum SKillState
        {
            Idle,
            Cooldown,
            /// <summary>
            /// Can't be used this sequence
            /// </summary>
            Silence
            //Persistent??
        }

        /// <summary>
        /// [Self, Support, Offensive]
        /// </summary>
        public enum TargetType
        {
            Self,
            Support,
            Offensive
        }

        public enum DominionType
        {
            Guard = 10000,
            Control,
            Provoke,
            Stance
        }

        public enum SkillInteractionType
        {
            Attack = EnumStats.OffensiveStatType.Attack,
            Persistent = EnumStats.OffensiveStatType.Persistent,
            DeBuff = EnumStats.OffensiveStatType.DeBuff,
            FollowUp = EnumStats.OffensiveStatType.FollowUp,

            Heal = EnumStats.SupportStatType.Heal,
            Buff = EnumStats.SupportStatType.Buff,
            ReceiveBuff = EnumStats.SupportStatType.ReceiveBuff,
            Shielding = EnumStats.SupportStatType.Shielding,

            MaxHealth = EnumStats.VitalityStatType.MaxHealth,
            MaxMortality = EnumStats.VitalityStatType.MaxMortality,
            DebuffResistance = EnumStats.VitalityStatType.DebuffResistance,
            DamageResistance = EnumStats.VitalityStatType.DamageResistance,

            InitiativeSpeed = EnumStats.ConcentrationStatType.InitiativeSpeed,
            InitialInitiative = EnumStats.ConcentrationStatType.InitialInitiative,
            ActionsPerSequence = EnumStats.ConcentrationStatType.ActionsPerSequence,
            Critical = EnumStats.ConcentrationStatType.Critical,

            Guard = DominionType.Guard,
            Control = DominionType.Control,
            Provoke = DominionType.Provoke,
            Stance = DominionType.Stance,

            Wait = 20000,
            Others
        }
    }
}
