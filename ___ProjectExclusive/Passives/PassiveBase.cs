using Characters;
using Skills;
using UnityEngine;

namespace Passives
{
    public abstract class SInjectionPassiveBase : ScriptableObject
    {
        public abstract void InjectPassive(CombatingEntity target);
    }

    public abstract class SCombatPassiveBase : ScriptableObject
    {
        public abstract bool CanApplyPassive(SEffectBase effect);
        public abstract float CalculateVariation(float currentValue);
    }
}
