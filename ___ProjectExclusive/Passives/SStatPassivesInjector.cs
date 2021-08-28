using System;
using Characters;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace Passives
{
    [CreateAssetMenu(fileName = "STATS - N [Passives]", 
        menuName = "Combat/Passives/Stats Buffer")]
    public class SStatPassivesInjector : SPassiveInjector<SStatsBase>
    {
        [SerializeField, PropertyOrder(-20)]
        private EnumStats.StatsType statsType = EnumStats.StatsType.Buff;


        protected override void DoInjection(CombatingEntity entity, SStatsBase stats)
        {
            var buffStats = entity.CombatStats.GetStats(statsType);
            stats.DoInjection(buffStats);
        }
    }
}
