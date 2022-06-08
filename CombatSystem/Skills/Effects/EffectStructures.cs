using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    [Serializable]
    public class EffectStructure<T> : IFullEffectStructureRead<T>
    {
        [TitleGroup("Master")]
        [SerializeField] protected T offensiveEffectType;
        [SerializeField] protected T supportEffectType;
        [SerializeField] protected T teamEffectType;

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
        [TitleGroup("Team")]
        [SerializeField] protected T guardingType;
        [SerializeField] protected T controlType;
        [SerializeField] protected T controlBurstType;
        [SerializeField] protected T stanceType;

        public T OffensiveEffectType => offensiveEffectType;
        public T SupportEffectType => supportEffectType;
        public T TeamEffectType => teamEffectType;
        public T DamageType => damageType;
        public T DamageOverTimeType => damageOverTimeType;
        public T DeBuffEffectType => deBuffEffectType;
        public T DeBurstEffectType => deBurstEffectType;
        public T HealType => healType;
        public T ShieldingType => shieldingType;
        public T GuardingType => guardingType;
        public T BuffEffectType => buffEffectType;
        public T BurstEffectType => burstEffectType;
        public T ControlType => controlType;
        public T ControlBurstType => controlBurstType;
        public T StanceType => stanceType;
    }

    [Serializable]
    public class ClassEffectStructure<T> : EffectStructure<T> where T : new()
    {
        public ClassEffectStructure()
        {
            offensiveEffectType = new T();
            supportEffectType = new T();
            teamEffectType = new T();

            damageType = new T();
            damageOverTimeType = new T();
            deBuffEffectType = new T();
            deBurstEffectType = new T();

            healType = new T();
            shieldingType = new T();
            buffEffectType = new T();
            burstEffectType = new T();

            guardingType = new T();
            controlType = new T();
            stanceType = new T();
            controlBurstType = new T();
        }
    }

    [Serializable]
    public class MonoEffectStructure<T> : IFullEffectStructureRead<T>
        where T : UnityEngine.Object
    {
        [TitleGroup("Master")]
        [SerializeField] private T offensiveEffectType;
        [SerializeField] private T supportEffectType;
        [SerializeField] private T teamEffectType;

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
        [TitleGroup("Team")]
        [SerializeField] private T guardingType;
        [SerializeField] private T controlType;
        [SerializeField] private T controlBurstType;
        [SerializeField] private T stanceType;

        public T OffensiveEffectType => offensiveEffectType;
        public T SupportEffectType => supportEffectType;
        public T TeamEffectType => teamEffectType;

        public T DamageType => (damageType) ? damageType : offensiveEffectType;
        public T DamageOverTimeType => (damageOverTimeType) ? damageOverTimeType : offensiveEffectType;
        public T DeBuffEffectType => (deBuffEffectType) ? deBuffEffectType : offensiveEffectType;
        public T DeBurstEffectType => (deBurstEffectType) ? deBurstEffectType : offensiveEffectType;

        public T HealType => (healType) ? healType : supportEffectType;
        public T ShieldingType => (shieldingType) ? shieldingType : supportEffectType;
        public T GuardingType => (guardingType) ? guardingType : supportEffectType;
        public T BuffEffectType => (buffEffectType) ? buffEffectType : supportEffectType;
        public T BurstEffectType => (burstEffectType) ? burstEffectType : supportEffectType;

        public T ControlType => (controlType) ? controlType : teamEffectType;
        public T ControlBurstType => (controlBurstType) ? controlBurstType : teamEffectType;
        public T StanceType => (stanceType) ? stanceType : teamEffectType;


    }
    [Serializable]
    public class PreviewMonoEffectStructure<T> : IFullEffectStructureRead<T> 
        where T : UnityEngine.Object
    {
        [TitleGroup("Master")]
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)] private T offensiveEffectType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)] private T supportEffectType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)] private T teamEffectType;

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

        [TitleGroup("Team")]
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)] private T guardingType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)] private T controlType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)] private T controlBurstType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)] private T stanceType;

        public T OffensiveEffectType => offensiveEffectType;
        public T SupportEffectType => supportEffectType;
        public T TeamEffectType => teamEffectType;

        public T DamageType => (damageType) ? damageType : offensiveEffectType;
        public T DamageOverTimeType => (damageOverTimeType) ? damageOverTimeType : offensiveEffectType;
        public T DeBuffEffectType => (deBuffEffectType) ? deBuffEffectType : offensiveEffectType;
        public T DeBurstEffectType => (deBurstEffectType) ? deBurstEffectType : offensiveEffectType;

        public T HealType => (healType) ? healType : supportEffectType;
        public T ShieldingType => (shieldingType) ? shieldingType : supportEffectType;
        public T GuardingType => (guardingType) ? guardingType : supportEffectType;
        public T BuffEffectType => (buffEffectType) ? buffEffectType : supportEffectType;
        public T BurstEffectType => (burstEffectType) ? burstEffectType : supportEffectType;

        public T ControlType => (controlType) ? controlType : teamEffectType;
        public T StanceType => (stanceType) ? stanceType : teamEffectType;
        public T ControlBurstType => (controlBurstType) ? controlBurstType : teamEffectType;

    }


    public interface IFullEffectStructureRead<out T> : IEffectTypeStructureRead<T>, IEffectStructureRead<T>
    { }

    public interface IEffectTypeStructureRead<out T>
    {
        T OffensiveEffectType { get; }
        T SupportEffectType { get; }
        T TeamEffectType { get; }
    }

    public interface IEffectStructureRead<out T> : IOffensiveEffectStructureRead<T>, ISupportEffectStructureRead<T>,
        ITeamEffectStructureRead<T>
    { }
    public interface IOffensiveEffectStructureRead<out T>
    {
        T DamageType { get; }
        T DamageOverTimeType { get; }
        T DeBuffEffectType { get; }
        T DeBurstEffectType { get; }
    }
    public interface ISupportEffectStructureRead<out T> : IProtectionEffectStructureRead<T>
    {
        T BuffEffectType { get; }
        T BurstEffectType { get; }
    }

    public interface IProtectionEffectStructureRead<out T>
    {
        T HealType { get; }
        T ShieldingType { get; }
        T GuardingType { get; }
    }

    public interface ITeamEffectStructureRead<out T>
    {
        T ControlType { get; }
        T StanceType { get; }
        T ControlBurstType { get; }
    }
}
