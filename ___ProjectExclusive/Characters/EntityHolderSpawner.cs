using System.Collections.Generic;
using JetBrains.Annotations;
using PathologicalGames;
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
        private readonly SpawnPool _characterPool;

        public static string PoolKey = "CharactersPool";
        public EntityHolderSpawner()
        {
            _characterPool = PoolManager.Pools.Create(PoolKey);
        }

        //TODO make it spawn based an scene; on change scene remove from Dictionary those elements
        public UCharacterHolder SpawnEntity([NotNull]GameObject prefab)
        {
            var pooledElement = _characterPool.Spawn(prefab);
            pooledElement.gameObject.SetActive(true);

            return pooledElement.GetComponent<UCharacterHolder>();
        }

        public void DeSpawn(CombatingEntity entity)
        {
            var element = entity.Holder.transform;
            _characterPool.Despawn(element);
        }
    }
}
