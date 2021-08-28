using System;
using Characters;
using Passives;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace CombatConditions
{
    [CreateAssetMenu(fileName = "Stats CONDITIONAL -N [Passive]",
        menuName = "Combat/Passives/Stats Conditional"),PropertyOrder(-100)]
    public class SConditionalStatsPassive : SPassiveInjectorBase, IConditionalStat
    {
        [SerializeField,PropertyOrder(-1)]
        private UserOnlyConditionParam conditionParam = new UserOnlyConditionParam();
        [SerializeField] private SStatsBase[] passiveStats = new SStatsBase[0];

        public override void InjectPassive(CombatingEntity entity)
        {
#if UNITY_EDITOR
            if (conditionParam.condition == null)
                throw new NullReferenceException("Condition is null; Can't inject with a" +
                                                 "null exception");
#else
            if (conditionParam.condition == null) return;
#endif

            InjectConditional();

            void InjectConditional()
            {
                foreach (SStatsBase stat in passiveStats)
                {
                    entity.Add(this, stat);
                }
            }
        }


        public bool CanBeUsed(CombatingEntity user)
            => conditionParam.CanBeUse(user);
    }
}
