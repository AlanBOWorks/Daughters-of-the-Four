using System;
using CombatSystem.Animations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Entity
{
    public class UCombatEntityBody : MonoBehaviour, ICombatEntityBody
    {
        [InfoBox("This component needs to be in Root",InfoMessageType.Warning)]
        [SerializeField] 
        private Transform targetHolderForUI;

        [ShowInInspector, DisableInEditorMode] 
        private ICombatEntityAnimator _animator;

        private void Awake()
        {
            _animator = GetComponent<ICombatEntityAnimator>();
            if (_animator == null)
            {
                var provisionalAnimator = new ProvisionalCombatAnimator();
                provisionalAnimator.Injection(transform);
                _animator = provisionalAnimator;
            }

            if (!targetHolderForUI) targetHolderForUI = transform;
        }


        private Transform _pointReference;

        public Transform GetUIHoverHolder() => targetHolderForUI;
        public ICombatEntityAnimator GetAnimator() => _animator;

        public Transform GetPointReference() => _pointReference;




        public void Injection(in CombatEntity user)
        {
            _animator.Injection(user);
        }

        public void InjectPositionReference(Transform reference)
        {
            _pointReference = reference;
        }

    }


    public interface ICombatEntityBody
    {
        Transform GetUIHoverHolder();
        ICombatEntityAnimator GetAnimator();
        void Injection(in CombatEntity user);
        void InjectPositionReference(Transform reference);
    }

}
