using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

namespace CombatSystem.Team
{
    /// <summary>
    /// Handles instantiations elements of [T] for [<see cref="CombatTeam"/>]0s members
    /// </summary>
    public abstract class TeamMembersTypeSpawner<T> where T : Object
    {
        [SerializeField]
        protected PrefabHolder prefabHolder = new PrefabHolder();
        [SerializeField] 
        private bool doPooling = true;


        private IEnumerable<CombatEntity> _entities;
        [ShowInInspector, DisableInEditorMode]
        private Queue<T> _activeElements;
        [ShowInInspector,DisableInEditorMode]
        private Stack<T> _pool;

        public bool IsValid() => prefabHolder.IsValid();


        public T GetPrefab() => prefabHolder.GetPrefab();
        private void Awake()
        {
            if(_activeElements != null) return;

            _activeElements = new Queue<T>();
            if(doPooling)
                _pool = new Stack<T>();
        }


        public void OnCombatPrepares(IEnumerable<CombatEntity> members, Action<KeyValuePair<CombatEntity,T>> onCreationCallback)
        {
            Awake();
            _entities = members;
            DoInstantiation(onCreationCallback);
        }

        public void OnCombatFinish(Action<T> onDisableCallback)
        {
            while (_activeElements.Count > 0)
            {
                var element = _activeElements.Dequeue();
                if (doPooling)
                {
                    _pool.Push(element);
                    onDisableCallback?.Invoke(element);
                }
                else
                    Object.Destroy(element);
            }
        }

        private void DoInstantiation(Action<KeyValuePair<CombatEntity, T>> onCreationCallback)
        {
            foreach (var entity in _entities)
            {
                var element = PoolOrInstantiate();
                _activeElements.Enqueue(element);

                OnInstantiationElement(element,entity, _activeElements.Count);
                onCreationCallback?.Invoke(new KeyValuePair<CombatEntity, T>(entity, element));
            }


            T PoolOrInstantiate()
            {
                return _pool != null && _pool.Count > 0
                    ? _pool.Pop()
                    : prefabHolder.SpawnElement();
            }
        }


        protected abstract void OnInstantiationElement(T element, CombatEntity entity, int count);



        [Serializable]
        protected sealed class PrefabHolder : PrefabInstantiationHandler<T>
        {
            
        }
    }
}
