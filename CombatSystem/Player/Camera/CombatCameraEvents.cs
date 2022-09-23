using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Player.Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player
{
    public sealed class CombatCameraEvents : ICameraHolderListener
    {
        public CombatCameraEvents()
        {
            _cameraHolderListeners = new HashSet<ICameraHolderListener>();
        }

        [ShowInInspector]
        private readonly HashSet<ICameraHolderListener> _cameraHolderListeners;

        public void Subscribe(ICameraHolderListener listener)
        {
            _cameraHolderListeners.Add(listener);
        }

        public void UnSubscribe(ICameraHolderListener listener)
        {
            _cameraHolderListeners.Remove(listener);
        }



        public void OnSwitchMainCamera(Camera combatCamera)
        {
            foreach (var listener in _cameraHolderListeners)
            {
                listener.OnSwitchMainCamera(combatCamera);
            }
        }

        public void OnSwitchBackCamera(Camera combatBackCamera)
        {
            foreach (var listener in _cameraHolderListeners)
            {
                listener.OnSwitchBackCamera(combatBackCamera);
            }
        }

        public void OnSwitchFrontCamera(Camera combatFrontCamera)
        {
            foreach (var listener in _cameraHolderListeners)
            {
                listener.OnSwitchFrontCamera(combatFrontCamera);
            }
        }
    }


    public interface ICameraHolderListener
    {
        void OnSwitchMainCamera(Camera combatCamera);
        void OnSwitchBackCamera(Camera combatBackCamera);
        void OnSwitchFrontCamera(Camera combatFrontCamera);
    }
}
