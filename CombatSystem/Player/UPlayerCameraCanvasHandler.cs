using System;
using CombatSystem.Player.Events;
using UnityEngine;

namespace CombatSystem.Player
{
    public class UPlayerCameraCanvasHandler : MonoBehaviour, ICameraHolderListener
    {
        [SerializeField]
        private Canvas mainCanvas;


        void Start()
        {
            var playerCamera = PlayerCombatSingleton.CombatMainCamera;

            mainCanvas = GetComponent<Canvas>();

            OnSwitchMainCamera(in playerCamera);
            PlayerCombatSingleton.PlayerCombatEvents.ManualSubscribe(this);
        }

        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.UnSubscribe(this);
        }

        public void OnSwitchMainCamera(in Camera combatCamera)
        {
            mainCanvas.worldCamera = combatCamera;
        }

        public void OnSwitchBackCamera(in Camera combatBackCamera)
        {
            throw new NotImplementedException();
        }
    }
}
