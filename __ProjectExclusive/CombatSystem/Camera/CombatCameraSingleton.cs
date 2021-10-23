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
            CameraSwitchListeners = new HashSet<ICombatCameraSwitchListener>();
        }

        public static CombatCameraSingleton GetInstance() => Instance;

        private static readonly HashSet<ICombatCameraSwitchListener> CameraSwitchListeners;
        private static Camera _combatMainCamera;
        private static Camera _combatUiCamera;

        public static void SubscribeListener(ICombatCameraSwitchListener listener)
        {
            listener.OnUICameraSwitch(_combatUiCamera);
            listener.OnMainCameraSwitch(_combatMainCamera);
            CameraSwitchListeners.Add(listener);
        }

        public static void UnSubscribe(ICombatCameraSwitchListener listener) 
            => CameraSwitchListeners.Remove(listener);

        public static void SwitchCombatMainCamera(Camera camera)
        {
            _combatMainCamera = camera;
            foreach (var listener in CameraSwitchListeners)
            {
                listener.OnMainCameraSwitch(_combatMainCamera);
            }

            if (_combatUiCamera != null)
            {
                var additionalCameraData = _combatMainCamera.GetUniversalAdditionalCameraData();
                additionalCameraData.cameraStack.Add(_combatUiCamera);
            }
        }

        public static void SwitchCombatUICamera(Camera camera)
        {
            if (_combatUiCamera != null && _combatMainCamera != null)
            {
                var additionalCameraData = _combatUiCamera.GetUniversalAdditionalCameraData();
                additionalCameraData.cameraStack.Remove(_combatUiCamera);
            }

            _combatUiCamera = camera;
            foreach (var listener in CameraSwitchListeners)
            {
                listener.OnUICameraSwitch(_combatUiCamera);
            }
        }
    }
}
