using System;
using CombatSystem.Player.Events;
using UnityEngine;

namespace CombatSystem.Player
{
    public class UCombatBackCameraInjector : MonoBehaviour, ICameraHolderListener
    {
        [SerializeField] private Canvas canvas;

        private void Start()
        {
            Injection(PlayerCombatSingleton.BackLayerCamera);
            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);
        }

        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.UnSubscribe(this);
        }


        private void Injection(Camera backCamera)
        {
            canvas.worldCamera = backCamera;
        }

        public void OnSwitchMainCamera(in Camera combatCamera)
        {
            
        }

        public void OnSwitchBackCamera(in Camera combatBackCamera)
        {
            Injection(combatBackCamera);
        }
    }
}
