using UnityEngine;

namespace CombatCamera
{
    [RequireComponent(typeof(Canvas))]
    public class UCanvasCameraInjector : MonoBehaviour
    {
        private void OnEnable()
        {
            var injectionOnCanvas = GetComponent<Canvas>();
            injectionOnCanvas.worldCamera = CombatCameraSingleton.CombatMainCamera;
        }

    }
}
