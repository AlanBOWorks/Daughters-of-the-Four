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
}
