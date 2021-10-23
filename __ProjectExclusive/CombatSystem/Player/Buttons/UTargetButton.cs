using System;
using CombatEntity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace __ProjectExclusive.Player
{
    public class UTargetButton : MonoBehaviour
    {
        [ShowInInspector]
        private CombatingEntity _currentUser;

        [ShowInInspector]
        private Camera _referenceCamera;
        private RectTransform _movingButton;

        private void Awake()
        {
            _movingButton = (RectTransform) transform;
        }

        public void Injection(Camera referenceCamera) => _referenceCamera = referenceCamera;
        public void Injection(CombatingEntity user) => _currentUser = user;

        public void Show()
        {
            gameObject.SetActive(true);
            enabled = true;
        }
        public void Hide()
        {
            enabled = false;
            gameObject.SetActive(false);
        }

        private void LateUpdate()
        {
            Transform onObject = _currentUser.InstantiatedHolder.transform;

            _movingButton.position = _referenceCamera.WorldToScreenPoint(onObject.position);
        }
    }
}

