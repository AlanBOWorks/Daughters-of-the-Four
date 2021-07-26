using CombatEffects;
using UnityEngine;

namespace Passives
{
    [CreateAssetMenu(fileName = "Damage Reduction [Passive Filter]",
        menuName = "Combat/Passive/Filter/Damage Reduction")]
    public class SDamageReductionFilter : SPassiveFilter
    {
        public override void DoPassiveFilter(
            ref float currentValue, 
            float originalValue, float effectValue)
        {
            currentValue -= effectValue;
            if (currentValue < 0) currentValue = 0;
        }

        public override bool CanApplyPassive(SEffectBase effect)
        {
            return effect is SEffectDamage;
        }
    }
}
