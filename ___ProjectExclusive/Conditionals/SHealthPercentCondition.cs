using Characters;
using CombatEffects;
using Stats;
using UnityEngine;

namespace CombatConditions
{

    [CreateAssetMenu(fileName = "TEMPORAL Stats - Health PERCENTAGE [Condition]",
        menuName = "Combat/Conditions/Health Percentage Check")]
    public class SHealthPercentCondition : SCondition
    {
        public override bool CanApply(CombatingEntity target, float checkValue)
        {
            float healthPercentage = UtilsCombatStats.HealthPercentage(target);

            return checkValue <= healthPercentage;
        }
    }
}
