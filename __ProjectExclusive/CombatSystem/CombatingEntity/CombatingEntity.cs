using System.Collections.Generic;
using CombatEffects;
using CombatSkills;
using CombatSystem.Events;
using Stats;
using CombatTeam;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatEntity
{
    public class CombatingEntity
    {
        public CombatingEntity(ICombatEntityProvider provider, CombatingTeam team)
        {
            CombatStats = provider.GenerateStatsHolder();
            AreaData = provider.GenerateAreaData();

            SkillsHolder 
                = new CombatEntitySkillsHolder(team, provider.ProvideStanceSkills(),AreaData.GetRole());
            SkillUsageTracker 
                = new SkillUsageTracker();

            Team = team;

            EventsHolder 
                = new CharacterEventsHolder<CombatingEntity,CombatingEntity, EffectResolution>();
        }


        [Title("Stats")]
        public readonly CombatStatsHolder CombatStats;
        public readonly CombatingAreaData AreaData;

        [Title("Skills")] 
        public readonly CombatEntitySkillsHolder SkillsHolder;
        public readonly SkillUsageTracker SkillUsageTracker;
        
        [Title("Team")]
        public readonly CombatingTeam Team;

        [Title("Events")]
        public readonly CharacterEventsHolder<CombatingEntity, CombatingEntity, EffectResolution> EventsHolder;


        public bool IsAlive() => CombatStats.CurrentMortality > 0;
        public bool CanAct() => CombatStats.CurrentActions > 0 && IsAlive();
    }


    public interface ICombatEntityProvider
    {
        CombatStatsHolder GenerateStatsHolder();
        CombatingAreaData GenerateAreaData();

        ITeamStanceStructureRead<ICollection<SkillProviderParams>> ProvideStanceSkills();
    }
}
