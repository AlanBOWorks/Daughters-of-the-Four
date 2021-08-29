using System.Collections.Generic;
using UnityEngine;

namespace Stats
{

    public interface IBuffHolder<out T>
    {
        T GetBuff();
        T GetBurst();
    }

    public interface IStatsHolder<out T> : IBuffHolder<T>
    {
        T GetBase();
    }


    public interface IFullStats<T> : IBasicStats<T>, ICombatHealthStats<T>,
        IFullStatsData<T>, IFullStatsInjection<T>,
        ITemporalStats<T>
    { }
    public interface IFullStatsData<out T> : IBasicStatsData<T>, ICombatHealthStatsData<T>,
        ITemporalStatsData<T>
    { }

    public interface IFullStatsInjection<in T> : IBasicStatsInjection<T>, ICombatHealthStatsInjector<T>,
        ITemporalStatsInjection<T>
    { }


    public interface IBasicStats<T> : IBasicStatsData<T>, IBasicStatsInjection<T>
    {
        new T AttackPower { get; set; }
        new T DeBuffPower { get; set; }
        new T StaticDamagePower { get; set; }
        new T HealPower { get; set; }
        new T BuffPower { get; set; }
        new T BuffReceivePower { get; set; }
        new T MaxHealth { get; set; }
        new T MaxMortalityPoints { get; set; }
        new T DamageReduction { get; set; }
        new T DeBuffReduction { get; set; }
        new T InitiativePercentage { get; set; }
        new T ActionsPerInitiative { get; set; }
        new T HarmonyAmount { get; set; }
        new T Enlightenment { get; set; }
        new T CriticalChance { get; set; }
        new T SpeedAmount { get; set; }
    }


    public interface IBasicStatsData<out T> :
        IOffensiveStatsData<T>,
        ISupportStatsData<T>,
        IVitalityStatsData<T>,
        IConcentrationStatsData<T>,
        ITemporalStatsData<T>
    { }

    public interface IBasicStatsInjection<in T> :
        IOffensiveStatsInjection<T>,
        ISupportStatsInjection<T>,
        IVitalityStatsInjection<T>,
        IConcentrationStatsInjection<T>,
        ITemporalStatsInjection<T>
    { }


    public interface ICompositeStatsStructure<out T>
    {
        T OffensiveStats { get; }
        T SupportStats { get; }
        T VitalityStats { get; }
        T ConcentrationStats { get; }
        T TemporalStats { get; }
    }

    public interface ICompositeStatsHolder<T>
    {
        IOffensiveStats<T> OffensiveStats { get; }
        ISupportStats<T> SupportStats { get; }
        IVitalityStats<T> VitalityStats { get; }
        IConcentrationStats<T> ConcentrationStats { get; }
        ITemporalStats<T> TemporalStats { get; }
    }

    public interface ICompositeStatsDataHolder<out T>
    {
        IOffensiveStatsData<T> OffensiveStats { get; }
        ISupportStatsData<T> SupportStats { get; }
        IVitalityStatsData<T> VitalityStats { get; }
        IConcentrationStatsData<T> ConcentrationStats { get; }
        ITemporalStatsData<T> TemporalStats { get; }
    }


    public interface ICollectionStats<T>
    {
        ICollection<IOffensiveStatsData<T>> OffensiveStats { get; }
        ICollection<ISupportStatsData<T>> SupportStats { get; }
        ICollection<IVitalityStatsData<T>> VitalityStats { get; }
        ICollection<IConcentrationStatsData<T>> ConcentrationStats { get; }
        ICollection<ITemporalStatsData<T>> TemporalStats { get; }
    }

    public interface IDictionaryStats<TKey, TValue>
    {
        Dictionary<IOffensiveStatsData<TKey>, TValue> OffensiveStats { get; }
        Dictionary<ISupportStatsData<TKey>, TValue> SupportStats { get; }
        Dictionary<IVitalityStatsData<TKey>, TValue> VitalityStats { get; }
        Dictionary<IConcentrationStatsData<TKey>, TValue> ConcentrationStats { get; }
        Dictionary<ITemporalStatsData<TKey>, TValue> TemporalStats { get; }
    }
}
