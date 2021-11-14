using System.Collections.Generic;
using CombatSkills;
using CombatTeam;
using Stats;
using UnityEngine;

namespace CombatEntity
{

    public interface ICombatEntityProvider :
        ICombatLoreProvider, ICombatVisualsProvider,
        ICombatStatsProvider, ICombatSkillsProvider
    {

    }

    public interface ICombatLoreProvider
    {
        string GetEntityName();
    }

    public interface ICombatVisualsProvider
    {
        UEntityHolder GetEntityPrefab();
    }

    public interface ICombatStatsProvider
    {
        CombatStatsHolder GenerateStatsHolder();
        AreaData GenerateAreaData();
    }

    public interface ICombatSkillsProvider
    {
        ITeamStanceStructureRead<ICollection<SkillProviderParams>> ProvideStanceSkills();
    }
}
