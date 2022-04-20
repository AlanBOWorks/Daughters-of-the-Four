using System;
using UnityEngine;

namespace CombatSystem.Player
{
    public class UCombatCameraCanvasHandler : MonoBehaviour
    {
        [SerializeField] private Canvas worldTypeCanvas;

        // Start is called before the first frame update
        void Start()
        {
            UpdateCanvasCamera();
        }

        private void OnEnable()
        {
            UpdateCanvasCamera();
        }

        private void UpdateCanvasCamera()
        {
            var playerCamera = Camera.main;
            worldTypeCanvas.worldCamera = playerCamera;
        }
    }
}
