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
        private UCombatAnimationsHandler animationsHandler;
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
                DOTween.Kill(holder.transform);
                holder.transform.DOPunchPosition(Vector3.up, DoSkillDuration, 4);

                yield return Timing.WaitForSeconds(DoSkillDuration + SpaceBetweenAnimations);
            }

            public void _DoReceiveSkillAnimation(SkillValuesHolders skillValues)
            {
                var holder = skillValues.Performer.InstantiatedHolder;
                DOTween.Kill(holder.transform);

                holder.transform.DOPunchPosition(Vector3.forward, ReceiveSkillDuration, 6);
            }
        }
    }

    public abstract class UCombatAnimationsHandler : MonoBehaviour, ICombatAnimationHandler
    {
        public abstract IEnumerator<float> _DoPerformSkillAnimation(SkillValuesHolders skillValues);
        public abstract void _DoReceiveSkillAnimation(SkillValuesHolders skillValues);

    }

    public interface ICombatAnimationHandler
    {

        IEnumerator<float> _DoPerformSkillAnimation(SkillValuesHolders skillValues);
        void _DoReceiveSkillAnimation(SkillValuesHolders skillValues);
    }
}
