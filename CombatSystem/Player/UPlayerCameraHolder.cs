using System;
using UnityEngine;

namespace CombatSystem.Player
{
    public class UPlayerCameraHolder : MonoBehaviour, IPlayerCameraStructureRead<Camera>
    {
        [SerializeField] private Camera combatMainCamera;
        [SerializeField] private Camera combatBackCamera;
        [SerializeField] private Camera combatFrontCamera;
        [SerializeField] private Camera combatCharacterBackCamera;
        [SerializeField] private Camera combatCharacterFrontCamera;

        private void Awake()
        {
            Camera injectionCamera = combatMainCamera 
                ? combatMainCamera
                : Camera.main;

            PlayerCombatSingleton.Injection(this);
        }

        public Camera GetMainCameraType => combatMainCamera;
        public Camera GetBackCameraType => combatBackCamera;

        public Camera GetCharacterBackCameraType => combatCharacterBackCamera;

        public Camera GetFrontCameraType => combatFrontCamera;
        public Camera GetCharacterFrontCameraType => combatCharacterFrontCamera;
    }

    public interface IPlayerCameraStructureRead<out T>
    {
        T GetMainCameraType { get; }

        T GetBackCameraType { get; }
        T GetCharacterBackCameraType { get; }
        T GetCharacterFrontCameraType { get; }
        T GetFrontCameraType { get; }
    }
}
