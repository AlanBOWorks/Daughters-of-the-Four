using System;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class BaseStats : BaseStats<float>
    {
        public BaseStats()
        {}

        public BaseStats(IBaseStatsRead<float> copyValues)
        {
            UtilStats.CopyValues(this, copyValues);
        }

        public BaseStats(IBaseStats<float> baseStats, IMasterStatsRead<float> upgradeStats)
        : this(baseStats)
        {
            UtilStats.MultiplyStats(this,this,upgradeStats);
        }

        public BaseStats(float overrideAllBy)
        {
            UtilStats.OverrideByValue(this,overrideAllBy);
        }

        public void Override(float value)
        {
            UtilStats.OverrideByValue(this,value);
        }

        public void ResetAsBurst() => Override(0);
    }

    public class ListStats : ListStats<float>
    {
        public ListStats(IBaseStatsRead<float> baseStats, int length = 1) : base(baseStats, ListOperation, length)
        {
        }

        private static float ListOperation(float currentAmount, float stats)
        {
            return currentAmount + stats;
        }
    }

    public class MasterStats : MasterStats<float>
    {
        public MasterStats() {}
        public MasterStats(float overrideBy) : base(overrideBy) { }
        public MasterStats(IMasterStatsRead<float> injection) : base(injection) { }

        public void OverrideStats(IMasterStatsRead<float> stats) => UtilStats.OverrideStats(this,stats);
    }

    public class BurstStats : IBaseStatsRead<float>
    {
        public BurstStats()
        {
            SelfBurst = new BaseStats();
            ReceivedBurst = new BaseStats();
        }

        public BurstStats(IBaseStatsRead<float> copyBurstStats)
        {
            SelfBurst = new BaseStats(copyBurstStats);
            ReceivedBurst = new BaseStats();
        }

        /// <summary>
        /// Buff that resets at start of the sequence (for design purposes)
        /// </summary>
        public readonly BaseStats SelfBurst;
        /// <summary>
        /// Buff that reset at end of the sequence (for design purposes)
        /// </summary>
        public readonly BaseStats ReceivedBurst;

        public BaseStats GetStats(bool isSelfBuff) => (isSelfBuff) ? SelfBurst : ReceivedBurst;

        public void ResetSelfBurst() => SelfBurst.ResetAsBurst();
        public void ResetReceiveBurst() => ReceivedBurst.ResetAsBurst();

        public float Attack => SelfBurst.Attack + ReceivedBurst.Attack;
        public float Persistent => SelfBurst.Persistent + ReceivedBurst.Persistent;
        public float Debuff => SelfBurst.Debuff + ReceivedBurst.Debuff;
        public float FollowUp => SelfBurst.FollowUp + ReceivedBurst.Debuff;
        public float Heal => SelfBurst.Heal + ReceivedBurst.Heal;
        public float Buff => SelfBurst.Buff + ReceivedBurst.Buff;
        public float ReceiveBuff => SelfBurst.ReceiveBuff + ReceivedBurst.ReceiveBuff;
        public float Shielding => SelfBurst.Shielding + ReceivedBurst.Shielding;
        public float MaxHealth => SelfBurst.MaxHealth + ReceivedBurst.MaxHealth;
        public float MaxMortality => SelfBurst.MaxMortality + ReceivedBurst.MaxMortality;
        public float DebuffResistance => SelfBurst.DebuffResistance + ReceivedBurst.DebuffResistance;
        public float DamageResistance => SelfBurst.DamageResistance + ReceivedBurst.DamageResistance;
        public float InitiativeSpeed => SelfBurst.InitiativeSpeed + ReceivedBurst.InitiativeSpeed;
        public float Critical => SelfBurst.Critical + ReceivedBurst.Critical;
        public float InitialInitiative => SelfBurst.InitialInitiative + ReceivedBurst.InitialInitiative;
        public float ActionsPerSequence => SelfBurst.ActionsPerSequence + ReceivedBurst.ActionsPerSequence;
    }
}
