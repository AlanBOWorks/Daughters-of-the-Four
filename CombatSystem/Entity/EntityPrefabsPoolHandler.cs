using System;
using CombatSystem._Core;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CombatSystem.Entity
{
    public sealed class EntityPrefabsPoolHandler : IOppositionTeamStructureRead<ITeamFullRolesStructureRead<Transform>>,
        ICombatStatesListener
    {
        private readonly PlayerPrefabsHolder _playerTeamType;
        private readonly PrefabsHolder _enemyTeamType;

        public EntityPrefabsPoolHandler()
        {
            _playerTeamType = new PlayerPrefabsHolder();
            _enemyTeamType = new PrefabsHolder();
        }

        [ShowInInspector,HorizontalGroup()]
        public ITeamFullRolesStructureRead<Transform> PlayerTeamType => _playerTeamType;
        [ShowInInspector,HorizontalGroup()]
        public ITeamFullRolesStructureRead<Transform> EnemyTeamType => _enemyTeamType;



        private sealed class PlayerPrefabsHolder : PrefabsHolder
        {
            private bool _instantiated;

            public override void PoolMembers(CombatTeam team, ITeamFullRolesStructureRead<Transform> positions)
            {
                if(!_instantiated)
                {
                    base.PoolMembers(team, positions);
                    _instantiated = true;
                }
                else
                {
                    Pool(in team);
                }
            }

            private void Pool(in CombatTeam team)
            {
                var keyValuePairs = UtilsTeam.GetEnumerable(team,this);
                foreach (var pair in keyValuePairs)
                {
                    var entity = pair.Key;
                    if(entity == null) continue;
                    var entityTransform = pair.Value;
                    var entityGO = entityTransform.gameObject;

                    UtilsEntity.HandleInjections(in entity, in entityGO);
                    entityGO.SetActive(true);

                }
            }

            public override void OnFinishCombat()
            {
                Hide();
            }

            private void Hide()
            {
                var allMembers = UtilsTeam.GetEnumerable(this);
                foreach (var member in allMembers)
                {
                    if(member == null) continue;
                    member.gameObject.SetActive(false);
                }
            }

            public void OnMembersSwitch()
            {
                Destroy();
                _instantiated = false;
            }
        }

        private class PrefabsHolder : TeamFullGroupStructure<Transform>
        {

            public virtual void PoolMembers(CombatTeam team, ITeamFullRolesStructureRead<Transform> positions)
            {
                var keyValuePairs = UtilsTeam.GetEnumerable(team, positions);
                int index = 0;
                foreach (var pair in keyValuePairs)
                {
                    int i = index;
                    index++;

                    var entity = pair.Key;
                    if(entity == null) continue;
                    var positionTransform = pair.Value;
                    if(!positionTransform) 
                        throw new NullReferenceException("Positions Transform was Null; it's necessary for instantiation");

                    var entityGameObject = UtilsEntity.InstantiateProviderBody(entity);
                    var entityTransform = entityGameObject.transform;

                    entityTransform.position = positionTransform.position;
                    entityTransform.rotation = positionTransform.rotation;

                    UtilsTeam.SetElement(i,this, entityTransform);
                }
            }

            public virtual void OnFinishCombat()
            {
                Destroy();
            }

            protected void Destroy()
            {
                var allMembers = UtilsTeam.GetEnumerable(this);
                foreach (var member in allMembers)
                {
                    if(!member) continue;
                    Object.Destroy(member.gameObject);
                }
            }
        }

        public void HandleTeams(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            var playerPositions = CombatSystemSingleton.PlayerPositionTransformReferences;
            _playerTeamType.PoolMembers(playerTeam, playerPositions);

            var enemyPositions = CombatSystemSingleton.EnemyPositionTransformReferences;
            _enemyTeamType.PoolMembers(enemyTeam, enemyPositions);
        }


        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            // The instantiation is made by CombatInitializationHandler.InitiateModels
        }

        public void OnCombatStart()
        {
        }

        public void OnCombatEnd()
        {
            _playerTeamType.OnFinishCombat();
            _enemyTeamType.OnFinishCombat();
        }

        public void OnCombatFinish(bool isPlayerWin)
        {
        }

        public void OnCombatQuit()
        {
            
        }
    }
}
