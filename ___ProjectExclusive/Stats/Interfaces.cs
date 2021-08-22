using Characters;
using UnityEngine;

namespace Stats
{

    public interface IOffensiveStats : IOffensiveStatsData, IOffensiveStatsInjection
    { }
    public interface IOffensiveStatsData
    {
        float GetAttackPower();
        float GetDeBuffPower();
        float GetStaticDamagePower();
    }

    public interface IOffensiveStatsInjection
    {
        float AttackPower { set; }
        float DeBuffPower { set; }
        float StaticDamagePower { set; }
    }

    public interface ISupportStats : ISupportStatsData, ISupportStatsInjection
    { }

    public interface ISupportStatsData
    {
        float GetHealPower();
        float GetBuffPower();
        float GetBuffReceivePower();
    }
    public interface ISupportStatsInjection
    {
        float HealPower { set; }
        float BuffPower { set; }
        float BuffReceivePower { set; }
    }

    public interface IVitalityStats : IVitalityStatsData, IVitalityStatsInjection
    { }
    public interface IVitalityStatsData
    {
        float GetMaxHealth();
        float GetMaxMortalityPoints();
        float GetDamageReduction();
        float GetDeBuffReduction();

        //Static reduction is determined by deBuffReduction (+ damage?)
    }

    public interface IVitalityStatsInjection
    {
        float MaxHealth { set; }
        float MaxMortalityPoints { set; }

        float DamageReduction { set; }
        float DeBuffReduction { set; }
    }


    /// <summary>
    /// Used only during the fights. <br></br>
    /// For variation, only the <seealso cref="CharacterCombatData.BaseStats"/>
    /// should be used
    /// </summary>
    public interface ICombatTemporalStats : ICombatTemporalStatsBase
    {
        float HealthPoints { get; set; }
        float ShieldAmount { get; set; }
        float MortalityPoints { get; set; }
        float AccumulatedStatic { get; set; }
    }

    public interface ICombatTemporalStatsBase : ICombatTemporalStatsBaseData, ICombatTemporalStatsBaseInjection
    { }

    public interface ICombatTemporalStatsBaseData : ICombatTempoStatsData, IHarmonyStatsData
    { }
    public interface ICombatTemporalStatsBaseInjection : ICombatTempoStatsInjection, IHarmonyStatsInjection
    { }

    public interface ICombatTempoStatsData
    {
        float GetInitiativePercentage();
        int GetActionsPerInitiative();
    }
    public interface ICombatTempoStatsInjection
    {
        float InitiativePercentage { set; }
        int ActionsPerInitiative { set; }
    }
    public interface IHarmonyStatsData
    {
        float GetHarmonyAmount();
    }

    public interface IHarmonyStatsInjection
    {
        float HarmonyAmount { set; }
    }


    public interface ISpecialStats : ISpecialStatsData, ISpecialStatsInjection
    { }

    public interface ISpecialStatsData
    {
        /// <summary>
        /// Affects Harmony
        /// </summary>
        float GetEnlightenment();
        float GetCriticalChance();
        float GetSpeedAmount();
    }

    public interface ISpecialStatsInjection
    {
        float Enlightenment { set; }
        float CriticalChance { set; }
        float SpeedAmount { set; }
    }

    public interface IStatsUpgradable
    {
        float OffensivePower { get; set; }
        float SupportPower { get; set; }
        float VitalityAmount { get; set; }
        float Enlightenment { get; set; }
    }
}
