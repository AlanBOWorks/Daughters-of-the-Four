using System;
using CombatSystem._Core;
using ExplorationSystem;
using UnityEngine;

namespace _Injectors.ExplorationCombat
{
    public class UExplorationCombatSingletonInstantiator : MonoBehaviour
    {
        private void Awake()
        {
            ExplorationCombatSingleton.Instantiate();
            Destroy(this);
        }

        private sealed class ExplorationCombatSingleton
        {
            static ExplorationCombatSingleton()
            {
                var sceneTransitionHandler = new ExplorationCombatScenesTransitionHandler();

                // Exploration's
                var explorationEvents = ExplorationSingleton.EventsHolder;
                explorationEvents.Subscribe(sceneTransitionHandler);

                // Combat's
                var combatEvents = CombatSystemSingleton.EventsHolder;
                combatEvents.Subscribe(sceneTransitionHandler);
                Debug.Log("ExplorationCombat Singleton Instantiated");
            }

            public static void Instantiate() { }
        }
    }

    
}
