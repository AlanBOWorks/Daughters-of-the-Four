using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using MEC;
using SCharacterCreator.Bones;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CombatSystem.Player.UI
{
    public sealed class UUIHoverEntityHolder : UUIHoverEntityBase, IEntityExistenceElement<UUIHoverEntityHolder>,
        IPointerEnterHandler, IPointerExitHandler
    {
        [Title("References")]
        [SerializeField] private UTargetButton targetButton;
        [SerializeField] private GameObject hoverFeedbackHolder;

        public UTargetButton GetTargetButton() => targetButton;
        public GameObject GetHoverFeedbackHolder() => hoverFeedbackHolder;

        protected override Transform GetFollowTransform(ICombatEntityBody body)
        {
            return body.PivotRootType;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            KeepTrackPosition = false;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            KeepTrackPosition = true;
        }
    }

    public abstract class UUIHoverEntityBase : MonoBehaviour, IEntityExistenceElement<UUIHoverEntityBase>
    {
        protected abstract Transform GetFollowTransform(ICombatEntityBody body);

        [NonSerialized]
        public bool KeepTrackPosition;

        public virtual void EntityInjection(CombatEntity entity)
        {
            var entityBody = entity.Body;

            if (entityBody != null)
            {
                _followReference = GetFollowTransform(entityBody);
            }
        }

        public void OnPreStartCombat()
        {

        }

        public void OnInstantiation()
        {
            gameObject.SetActive(true);
        }

        public void OnDestruction()
        {
            gameObject.SetActive(false);
        }

        private Camera _playerCamera;
        [Title("RunTime")]
        [ShowInInspector, HideInEditorMode]
        private Transform _followReference;
        private RectTransform _rectTransform;


        private void Awake()
        {
            _rectTransform = (RectTransform)transform;
            KeepTrackPosition = true;
        }
        private void OnEnable()
        {
            _playerCamera = PlayerCombatSingleton.CamerasHolder.GetMainCameraType;
        }

        private void LateUpdate()
        {
            if(KeepTrackPosition)
                _rectTransform.position = CalculateScreenPoint();
        }

        protected virtual Vector3 GetTargetPosition()
        {
            return _followReference.position;
        }

        protected virtual Vector3 CalculateScreenPoint()
        {
            return _playerCamera.WorldToScreenPoint(GetTargetPosition());
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
