using System.Collections.Generic;
using CombatEntity;
using CombatTeam;
using UnityEngine;

namespace CombatSystem.PositionHandlers
{
    public class PositionSpawner : ICombatPreparationListener, ICombatFinishListener
    {
        public PositionSpawner()
        {
            _removeEntitiesOnFinish = new Queue<UEntityHolder>();
        }

        private readonly Queue<UEntityHolder> _removeEntitiesOnFinish;
        private CombatingTeam _currentPlayerTeam;
        private CombatingTeam _currentEnemyTeam;

        public void OnPreparationCombat(CombatingTeam playerTeam, CombatingTeam enemyTeam)
        {
            _currentPlayerTeam = playerTeam;
            _currentEnemyTeam = enemyTeam;
        }

        public void OnAfterLoadScene()
        {
            var positions = CombatSystemSingleton.PositionProvider;
            var playerPositions = positions.GetPlayerTeam();
            var enemyPositions = positions.GetEnemyTeam();

            UtilsTeam.DoActionOnTeam(_currentPlayerTeam,playerPositions,SpawnEntity);
            UtilsTeam.DoActionOnTeam(_currentEnemyTeam,enemyPositions,SpawnEntity);


            void SpawnEntity(CombatingEntity entity, Transform onTransform)
            {
                UEntityHolder holder = entity.GetEntityPrefab();
                if (holder == null)
                {
#if UNITY_EDITOR
                    Debug.LogWarning("NULL entity Character prefab: instantiation of provisional object");
#endif

                    var provisionalPrefab = GlobalCombatParametersSingleton.ProvisionalEntityHolderPrefab;
                    holder = provisionalPrefab;
                }

                Object.Instantiate(holder);
                holder.Inject(entity);
                holder.HandleTransformSpawn(onTransform);

                _removeEntitiesOnFinish.Enqueue(holder);
            }
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
