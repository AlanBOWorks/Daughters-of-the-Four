using System;
using CombatCamera;
using CombatEntity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace __ProjectExclusive.Player
{
    public class UPivotOverEntity : MonoBehaviour
    {
        private void Awake()
        {
            _movingButton = (RectTransform)transform;
        }

        [Title("References")]
        [SerializeField]
        private PivotOverEntityReferences references = new PivotOverEntityReferences();

        [Title("Parameters")]
        [SerializeField, Range(0,1)]
        private float fixedPointLerp = .1f;

        [SerializeField] 
        private Vector3 fixedPointOffset = Vector3.up * .5f;


        [ShowInInspector, HideInEditorMode]
        private CombatingEntity _currentUser;

        private RectTransform _movingButton;

        public void Injection(CombatingEntity user) => _currentUser = user;
        internal PivotOverEntityReferences GetReferences() => references;

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
            if(_currentUser == (null))
                return;
            var holder = _currentUser.InstantiatedHolder;
            if(ReferenceEquals(holder,null)) 
                return;
            Transform holderTransform = holder.transform;


            Transform pivot = holder.GetUIPivot();

            Vector3 canvasPosition = Vector3.LerpUnclamped(
                pivot.position,
                    (holderTransform.position + fixedPointOffset),
                fixedPointLerp);

            _movingButton.position = CombatCameraSingleton.CombatMainCamera.WorldToScreenPoint(canvasPosition);
        }
    }

    [Serializable]
    internal class PivotOverEntityReferences
    {
        [SerializeField] private UTargetButton targetButton;
        public UTargetButton GetTargetButton() => targetButton;
    }
}
