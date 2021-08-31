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
        private EnumStats.BuffType statsType = EnumStats.BuffType.Buff;
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
                var buffStats = UtilsEnumStats.GetStatsHolder(entity.CombatStats, statsType);
                foreach (SStatsBase stat in stats)
                {
                    stat.DoInjection(buffStats);
                }
            }
            void InjectConditional()
            {
                IBuffHolder<ConditionalStats> conditionalStatsHolder = entity.PassivesHolder.ConditionalStats;
                var conditionalStats = UtilsEnumStats.GetStatsHolder(conditionalStatsHolder, statsType);
                foreach (SStatsBase stat in stats)
                {
                    conditionalStats.Add(stat, this);
                }
            }
        }

        public bool CanBeUsed(CombatingEntity user)
            => conditionParam.CanBeUse(user);

        protected override string AssetPrefix()
        {
            return $"STATS {statsType} - ";
        }
    }
}
