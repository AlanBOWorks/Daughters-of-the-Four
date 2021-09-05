using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Stats
{
    /// <summary>
    /// Used just to know if the interface is a Stat type  
    /// </summary>
    public interface IStatsData 
    { }

    public interface IConditionalStat : IStatsData
    {
        bool CanBeUsed(CombatingEntity user);
    }


    // it's is float because upgrades could be increased in half
    public interface IStatsUpgradable : IMasterStats<float>
    {
        //TODO UpgradeLevel?
    }


    public interface IMasterStats<T> : IMasterStatsData<T>, IMasterStatsInjection<T> ,IStatsData
    {
        new T OffensivePower { get; set; }
        new T SupportPower { get; set; }
        new T VitalityAmount { get; set; }
        new T ConcentrationAmount { get; set; }
    }

    public interface IMasterStatsData<out T>
    {
        T OffensivePower { get; }
        T SupportPower { get; }
        T VitalityAmount { get; }
        T ConcentrationAmount { get; }
    }

    public interface IMasterStatsInjection<in T>
    {
        T OffensivePower { set; }
        T SupportPower { set; }
        T VitalityAmount { set; }
        T ConcentrationAmount { set; }
    }
    
    public interface IOffensiveStats<T> : IOffensiveStatsData<T>, IOffensiveStatsInjection<T>
    {
        new T AttackPower { get; set; }
        new T DeBuffPower { get; set; }
        new T StaticDamagePower { get; set; }
    }

    public interface IOffensiveStatsData<out T> : IStatsData
    {
        T AttackPower { get; }
        T DeBuffPower { get; }
        T StaticDamagePower { get; }
    }
    public interface IOffensiveStatsInjection<in T>
    {
        T AttackPower { set; }
        T DeBuffPower { set; }
        T StaticDamagePower { set; }
    }
    public interface ISupportStats<T> : ISupportStatsData<T>, ISupportStatsInjection<T>
    {
        new T HealPower { get; set; }
        new T BuffPower { get; set; }
        new T BuffReceivePower { get; set; }
    }
   
    public interface ISupportStatsData<out T> : IStatsData
    {
        T HealPower { get; }
        T BuffPower { get; }
        T BuffReceivePower { get; }
    }
    public interface ISupportStatsInjection<in T>
    {
        T HealPower { set; }
        T BuffPower { set; }
        T BuffReceivePower { set; }
    }

    public interface IVitalityStats<T> : IVitalityStatsData<T>, IVitalityStatsInjection<T>
    {
        new T MaxHealth { get; set; }
        new T MaxMortalityPoints { get; set; }
        new T DamageReduction { get; set; }
        new T DeBuffReduction { get; set; }
    }

    public interface IVitalityStatsData<out T> : IStatsData
    {
        T MaxHealth { get; }
        T MaxMortalityPoints { get; }
        T DamageReduction { get; }
        T DeBuffReduction { get; }

        //Static reduction is determined by deBuffReduction (+ damage?)
    }
    public interface IVitalityStatsInjection<in T>
    {
        T MaxHealth { set; }
        T MaxMortalityPoints { set; }

        T DamageReduction { set; }
        T DeBuffReduction { set; }
    }
    
    /// <summary>
    /// Used only during the fights. <br></br>
    /// For variation, only the <seealso cref="CombatStatsHolder.BaseStats"/>
    /// should be used
    /// </summary>
    public interface ICombatHealthStats<T> : ICombatHealthStatsData<T>,ICombatHealthStatsInjector<T>
    {
        new T HealthPoints { get; set; }
        new T ShieldAmount { get; set; }
        new T MortalityPoints { get; set; }
        new T AccumulatedStatic { get; set; }
    }
    public interface ICombatHealthStatsData<out T>
    {
        T HealthPoints { get; }
        T ShieldAmount { get; }
        T MortalityPoints { get; }
        T AccumulatedStatic { get; }
    }
    public interface ICombatHealthStatsInjector<in T>
    {
        T HealthPoints { set; }
        T ShieldAmount { set; }
        T MortalityPoints { set; }
        T AccumulatedStatic { set; }
    }

    ///  /// <summary>
    /// <inheritdoc cref="ICombatHealthStats{T}"/>
    /// </summary>
    public interface ITemporalStats<T> : ITemporalStatsData<T>, ITemporalStatsInjection<T>
    {
        new T InitiativePercentage { get; set; }
        new T ActionsPerInitiative { get; set; }
        new T HarmonyAmount { get; set; }
    }


    public interface ITemporalStatsData<out T> 
    { 
        T InitiativePercentage { get; }
        T ActionsPerInitiative { get; }
        T HarmonyAmount { get; }
    }

    public interface ITemporalStatsInjection<in T> 
    {
        T InitiativePercentage { set; }
        T ActionsPerInitiative { set; }
        T HarmonyAmount { set; }
    }

    public interface IConcentrationStats<T> : IConcentrationStatsData<T>, IConcentrationStatsInjection<T>
    {
        new T DisruptionResistance { get; set; }
        new T CriticalChance { get; set; }
        new T SpeedAmount { get; set; }
    }
    public interface IConcentrationStatsData<out T> : IStatsData
    {
        /// <summary>
        /// Affects Harmony
        /// </summary>
        T DisruptionResistance { get; }

        T CriticalChance { get; }
        T SpeedAmount { get; }
    }

    public interface IConcentrationStatsInjection<in T>
    {
        T DisruptionResistance { set; }
        T CriticalChance { set; }
        T SpeedAmount { set; }
    }






}
