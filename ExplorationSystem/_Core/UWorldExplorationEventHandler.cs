using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ExplorationSystem._Core
{
    public class UWorldExplorationEventHandler : MonoBehaviour, IWorldSceneListener
    {
        [ShowInInspector,ShowIf("_handler")]
        private WorldExplorationHandler _handler;

        private IExplorationSceneDataHolder GetCurrentScene() => _handler.GetDataHolder();


        private void Awake()
        {
            _handler = PlayerExplorationSingleton.WorldExplorationHandler;
            PlayerExplorationSingleton.EventsHolder.Subscribe(this);
        }
        private void OnDestroy()
        {
            PlayerExplorationSingleton.EventsHolder.UnSubscribe(this);
        }


        private void OnEnable()
        {
            PlayerExplorationSingleton.EventsHolder.OnWorldSceneOpen(GetCurrentScene());
        }


        [SerializeField] private GameObject hideOnGameWorldUnloaded;
        public void OnWorldSceneOpen(IExplorationSceneDataHolder lastMap)
        {
            hideOnGameWorldUnloaded.SetActive(true);
        }

        public void OnWorldMapClose(IExplorationSceneDataHolder targetMap)
        {
            _handler.OnWorldMapClose(targetMap);
            hideOnGameWorldUnloaded.SetActive(false);
            PlayerExplorationSingleton.EventsHolder.OnSceneChange(targetMap);
        }
    }
}
