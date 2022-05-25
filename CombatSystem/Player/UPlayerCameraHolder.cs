using System;
using UnityEngine;

namespace CombatSystem.Player
{
    public class UPlayerCameraHolder : MonoBehaviour, IPlayerCameraStructureRead<Camera>
    {
        [SerializeField] private Camera combatMainCamera;
        [SerializeField] private Camera combatBackCamera;
        [SerializeField] private Camera combatFrontCamera;
        [SerializeField] private Camera combatCharacterCamera;

        private void Awake()
        {
            Camera injectionCamera = combatMainCamera 
                ? combatMainCamera
                : Camera.main;

            PlayerCombatSingleton.Injection(this);
        }

        public Camera GetMainCameraType => combatMainCamera;
        public Camera GetBackCameraType => combatBackCamera;
        public Camera GetFrontCameraType => combatFrontCamera;
        public Camera GetCharacterCameraType => combatCharacterCamera;
    }

    public interface IPlayerCameraStructureRead<out T>
    {
        T GetMainCameraType { get; }
        T GetBackCameraType { get; }
        T GetFrontCameraType { get; }
        T GetCharacterCameraType { get; }
    }
}
