using Characters;
using UnityEngine;

namespace Stats
{

    public interface IOffensiveStats
    {
        float AttackPower { get; set; }
        float DeBuffPower { get; set; }
    }

    public interface ISupportStats
    {
        float HealPower { get; set; }
        float BuffPower { get; set; }
    }


    public interface IVitalityStats
    {
        float MaxHealth { get; set; }
        float MaxMortalityPoints { set; get; }

        float DamageReduction { set; get; }
        float DeBuffReduction { set; get; }
        //Static reduction is determined by deBuffReduction (+ damage?)
    }


    /// <summary>
    /// Used only during the fights. <br></br>
    /// For variation, only the <seealso cref="CharacterCombatData.BaseStats"/>
    /// should be used
    /// </summary>
    public interface ICombatTemporalStats
    {
        float HealthPoints { get; set; }
        float ShieldAmount { get; set; }
        float MortalityPoints { get; set; }
        float HarmonyAmount { get; set; }
        float InitiativePercentage { get; set; }
        int ActionsPerInitiative { get; set; }
    }

    public interface ISpecialStats
    {
        /// <summary>
        /// Affects Harmony
        /// </summary>
        float Enlightenment { get; set; }
        float CriticalChance { get; set; }
        float SpeedAmount { get; set; }
    }


    public interface IStatsUpgradable
    {
        float OffensivePower { get; set; }
        float SupportPower { get; set; }
        float VitalityAmount { get; set; }
        float Enlightenment { get; set; }
    }
}
