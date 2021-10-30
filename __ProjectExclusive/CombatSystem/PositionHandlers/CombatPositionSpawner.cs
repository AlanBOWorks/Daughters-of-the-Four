using System.Collections.Generic;
using CombatEntity;
using CombatTeam;
using UnityEngine;

namespace CombatSystem.PositionHandlers
{
    public class CombatPositionSpawner : ICombatPreparationListener, ICombatFinishListener
    {
        public CombatPositionSpawner()
        {
            _removeEntitiesOnFinish = new Queue<UEntityHolder>();
        }

        private readonly Queue<UEntityHolder> _removeEntitiesOnFinish;
        private CombatingTeam _currentPlayerTeam;
        private CombatingTeam _currentEnemyTeam;


        private void SpawnEntities()
        {
            var positions = CombatSystemSingleton.PositionProvider;
            if(positions == null) return;

            var playerPositions = positions.GetPlayerTeam();
            var enemyPositions = positions.GetEnemyTeam();

            UtilsTeam.DoActionOnTeam(_currentPlayerTeam, playerPositions, SpawnEntity);
            UtilsTeam.DoActionOnTeam(_currentEnemyTeam, enemyPositions, SpawnEntity);


            void SpawnEntity(CombatingEntity entity, Transform onTransform)
            {
                if (entity == null || onTransform == null)
                {
                    Debug.LogWarning("Entity and/or combatPosition[Transform] were null. Ignoring spawn of character");
                    return;
                }

                UEntityHolder holder = entity.GetEntityPrefab();
                if (holder == null)
                {
#if UNITY_EDITOR
                    Debug.LogWarning("NULL entity Character prefab: instantiation of provisional object");
#endif

                    var provisionalPrefab = GlobalCombatParametersSingleton.ProvisionalEntityHolderPrefab;
                    holder = provisionalPrefab;
                }

                var instantiatedObject = Object.Instantiate(holder);
                instantiatedObject.Inject(entity);
                instantiatedObject.HandleTransformSpawn(onTransform);

                entity.Injection(instantiatedObject);

                _removeEntitiesOnFinish.Enqueue(instantiatedObject);
            }
        }

        public void OnPreparationCombat(CombatingTeam playerTeam, CombatingTeam enemyTeam)
        {
            _currentPlayerTeam = playerTeam;
            _currentEnemyTeam = enemyTeam;
        }

        public void OnAfterLoads()
        {
            SpawnEntities();
        }


        public void OnFinish(CombatingTeam wonTeam)
        {
            while (_removeEntitiesOnFinish.Count > 0)
            {
                var entityHolder = _removeEntitiesOnFinish.Dequeue();
                Object.Destroy(entityHolder);
            }

            _currentPlayerTeam = null;
            _currentEnemyTeam = null;
        }



    }
}
