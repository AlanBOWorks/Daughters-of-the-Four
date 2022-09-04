using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ExplorationSystem
{
    // todo make it to a normal Object and use it in the ExplorationSystem
    public class UEnemyExplorationTeamHolder : MonoBehaviour, ISceneChangeListener, IWorldSceneListener
    {

        [Title("Behaviour")]
        [ShowInInspector, EnableIf("_teamWrapper"), DisableInEditorMode]
        private EnemyTeamWrapper _teamWrapper;

#if UNITY_EDITOR
        [ShowInInspector, ShowIf("_dataHolder")] 
        private SExplorationSceneDataHolder _dataHolder;
#endif

        private void Awake()
        {
            _teamWrapper = new EnemyTeamWrapper();
            ExplorationSingleton.EventsHolder.Subscribe(this);
            ExplorationSingleton.SceneEnemyTeamsHolder = _teamWrapper;
        }
        private void OnDestroy()
        {
            ExplorationSingleton.EventsHolder.UnSubscribe(this);
        }
        public void OnSceneChange(IExplorationSceneDataHolder sceneData)
        {
            _teamWrapper.Injection(sceneData);

#if UNITY_EDITOR
            if (sceneData is SExplorationSceneDataHolder assetDataHolder)
                _dataHolder = assetDataHolder;
#endif

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
            [ShowInInspector,HorizontalGroup]
            private IExplorationSceneEntitiesHolder _entitiesHolder;
            [ShowInInspector,HorizontalGroup, ShowIf("_halfMapEntitiesHolder")]
            private IExplorationSceneEntitiesHolder _halfMapEntitiesHolder;

            [ShowInInspector]
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
            }


            private IExplorationSceneEntitiesHolder GetCurrentEntitiesHolder()
            {
                return (_halfMapEntitiesHolder == null || !IsHalfMapReached) 
                    ? _entitiesHolder 
                    : _halfMapEntitiesHolder;
            }

            // Selector
            private void DoSelection(IEnumerable<ICombatEntityProvider> team)
            {
                _selectedEntities.Clear();
               _selectedEntities.HandleMembers(team);
            }

            // Getters
            public ICombatTeamProvider BasicThreatType => DoBasicTeamSelection();
            [Button]
            private ICombatTeamProvider DoBasicTeamSelection()
            {
                var currentHolder = GetCurrentEntitiesHolder();
                DoSelection(currentHolder.GetBasicEntities());
                return _selectedEntities;
            }

            public ICombatTeamProvider EliteThreatType => DoEliteTeamSelection();
            private ICombatTeamProvider DoEliteTeamSelection()
            {
                var currentHolder = GetCurrentEntitiesHolder();
                DoSelection(currentHolder.GetBasicEntities());
                return _selectedEntities;
            }

            public ICombatTeamProvider BossThreatType => DoBossTeamSelection();
            private ICombatTeamProvider DoBossTeamSelection()
            {
                throw new NotImplementedException();
            }


            public IEnumerable<ICombatEntityProvider> GetSelectedCharacters() => _selectedEntities;




            private sealed class CombatTeamProviderWrapper : List<ICombatEntityProvider>, ICombatTeamProvider
            {
                public IEnumerable<ICombatEntityProvider> GetSelectedCharacters() => this;

                public void HandleMembers(IEnumerable<ICombatEntityProvider> members)
                {
                    foreach (var entityProvider in members)
                    {
                        if(entityProvider == null) continue;
                        Add(entityProvider);
                    }
                }
            }
        }
    }
}
