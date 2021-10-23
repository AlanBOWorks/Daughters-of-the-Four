using System;
using UnityEngine;

namespace CombatCamera
{
    public class UCombatMainCameraInjector : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        private void Awake()
        {
            CombatCameraSingleton.SwitchCombatMainCamera(mainCamera);
        }
    }
}
