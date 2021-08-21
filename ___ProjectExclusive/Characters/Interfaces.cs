using Stats;
using UnityEngine;

namespace Characters
{
    public interface ICharacterLore
    {
        string CharacterName { get; }
    }

    public interface ICharacterFullStats : ICharacterBasicStats,ICharacterFullStatsData, ICharacterFullStatsInjection
    { }

    public interface ICharacterFullStatsInjection : ICharacterBasicStatsInjection, ICombatTemporalStats
    { }
    public interface ICharacterFullStatsData : ICharacterBasicStatsData, ICombatTemporalStats
    { }

    public interface ICharacterBasicStats :ICharacterBasicStatsData, ICharacterBasicStatsInjection
    { }

    public interface ICharacterBasicStatsData : IOffensiveStatsData, ISupportStatsData,
        IVitalityStatsData, ISpecialStatsData, ICombatTemporalStatsBaseData
    { }

    public interface ICharacterBasicStatsInjection : IOffensiveStatsInjection, ISupportStatsInjection,
        IVitalityStatsInjection, ISpecialStatsInjection, ICombatTemporalStatsBaseInjection
    { }

    public interface ICharacterBasicStatsData<out T>
    {
        T OffensiveStats { get; }
        T SupportStats { get; }
        T VitalityStats { get; }
        T SpecialStats { get; }
        T TemporalStats { get; }
    }

    public interface ICharacterBasicStatsInjection<in T>
    {
        T OffensiveStats { set; }
        T SupportStats { set; }
        T VitalityStats { set; }
        T SpecialStats { set; }
        T TemporalStats { set; }
    }

}
