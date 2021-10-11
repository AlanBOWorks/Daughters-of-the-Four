using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Stats
{
    public sealed class CombatStatsHolder : CombatStats<float, int>, IBehaviourStatsRead<IBaseStats<float>>
    {
        /// <summary>_<br></br>
        /// Used for special initial/passives stats for Buff and/or Burst type holder;<br></br>
        /// For the rest use the [<see cref="CombatStatsHolder(Stats.IBaseStatsRead{float})"/>]
        /// constructor
        /// </summary>
        public CombatStatsHolder(
            IBaseStatsRead<float> copyBaseStats, 
            IBaseStatsRead<float> copyBuffStats, 
            IBaseStatsRead<float> copyBurstStats)
        : 
        this(
            new BaseStats(copyBaseStats),
            new BaseStats(copyBuffStats),
            new BaseStats(copyBurstStats))
        { }


        public CombatStatsHolder(IBaseStatsRead<float> copyBaseStats)
        :
        this(
            new BaseStats(copyBaseStats),
            new BaseStats(),
            new BaseStats())
        { }

        private CombatStatsHolder(BaseStats baseStats, BaseStats buffStats, BaseStats burstStats)
        {
            _baseStats = baseStats ?? throw new NullReferenceException("Introduced [Base Stats] were null");
            _buffStats = buffStats ?? throw new NullReferenceException("Introduced [Buff Stats] were null");
            _burstStats = burstStats ?? throw new NullReferenceException("Introduced [Burst Stats] were null");

            var baseList = new ListStats(BaseStats);
            var buffList = new ListStats(BuffStats);
            var burstList = new ListStats(BurstStats);

            ListStatsHolder = new StatBehaviourStructure<ListStats>(baseList,buffList,burstList);
            var mathematicalStats = new MathematicalStats(baseList, buffList, burstList);

            //MathematicaStats is the one which gives the stats to external entities
            MainStats = mathematicalStats;
            MasterStats = mathematicalStats.MasterStats;

            CurrentMortality = MaxMortality;
            CurrentHealth = MaxHealth;
        }

        [ShowInInspector] 
        private BaseStats _baseStats;
        [ShowInInspector,HorizontalGroup()]
        private BaseStats _buffStats;
        [ShowInInspector,HorizontalGroup()]
        private BaseStats _burstStats;

        /// <summary>
        /// Reference to the basic BaseStats (to modify)
        /// </summary>
        public IBaseStats<float> BaseStats => _baseStats;
        /// <summary>
        /// Reference to the basic BuffStats
        /// </summary>
        public IBaseStats<float> BuffStats => _buffStats;
        /// <summary>
        /// Reference to the basic BurstStat
        /// </summary>
        public IBaseStats<float> BurstStats => _burstStats;


        public readonly IMasterStats<float> MasterStats;
        /// <summary>
        /// Reference to a List Structured class for collection type of Stats (conditional primarily)
        /// </summary>
        public readonly StatBehaviourStructure<ListStats> ListStatsHolder;


        public void ResetBurst()
        {
            _burstStats.ResetAsBurst();
        }

        public void SubscribeStats(IBaseStatsRead<float> stats, EnumStats.BuffType type)
        {
            //TODO Check if is conditional stats, if true > then add

            //else
            IBaseStats<float> targetStats = UtilStats.GetElement(this, type);
            UtilStats.SumStats(targetStats,stats);
        }



    }
}
