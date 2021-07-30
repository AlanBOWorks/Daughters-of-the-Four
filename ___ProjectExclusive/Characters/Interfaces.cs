using Stats;
using UnityEngine;

namespace Characters
{
    public interface ICharacterLore
    {
        string CharacterName { get; }
    }

    public interface ICharacterFullStats : ICharacterBasicStats, ICombatTemporalStats
    { }

    public interface ICharacterBasicStats : IOffensiveStats,ISupportStats, 
        IVitalityStats, ISpecialStats, ICombatTemporalStatsBase
    { }


}
