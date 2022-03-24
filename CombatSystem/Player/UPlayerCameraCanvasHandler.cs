using System;
using CombatSystem.Player.Events;
using UnityEngine;

namespace CombatSystem.Player
{
    [RequireComponent(typeof(Canvas))]
    public class UPlayerCameraCanvasHandler : MonoBehaviour, ICameraHolderListener
    {
        private Canvas _canvas;
        void Start()
        {
            var playerCamera = PlayerCombatSingleton.InterfaceCombatCamera;

            _canvas = GetComponent<Canvas>();

            OnSwitchCamera(in playerCamera);
            PlayerCombatSingleton.PlayerCombatEvents.ManualSubscribe(this);
        }

        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.ManualUnSubscribe(this);
        }

        public void OnSwitchCamera(in Camera combatCamera)
        {
            _canvas.worldCamera = combatCamera;
        }
    }
}
