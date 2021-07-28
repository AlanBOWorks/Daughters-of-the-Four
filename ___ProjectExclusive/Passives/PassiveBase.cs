
using System;
using Characters;
using CombatEffects;

namespace Passives
{

    public interface IPassiveEffect
    {
        void DoPassiveFilter(
            ref EffectArguments arguments,
            ref float currentValue,
            float originalValue,
            float effectValueModifier = 1);
    }
}
