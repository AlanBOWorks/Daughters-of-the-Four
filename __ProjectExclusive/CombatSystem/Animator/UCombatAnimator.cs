using System;
using System.Collections.Generic;
using CombatEntity;
using CombatSkills;
using DG.Tweening;
using MEC;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace CombatSystem.Animator
{
    public class UCombatAnimator : UEntityHolderListener
    {
        [SerializeField, HideInPlayMode] 
        private UCombatAnimationsHandler animationsHandler;

        public override void Inject(UEntityHolder holder)
        {
            ICombatAnimationHandler animationHandler;
            if (animationsHandler == null)
            {
                animationHandler = new ProvisionalAnimator();
            }
            else
            {
                animationHandler = animationsHandler;
            }

            holder.AnimationHandler = animationHandler;
        }


        private class ProvisionalAnimator : ICombatAnimationHandler
        {
            private const float SpaceBetweenAnimations = .2f;
            private const float DoSkillDuration = .8f;
            private const float ReceiveSkillDuration = .6f;
            public void DoIntroductionAnimation(CombatingEntity user)
            {
                
            }

            public void DoSwitchToIdleState(CombatingEntity user)
            {
                
            }

            public void DoPerformSkillAnimation(SkillValuesHolders skillValues)
            {
                var holder = skillValues.Performer.InstantiatedHolder;
                DOTween.Kill(holder.transform);
                holder.transform.DOPunchPosition(Vector3.up, DoSkillDuration, 4);
            }

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
        public abstract void DoIntroductionAnimation(CombatingEntity user);
        public abstract void DoSwitchToIdleState(CombatingEntity user);
        public abstract void DoPerformSkillAnimation(SkillValuesHolders skillValues);
        public abstract IEnumerator<float> _DoPerformSkillAnimation(SkillValuesHolders skillValues);
        public abstract void _DoReceiveSkillAnimation(SkillValuesHolders skillValues);
    }

    public interface ICombatAnimationHandler
    {
        void DoIntroductionAnimation(CombatingEntity user);
        void DoSwitchToIdleState(CombatingEntity user);

        /// <summary>
        /// Used by the system to wait a special animation to be finish
        /// </summary>
        IEnumerator<float> _DoPerformSkillAnimation(SkillValuesHolders skillValues);
        void DoPerformSkillAnimation(SkillValuesHolders skillValues);

        void _DoReceiveSkillAnimation(SkillValuesHolders skillValues);
    }

    public abstract class SCombatStandByAnimationsHolder : ScriptableObject
    {
        public abstract AnimationClip GetStartingCombatAnimation();
        public abstract AnimationClip GetIdleAnimation(CombatingEntity user);
    }

    public abstract class SCombatAnimationsHolder : ScriptableObject
    {
        public abstract AnimationClip GetPerformActionAnimation(SkillValuesHolders skillValues);
        public abstract AnimationClip GetReceiveActionAnimation(SkillValuesHolders skillValues);
    }
}
