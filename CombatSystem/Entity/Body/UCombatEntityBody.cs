using System;
using CombatSystem.Animations;
using SCharacterCreator.Bones;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Entity
{
    public class UCombatEntityBody : MonoBehaviour, ICombatEntityBody
    {
        [ShowInInspector, DisableInEditorMode] 
        private ICombatEntityAnimator _animator;


        private void Awake()
        {
            if (!baseRootType) baseRootType = transform.root;
            if (!targetHolderForUI) targetHolderForUI = baseRootType;
            if (!headRootType) headRootType = targetHolderForUI;

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
        [Title("Transforms")]
        [InfoBox("This component needs to be in Root", InfoMessageType.Warning)]
        [SerializeField]
        private Transform targetHolderForUI;
        [SerializeField] 
        private Transform baseRootType;
        [SerializeField] 
        private Transform headRootType;

        public ICombatEntityAnimator GetAnimator() => _animator;
        public Transform GetPointReference() => _pointReference;

        public Transform BaseRootType => baseRootType;
        public Transform PivotRootType => targetHolderForUI;
        public Transform HeadRootType => headRootType;



        public void Injection(in CombatEntity user)
        {
            _animator.Injection(user);
        }

        public void InjectPositionReference(Transform reference)
        {
            _pointReference = reference;
        }



    }


    public interface ICombatEntityBody : IHumanoidRootsStructureRead<Transform>
    {
        ICombatEntityAnimator GetAnimator();
        void Injection(in CombatEntity user);
        void InjectPositionReference(Transform reference);
    }

}
