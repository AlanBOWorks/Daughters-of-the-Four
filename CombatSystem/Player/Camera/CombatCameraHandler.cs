using System;
using UnityEngine;

namespace CombatSystem.Player
{
    public static class CombatCameraHandler
    {
        private static IPlayerCameraStructureRead<Camera> _mainCamerasHolder;

        public static Camera MainCamera => _mainCamerasHolder.MainCameraType;
        public static ICombatBackFrontCamerasStructureRead<Camera> BackFrontCameras => _mainCamerasHolder;
        public static ICombatCharacterCamerasStructureRead<Camera> CharacterCameras => _mainCamerasHolder;

        private static Camera _backUpCameraForResetStates;
        public static void Injection(IPlayerCameraStructureRead<Camera> mainStructure, Camera backUpCameraForResetState)
        {
            _mainCamerasHolder = mainStructure;
            _backUpCameraForResetStates = backUpCameraForResetState;
        }


        public static void DoMainCameraSubstitution(Camera substitutionCamera)
        {
            if(_mainCamerasHolder == null) throw new NullReferenceException("Static Injection wasn't made");
            var mainCamera = MainCamera;
            var mainCameraTransform = mainCamera.transform;
            var substitutionCameraTransform = substitutionCamera.transform;

            mainCameraTransform.position = substitutionCameraTransform.position;
            mainCameraTransform.rotation = substitutionCameraTransform.rotation;
            mainCamera.CopyFrom(substitutionCamera);
            float targetFow = substitutionCamera.fieldOfView;
            CopyFow(_mainCamerasHolder.CharacterBackCameraType);
            CopyFow(_mainCamerasHolder.CharacterFrontCameraType);
            CopyFow(_mainCamerasHolder.BackCameraType);

            void CopyFow(Camera camera)
            {
                camera.fieldOfView = targetFow;
            }
        }

        public static void ResetMainCameraState()
        {
            MainCamera.CopyFrom(_backUpCameraForResetStates);
        }

    }
}
