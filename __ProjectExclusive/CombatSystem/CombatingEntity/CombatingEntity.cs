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

            GuardHandler = new GuardHandler(this);

            Team = team;
            TargetingHolder = new CombatTargetingHolder(this);

            EventsHolder 
                = new CharacterEventsHolder<CombatingEntity,CombatingEntity, SkillComponentResolution>();

            // Injection
            CombatStats.Injection(this);
        }

        private readonly ICombatEntityProvider _provider;

        private readonly UEntityHolder _entityHolderPrefab;

        [Title("Stats")]
        public readonly CombatStatsHolder CombatStats;
        public readonly CombatingAreaData AreaData;

        [Title("Skills")] 
        public readonly CombatEntitySkillsHolder SkillsHolder;
        public readonly SkillUsageTracker SkillUsageTracker;

        [Title("Guarding")] 
        public readonly GuardHandler GuardHandler;

        [Title("Team")]
        public readonly CombatingTeam Team;
        [ShowInInspector]
        public readonly CombatTargetingHolder TargetingHolder;

        [Title("Events")]
        public readonly CharacterEventsHolder<CombatingEntity, CombatingEntity, SkillComponentResolution> EventsHolder;

        [Title("Unity.Object")]
        [ShowInInspector]
        public UEntityHolder InstantiatedHolder { get; private set; }

        public string GetEntityName() => _provider.GetEntityName();
        public UEntityHolder GetEntityPrefab() => _entityHolderPrefab;

        public bool IsAlive() => UtilsCombatStats.IsAlive(CombatStats);
        public bool CanAct() => UtilsCombatStats.CanAct(CombatStats);


        public void Injection(UEntityHolder instantiatedHolder) => InstantiatedHolder = instantiatedHolder;
    }


}
