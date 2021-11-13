using System;
using System.Collections.Generic;
using CombatEntity;
using CombatSystem;
using UnityEngine;

namespace CombatTeam
{
    public abstract class UPersistentTeamStructurePoolerBase<T> : MonoBehaviour, ICombatTeamsStructureRead<T>,
        ICombatPreparationListener, ICombatDisruptionListener
    where T : UnityEngine.Object
    {
        protected virtual void Awake()
        {
            _playerElementsPool = new PoolerHandler();
            _enemyElementsPool = new PoolerHandler();

            var preparationHandler = CombatSystemSingleton.CombatPreparationHandler;
            preparationHandler.Subscribe(this as ICombatDisruptionListener);
            preparationHandler.Subscribe(this as ICombatPreparationListener);
        }

        [SerializeField] private T playerPrefab;
        [SerializeField] private T enemyPrefab;

        private PoolerHandler _playerElementsPool;
        private PoolerHandler _enemyElementsPool;
        public T GetPlayerTeam() => PoolElement(_playerElementsPool, playerPrefab);
        public T GetEnemyTeam() => PoolElement(_enemyElementsPool, enemyPrefab);

        private T PoolElement(PoolerHandler pooler, T prefab)
        {
            T pooledElement = pooler.PoolElement(prefab, transform);
            OnPoolElement(ref pooledElement);
            return pooledElement;
        }
        protected abstract void OnPoolElement(ref T instantiatedElement);
        protected abstract void OnPreparationEntity(CombatingEntity entity, T element);
        public virtual void OnPreparationCombat(CombatingTeam playerTeam, CombatingTeam enemyTeam)
        {
            DoPreparationPool(playerTeam,_playerElementsPool,playerPrefab);
            DoPreparationPool(enemyTeam,_enemyElementsPool,enemyPrefab);

            void DoPreparationPool(CombatingTeam team, PoolerHandler pooler, T prefab)
            {
                foreach (var member in team)
                {
                    var pooledElement = PoolElement(pooler, prefab);
                    OnPreparationEntity(member,pooledElement);
                }
            }
        }

        public abstract void OnAfterLoadsCombat();
        public abstract void OnCombatPause();
        public abstract void OnCombatResume();

        public virtual void OnCombatExit()
        {
            _playerElementsPool.ResetPools();
            _enemyElementsPool.ResetPools();
        }



        private sealed class PoolerHandler
        {
            public PoolerHandler()
            {
                _elementsPool = new Queue<T>();
                _trackPooledElement = new Queue<T>();
            }

            private readonly Queue<T> _elementsPool;
            private readonly Queue<T> _trackPooledElement;

            internal T PoolElement(T prefab, Transform parent)
            {
                T pooledElement;
                if (_elementsPool.Count > 0)
                    pooledElement = _elementsPool.Dequeue();
                else
                {
                    T instantiateClonePrefab = Instantiate(prefab, parent);
                    pooledElement = instantiateClonePrefab;
                }

                _trackPooledElement.Enqueue(pooledElement);
                return pooledElement;
            }

            internal void ResetPools()
            {
                while (_trackPooledElement.Count > 0)
                {
                    _elementsPool.Enqueue(_trackPooledElement.Dequeue());
                }
            }
        }

    }
}
