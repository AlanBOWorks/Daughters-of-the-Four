using UnityEngine;

namespace CombatCamera
{
    public interface ICombatCameraSwitchListener
    {
        void OnMainCameraSwitch(Camera mainCamera);
        void OnUICameraSwitch(Camera uiCamera);
    }
}
