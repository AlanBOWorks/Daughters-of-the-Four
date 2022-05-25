using System;
using CombatSystem.Player.Events;
using UnityEngine;

namespace CombatSystem.Player
{
    public class UCombatCamerasInjector : MonoBehaviour, ICameraHolderListener, IPlayerCameraStructureRead<Canvas[]>
    {
        [SerializeReference] private Canvas[] mainCameraCanvases;
        [SerializeReference] private Canvas[] backCameraCanvases;
        [SerializeReference] private Canvas[] frontCameraCanvases;


        public Canvas[] GetMainCameraType => mainCameraCanvases;
        public Canvas[] GetBackCameraType => backCameraCanvases;
        public Canvas[] GetFrontCameraType => frontCameraCanvases;
        public Canvas[] GetCharacterCameraType => null;

        private void Start()
        {
            DoInjection(PlayerCombatSingleton.CamerasHolder);
            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);
        }

        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.UnSubscribe(this);
        }

        private void DoInjection(IPlayerCameraStructureRead<Camera> cameras)
        {
            Injection(mainCameraCanvases,cameras.GetMainCameraType);
            Injection(backCameraCanvases,cameras.GetBackCameraType);
            Injection(frontCameraCanvases,cameras.GetFrontCameraType);
        }

        private static void Injection(Canvas[] canvases, Camera backCamera)
        {
            if(canvases == null) return;

            foreach (var canvas in canvases)
            {
                canvas.worldCamera = backCamera;
            }
        }

        public void OnSwitchMainCamera(in Camera combatCamera)
        {
            Injection(mainCameraCanvases,combatCamera);

        }

        public void OnSwitchBackCamera(in Camera combatBackCamera)
        {
            Injection(backCameraCanvases,combatBackCamera);
        }

        public void OnSwitchFrontCamera(in Camera combatFrontCamera)
        {
            Injection(frontCameraCanvases,combatFrontCamera);
        }
    }
}
