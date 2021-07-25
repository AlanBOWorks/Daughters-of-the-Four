
using Characters;
using CombatEffects;
using UnityEngine;

namespace Passives
{
    public abstract class SInjectionPassiveBase : SPassiveEffectInjection, IPassiveTooltip
    {
        [SerializeField,Delayed]
        private string passiveName = "NULL";
        public string PassiveName => passiveName;


        protected new const string InjectionNamePrefix = "Injection Passive [Preset]";
    }

    public abstract class SPassiveEffectInjection : ScriptableObject
    {
        public abstract void InjectPassive(CombatingEntity target, float modifier = 1);

        protected const string InjectionNamePrefix = "Injection Passive [Effect]";
    }

    public interface IPassiveTooltip
    {
        string PassiveName { get; }
    }

    public interface IPassiveEffect
    {
        void DoPassiveFilter(
            ref EffectArguments arguments,
            ref float currentValue,
            float originalValue);
    }
}
