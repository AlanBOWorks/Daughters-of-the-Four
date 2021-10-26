using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace CombatCamera
{
    public sealed class CombatCameraSingleton
    {
        private static readonly CombatCameraSingleton Instance = new CombatCameraSingleton();
        static CombatCameraSingleton()
        {
           
        }

        public static CombatCameraSingleton GetInstance() => Instance;

        public static Camera CombatMainCamera { get; private set; }
        public static Camera CombatUICamera { get; private set; }


        public static void SwitchCombatMainCamera(Camera camera)
        {
            CombatMainCamera = camera;
           

            if (CombatUICamera != null)
            {
                var additionalCameraData = CombatMainCamera.GetUniversalAdditionalCameraData();
                additionalCameraData.cameraStack.Add(CombatUICamera);
            }
        }

        public static void SwitchCombatUICamera(Camera camera)
        {
            if (CombatUICamera != null && CombatMainCamera != null)
            {
                var additionalCameraData = CombatUICamera.GetUniversalAdditionalCameraData();
                additionalCameraData.cameraStack.Remove(CombatUICamera);
            }

            CombatUICamera = camera;
        }
    }
}
