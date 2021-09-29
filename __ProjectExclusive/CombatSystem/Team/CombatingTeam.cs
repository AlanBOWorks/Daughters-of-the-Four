using System;
using System.Collections.Generic;
using CombatEntity;
using CombatSystem.Events;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace CombatTeam
{
    public sealed class CombatingTeam : HashSet<CombatingEntity>, ITeamStructureRead<CombatingEntity>,
        ITeamStateChangeListener
    {
        public CombatingTeam(ITeamProvider generateFromProvider)
        {
            _livingEntitiesTracker = new HashSet<CombatingEntity>();

            Vanguard = GenerateEntity(generateFromProvider.Vanguard);
            Attacker = GenerateEntity(generateFromProvider.Attacker);
            Support = GenerateEntity(generateFromProvider.Support);

            Events = new TeamEvents();

            CombatingEntity GenerateEntity(ICombatEntityProvider entityProvider)
            {
                if (entityProvider == null) return null;
                var generatedEntity = new CombatingEntity(entityProvider,this);
                Add(generatedEntity);
                _livingEntitiesTracker.Add(generatedEntity);

                return generatedEntity;
            }
        }

        private readonly HashSet<CombatingEntity> _livingEntitiesTracker;
        public bool HasLivingEntities() => _livingEntitiesTracker.Count > 0;

        [Title("Data")]
        public EnumTeam.TeamStance CurrentStance { get; private set; }

        [Title("Members")]
        [ShowInInspector]
        public CombatingEntity Vanguard { get; }
        [ShowInInspector]
        public CombatingEntity Attacker { get; }
        [ShowInInspector]
        public CombatingEntity Support { get; }
        [Title("Events")]
        [ShowInInspector]
        public readonly TeamEvents Events;


        public CombatingTeam EnemyTeam;


        public void OnStanceChange(EnumTeam.TeamStance switchStance)
        {
            CurrentStance = switchStance;
        }

        public void OnMemberDeath(CombatingEntity member)
        {
            if (_livingEntitiesTracker.Contains(member))
                _livingEntitiesTracker.Remove(member);
        }
    }

}
