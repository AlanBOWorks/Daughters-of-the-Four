using System;
using _CombatSystem;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace Characters
{
    public class UCharacterHolder : MonoBehaviour
    {
        [Title("Params")]
        public bool isPlayerCharacter = false;

        [Title("References")]
        public Transform meshTransform = null;
        public Transform targetTransform = null;

        public void Injection(CombatingEntity entity)
        {
            Entity = entity;
            BaseStats = entity.CombatStats;
            entity.Holder = this;

            var animator = GetComponent<ICharacterCombatAnimator>();
            if(IsGenericAnimator() || animator == null)
            {
                entity.CombatAnimator = ProvisionalCharacterAnimator.ProvisionalAnimator;
                return;
            }
            entity.CombatAnimator = animator;
            animator.DoInitialAnimation();

            bool IsGenericAnimator()
            {
                var skipType = CombatSystemSingleton.ParamsVariable.skipAnimationsType;
                if (isPlayerCharacter)
                    return skipType == SCombatParams.SkipAnimationsType.All;
                else
                    return skipType == SCombatParams.SkipAnimationsType.Enemy;
            }
        }


        [ShowInInspector,DisableInEditorMode]
        public CombatingEntity Entity { get; private set; }
        public IFullStatsData BaseStats { get; private set; }


    }
}
