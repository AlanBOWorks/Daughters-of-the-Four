using System;
using CombatSystem._Core;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CombatSystem.Entity
{
    public sealed class EntityPrefabsPoolHandler : IOppositionTeamStructureRead<ITeamFullStructureRead<Transform>>,
        ICombatTerminationListener
    {
        [ShowInInspector,HorizontalGroup()]
        private readonly PrefabsHolder _playerTeamType;
        [ShowInInspector,HorizontalGroup()]
        private readonly PrefabsHolder _enemyTeamType;

        public EntityPrefabsPoolHandler()
        {
            _playerTeamType = new PlayerPrefabsHolder();
            _enemyTeamType = new PrefabsHolder();
        }

        public ITeamFullStructureRead<Transform> PlayerTeamType => _playerTeamType;
        public ITeamFullStructureRead<Transform> EnemyTeamType => _enemyTeamType;

        public ITeamFullStructureRead<Transform> PlayerOnNullPositionReference;
        public ITeamFullStructureRead<Transform> EnemyOnNullPositionReference;

        public void HandleTeams(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            var playerPositions = CombatSystemSingleton.PlayerPositionTransformReferences 
                ? CombatSystemSingleton.PlayerPositionTransformReferences 
                : PlayerOnNullPositionReference;
            _playerTeamType.PoolMembers(playerTeam, playerPositions);

            var enemyPositions = CombatSystemSingleton.EnemyPositionTransformReferences 
                ? CombatSystemSingleton.EnemyPositionTransformReferences 
                : EnemyOnNullPositionReference;
            _enemyTeamType.PoolMembers(enemyTeam, enemyPositions);
        }


        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            // The instantiation is made by CombatInitializationHandler.InitiateModels
        }

        public void OnCombatStart()
        {
        }

        public void OnCombatFinish(UtilsCombatFinish.FinishType finishType)
        {
        }

        public void OnCombatFinishHide(UtilsCombatFinish.FinishType finishType)
        {
            _playerTeamType.OnFinishCombat();
            _enemyTeamType.OnFinishCombat();
        }



        private sealed class PlayerPrefabsHolder : PrefabsHolder
        {
            private bool _instantiated;

            public override void PoolMembers(CombatTeam team, ITeamFullStructureRead<Transform> positions)
            {
                if(!_instantiated)
                {
                    base.PoolMembers(team, positions);
                    _instantiated = true;
                }
                else
                {
                    Pool(team);
                }
            }

            private void Pool(CombatTeam team)
            {
                
                var members = team.GetAllEntities();
                var keyValuePairs = UtilsTeam.GetEnumerable(members, this);
                int index = 0;
                foreach ((CombatEntity entity, Transform entityTransform) in keyValuePairs)
                {
                    if(entity == null) continue;

                    var entityGO = entityTransform.gameObject;

                    UtilsEntity.HandleInjections(in entity, in entityGO);
                    entityGO.SetActive(true);

                    var body = entity.Body;
                    body.Injection(in entity);
                    body.InjectPositionReference(entityTransform);
                    index++;
                }
            }

            public override void OnFinishCombat()
            {
                Hide();
            }

            private void Hide()
            {
                var allMembers = UtilsTeam.GetEnumerable(this as ITeamFullStructureRead<Transform>);
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

            public virtual void PoolMembers(CombatTeam team, ITeamFullStructureRead<Transform> positions)
            {
                var members = team.GetAllEntities();
                var keyValuePairs = UtilsTeam.GetEnumerable(members, positions);
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

                    entityGameObject.layer = CombatRenderLayers.CombatCharacterBackIndex;
                    entityTransform.position = positionTransform.position;
                    entityTransform.rotation = positionTransform.rotation;

                    var entityBody = entity.Body;
                    entityBody.InjectPositionReference(entityTransform);
                    entityBody.GetAnimator().Injection(entity);

                    UtilsTeam.SetElement(i,this, entityTransform);

                }
            }

            public virtual void OnFinishCombat()
            {
                Destroy();
            }

            protected void Destroy()
            {
                var allMembers = UtilsTeam.GetEnumerable(this as ITeamFullStructureRead<Transform>);
                foreach (var member in allMembers)
                {
                    if(!member) continue;
                    Object.Destroy(member.gameObject);
                }
            }
        }

    }
}
