using Characters;
using CombatConditions;
using UnityEngine;

namespace Stats
{
    public abstract class ConditionalStats
    {
        private readonly ConditionParam _conditionParam;

        protected ConditionalStats(ConditionParam conditionParam)
        {
            _conditionParam = conditionParam;
        }

        public bool IsStatValid(CombatingEntity target)
        {
            return _conditionParam.CanApply(target);
        }

        public bool IsStatValid(CombatingEntity user, CombatingEntity target)
        {
            return _conditionParam.CanApply(user, target);
        }
    }

    public class ConditionalOffensiveStats : IOffensiveStatsData
    {
        private readonly IOffensiveStatsData _dataReference;

        public float GetAttackPower()
        {
            throw new System.NotImplementedException();
        }

        public float GetDeBuffPower()
        {
            throw new System.NotImplementedException();
        }

        public float GetStaticDamagePower()
        {
            throw new System.NotImplementedException();
        }
    }
}
