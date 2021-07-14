using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Characters
{
    public class UCharacterHolder : MonoBehaviour
    {
        [Title("References")]
        public Transform meshTransform = null;
        public Transform targetTransform = null;

        public void Injection(CombatingEntity entity)
        {
            Entity = entity;
            BaseStats = entity.CombatStats;
            entity.Holder = this;

            var animator = GetComponent<ICharacterCombatAnimator>();
            if(animator == null) return;
            entity.CombatAnimator = animator;
            animator.DoInitialAnimation();
        }
        [ShowInInspector,DisableInEditorMode]
        public CombatingEntity Entity { get; private set; }
        public ICharacterFullStats BaseStats { get; private set; }


    }
}
