using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utils
{

    [Serializable]
    public class PrefabInstantiationHandler<TValue>
        where TValue : Object
    {
        [SerializeField] private TValue prefab;
        [SerializeField] private Transform instantiationParent;

        public TValue SpawnElement()
        {
            TValue element = Object.Instantiate(prefab, instantiationParent);
            return element;
        }
        public Transform GetInstantiationParent() => instantiationParent;

        public bool IsValid() => prefab;

        public TValue GetPrefab() => prefab;

        public static void TryHide<T>(PrefabInstantiationHandler<T> handler) where T : MonoBehaviour
        {
            handler.prefab.gameObject.SetActive(false);
        }
    }

    [Serializable]
    public class PrefabInstantiationHandlerPool<TValue> : PrefabInstantiationHandler<TValue>
        where TValue : Object
    {
        [SerializeField]
        private bool doPooling = true;

        [ShowInInspector,HorizontalGroup(), DisableInEditorMode]
        private Queue<TValue> _activeElements;
        [ShowInInspector, HorizontalGroup(), DisableInEditorMode]
        private Stack<TValue> _pool;

        public IReadOnlyCollection<TValue> GetActiveElements() => _activeElements;

        public void Instantiations()
        {
            if (_activeElements != null) return;

            _activeElements = new Queue<TValue>();
            if (doPooling)
                _pool = new Stack<TValue>();
        }

        public TValue GetElement()
        {
            var element = (_pool != null && _pool.Count > 0)
                ? _pool.Pop()
                : SpawnElement();

            OnSpawnElement(element);
            return element;
        }

        /// <summary>
        /// Callback when the element is instantiated or pooled; <br></br>
        /// The element is added to [<see cref="_activeElements"/>] inside here
        /// </summary>
        protected virtual void OnSpawnElement(TValue element)
        {
            _activeElements.Enqueue(element);
        }

        public void PushElement(TValue element)
        {
            if(_pool != null) _pool.Push(element);
            else Object.Destroy(element);
        }

        public void DoPopKeys<TKeys>(IEnumerable<TKeys> keys, Action<TKeys, TValue> onCreationCallback)
        {
            foreach (var key in keys)
            {
                var element = GetElement();
                onCreationCallback?.Invoke(key,element);
            }
        }

        public void ReturnActives(Action<TValue> onDisableCallback)
        {
            while (_activeElements.Count > 0)
            {
                var element = _activeElements.Dequeue();
                PushElement(element);
                onDisableCallback?.Invoke(element);
            }
        }

    }

    public class PrefabDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TValue : Component
    {
        public void SafeRemoveKey(TKey key)
        {
            if(!ContainsKey(key)) return;
            RemoveAndDestroy(key);
        }
        public void RemoveAndDestroy(TKey key)
        {
            var element = this[key];
            Object.Destroy(element);
        }

        public void ClearDestroyGameObjects()
        {
            foreach (var element in Values)
            {
                string log = element.name;
                Object.Destroy(element.gameObject);
                if (!element)
                    log += "removed";
                else
                {
                    log += "Cant Removed";
                }
                Debug.Log(log);
            }
            Clear();
        }
    }
}
