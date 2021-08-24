using Characters;
using UnityEngine;

namespace Stats
{
    public interface IStatsPrimordial : IStatsPrimordial<float>
    { }
    public interface IStatsPrimordial<T>
    {
        T OffensivePower { get; set; }
        T SupportPower { get; set; }
        T VitalityAmount { get; set; }
        T ConcentrationAmount { get; set; }
    }


    public interface IStatsUpgradable : IStatsPrimordial
    {
        //TODO UpgradeLevel?
    }

    public interface IOffensiveStats : IOffensiveStatsData, IOffensiveStatsInjection
    { }
    public interface IOffensiveStatsData : IOffensiveStatsData<float>
    { }

    public interface IOffensiveStatsInjection : IOffensiveStatsInjection<float>
    { }
    public interface IOffensiveStatsData<out T>
    {
        T GetAttackPower();
        T GetDeBuffPower();
        T GetStaticDamagePower();
    }
    public interface IOffensiveStatsInjection<in T>
    {
        T AttackPower { set; }
        T DeBuffPower { set; }
        T StaticDamagePower { set; }
    }

    public interface ISupportStats : ISupportStatsData, ISupportStatsInjection
    { }
    public interface ISupportStatsData : ISupportStatsData<float>
    { }
    public interface ISupportStatsInjection : ISupportStatsInjection<float>
    { }
    public interface ISupportStatsData<out T>
    {
        T GetHealPower();
        T GetBuffPower();
        T GetBuffReceivePower();
    }
    public interface ISupportStatsInjection<in T>
    {
        T HealPower { set; }
        T BuffPower { set; }
        T BuffReceivePower { set; }
    }

    public interface IVitalityStats : IVitalityStatsData, IVitalityStatsInjection
    { }
    public interface IVitalityStatsData : IVitalityStatsData<float>
    {}
    public interface IVitalityStatsData<out T>
    {
        T GetMaxHealth();
        T GetMaxMortalityPoints();
        T GetDamageReduction();
        T GetDeBuffReduction();

        //Static reduction is determined by deBuffReduction (+ damage?)
    }

    public interface IVitalityStatsInjection : IVitalityStatsInjection<float>
    {}
    public interface IVitalityStatsInjection<in T>
    {
        T MaxHealth { set; }
        T MaxMortalityPoints { set; }

        T DamageReduction { set; }
        T DeBuffReduction { set; }
    }


    ///  /// <summary>
    /// <inheritdoc cref="ICombatTemporalStats{T}"/>
    /// </summary>
    public interface ICombatTemporalStats : ICombatTemporalStats<float>
    { }
    /// <summary>
    /// Used only during the fights. <br></br>
    /// For variation, only the <seealso cref="CombatStatsHolder.BaseStats"/>
    /// should be used
    /// </summary>
    public interface ICombatTemporalStats<T> : ICombatTemporalStatsBase
    {
        T HealthPoints { get; set; }
        T ShieldAmount { get; set; }
        T MortalityPoints { get; set; }
        T AccumulatedStatic { get; set; }
    }

    public interface ICombatTemporalStatsBase : ICombatTemporalStatsBaseData, ICombatTemporalStatsBaseInjection
    { }

    public interface ICombatTemporalStatsBaseData : ICombatTempoStatsData, IHarmonyStatsData
    { }
    public interface ICombatTemporalStatsBaseInjection : ICombatTempoStatsInjection, IHarmonyStatsInjection
    { }

    public interface ICombatTempoStatsData : ICombatTempoStatsData<float>
    {}
    public interface ICombatTempoStatsInjection : ICombatTempoStatsInjection<float>
    {}
    public interface IHarmonyStatsData : IHarmonyStatsData<float>
    {}

    public interface IHarmonyStatsInjection : IHarmonyStatsInjection<float>
    {}

    public interface ICombatTempoStatsData<out T>
    {
        T GetInitiativePercentage();
        int GetActionsPerInitiative();
    }
    public interface ICombatTempoStatsInjection<in T>
    {
        T InitiativePercentage { set; }
        int ActionsPerInitiative { set; }
    }
    public interface IHarmonyStatsData<out T>
    {
        T GetHarmonyAmount();
    }

    public interface IHarmonyStatsInjection<in T>
    {
        T HarmonyAmount { set; }
    }



    public interface IConcentrationStats : IConcentrationStatsData, IConcentrationStatsInjection
    { }

    public interface IConcentrationStatsData : IConcentrationStatsData<float>
    {}
    public interface IConcentrationStatsData<out T>
    {
        /// <summary>
        /// Affects Harmony
        /// </summary>
        T GetEnlightenment();
        T GetCriticalChance();
        T GetSpeedAmount();
    }

    public interface IConcentrationStatsInjection : IConcentrationStatsInjection<float>
    {}
    public interface IConcentrationStatsInjection<in T>
    {
        T Enlightenment { set; }
        T CriticalChance { set; }
        T SpeedAmount { set; }
    }






    public interface IFullStats : IBasicStats, IFullStatsData, IFullStatsInjection
    { }

    public interface IFullStatsInjection : IBasicStatsInjection, ICombatTemporalStats
    { }
    public interface IFullStatsData : IBasicStatsData, ICombatTemporalStats
    { }

    public interface IBasicStats : IBasicStatsData, IBasicStatsInjection
    { }

    public interface IBasicStatsData : IOffensiveStatsData, ISupportStatsData,
        IVitalityStatsData, IConcentrationStatsData, ICombatTemporalStatsBaseData
    { }

    public interface IBasicStatsInjection : IOffensiveStatsInjection, ISupportStatsInjection,
        IVitalityStatsInjection, IConcentrationStatsInjection, ICombatTemporalStatsBaseInjection
    { }
}
