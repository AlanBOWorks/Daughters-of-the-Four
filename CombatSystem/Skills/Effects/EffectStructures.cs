using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace CombatSystem.Skills.Effects
{
    [Serializable]
    public class EffectStructure<T> : IFullEffectStructureRead<T>
    {
        [TitleGroup("Master")]
        [SerializeField] protected T offensiveEffectType;
        [SerializeField] protected T supportEffectType;
        [FormerlySerializedAs("teamEffectType")] 
        [SerializeField] protected T vanguardEffectType;
        [SerializeField] protected T flexibleEffectType;

        [TitleGroup("Offensive")]
        [SerializeField] protected T damageType;
        [SerializeField] protected T damageOverTimeType;
        [SerializeField] protected T deBuffEffectType;
        [SerializeField] protected T deBurstEffectType;
        [TitleGroup("Support")]
        [SerializeField] protected T healType;
        [SerializeField] protected T shieldingType;
        [SerializeField] protected T buffEffectType;
        [SerializeField] protected T burstEffectType;

        [TitleGroup("Vanguard")]
        [SerializeField] protected T guardingType;
        [SerializeField] protected T counterType;
        [SerializeField] protected T revengeType;


        [TitleGroup("Flexible")]
        [SerializeField] protected T controlType;
        [SerializeField] protected T stanceType;
        [SerializeField] protected T initiativeType;

        public T OffensiveEffectType => offensiveEffectType;
        public T SupportEffectType => supportEffectType;
        public T VanguardEffectType => vanguardEffectType;
        public T FlexibleEffectType => flexibleEffectType;
        public T DamageType => damageType;
        public T DamageOverTimeType => damageOverTimeType;
        public T DeBuffEffectType => deBuffEffectType;
        public T DeBurstEffectType => deBurstEffectType;
        public T HealType => healType;
        public T ShieldingType => shieldingType;
        public T GuardingType => guardingType;
        public T CounterType => counterType;
        public T RevengeType => revengeType;
        public T BuffEffectType => buffEffectType;
        public T BurstEffectType => burstEffectType;
        public T ControlType => controlType;
        public T InitiativeType => initiativeType;
        public T StanceType => stanceType;
    }

    [Serializable]
    public class ClassEffectStructure<T> : EffectStructure<T> where T : new()
    {
        public ClassEffectStructure()
        {
            offensiveEffectType = new T();
            supportEffectType = new T();
            vanguardEffectType = new T();

            damageType = new T();
            damageOverTimeType = new T();
            deBuffEffectType = new T();
            deBurstEffectType = new T();

            healType = new T();
            shieldingType = new T();
            buffEffectType = new T();
            burstEffectType = new T();

            guardingType = new T();
            counterType = new T();
            revengeType = new T();

            controlType = new T();
            stanceType = new T();
            initiativeType = new T();
        }
    }

    [Serializable]
    public class MonoEffectStructure<T> : IFullEffectStructureRead<T>
        where T : UnityEngine.Object
    {
        [TitleGroup("Master")]
        [SerializeField] private T offensiveEffectType;
        [SerializeField] private T supportEffectType;
        [FormerlySerializedAs("teamEffectType")] 
        [SerializeField] private T vanguardEffectType;
        [SerializeField] private T flexibleEffectType;

        [TitleGroup("Offensive")]
        [SerializeField] private T damageType;
        [SerializeField] private T damageOverTimeType;
        [SerializeField] private T deBuffEffectType;
        [SerializeField] private T deBurstEffectType;
        [TitleGroup("Support")]
        [SerializeField] private T healType;
        [SerializeField] private T shieldingType;
        [SerializeField] private T buffEffectType;
        [SerializeField] private T burstEffectType;
        [TitleGroup("Vanguard")]
        [SerializeField] private T guardingType;
        [SerializeField] private T counterType;
        [SerializeField] private T revengeType;


        [TitleGroup("Flexible")]
        [SerializeField] private T controlType;
        [SerializeField] private T stanceType;
        [SerializeField] private T initiativeType;

        public T OffensiveEffectType => offensiveEffectType;
        public T SupportEffectType => supportEffectType;
        public T VanguardEffectType => vanguardEffectType;
        public T FlexibleEffectType => flexibleEffectType;

        public T DamageType => (damageType) ? damageType : offensiveEffectType;
        public T DamageOverTimeType => (damageOverTimeType) ? damageOverTimeType : offensiveEffectType;
        public T DeBuffEffectType => (deBuffEffectType) ? deBuffEffectType : offensiveEffectType;
        public T DeBurstEffectType => (deBurstEffectType) ? deBurstEffectType : offensiveEffectType;

        public T HealType => (healType) ? healType : supportEffectType;
        public T ShieldingType => (shieldingType) ? shieldingType : supportEffectType;
        public T BuffEffectType => (buffEffectType) ? buffEffectType : supportEffectType;
        public T BurstEffectType => (burstEffectType) ? burstEffectType : supportEffectType;


        public T GuardingType => (guardingType) ? guardingType : vanguardEffectType;
        public T CounterType => (counterType) ? counterType : vanguardEffectType;
        public T RevengeType => (revengeType) ? revengeType : vanguardEffectType;


        public T ControlType => (controlType) ? controlType : flexibleEffectType;
        public T StanceType => (stanceType) ? stanceType : flexibleEffectType;
        public T InitiativeType => (initiativeType) ? initiativeType : flexibleEffectType;


    }
    [Serializable]
    public class PreviewMonoEffectStructure<T> : IFullEffectStructureRead<T> 
        where T : UnityEngine.Object
    {
        [TitleGroup("Master")]
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)] private T offensiveEffectType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)] private T supportEffectType;
        [FormerlySerializedAs("teamEffectType")] 
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)] private T vanguardEffectType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)] private T flexibleEffectType;

        [TitleGroup("Offensive")]
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)] private T damageType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)] private T damageOverTimeType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)] private T deBuffEffectType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)] private T deBurstEffectType;

        [TitleGroup("Support")]
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)] private T healType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)] private T shieldingType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)] private T buffEffectType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)] private T burstEffectType;

        [TitleGroup("Vanguard")]
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)] private T guardingType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)] private T counterType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)] private T revengeType;

        [TitleGroup("Flexible")]
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)] private T controlType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)] private T stanceType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)] private T initiativeType;

        public T OffensiveEffectType => offensiveEffectType;
        public T SupportEffectType => supportEffectType;
        public T VanguardEffectType => vanguardEffectType;
        public T FlexibleEffectType => flexibleEffectType;

        public T DamageType => (damageType) ? damageType : offensiveEffectType;
        public T DamageOverTimeType => (damageOverTimeType) ? damageOverTimeType : offensiveEffectType;
        public T DeBuffEffectType => (deBuffEffectType) ? deBuffEffectType : offensiveEffectType;
        public T DeBurstEffectType => (deBurstEffectType) ? deBurstEffectType : offensiveEffectType;

        public T HealType => (healType) ? healType : supportEffectType;
        public T ShieldingType => (shieldingType) ? shieldingType : supportEffectType;
        public T BuffEffectType => (buffEffectType) ? buffEffectType : supportEffectType;
        public T BurstEffectType => (burstEffectType) ? burstEffectType : supportEffectType;


        public T GuardingType => (guardingType) ? guardingType : vanguardEffectType;
        public T CounterType => (counterType) ? counterType : vanguardEffectType;
        public T RevengeType => (revengeType) ? revengeType : vanguardEffectType;


        public T ControlType => (controlType) ? controlType : flexibleEffectType;
        public T StanceType => (stanceType) ? stanceType : flexibleEffectType;
        public T InitiativeType => (initiativeType) ? initiativeType : flexibleEffectType;

    }


    public interface IFullEffectStructureRead<out T> : IEffectTypeStructureRead<T>, IEffectStructureRead<T>
    { }

    public interface IEffectTypeStructureRead<out T>
    {
        T OffensiveEffectType { get; }
        T SupportEffectType { get; }
        T VanguardEffectType { get; }
        T FlexibleEffectType { get; }
    }

    public interface IEffectStructureRead<out T> : 
        IOffensiveEffectStructureRead<T>, 
        ISupportEffectStructureRead<T>,
        IVanguardEffectStructureRead<T>,
        IFlexibleEffectStructureRead<T>
    { }
    public interface IOffensiveEffectStructureRead<out T>
    {
        T DamageType { get; }
        T DamageOverTimeType { get; }
        T DeBuffEffectType { get; }
        T DeBurstEffectType { get; }
    }
    public interface ISupportEffectStructureRead<out T> 
    {
        T HealType { get; }
        T ShieldingType { get; }
        T BuffEffectType { get; }
        T BurstEffectType { get; }
    }


    public interface IVanguardEffectStructureRead<out T>
    {
        T GuardingType { get; }
        T CounterType { get; }
        T RevengeType { get; }
    }

    public interface IFlexibleEffectStructureRead<out T>
    {
        T StanceType { get; }
        T ControlType { get; }
        T InitiativeType { get; }
    }
}
