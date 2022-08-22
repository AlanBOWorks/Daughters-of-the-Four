using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ExplorationSystem._Core
{
    public class UWorldExplorationHandler : MonoBehaviour
    {
        [ShowInInspector,ShowIf("_handler")]
        private WorldExplorationHandler _handler;

        private IExplorationSceneDataHolder GetCurrentScene() => _handler.GetDataHolder();

        private void Awake()
        {
            _handler = PlayerExplorationSingleton.WorldExplorationHandler;
        }

        private void OnEnable()
        {
            PlayerExplorationSingleton.EventsHolder.OnWorldSceneOpen(GetCurrentScene());
        }
    }
}
