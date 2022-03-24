using System;
using UnityEngine;

namespace CombatSystem.Player
{
    public class UPlayerCameraHolder : MonoBehaviour
    {
        [SerializeField] private Camera interfaceCombatCamera;

        private void Awake()
        {
            Camera injectionCamera = interfaceCombatCamera 
                ? interfaceCombatCamera 
                : Camera.main;

            PlayerCombatSingleton.InjectCombatCamera(in injectionCamera);

            Destroy(this);
        }
    }
}
