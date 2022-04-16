using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UUIHoverEntityHolder : MonoBehaviour, IEntityExistenceElement<UUIHoverEntityHolder>
    {
        [SerializeField] private UTargetButton targetButton;
        [SerializeField] private UVitalityInfo healthInfo;

        public UTargetButton GetTargetButton() => targetButton;
        public UVitalityInfo GetHealthInfo() => healthInfo;
       
        public void EntityInjection(in CombatEntity entity)
        {
            var entityBody = entity.Body;

            _followReference = entityBody != null 
                ? entityBody.GetUIHoverHolder() 
                : entity.InstantiationReference.transform;
        } 
        public void OnPreStartCombat()
        {
            
        }

        public void ShowElement()
        {
            gameObject.SetActive(true);
        }

        public void HideElement()
        {
            gameObject.SetActive(false);
        }

        private Camera _playerCamera;
        [ShowInInspector]
        private Transform _followReference;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = (RectTransform) transform;
        }
        private void OnEnable()
        {
            _playerCamera = PlayerCombatSingleton.InterfaceCombatCamera;
        }

        private void LateUpdate()
        {
            var targetPoint = _playerCamera.WorldToScreenPoint(_followReference.position);
            _rectTransform.position = targetPoint;
        }


        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            HideInstant();
        }

        public void HideInstant()
        {
            gameObject.SetActive(false);
        }

    }
}