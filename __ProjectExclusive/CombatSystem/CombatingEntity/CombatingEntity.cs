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

            // Combat Data
            CombatStats = provider.GenerateStatsHolder();
            AreaData = provider.GenerateAreaData();

            SkillsHolder 
                = new CombatEntitySkillsHolder(team, provider.ProvideStanceSkills(),AreaData.GetRole());
            SkillUsageTracker 
                = new SkillUsageTracker();

            // Special Mechanics
            FollowUpHandler = new FollowUpHandler(this);
            GuardHandler = new GuardHandler(this);
            ProvokeEffects = new ProvokeEffectsHolder(this);

            // Combat Entity's Holders
            Team = team;
            TargetingHolder = new CombatTargetingHolder(this);

            // Events
            EventsHolder = new CombatingEntityEvents();


            // Injection
            CombatStats.Injection(this);
        }

        private readonly ICombatEntityProvider _provider;

        private readonly UEntityHolder _entityHolderPrefab;

        [Title("Stats")]
        public readonly CombatStatsHolder CombatStats;
        public readonly AreaData AreaData;

        [Title("Skills")] 
        public readonly CombatEntitySkillsHolder SkillsHolder;
        public readonly SkillUsageTracker SkillUsageTracker;

        [Title("Follow Up")] 
        public readonly FollowUpHandler FollowUpHandler;

        [Title("Guarding")] 
        public readonly GuardHandler GuardHandler;

        [Title("Provoke")] 
        public readonly ProvokeEffectsHolder ProvokeEffects;

        [Title("Team")]
        public readonly CombatingTeam Team;
        [ShowInInspector]
        public readonly CombatTargetingHolder TargetingHolder;

        [Title("Events")]
        public readonly CombatingEntityEvents EventsHolder;


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
