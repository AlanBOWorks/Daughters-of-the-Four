using System;
using UnityEngine;

namespace CombatSystem.Player
{
    public class UPlayerCameraHolder : MonoBehaviour
    {
        [SerializeField] private Camera combatMainCamera;
        [SerializeField] private Camera combatBackCamera;

        private void Awake()
        {
            Camera injectionCamera = combatMainCamera 
                ? combatMainCamera
                : Camera.main;

            PlayerCombatSingleton.InjectCombatMainCamera(in injectionCamera);
            PlayerCombatSingleton.InjectCombatBackCamera(in combatBackCamera);

            Destroy(this);
        }
    }
}
