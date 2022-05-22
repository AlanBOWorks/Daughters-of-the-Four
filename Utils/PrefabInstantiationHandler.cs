using System;
using System.Collections.Generic;
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
    }

    public class PrefabDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TValue : Object
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

        public void ClearDestroy()
        {
            foreach (var element in Values)
            {
                Object.Destroy(element);
            }
            Clear();
        }
    }
}
