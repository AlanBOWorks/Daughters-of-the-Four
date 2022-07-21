using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using MEC;
using SCharacterCreator.Bones;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UUIHoverEntityHolder : UUIHoverEntityBase, IEntityExistenceElement<UUIHoverEntityHolder>
    {
        [Title("References")]
        [SerializeField] private UTargetButton targetButton;
        [SerializeField] private UVitalityInfo healthInfo;
        [SerializeField] private GameObject hoverFeedbackHolder;

        public UTargetButton GetTargetButton() => targetButton;
        public UVitalityInfo GetHealthInfo() => healthInfo;
        public GameObject GetHoverFeedbackHolder() => hoverFeedbackHolder;

        protected override Transform GetFollowTransform(ICombatEntityBody body)
        {
            return body.PivotRootType;
        }

        private const float DynamicPointPercent = .1f;
        private Vector3 _canvasAnchorPoint;


        public void InjectAnchorPosition(Vector2 point)
        {
            _canvasAnchorPoint = point;
        }
        protected override Vector3 CalculateScreenPoint()
        {
            var transformPoint = base.CalculateScreenPoint();
            return Vector3.LerpUnclamped(_canvasAnchorPoint, transformPoint, DynamicPointPercent);
        }
    }

    public abstract class UUIHoverEntityBase : MonoBehaviour, IEntityExistenceElement<UUIHoverEntityBase>
    {
        protected abstract Transform GetFollowTransform(ICombatEntityBody body);

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
        }
        private void OnEnable()
        {
            _playerCamera = PlayerCombatSingleton.CamerasHolder.GetMainCameraType;
        }

        private void LateUpdate()
        {
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
