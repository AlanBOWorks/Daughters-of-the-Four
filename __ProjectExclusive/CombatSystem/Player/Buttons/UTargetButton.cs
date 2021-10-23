using System;
using CombatEntity;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace __ProjectExclusive.Player
{
    public class UTargetButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private void Awake()
        {
            _movingButton = (RectTransform)transform;
        }

        [ShowInInspector]
        private CombatingEntity _currentUser;

        [ShowInInspector]
        private Camera _referenceCamera;
        private RectTransform _movingButton;

        private UTargetButtonsHolder _holder;


        public CombatingEntity GetEntity() => _currentUser;


        public void Injection(UTargetButtonsHolder holder) => _holder = holder;
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


        public void OnPointerClick(PointerEventData eventData)
        {
            DOTween.Kill(transform);
            transform.localScale = Vector3.one;
            transform.DOPunchScale(new Vector3(1.1f, 1.1f, 1), .2f);
            _holder.OnTargetSelect(this);
        }

        public const float PointerClickAnimationDuration = .2f;
        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(new Vector3(1.1f, 1.1f, 1), PointerClickAnimationDuration);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DOTween.Kill(transform);
            transform.localScale = Vector3.one;
        }
    }
}

