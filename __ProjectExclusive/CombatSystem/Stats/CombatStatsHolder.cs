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
            BaseStats = baseStats ?? throw new NullReferenceException("Introduced [Base Stats] were null");
            BuffStats = buffStats ?? throw new NullReferenceException("Introduced [Buff Stats] were null");
            BurstStats = burstStats ?? throw new NullReferenceException("Introduced [Burst Stats] were null");

            var baseList = new ListStats(BaseStats);
            var buffList = new ListStats(BuffStats);
            var burstList = new ListStats(BurstStats);


            var mathematicalStats = new MathematicalStats(baseList, buffList, burstList);

            //MathematicaStats is the one which gives the stats to external entities
            MainStats = mathematicalStats;
            _listStats = new ListBehaviourHolder(baseList, buffList, burstList);

            CurrentMortality = MaxMortality;
            CurrentHealth = MaxHealth;
        }

        [ShowInInspector]
        public IBaseStats<float> BaseStats { get; }
        [ShowInInspector,HorizontalGroup()]
        public IBaseStats<float> BuffStats { get; }
        [ShowInInspector,HorizontalGroup()]
        public IBaseStats<float> BurstStats { get; }

        // TODO when conditional stats are created, add this to mathematicals
        private readonly IBehaviourStatsRead<ListStats> _listStats; //This is for the conditionals

        public void ResetBurst()
        {
            UtilStats.OverrideByValue(BurstStats,0);
            _listStats.BurstStats.ResetAsBurstType();
        }

        public void SubscribeStats(IBaseStatsRead<float> stats, EnumStats.BehaviourType type)
        {
            //TODO Check if is conditional stats, if true > then add

            //else
            IBaseStats<float> targetStats = UtilStats.GetElement(this, type);
            UtilStats.SumStats(targetStats,stats);
        }



        private class MathematicalStats : BehaviourCombinedStats<float>
        {
            public MathematicalStats(
                IBaseStatsRead<float> baseStats, IBaseStatsRead<float> buffStats, IBaseStatsRead<float> burstStats)
                : base(baseStats, buffStats, burstStats, UtilStats.CalculateStat)
            { }
        }

        private class ListBehaviourHolder : StatBehaviourStructure<ListStats>
        {
            public ListBehaviourHolder(ListStats baseStats, ListStats buffStats, ListStats burstStats)
                : base(baseStats, buffStats, burstStats)
            {
            }
        }
    }
}
