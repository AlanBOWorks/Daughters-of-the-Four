using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utils
{
    /// <summary>
    /// Pool for [<see cref="MonoBehaviour"/>]; <br></br>
    ///<br></br>
    /// CAUTION:<br></br>
    /// Requires instantiation through [<seealso cref="Awake"/>]
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class MonoObjectPoolBase<T> where T : MonoBehaviour
    {
        [Title("References")] 
        [SerializeField] private Transform instantiationParent;
        [SerializeField] private Transform onPoolParent;
        [SerializeField] private T poolElement;
        [Title("Pools")]
        [NonSerialized,DisableInEditorMode, HorizontalGroup("Pool"), ShowInInspector]
        protected Queue<T> InactivePool = new Queue<T>();

        public Transform GetInstantiationParent() => instantiationParent;

        public virtual void Awake()
        {
            poolElement.gameObject.SetActive(false);
            InactivePool = new Queue<T>();
        }

        private T PoolOrInstantiate()
        {
            if (InactivePool.Count > 0)
            {
                var element = InactivePool.Dequeue();
                element.transform.SetParent(instantiationParent);
                return element;
            }

            return Object.Instantiate(poolElement, instantiationParent);
        }

        protected T GetElement()
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

            InactivePool.Enqueue(element);
        }
    }

    /// <summary>
    /// <inheritdoc cref="MonoObjectPoolBase{T}"/>
    /// </summary>
    [Serializable]
    public abstract class MonoObjectPool<T> : MonoObjectPoolBase<T>, IMonoObjectPool<T> where T : MonoBehaviour
    {
        public T Get() => GetElement();
        

        public void Clear()
        {
            while (InactivePool.Count > 0)
            {
                var element = InactivePool.Dequeue();
                Object.Destroy(element);
            }
        }

        public int CountInactive => InactivePool.Count;
    }

    /// <summary>
    /// <inheritdoc cref="MonoObjectPoolBase{T}"/>
    /// </summary>
    [Serializable]
    public abstract class TrackedMonoObjectPool<T> : MonoObjectPoolBase<T>, IMonoObjectPool<T> where T : MonoBehaviour
    {
        [Title("Actives")]
        [NonSerialized, DisableInEditorMode,HorizontalGroup("Pool"),ShowInInspector]
        private HashSet<T> _activePool = new HashSet<T>();

        public bool IsActive() => _activePool.Count > 0;
        public IReadOnlyCollection<T> GetActiveElements() => _activePool;

        public override void Awake()
        {
            base.Awake();
            _activePool = new HashSet<T>();
        }

        public T Get()
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

        public void Clear()
        {
            while (InactivePool.Count > 0)
            {
                var element = InactivePool.Dequeue();
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

        public int CountInactive => InactivePool.Count;
        public int CountActive => _activePool.Count;
    }

    public interface IMonoObjectPool<T>
    {
        T Get();
        void Release(T element);
        void Clear();
        int CountInactive { get; }
    }
}
