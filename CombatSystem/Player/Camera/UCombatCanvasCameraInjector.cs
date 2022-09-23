using System;
using CombatSystem.Player.Events;
using UnityEngine;

namespace CombatSystem.Player
{
    public class UCombatCanvasCameraInjector : MonoBehaviour, ICameraHolderListener, IPlayerCameraStructureRead<Canvas[]>
    {
        [SerializeReference] private Canvas[] mainCameraCanvases;
        [SerializeReference] private Canvas[] backCameraCanvases;
        [SerializeReference] private Canvas[] frontCameraCanvases;


        public Canvas[] MainCameraType => mainCameraCanvases;
        public Canvas[] BackCameraType => backCameraCanvases;
        public Canvas[] CharacterBackCameraType => null;
        public Canvas[] FrontCameraType => frontCameraCanvases;
        public Canvas[] CharacterFrontCameraType => null;

        private void Start()
        {
            DoInjection(CombatCameraHandler.MainCamera);
            DoInjection(CombatCameraHandler.BackFrontCameras);
            PlayerCombatSingleton.CameraEvents.Subscribe(this);
        }

        private void OnDestroy()
        {
            PlayerCombatSingleton.CameraEvents.UnSubscribe(this);
        }

        private void DoInjection(ICombatBackFrontCamerasStructureRead<Camera> cameras)
        {
            Injection(backCameraCanvases,cameras.BackCameraType);
            Injection(frontCameraCanvases,cameras.FrontCameraType);
        }

        private void DoInjection(Camera mainCamera)
        {
            Injection(mainCameraCanvases, mainCamera);
        }

        private static void Injection(Canvas[] canvases, Camera backCamera)
        {
            if(canvases == null) return;

            foreach (var canvas in canvases)
            {
                canvas.worldCamera = backCamera;
            }
        }

        public void OnSwitchMainCamera(Camera combatCamera)
        {
            Injection(mainCameraCanvases,combatCamera);

        }

        public void OnSwitchBackCamera(Camera combatBackCamera)
        {
            Injection(backCameraCanvases,combatBackCamera);
        }

        public void OnSwitchFrontCamera(Camera combatFrontCamera)
        {
            Injection(frontCameraCanvases,combatFrontCamera);
        }
    }
}
