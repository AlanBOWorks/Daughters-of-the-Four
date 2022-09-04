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
            _handler = ExplorationSingleton.WorldExplorationHandler;
            ExplorationSingleton.EventsHolder.Subscribe(this);
        }
        private void OnDestroy()
        {
            ExplorationSingleton.EventsHolder.UnSubscribe(this);
        }


        private void OnEnable()
        {
            ExplorationSingleton.EventsHolder.OnWorldSceneOpen(GetCurrentScene());
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
            ExplorationSingleton.EventsHolder.OnSceneChange(targetMap);
        }
    }
}
