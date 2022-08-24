using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ExplorationSystem
{
    public class UEnemyExplorationTeamHolder : MonoBehaviour, ISceneChangeListener, IWorldSceneListener
    {

        [Title("Behaviour")]
        [ShowInInspector, DisableIf("_teamWrapper"), DisableInEditorMode]
        private EnemyTeamWrapper _teamWrapper;

        private void Awake()
        {
            _teamWrapper = new EnemyTeamWrapper();
            PlayerExplorationSingleton.EventsHolder.Subscribe(this);
        }
        private void OnDestroy()
        {
            PlayerExplorationSingleton.EventsHolder.UnSubscribe(this);
        }
        public void OnSceneChange(IExplorationSceneDataHolder sceneData)
        {
            _teamWrapper.Injection(sceneData);
        }

        public void OnWorldSceneOpen(IExplorationSceneDataHolder lastMap)
        {
            _teamWrapper.ResetState();
        }

        public void OnWorldMapClose(IExplorationSceneDataHolder targetMap)
        {
            if (targetMap == null)
                _teamWrapper.ResetState();
        }



        private sealed class EnemyTeamWrapper : IExplorationThreatsStructureRead<ICombatTeamProvider>
        {
            [ShowInInspector]
            private IExplorationSceneEntitiesHolder _entitiesHolder;
            private IExplorationSceneEntitiesHolder _halfMapEntitiesHolder;
            private IExplorationEntitiesSpawnRateHolder _spawnRates;
            private readonly CombatTeamProviderWrapper _selectedEntities;


            public bool IsHalfMapReached;

            public EnemyTeamWrapper()
            {
                _selectedEntities = new CombatTeamProviderWrapper();
            }


            public void ResetState()
            {
                _selectedEntities.Clear();
                IsHalfMapReached = false;
            }
            public void Injection(IExplorationSceneDataHolder dataHolder)
            {
                ResetState();
                _entitiesHolder = dataHolder.GetEntities();
                _halfMapEntitiesHolder = dataHolder.GetHalfMapEntities();
                _spawnRates = dataHolder.GetSpawnRates();
            }


            public ICombatTeamProvider BasicThreatType => DoBossTeamSelection();
            private ICombatTeamProvider DoBasicTeamSelection()
            {
                _selectedEntities.Clear();
                return _selectedEntities;
            }

            public ICombatTeamProvider EliteThreatType => DoEliteTeamSelection();
            private ICombatTeamProvider DoEliteTeamSelection()
            {
                _selectedEntities.Clear();
                throw new NotImplementedException();
            }

            public ICombatTeamProvider BossThreatType => DoBossTeamSelection();
            private ICombatTeamProvider DoBossTeamSelection()
            {
                _selectedEntities.Clear();
                throw new NotImplementedException();
            }

            public IEnumerable<ICombatEntityProvider> GetSelectedCharacters() => _selectedEntities;




            private sealed class CombatTeamProviderWrapper : List<ICombatEntityProvider>, ICombatTeamProvider
            {
                public IEnumerable<ICombatEntityProvider> GetSelectedCharacters() => this;
            }
        }
    }
}
