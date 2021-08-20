using Characters;
using CombatEffects;
using Stats;
using UnityEngine;

namespace CombatConditions
{

    [CreateAssetMenu(fileName = "Health Condition - N [Preset]",
        menuName = "Combat/Conditions/Effect/Health")]
    public class SHealthCondition : SEffectCondition
    {
        [SerializeField, Tooltip("If is the [user] or the [target] that needs to be check")] 
        private bool isUserCheck = true;

        [SerializeField, Tooltip("If the [target check] has more HP than the [check value]")] 
        private bool isBiggerCheck = true;
        public override bool CanApply(ref EffectArguments arguments,float healthCheck)
        {
            var checkEntity = isUserCheck
                ? arguments.Arguments.User
                : arguments.Arguments.InitialTarget;

            float percentage = UtilsCombatStats.HealthPercentage(checkEntity);

            bool isHealthLower = percentage < healthCheck;
            return isHealthLower ^ isBiggerCheck;
        }
    }
}
