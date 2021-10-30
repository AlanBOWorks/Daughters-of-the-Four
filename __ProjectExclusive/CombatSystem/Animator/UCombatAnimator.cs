using System;
using System.Collections.Generic;
using CombatEntity;
using CombatSkills;
using DG.Tweening;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Animator
{
    public class UCombatAnimator : UEntityHolderListener
    {
        [SerializeField, HideInPlayMode] 
        private SCombatAnimationsHandler animationsHandler;
        [HideInEditorMode,ShowInInspector]
        private ICombatAnimationHandler _animationHandler;

        public IEnumerator<float> HandleAction(SkillValuesHolders skillValues)
            => animationsHandler._DoPerformSkillAnimation(skillValues);

        public override void Inject(UEntityHolder holder)
        {
            if (animationsHandler == null)
            {
                _animationHandler = new ProvisionalAnimator();
            }
            else
            {
                _animationHandler = animationsHandler;
            }

            holder.AnimationHandler = _animationHandler;
        }





        private class ProvisionalAnimator : ICombatAnimationHandler
        {
            private const float SpaceBetweenAnimations = .2f;
            private const float DoSkillDuration = .8f;
            private const float ReceiveSkillDuration = .6f;
            public IEnumerator<float> _DoPerformSkillAnimation(SkillValuesHolders skillValues)
            {
                var holder = skillValues.Performer.InstantiatedHolder;
                holder.transform.DOPunchPosition(Vector3.up, DoSkillDuration, 4);

                yield return Timing.WaitForSeconds(DoSkillDuration + SpaceBetweenAnimations);
            }

            public IEnumerator<float> _DoReceiveSkillAnimation(SkillValuesHolders skillValues)
            {
                var holder = skillValues.Performer.InstantiatedHolder;
                holder.transform.DOPunchPosition(Vector3.forward, ReceiveSkillDuration, 6);

                yield return Timing.WaitForSeconds(DoSkillDuration + SpaceBetweenAnimations);
            }
        }
    }

    public abstract class SCombatAnimationsHandler : ScriptableObject, ICombatAnimationHandler
    {
        public abstract IEnumerator<float> _DoPerformSkillAnimation(SkillValuesHolders skillValues);
        public abstract IEnumerator<float> _DoReceiveSkillAnimation(SkillValuesHolders skillValues);

    }

    public interface ICombatAnimationHandler
    {

        IEnumerator<float> _DoPerformSkillAnimation(SkillValuesHolders skillValues);
        IEnumerator<float> _DoReceiveSkillAnimation(SkillValuesHolders skillValues);
    }
}
