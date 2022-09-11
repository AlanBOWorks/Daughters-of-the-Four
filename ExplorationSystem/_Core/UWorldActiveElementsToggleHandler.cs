using System;
using UnityEngine;

namespace ExplorationSystem._Core
{
    public class UWorldActiveElementsToggleHandler : MonoBehaviour, IWorldSceneChangeListener
    {
        [SerializeField] private GameObject[] onWorldSelectedHide;
        [SerializeField] private GameObject[] onWorldMapReturnsHide;

        private void Awake()
        {
            ExplorationSingleton.EventsHolder.Subscribe(this);
        }

        private void OnDestroy()
        {
            ExplorationSingleton.EventsHolder.Subscribe(this);
        }

        private void ToggleElements(bool isLoadingAMap)
        {
            foreach (GameObject element in onWorldSelectedHide)
            {
                element.SetActive(!isLoadingAMap);
            }

            foreach (GameObject element in onWorldMapReturnsHide)
            {
                element.SetActive(isLoadingAMap);
            }
        }

        public void OnWorldSceneEnters(IExplorationSceneDataHolder lastMap)
        {
            if(lastMap == null) return;
            ToggleElements(false);
        }

        public void OnWorldSceneSubmit(IExplorationSceneDataHolder targetMap)
        {
        }

        public void OnWorldSelectSceneLoad(IExplorationSceneDataHolder loadedMap)
        {
            ToggleElements(true);
        }
    }
}
