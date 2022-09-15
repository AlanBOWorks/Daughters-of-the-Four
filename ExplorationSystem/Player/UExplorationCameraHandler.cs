using UnityEngine;

namespace ExplorationSystem.Player
{
    public class UExplorationCameraHandler : MonoBehaviour, IExplorationOnCombatListener
    {
        [SerializeField] private Camera explorationCamera;


        public void OnExplorationCombatLoadFinish(EnumExploration.ExplorationType type)
        {
            explorationCamera.enabled = false;
        }

        public void OnExplorationReturnFromCombat(EnumExploration.ExplorationType fromCombatType)
        {
            explorationCamera.enabled = true;
        }
    }
}
