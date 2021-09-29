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
        IVitalityChangeListener<CombatingEntity,float>
    {
        public CombatingTeam(ITeamProvider generateFromProvider)
        {
            _stanceHandler = new TeamStanceHandler();
            _livingEntitiesTracker = new HashSet<CombatingEntity>();

            Vanguard = GenerateEntity(generateFromProvider.Vanguard);
            Attacker = GenerateEntity(generateFromProvider.Attacker);
            Support = GenerateEntity(generateFromProvider.Support);

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


        [ShowInInspector]
        public CombatingEntity Vanguard { get; }
        [ShowInInspector]
        public CombatingEntity Attacker { get; }
        [ShowInInspector]
        public CombatingEntity Support { get; }

        private readonly TeamStanceHandler _stanceHandler;


        public CombatingTeam EnemyTeam;

        public void Subscribe(ITeamStanceListener listener)
            => _stanceHandler.Add(listener);
        public void SwitchStance(EnumTeam.TeamStance targetStance)
            => _stanceHandler.Invoke(targetStance);


        public void OnRecoveryReceiveAction(CombatingEntity element, float value)
        {
        }

        public void OnDamageReceiveAction(CombatingEntity element, float value)
        {
        }

        public void OnShieldLost(CombatingEntity element, float value)
        {
        }

        public void OnHealthLost(CombatingEntity element, float value)
        {
        }

        public void OnMortalityDeath(CombatingEntity element, float value)
        {
            if (_livingEntitiesTracker.Contains(element))
                _livingEntitiesTracker.Remove(element);
        }
    }

    internal class TeamStanceHandler : HashSet<ITeamStanceListener>
    {
        public void Invoke(EnumTeam.TeamStance targetStance)
        {
            foreach (ITeamStanceListener listener in this)
            {
                listener.OnStanceChange(targetStance);
            }
        }
    }
}
