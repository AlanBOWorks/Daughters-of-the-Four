using System;
using System.Collections.Generic;
using CombatEffects;
using CombatEntity;
using CombatSystem;
using CombatSystem.Events;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace CombatTeam
{
    public sealed class CombatingTeam : List<CombatingEntity>, ITeamRoleStructureRead<CombatingEntity>,
        ITeamStateChangeListener
    {
        public CombatingTeam(ITeamProvider generateFromProvider)
        {
            LivingEntitiesTracker = new List<CombatingEntity>();

            Vanguard = GenerateEntity(generateFromProvider.Vanguard);
            Attacker = GenerateEntity(generateFromProvider.Attacker);
            Support = GenerateEntity(generateFromProvider.Support);

            Events = new TeamEvents();
            ProvokeHandler = new ProvokeHandler();

            TeamStats = new TeamStats();

            foreach (var member in this)
            {
                member.TargetingHolder.Inject(this);
            }



            CombatingEntity GenerateEntity(ICombatEntityProvider entityProvider)
            {
                if (entityProvider == null) return null;
                var generatedEntity = new CombatingEntity(entityProvider,this);
                Add(generatedEntity);
                LivingEntitiesTracker.Add(generatedEntity);
                CombatSystemSingleton.AllEntities.Add(generatedEntity);

                return generatedEntity;
            }
        }

        public readonly List<CombatingEntity> LivingEntitiesTracker;
        public bool HasLivingEntities() => LivingEntitiesTracker.Count > 0;

        [Title("Data")] 
        public readonly TeamStats TeamStats;

        public EnumTeam.TeamStance CurrentStance => TeamStats.CurrentStance;

        [Title("Members")]
        [ShowInInspector]
        public CombatingEntity Vanguard { get; }
        [ShowInInspector]
        public CombatingEntity Attacker { get; }
        [ShowInInspector]
        public CombatingEntity Support { get; }

        public CombatingEntity CollectFrontMostMember() => LivingEntitiesTracker[0];
        public CombatingEntity CollectBackMostMember() => LivingEntitiesTracker[LivingEntitiesTracker.Count - 1];


        [Title("Events")]
        [ShowInInspector]
        public readonly TeamEvents Events;
        [ShowInInspector]
        public readonly ProvokeHandler ProvokeHandler;

        public CombatingTeam EnemyTeam { get; private set; }
        public void Injection(CombatingTeam enemyTeam) => EnemyTeam = enemyTeam;

        public void BurstControl(float addition) => TeamStats.BurstControl += addition;

        public void CompeteControl(float variation)
        {
            TeamStats.CompetingControl += variation;
            EnemyTeam.TeamStats.CompetingControl = TeamStats.CompetingControl;
        }

        public void OnStanceChange(EnumTeam.TeamStance switchStance)
        {
            TeamStats.CurrentStance = switchStance;
        }

        public void OnMemberDeath(CombatingEntity member)
        {
            foreach (var entity in LivingEntitiesTracker)
            {
                if(entity != member) continue;
                LivingEntitiesTracker.Remove(member);
                foreach (var ally in LivingEntitiesTracker)
                {
                    ally.TargetingHolder.SelfAlliesElement.Remove(member);
                }
                break;
            }
        }

        public void DoResetBurst() => TeamStats.DoResetBurst();
    }

}
