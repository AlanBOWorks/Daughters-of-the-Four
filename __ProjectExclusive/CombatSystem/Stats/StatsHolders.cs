using System;
using System.Collections.Generic;
using CombatEntity;
using Sirenix.OdinInspector;
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

    public class MasterStats : MasterStats<float>
    {
        public MasterStats() {}
        public MasterStats(float overrideBy) : base(overrideBy) { }
        public MasterStats(IMasterStatsRead<float> injection) : base(injection) { }

        public void OverrideStats(IMasterStatsRead<float> stats) => UtilStats.OverrideStats(this,stats);
    }


    public class PairStatsHolder : IBaseStatsRead<float>
    {
        protected PairStatsHolder()
        {}


        protected IBaseStatsRead<float> Primary;
        protected IBaseStatsRead<float> Secondary;

        public float Attack => Primary.Attack + Secondary.Attack;
        public float Persistent => Primary.Persistent + Secondary.Persistent;
        public float Debuff => Primary.Debuff + Secondary.Debuff;
        public float FollowUp => Primary.FollowUp + Secondary.Debuff;
        public float Heal => Primary.Heal + Secondary.Heal;
        public float Buff => Primary.Buff + Secondary.Buff;
        public float ReceiveBuff => Primary.ReceiveBuff + Secondary.ReceiveBuff;
        public float Shielding => Primary.Shielding + Secondary.Shielding;
        public float MaxHealth => Primary.MaxHealth + Secondary.MaxHealth;
        public float MaxMortality => Primary.MaxMortality + Secondary.MaxMortality;
        public float DebuffResistance => Primary.DebuffResistance + Secondary.DebuffResistance;
        public float DamageResistance => Primary.DamageResistance + Secondary.DamageResistance;
        public float InitiativeSpeed => Primary.InitiativeSpeed + Secondary.InitiativeSpeed;
        public float Critical => Primary.Critical + Secondary.Critical;
        public float InitialInitiative => Primary.InitialInitiative + Secondary.InitialInitiative;
        public float ActionsPerSequence => Primary.ActionsPerSequence + Secondary.ActionsPerSequence;
    }

    public class PairWithFollowUpStats : PairStatsHolder
    {
        public PairWithFollowUpStats(BaseStats baseStats)
        {
            BaseStats = baseStats;
            FollowUpStats = new FollowUpStats();

            Primary = BaseStats;
            Secondary = FollowUpStats;
        }
        [ShowInInspector]
        public readonly BaseStats BaseStats;
        [ShowInInspector]
        public readonly FollowUpStats FollowUpStats;
    }

    public class PairWithConditionalStats : PairStatsHolder
    {
        public PairWithConditionalStats(BaseStats baseStats)
        {
            BaseStats = baseStats;
            ConditionalStat = new ConditionalStat();

            Primary = BaseStats;
            Secondary = ConditionalStat;
        }


        [ShowInInspector]
        public readonly BaseStats BaseStats;
        [ShowInInspector]
        public readonly ConditionalStat ConditionalStat;
    }

    public class BurstStats : PairStatsHolder
    {
        public BurstStats()
        {
            SelfBurst = new BaseStats();
            ReceivedBurst = new BaseStats();
            InjectStatsInBase();
        }

        public BurstStats(IBaseStatsRead<float> copyBurstStats) 
        {
            SelfBurst = new BaseStats(copyBurstStats);
            ReceivedBurst = new BaseStats();
            InjectStatsInBase();
        }

        private void InjectStatsInBase()
        {
            Primary = SelfBurst;
            Secondary = ReceivedBurst;
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

    }
}
