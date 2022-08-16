using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ExplorationSystem
{
    public class UEnemyExplorationTeamHolder : MonoBehaviour, ISceneChangeListener
    {

        [Title("Behaviour")]
        [ShowInInspector, DisableIf("_teamWrapper"), DisableInEditorMode]
        private EnemyTeamWrapper _teamWrapper;

        private void Awake()
        {
            _teamWrapper = new EnemyTeamWrapper();
            PlayerExplorationSingleton.EventsHolder.Subscribe(this);
        }

        private sealed class EnemyTeamWrapper : IExplorationThreatsStructureRead<ICombatTeamProvider>
        {
            [ShowInInspector, HorizontalGroup("Basic enemies")]
            private readonly BasicEntitiesWrapper _basicEnemies;
            [ShowInInspector, HorizontalGroup("Basic enemies")]
            private readonly HashSet<ICombatEntityProvider> _weakEntities;

            private IExplorationEntitiesSpawnRateHolder _spawnRates;
            private readonly TeamWrapper _selectedEntities;
            public EnemyTeamWrapper()
            {
                _weakEntities = new HashSet<ICombatEntityProvider>();
                _basicEnemies = new BasicEntitiesWrapper();

                _selectedEntities = new TeamWrapper();
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


            public void Injection(IExplorationSceneDataHolder dataHolder)
            {
                ResetState();
                _spawnRates = dataHolder;

                var basicEntities = dataHolder.GetBasicEntities();
                var basicEntitiesEnumerable = UtilsTeam.GetEnumerable(_basicEnemies, basicEntities);
                foreach (var (collection, sceneEntities) in basicEntitiesEnumerable)
                {
                    Injection(collection, sceneEntities);
                }
            }

            private void ResetState()
            {
                _selectedEntities.Clear();

                _basicEnemies.Clear();
                _weakEntities.Clear();
            }

            private void Injection(ISet<ICombatEntityProvider> collection,
                IEnumerable<SceneEntitiesData> basicEntities)
            {
                foreach (var data in basicEntities)
                {
                    var entity = data.GetEntityPreset();
                    collection.Add(entity);
                    if (data.IgnoreVariations()) continue;
                    _weakEntities.Add(entity);
                }
            }


            private sealed class BasicEntitiesWrapper : ITeamFlexStructureRead<HashSet<ICombatEntityProvider>>
            {
                public BasicEntitiesWrapper()
                {
                    VanguardType = new HashSet<ICombatEntityProvider>();
                    AttackerType = new HashSet<ICombatEntityProvider>();
                    SupportType = new HashSet<ICombatEntityProvider>();
                    FlexType = new HashSet<ICombatEntityProvider>();
                }

                public void Clear()
                {
                    VanguardType.Clear();
                    AttackerType.Clear();
                    SupportType.Clear();
                    FlexType.Clear();
                }

                public HashSet<ICombatEntityProvider> VanguardType { get; }
                public HashSet<ICombatEntityProvider> AttackerType { get; }
                public HashSet<ICombatEntityProvider> SupportType { get; }
                public HashSet<ICombatEntityProvider> FlexType { get; }
            }

            private sealed class TeamWrapper : List<ICombatEntityProvider>, ICombatTeamProvider
            {
                public IEnumerable<ICombatEntityProvider> GetSelectedCharacters() => this;
            }
        }

        public void OnSceneChange(IExplorationSceneDataHolder sceneData)
        {
            _teamWrapper.Injection(sceneData);
        }
    }
}
