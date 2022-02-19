using UnityEngine;

namespace CombatSystem.Stats
{
    public interface IStatsTypesRead<out T>
    {
        T BaseType { get; }
        T BuffType { get; }
        T BurstType { get; }
    }

    public interface IStats<T> : IBasicStats<T>,
        IStatsRead<T>, IStatsInject<T>,
        IMainStats<T>
    { }

    public interface IStatsInject<in T> : IMainStatsInject<T>, IBasicStatsInject<T>
    { }
    public interface IStatsRead<out T> : IMainStatsRead<T>, IBasicStatsRead<T>
    { }

    public interface IBasicStats<T> : IBasicStatsInject<T>, IBasicStatsRead<T>, 
        IOffensiveStats<T>, ISupportStats<T>,
        IVitalityStats<T>, IConcentrationStats<T>
    {

    }

    public interface IBasicStatsRead<out T> : 
        IOffensiveStatsRead<T>, ISupportStatsRead<T>,
        IVitalityStatsRead<T>, IConcentrationStatsRead<T>
    { }

    public interface IBasicStatsInject<in T> :
        IOffensiveStatsInject<T>, ISupportStatsInject<T>,
        IVitalityStatsInject<T>, IConcentrationStatsInject<T>
    { }

    // WRAPPERS
    public interface IMainStats<T> : IMainStatsRead<T>, IMainStatsInject<T>
    {
        new T OffensiveType { get; set; }
        new T SupportType { get; set; }
        new T VitalityType { get; set; }
        new T ConcentrationType { get; set; }
    }

    public interface IOffensiveStats<T> : IOffensiveStatsRead<T>, IOffensiveStatsInject<T>
    {
        new T AttackType { get; set; }
        new T OverTimeType { get; set; }
        new T DeBuffType { get; set; }
        new T FollowUpType { get; set; }
    }

    public interface ISupportStats<T> : ISupportStatsRead<T>, ISupportStatsInject<T>
    {
        new T HealType { get; set; }
        new T ShieldingType { get; set; }
        new T BuffType { get; set; }
        new T ReceiveBuffType { get; set; }
    }

    public interface IVitalityStats<T> : IVitalityStatsRead<T>, IVitalityStatsInject<T>
    {
        new T HealthType { get; set; }
        new T MortalityType { get; set; }
        new T DamageReductionType { get; set; }
        new T DeBuffResistanceType { get; set; }
    }

    public interface IConcentrationStats<T> : IConcentrationStatsRead<T>, IConcentrationStatsInject<T>
    {
        new T ActionsType { get; set; }
        new T SpeedType { get; set; }
        new T ControlType { get; set; }
        new T CriticalType { get; set; }
    }


    // GETTERS

    public interface IMainStatsRead<out T>
    {
        T OffensiveType { get; }
        T SupportType { get; }
        T VitalityType { get; }
        T ConcentrationType { get; }
    }

    public interface IOffensiveStatsRead<out T>
    {
        T AttackType { get; }
        T OverTimeType { get; }
        T DeBuffType { get; }
        T FollowUpType { get; }
    }

    public interface ISupportStatsRead<out T>
    {
        T HealType { get; }
        T ShieldingType { get; }
        T BuffType { get; }
        T ReceiveBuffType { get; }
    }

    public interface IVitalityStatsRead<out T>
    {
        T HealthType { get; }
        T MortalityType { get; }
        T DamageReductionType { get; }
        T DeBuffResistanceType { get; }
    }

    public interface IConcentrationStatsRead<out T>
    {
        T ActionsType { get; }
        T SpeedType { get; }
        T ControlType { get; }
        T CriticalType { get; }
    }



    // INJECTORS

    public interface IMainStatsInject<in T>
    {
        T OffensiveType { set; }
        T SupportType { set; }
        T VitalityType { set; }
        T ConcentrationType { set; }
    }

    public interface IOffensiveStatsInject<in T>
    {
        T AttackType { set; }
        T OverTimeType { set; }
        T DeBuffType { set; }
        T FollowUpType { set; }
    }

    public interface ISupportStatsInject<in T>
    {
        T HealType { set; }
        T ShieldingType { set; }
        T BuffType { set; }
        T ReceiveBuffType { set; }
    }

    public interface IVitalityStatsInject<in T>
    {
        T HealthType { set; }
        T MortalityType { set; }
        T DamageReductionType { set; }
        T DeBuffResistanceType { set; }
    }

    public interface IConcentrationStatsInject<in T>
    {
        T ActionsType { set; }
        T SpeedType { set; }
        T ControlType { set; }
        T CriticalType { set; }
    }

    public interface ICombatStats<T> : ICombatStatsInject<T>, ICombatStatsRead<T>
    {
        new T CurrentHealth { get; set; }
        new T CurrentMortality { get; set; }
        new T CurrentShields { get; set; }
        new T UsedActions { get; set; }
        new T CurrentInitiative { get; set; }
    }

    public interface ICombatStatsInject<in T> 
    {
        T CurrentHealth { set; }
        T CurrentMortality { set; }
        T CurrentShields { set; }
        T UsedActions { set; }
        T CurrentInitiative { set; }
    }
    public interface ICombatStatsRead<out T> 
    {
        T CurrentHealth { get; }
        T CurrentMortality { get; }
        T CurrentShields { get; }
        T UsedActions { get; }
        T CurrentInitiative { get; }
    }

    public interface IHealthStats<T>
    {
        T CurrentHealth { get; set; }
        T CurrentMortality { get; set; }
        T CurrentShields { get; set; }
        T DamageReductionType { get; }
    }
}
