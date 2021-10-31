using System;
using CombatEntity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Stats
{
    public sealed class CombatStatsHolder : CombatStats<float, int>, IBehaviourStatsRead<IBaseStatsRead<float>>
    {
        public CombatStatsHolder(IBaseStatsRead<float> copyBaseStats)
        :
        this(
            new BaseStats(copyBaseStats),
            new BaseStats(),
            new BurstStats())
        { }

        private CombatStatsHolder(BaseStats baseStats, BaseStats buffStats, BurstStats burstStats)
        {
            _baseStats = baseStats ?? throw new NullReferenceException("Introduced [Base Stats] were null");
            _burstStats = burstStats ?? throw new NullReferenceException("Introduced [Burst Stats] were null");


            // buff stats are the only ones with special conditional stats addition; The rest will be checked before
            // adding, while buff types are checked in each calculation
            if (buffStats == null)
                throw new NullReferenceException("Introduced [Buff Stats] were null");
            _buffStats = new PairWithConditionalStats(buffStats);


            var mathematicalStats = new MathematicalStats(_baseStats, _buffStats, _burstStats);

            //MathematicaStats is the one which gives the stats to external entities
            MainStats = mathematicalStats;
            MasterStats = mathematicalStats.MasterStats;

            CurrentMortality = MaxMortality;
            CurrentHealth = MaxHealth;
        }

        [ShowInInspector]
        public readonly IMasterStats<float> MasterStats;
        [ShowInInspector] 
        private BaseStats _baseStats;
        [ShowInInspector,HorizontalGroup()]
        private PairWithConditionalStats _buffStats;
        [ShowInInspector,HorizontalGroup()]
        private BurstStats _burstStats;

        public BurstStats GetBurstStat() => _burstStats;

        /// <summary>
        /// Reference to the basic BaseStats (to modify)
        /// </summary>
        public IBaseStatsRead<float> BaseStats => _baseStats;
        /// <summary>
        /// Reference to the basic BuffStats
        /// </summary>
        public IBaseStatsRead<float> BuffStats => _buffStats;
        /// <summary>
        /// Reference to the basic BurstStat
        /// </summary>
        public IBaseStatsRead<float> BurstStats => _burstStats;

        public void Injection(CombatingEntity user)
        {
            _buffStats.ConditionalStat.Injection(user);
        }

        public bool HasNoActions() => CurrentActions <= 0;

        /// <summary>
        /// Used for additive type of buff (add, sum, multiply). For conditional types use the special
        /// getter for conditional buffs
        /// </summary>
        public IBaseStats<float> GetBuffableStats(EnumStats.BuffType buffType, bool isSelfBuff)
        {
            switch (buffType)
            {
                case EnumStats.BuffType.Base:
                    return _baseStats;
                case EnumStats.BuffType.Buff:
                    return _buffStats.BaseStats;
                case EnumStats.BuffType.Burst:
                    return _burstStats.GetStats(isSelfBuff);
                default:
                    throw new ArgumentOutOfRangeException(nameof(buffType), buffType, null);
            }
        }
    }
}
