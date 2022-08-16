using System;
using System.Collections.Generic;
using Common;
using ExplorationSystem.Elements;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace ExplorationSystem.Structures
{
    public class UExplorationThemeSpawner : MonoBehaviour
    {
        [SerializeField]
        private PrefabHolder prefabHolder = new PrefabHolder();

        [SerializeField, DisableInEditorMode,DisableInPlayMode] 
        private List<UExplorationElementHolder> instantiatedElements;

        private void Start()
        {
            if(instantiatedElements.Count > 0) return;
            DoInstantiations();
        }

        [Button]
        private void DoInstantiations()
        {
            var themeHolder= PlayerExplorationSingleton.ExplorationThemeHolder;
            var themeEnumerable = UtilsExploration.GetEnumerable(EnumExploration.StaticEnums, themeHolder);

            if (instantiatedElements.Count > 0) DestroyInstantiatedElements();

            int i = 0;
            foreach ((EnumExploration.ExplorationType explorationType, IThemeHolder themeElement) in themeEnumerable)
            {
                var spawnElement = prefabHolder.SpawnElement();
                instantiatedElements.Add(spawnElement);

                spawnElement.Injection(themeElement.GetThemeIcon());
                spawnElement.Injection(themeElement.GetThemeColor());
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

        [Button]
        private void DestroyInstantiatedElements()
        {
            foreach (var element in instantiatedElements)
            {
                DestroyImmediate(element.gameObject);
            }
            instantiatedElements.Clear();
        }


        [Serializable]
        private sealed class PrefabHolder : PrefabInstantiationHandler<UExplorationElementHolder>
        {
            
        }
    }
}
