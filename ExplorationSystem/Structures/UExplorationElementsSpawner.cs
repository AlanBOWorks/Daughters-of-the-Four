using System;
using System.Collections.Generic;
using Common;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace ExplorationSystem.Structures
{
    public class UExplorationElementsSpawner : MonoBehaviour, IWorldSceneChangeListener
    {
        [SerializeField]
        private PoolHolder elementsHolder = new PoolHolder();

        private void Awake()
        {
            elementsHolder.Awake();
            ExplorationSingleton.EventsHolder.Subscribe(this);
        }

        private void OnDestroy()
        {
            ExplorationSingleton.EventsHolder.UnSubscribe(this);
        }

        [Button]
        private void SpawnElements()
        {
            var themeHolder= PlayerExplorationSingleton.ExplorationThemeHolder;
            var themeEnumerable = UtilsExploration.GetEnumerable(EnumExploration.StaticEnums, themeHolder);


            int i = 0;
            foreach ((EnumExploration.ExplorationType explorationType, IThemeHolder themeElement) in themeEnumerable)
            {
                var spawnElement = elementsHolder.PopElementSafe(true);

                spawnElement.Injection(themeElement.GetThemeIcon());
                spawnElement.Injection(explorationType);

                var elementTransform = spawnElement.transform;
                var position = elementTransform.localPosition;
                position.x += i * 100 - 300;
                position.y = 0;

                elementTransform.localPosition = position;
                spawnElement.gameObject.SetActive(true);
                i++;
            }
        }
        public void OnWorldSceneEnters(IExplorationSceneDataHolder lastMap)
        {
        }

        public void OnWorldSceneSubmit(IExplorationSceneDataHolder targetMap)
        {
        }

        public void OnWorldSelectSceneLoad(IExplorationSceneDataHolder loadedMap)
        {
            SpawnElements();
        }


        [Button]
        private void DestroyInstantiatedElements()
        {
            elementsHolder.ReturnElementsToPool();
        }


        [Serializable]
        private sealed class PoolHolder : TrackedMonoObjectPool<UExplorationElementHolder>
        {
            
        }

    }
}
