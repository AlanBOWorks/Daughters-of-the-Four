using System;
using Characters;
using CombatConditions;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace Passives
{
    [CreateAssetMenu(fileName = "STATS - N [Passives]", 
        menuName = "Combat/Passives/Stats Buffer")]
    public class SStatPassivesInjector : SPassiveInjector,IConditionalStat
    {
        [SerializeField, PropertyOrder(-20)]
        private EnumStats.StatsType statsType = EnumStats.StatsType.Buff;
        [SerializeField]
        private SStatsBase[] stats = new SStatsBase[0];

        [SerializeField]
        private UserOnlyConditionParam conditionParam;

        public override void InjectPassive(CombatingEntity entity)
        {
            if (conditionParam.condition == null)
                InjectDirectly();
            else
                InjectConditional();


            void InjectDirectly()
            {
                var buffStats = entity.CombatStats.GetStatsHolder(statsType);
                foreach (SStatsBase stat in stats)
                {
                    stat.DoInjection(buffStats);
                }
            }
            void InjectConditional()
            {
                foreach (SStatsBase stat in stats)
                {
                    //TODO
                }
            }
        }

        public bool CanBeUsed(CombatingEntity user)
            => conditionParam.CanBeUse(user);
    }
}
