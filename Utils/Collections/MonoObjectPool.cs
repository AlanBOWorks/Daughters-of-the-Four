using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Utils
{
    /// <summary>
    /// Pool for [<see cref="Component"/>]; <br></br>
    ///<br></br>
    /// CAUTION:<br></br>
    /// Requires instantiation through [<seealso cref="Awake"/>]
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class MonoObjectPoolBase<T> : IObjectPoolBasic where T : Component
    {
        [Title("Parents")] 
        [Tooltip("After pool/instantiate element, which parent the created element should go")]
        [SerializeField] private Transform instantiationParent;
        [Tooltip("After returning the element into the Pool, which parent should return")]
        [SerializeField] private Transform onPoolParent;
        [Title("Element")] 
        [SerializeField] private T poolElement;
        [Title("Pools")]
        [SerializeField,DisableInEditorMode, HorizontalGroup("Pool"),ShowInInspector]
        protected Queue<T> inactivePool = new Queue<T>();

        public Transform GetInstantiationParent() => instantiationParent;

        public virtual void Awake()
        {
            poolElement.gameObject.SetActive(false);
            inactivePool.Clear();
        }

        public virtual void Clear()
        {
            while (inactivePool.Count > 0)
            {
                var element = inactivePool.Dequeue();
                Object.Destroy(element);
            }
        }

        public int CountInactive => inactivePool.Count;

        private T PoolOrInstantiate()
        {
            if (inactivePool.Count > 0)
            {
                var element = inactivePool.Dequeue();
                element.transform.SetParent(instantiationParent);
                return element;
            }

            return Object.Instantiate(poolElement, instantiationParent);
        }

        protected virtual T GetElement()
        {
            var element = PoolOrInstantiate();
            element.gameObject.SetActive(true);
            return element;
        }

        public virtual void Release(T element)
        {
            var elementGameObject = element.gameObject;
            elementGameObject.SetActive(false);
            elementGameObject.transform.SetParent(onPoolParent);

            inactivePool.Enqueue(element);
        }
    }

    /// <summary>
    /// <inheritdoc cref="MonoObjectPoolBase{T}"/>
    /// </summary>
    [Serializable]
    public abstract class MonoObjectPool<T> : MonoObjectPoolBase<T>, IObjectPool<T> where T : Component
    {
        public T Pop() => GetElement();
    }

    /// <summary>
    /// <inheritdoc cref="MonoObjectPoolBase{T}"/>
    /// </summary>
    [Serializable]
    public abstract class TrackedMonoObjectPool<T> : MonoObjectPoolBase<T>, IObjectPool<T>, IObjectPoolTracked<T> where T : Component
    {
        [Title("Actives")]
        [NonSerialized, DisableInEditorMode,HorizontalGroup("Pool"),ShowInInspector]
        private HashSet<T> _activePool = new HashSet<T>();

        public bool IsActive() => _activePool.Count > 0;
        public IEnumerable<T> GetActiveElements() => _activePool;

        public override void Awake()
        {
            base.Awake();
            _activePool = new HashSet<T>();
        }

        public T Pop()
        {
            var element = GetElement();
            _activePool.Add(element);

            return element;
        }

        public override void Release(T element)
        {
            base.Release(element);
            _activePool.Remove(element);
        }

        public override void Clear()
        {
            while (inactivePool.Count > 0)
            {
                var element = inactivePool.Dequeue();
                Object.Destroy(element);
            }

            foreach (T element in _activePool)
            {
                Object.Destroy(element);
            }
            _activePool.Clear();
        }

        public void ReturnToElementsToPool()
        {
            foreach (T element in _activePool)
            {
                base.Release(element);
            }
            _activePool.Clear();
        }

        public int CountActive => _activePool.Count;
    }

    public abstract class DictionaryMonoObjectPool<TKey,TValue> : 
        IDictionaryObjectPool<TKey, TValue>, 
        IObjectPoolTracked<TValue> where TValue : Component
    {
        [SerializeField]
        private Pool pool = new Pool();
        private Dictionary<TKey, TValue> _activePool;
        public IReadOnlyDictionary<TKey, TValue> GetDictionary() => _activePool;


        public bool IsActive() => _activePool.Count > 0;
        public int CountActive => _activePool.Count;

        public IEnumerable<TValue> GetActiveElements()
        {
            foreach (var pair in _activePool)
            {
                yield return pair.Value;
            }
        }

        public void ReturnToElementsToPool()
        {
            foreach ((TKey key, TValue element) in _activePool)
            {
                Release(key,element);
            }
            _activePool.Clear();
        }

        public void Awake()
        {
            _activePool = new Dictionary<TKey, TValue>();
            pool.Awake();
        }

        public TValue SafeGetActive(TKey key) 
            => (_activePool.ContainsKey(key)) ? _activePool[key] : null;

        public TValue Pop(TKey key)
        {
            var element = pool.Pop();
            _activePool.Add(key,element);
            return element;
        }

        public void Release(TKey key, TValue element)
        {
            if(!_activePool.ContainsKey(key)) return;

            pool.Release(element);
            _activePool.Remove(key);
        }

        public void Clear()
        {
            _activePool.Clear();
            pool.Clear();
        }

        public int CountInactive => pool.CountInactive;

        [Serializable]
        private sealed class Pool : MonoObjectPool<TValue>
        {
           
        }

    }

    public interface IObjectPool<T>
    {
        T Pop();
        void Release(T element);
    }

    public interface IDictionaryObjectPool<in TKey,TValue>
    {
        TValue Pop(TKey key);
        void Release(TKey key, TValue element);
    }

    public interface IObjectPoolTracked<out T> : IObjectPoolBasic
    {
        bool IsActive();
        int CountActive { get; }
        IEnumerable<T> GetActiveElements();
        void ReturnToElementsToPool();
    }

    public interface IObjectPoolBasic
    {
        void Awake();
        void Clear();
        int CountInactive { get; }
    }
}
