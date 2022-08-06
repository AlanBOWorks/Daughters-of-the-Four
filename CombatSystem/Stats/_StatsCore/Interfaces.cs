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

    public interface IBasicStats : IBasicStats<float>, 
        IOffensiveStats, ISupportStats,
        IVitalityStats, IConcentrationStats
    { }

    public interface IBasicStats<T> : IBasicStatsInject<T>, IBasicStatsRead<T>, 
        IOffensiveStats<T>, ISupportStats<T>,
        IVitalityStats<T>, IConcentrationStats<T>
    { }

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
        new T OffensiveStatType { get; set; }
        new T SupportStatType { get; set; }
        new T VitalityStatType { get; set; }
        new T ConcentrationStatType { get; set; }
    }


    public interface IOffensiveStats : IOffensiveStats<float> { }
    public interface IOffensiveStats<T> : IOffensiveStatsRead<T>, IOffensiveStatsInject<T>
    {
        new T AttackType { get; set; }
        new T OverTimeType { get; set; }
        new T DeBuffType { get; set; }
        new T FollowUpType { get; set; }
    }

    public interface ISupportStats : ISupportStats<float> { }
    public interface ISupportStats<T> : ISupportStatsRead<T>, ISupportStatsInject<T>
    {
        new T HealType { get; set; }
        new T ShieldingType { get; set; }
        new T BuffType { get; set; }
        new T ReceiveBuffType { get; set; }
    }

    public interface IVitalityStats : IVitalityStats<float>{ }
    public interface IVitalityStats<T> : IVitalityStatsRead<T>, IVitalityStatsInject<T>
    {
        new T HealthType { get; set; }
        new T MortalityType { get; set; }
        new T DamageReductionType { get; set; }
        new T DeBuffResistanceType { get; set; }
    }

    public interface IConcentrationStats : IConcentrationStats<float> { }
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
        T OffensiveStatType { get; }
        T SupportStatType { get; }
        T VitalityStatType { get; }
        T ConcentrationStatType { get; }
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
        T OffensiveStatType { set; }
        T SupportStatType { set; }
        T VitalityStatType { set; }
        T ConcentrationStatType { set; }
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

    public interface ICombatStats<T> :  IDamageableStats<T>
    {
        T UsedActions { get; set; }
        T CurrentInitiative { get; set; }
    }

  
    public interface IDamageableStats<T>
    {
        T CurrentHealth { get; set; }
        T CurrentMortality { get; set; }
        T CurrentShields { get; set; }
    }
}
