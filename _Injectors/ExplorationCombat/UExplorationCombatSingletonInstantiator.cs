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
    }

    internal sealed class ExplorationCombatSingleton
    {

        static ExplorationCombatSingleton()
        {
            SceneTransitionHandler = new ExplorationCombatScenesTransitionHandler();

            // Exploration's
            var explorationEvents = ExplorationSingleton.EventsHolder;
            explorationEvents.Subscribe(SceneTransitionHandler);

            // Combat's
            var combatEvents = CombatSystemSingleton.EventsHolder;
            combatEvents.Subscribe(SceneTransitionHandler);
            Debug.Log("ExplorationCombat Singleton Instantiated");
        }

        public static void Instantiate() { }

        public static readonly ExplorationCombatScenesTransitionHandler SceneTransitionHandler;
    }
}
