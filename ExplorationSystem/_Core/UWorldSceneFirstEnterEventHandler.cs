using System;
using UnityEngine;

namespace ExplorationSystem._Core
{
    public class UWorldSceneFirstEnterEventHandler : MonoBehaviour
    {
        private void Awake()
        {
            ExplorationSingleton.EventsHolder.OnWorldSceneEnters(null);
            Destroy(this);
        }
    }
}
