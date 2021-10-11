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
}
