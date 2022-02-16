using System;
using UnityEngine;

namespace CombatSystem.Player
{
    public class UPlayerCameraHolder : MonoBehaviour
    {
        [SerializeField] private Camera interfaceCombatCamera;

        private void Awake()
        {
            if (interfaceCombatCamera)
            {
                PlayerCombatSingleton.InterfaceCombatCamera = interfaceCombatCamera;
            }
            else
            {
                PlayerCombatSingleton.InterfaceCombatCamera = Camera.main;
            }
        }
    }
}
