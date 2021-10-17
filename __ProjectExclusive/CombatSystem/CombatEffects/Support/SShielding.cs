using CombatEntity;
using CombatSkills;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Shielding [Effect]", 
        menuName = "Combat/Effect/Shielding")]
    public class SShielding : SEffect
    {
        private const float AdditionShieldingAddition = .5f;
        protected override void DoEffectOn(SkillValuesHolders values, CombatingEntity effectTarget, float effectValue, bool isCritical)
        {
            if (isCritical)
                effectValue += AdditionShieldingAddition;

            UtilsCombatStats.DoShielding(values.User.CombatStats,effectTarget.CombatStats,effectValue);
        }
    }
}
