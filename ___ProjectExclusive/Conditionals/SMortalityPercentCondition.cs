
using Characters;
using Stats;

namespace CombatConditions
{
    public class SMortalityPercentCondition : SCondition
    {
        public override bool CanApply(CombatingEntity target, float checkValue)
        {
            float mortalityPercent = UtilsCombatStats.MortalityPercent(target);

            return checkValue <= mortalityPercent;
        }
    }
}
