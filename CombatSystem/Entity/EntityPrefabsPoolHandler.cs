using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CombatSystem.Entity
{
    public sealed class EntityPrefabsPoolHandler : ICombatTerminationListener
    {
        [Title("Player")]
        [ShowInInspector,HorizontalGroup()]
        private readonly Dictionary<ICombatEntityProvider, GameObject> _playerPrefabs;
        [ShowInInspector,HorizontalGroup()]
        private readonly Queue<GameObject> _playerActiveMembers;
        [Title("Enemy")]
        [ShowInInspector, HorizontalGroup()]
        private readonly Queue<GameObject> _enemyEntitiesObjects;

        public EntityPrefabsPoolHandler()
        {
            _playerPrefabs = new Dictionary<ICombatEntityProvider, GameObject>();
            _playerActiveMembers = new Queue<GameObject>();
            _enemyEntitiesObjects = new Queue<GameObject>();
        }


        public ITeamFullStructureRead<Transform> PlayerOnNullPositionReference;
        public ITeamFullStructureRead<Transform> EnemyOnNullPositionReference;

        public void HandleTeams(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            var playerPositions = CombatSystemSingleton.PlayerPositionTransformReferences 
                ? CombatSystemSingleton.PlayerPositionTransformReferences 
                : PlayerOnNullPositionReference;

            var enemyPositions = CombatSystemSingleton.EnemyPositionTransformReferences 
                ? CombatSystemSingleton.EnemyPositionTransformReferences 
                : EnemyOnNullPositionReference;

            HandlePlayerEntities(playerTeam, playerPositions);
            HandleEnemyEntities(enemyTeam,enemyPositions);
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
           HidePlayerEntities();
           ClearEnemies();
        }

        private void HandlePlayerEntities(CombatTeam team, ITeamFullStructureRead<Transform> positions)
        {
            foreach (var entity in team.GetAllMembers())
            {
                var entityProvider = entity.Provider;
                var providerPrefab = entityProvider.GetVisualPrefab();
                GameObject entityGameObject;
                if (_playerPrefabs.ContainsKey(entityProvider))
                {
                    entityGameObject = _playerPrefabs[entityProvider];
                    entityGameObject.SetActive(true);
                    UtilsEntity.HandleInjections(entity, entityGameObject);
                }
                else
                {
                    entityGameObject = UtilsEntity.InstantiateProviderBody(entity);
                    _playerPrefabs.Add(entityProvider, entityGameObject);
                    Object.DontDestroyOnLoad(entityGameObject);
                }
                HandleEntity(entity, entityGameObject, positions);
                _playerActiveMembers.Enqueue(entityGameObject);
            }
        }

        private void HandleEnemyEntities(CombatTeam team, ITeamFullStructureRead<Transform> positions)
        {
            foreach (var entity in team.GetAllMembers())
            {
                var entityGameObject = UtilsEntity.InstantiateProviderBody(entity);
                HandleEntity(entity,entityGameObject, positions);
                _enemyEntitiesObjects.Enqueue(entityGameObject);
            }
        }

        private static void HandleEntity(CombatEntity entity, GameObject entityGameObject, ITeamFullStructureRead<Transform> positions)
        {
            var positionTransform = UtilsTeam.GetElement(entity.ActiveRole, positions);
            HandleEntity(entity, entityGameObject, positionTransform);
        }
        private static void HandleEntity(CombatEntity entity, GameObject entityGameObject, Transform positionTransform)
        {
            var entityTransform = entityGameObject.transform;

            entityGameObject.layer = CombatRenderLayers.CombatCharacterBackIndex;
            entityTransform.position = positionTransform.position;
            entityTransform.rotation = positionTransform.rotation;

            var entityBody = entity.Body;
            entityBody.InjectPositionReference(entityTransform);
            entityBody.GetAnimator().Injection(entity);
        }

        private void HidePlayerEntities()
        {
            while (_playerActiveMembers.Count > 0)
            {
                var entityObject = _playerActiveMembers.Dequeue();
                entityObject.gameObject.SetActive(false);
            }
        }
        private void ClearEnemies()
        {
            while (_enemyEntitiesObjects.Count > 0)
            {
                var entityObject = _enemyEntitiesObjects.Dequeue();
                Object.Destroy(entityObject);
            }
        }

    }
}
