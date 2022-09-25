using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Injectors.ExplorationCombat
{
    public class UExplorationCombatDebugger : MonoBehaviour
    {
        [SerializeField, HideInPlayMode] private bool avoidGoingToExplorationMapAfterCombat = true;

        private void Start()
        {
            ExplorationCombatSingleton.SceneTransitionHandler.IgnoreMapTransitionAfterCombat =
                avoidGoingToExplorationMapAfterCombat;
        }
    }
}
