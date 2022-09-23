using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Player
{
    public class UPlayerCombatCameraHolder : MonoBehaviour, IPlayerCameraStructureRead<Camera>,
        ICombatTerminationListener, ICombatPreparationListener
    {
        [SerializeField] private Camera combatMainCamera;
        [SerializeField] private Camera combatBackCamera;
        [SerializeField] private Camera combatFrontCamera;
        [SerializeField] private Camera combatCharacterBackCamera;
        [SerializeField] private Camera combatCharacterFrontCamera;

        private void Awake()
        {
            var instantiatedCameraGameObject = new GameObject("Camera for Reset [Copy]");
            instantiatedCameraGameObject.SetActive(false);
            var instantiatedCameraTransform = instantiatedCameraGameObject.transform;
            instantiatedCameraTransform.parent = combatMainCamera.transform;
            instantiatedCameraTransform.localPosition = Vector3.zero;
            instantiatedCameraTransform.localRotation = Quaternion.identity;

            var backUpCameraForResetState = instantiatedCameraGameObject.AddComponent<Camera>();
            backUpCameraForResetState.CopyFrom(combatMainCamera);

            CombatCameraHandler.Injection(this,backUpCameraForResetState);
            CombatSystemSingleton.EventsHolder.Subscribe(this);
        }

        private void OnDestroy()
        {
            CombatSystemSingleton.EventsHolder.UnSubscribe(this);
        }

        public Camera MainCameraType => combatMainCamera;
        public Camera BackCameraType => combatBackCamera;

        public Camera CharacterBackCameraType => combatCharacterBackCamera;

        public Camera FrontCameraType => combatFrontCamera;
        public Camera CharacterFrontCameraType => combatCharacterFrontCamera;


        public void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            gameObject.SetActive(true);
        }
        public void OnCombatFinish(UtilsCombatFinish.FinishType finishType)
        {
        }

        public void OnCombatFinishHide(UtilsCombatFinish.FinishType finishType)
        {
            gameObject.SetActive(false);
        }

    }

    public interface IPlayerCameraStructureRead<out T> : ICombatMainCameraRead<T>, ICombatCharacterCamerasStructureRead<T>,
        ICombatBackFrontCamerasStructureRead<T>
    { }

    public interface ICombatMainCameraRead<out T>
    {
        T MainCameraType { get; }

    }

    public interface ICombatCharacterCamerasStructureRead<out T>
    {
        T CharacterBackCameraType { get; }
        T CharacterFrontCameraType { get; }
    }

    public interface ICombatBackFrontCamerasStructureRead<out T>
    {
        T BackCameraType { get; }

        T FrontCameraType { get; }
    }
}
