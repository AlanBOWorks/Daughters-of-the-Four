using Characters;
using CombatEffects;
using Stats;
using UnityEngine;

namespace CombatConditions
{

    [CreateAssetMenu(fileName = "Health Condition - N [Preset]",
        menuName = "Combat/Conditions/Health")]
    public class SHealthCondition : SCombatCondition
    {
        [SerializeField, Tooltip("If is the [user] or the [target] that needs to be check")] 
        private bool isUserCheck = true;

        [SerializeField, Tooltip("If the [target check] has more HP than the [check value]")] 
        private bool isBiggerCheck = true;
        public override bool CanApply(CombatConditionArguments arguments,float healthCheck)
        {
            var checkEntity = isUserCheck
                ? arguments.User
                : arguments.Target;

            float percentage = UtilsCombatStats.HealthPercentage(checkEntity);

            if (isBiggerCheck)
            {
                return percentage >= healthCheck;
            }
            else
            {
                return percentage < healthCheck; 
                // [<] because two conditions of opposite effect with [<=] && [>=] could trigger.
                // [>=] has priority since it could be a
                // (value >= 100% == MaxHealth) that is quite common
                // while
                // (value <= 0 == Dead) which is another special condition with special needs
            }
        }
    }
}
