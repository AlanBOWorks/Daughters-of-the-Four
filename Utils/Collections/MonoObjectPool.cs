using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Utils
{

    public static class UtilsPool
    {
        public static TElement PoolElement<TPool, TElement>(TPool pool, TElement prefab, Transform onParent)
            where TPool : IObjectPoolBasic, IObjectPool<TElement>
        where TElement : Component
        {
            if (pool.CountInactive <= 0) return Object.Instantiate(prefab, onParent);

            var element = pool.Pop();
            element.transform.SetParent(onParent);
            return element;
        }

        public static TElement PoolElement<TElement>(Queue<TElement> pool, TElement prefab, Transform onParent)
            where TElement : Component
        {
            if (pool.Count <= 0) return Object.Instantiate(prefab, onParent);

            var element = pool.Dequeue();
            element.transform.SetParent(onParent);
            return element;
        }
    }

    /// <summary>
    /// <inheritdoc cref="MonoObjectPool{T}"/>
    /// </summary>
    [Serializable]
    public abstract class MonoObjectPoolBasic<T> : IObjectPool<T>, IObjectPoolBasic where T : Component
    {
        [Title("Element")]
        [SerializeField] private T poolElement;
        [Title("Pools")]
        [SerializeField, DisableInEditorMode, HorizontalGroup("Pool"), ShowInInspector]
        protected Queue<T> inactivePool = new Queue<T>();

        public T GetCloneableElement() => poolElement;


        public T Pop()
        {
            return inactivePool.Count > 0 ? inactivePool.Dequeue() : Object.Instantiate(poolElement);
        }

        public T Pop(Transform onParent)
        {
            if (inactivePool.Count > 0)
            {
                var popElement = inactivePool.Dequeue();
                popElement.transform.SetParent(onParent);
                return poolElement;
            }
            else
                return Object.Instantiate(poolElement, onParent);
        }


        public void Release(T element)
        {
            Release(element, false);
        }

        public void Release(T element, bool active)
        {
            element.gameObject.SetActive(active);
            inactivePool.Enqueue(element);
        }


        public void Awake()
        {
            poolElement.gameObject.SetActive(false);
        }

        public void Clear()
        {
            while (inactivePool.Count > 0)
            {
                var element = inactivePool.Dequeue();
                Object.Destroy(element);
            }
        }

        public int CountInactive => inactivePool.Count;
    }

    /// <summary>
    /// Pool for [<see cref="Component"/>]; <br></br>
    ///<br></br>
    /// CAUTION:<br></br>
    /// Requires instantiation through [<seealso cref="Awake"/>]
    /// </summary>
    [Serializable]
    public abstract class MonoObjectPool<T> : IObjectPool<T>, IObjectPoolBasic where T : Component
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

        /// <summary>
        /// Does a [<see cref="Pop"/>], but with instantiation if there's no element in the pool and
        /// actives the element on pop
        /// </summary>
        /// <returns></returns>
        public virtual T PopElementSafe(bool activeOnSpawn = true)
        {
            var element = UtilsPool.PoolElement(inactivePool,poolElement,instantiationParent);
            element.gameObject.SetActive(activeOnSpawn);
            return element;
        }

        /// <summary>
        /// Pops an element from [<seealso cref="inactivePool"/>], but doesn't instantiates nor
        /// handles activation;<br></br>
        /// This is meant for manual handling, for desired behaviour use [<see cref="PopElementSafe"/>] instead.
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            return inactivePool.Dequeue();
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
    /// <inheritdoc cref="MonoObjectPool{T}"/>
    /// </summary>
    [Serializable]
    public abstract class TrackedMonoObjectPool<T> : MonoObjectPool<T>, IObjectPool<T>, IObjectPoolTracked<T> where T : Component
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

        public override T PopElementSafe(bool activeOnSpawn = true)
        {
            var element = base.PopElementSafe();
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

        public void ReturnElementsToPool()
        {
            foreach (T element in _activePool)
            {
                base.Release(element);
            }
            _activePool.Clear();
        }

        public int CountActive => _activePool.Count;
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
        void ReturnElementsToPool();
    }

    public interface IObjectPoolBasic
    {
        void Awake();
        void Clear();
        int CountInactive { get; }
    }
}
