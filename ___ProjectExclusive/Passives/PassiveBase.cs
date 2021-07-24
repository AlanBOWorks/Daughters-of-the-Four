
using Characters;
using Skills;
using UnityEngine;

namespace CombatEffects
{
    public abstract class SInjectionPassiveBase : ScriptableObject, IPassiveTooltip
    {
        [SerializeField,Delayed]
        private string passiveName = "NULL";
        public string PassiveName => passiveName;

        public abstract void InjectPassive(CombatingEntity target);

        protected const string InjectionNamePrefix = "Injection Passive [Preset]";
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
