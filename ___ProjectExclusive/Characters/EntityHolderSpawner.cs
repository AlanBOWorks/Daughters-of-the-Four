using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

//TODO make it Character
namespace Characters
{
    /// <summary>
    /// It spawns the <seealso cref="CombatingEntity.InstantiationPrefab"/> and Pools then if required.
    /// Will return <seealso cref="UCharacterHolder"/> for references purposes.
    /// </summary>
    public class EntityHolderSpawner
    {
        private readonly Dictionary<GameObject, SpawnPool> _prefabPools;

        private const int PredictedAmountOfEntities = 16;
        public EntityHolderSpawner()
        {
            _prefabPools = new Dictionary<GameObject, SpawnPool>(PredictedAmountOfEntities);
        }

        //TODO make it spawn based an scene; on change scene remove from Dictionary those elements
        public UCharacterHolder SpawnEntity([NotNull]GameObject prefab)
        {
            bool canPool = CanPool(prefab);
            UCharacterHolder spawn = canPool 
                ? _prefabPools[prefab].Dequeue() 
                : InstantiateCharacter(prefab);

            spawn.gameObject.SetActive(true);
            return spawn;
        }

        public void DeSpawn(CombatingEntity entity)
        {
            SpawnPool pool =
                _prefabPools[entity.InstantiationPrefab];

            UCharacterHolder holder = entity.Holder;
            holder.gameObject.SetActive(false);
            pool.Enqueue(holder);
        }


        private bool CanPool(GameObject prefab)
        {
            return _prefabPools.ContainsKey(prefab) && _prefabPools[prefab].Count > 0;
        }

        private UCharacterHolder InstantiateCharacter(GameObject prefab)
        {
            UCharacterHolder holder = 
                Object.Instantiate(prefab).GetComponent<UCharacterHolder>();
            
            AddToDictionary(prefab,holder);
            return holder;
        }

        private void AddToDictionary(GameObject prefab, UCharacterHolder holder)
        {
            SpawnPool pool;
            if (_prefabPools.ContainsKey(prefab))
            {
                pool = _prefabPools[prefab];
            }
            else
            {
                pool = new SpawnPool();
                _prefabPools[prefab] = pool;
            }
            pool.Enqueue(holder);
        }

        internal class SpawnPool : Queue<UCharacterHolder> {}
    }
}
