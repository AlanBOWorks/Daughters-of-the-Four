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
            _provider = provider;
            // Unity.Objects
            _entityHolderPrefab = provider.GetEntityPrefab();

            // Data
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

        private readonly ICombatEntityProvider _provider;

        private readonly UEntityHolder _entityHolderPrefab;

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

        public string GetEntityName() => _provider.GetEntityName();
        public UEntityHolder GetEntityPrefab() => _entityHolderPrefab;

        public bool IsAlive() => UtilsCombatStats.IsAlive(CombatStats);
        public bool CanAct() => UtilsCombatStats.CanAct(CombatStats);

    }


    public interface ICombatEntityProvider
    {
        string GetEntityName();
        UEntityHolder GetEntityPrefab();
        CombatStatsHolder GenerateStatsHolder();
        CombatingAreaData GenerateAreaData();

        ITeamStanceStructureRead<ICollection<SkillProviderParams>> ProvideStanceSkills();
    }

}
