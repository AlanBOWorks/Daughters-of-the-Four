using System;
using Characters;
using Stats;
using UnityEngine;

namespace Passives
{
    [CreateAssetMenu(fileName = "STATS - N [Passives]", 
        menuName = "Combat/Passives/Stats")]
    public class SStatPassivesHolder : SPassiveInjector<SStatsBase>
    {
        [SerializeField] protected PassiveStats passiveStats = new PassiveStats();
        protected override PassiveInjectorsHolder<SStatsBase> GetHolder() => passiveStats;


        [Serializable]
        protected class PassiveStats : PassiveInjectorsHolder<SStatsBase>
        {
            protected override void DoInjection(CombatingEntity entity, SStatsBase element)
            {
                throw new NotImplementedException();
            }
        }

    }
}
