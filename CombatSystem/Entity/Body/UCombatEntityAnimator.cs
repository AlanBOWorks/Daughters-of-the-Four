using System;
using System.Collections.Generic;
using Animancer;
using CombatSystem.Animations;
using CombatSystem.Skills;
using DG.Tweening;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace CombatSystem.Entity
{
    public abstract class UCombatEntityAnimator : MonoBehaviour, ICombatEntityAnimator,
        IEntityAnimationsLayerStructureRead<AnimancerLayer>
    {
        [TitleGroup("References")]
        [SerializeField] protected AnimancerComponent animancer;
        [Title("Params")]
        [SerializeField, SuffixLabel("s(%)"), Range(0, 1)]
        protected float idleLayerFade = .2f;
        [SerializeField, SuffixLabel("s(%)"), Range(0, 1)]
        protected float actionLayerFade = .12f;
        [SerializeField] 
        private SRange speedRandomness = new SRange(.8f,1.2f);

      

        protected CombatEntity Entity { get; private set; }
        
        public AnimancerLayer IdleAnimationType => UtilsAnimations.GetIdleLayer(animancer);
        public AnimancerLayer ActionAnimationType => UtilsAnimations.GetActionLayer(animancer);

        protected abstract AnimationClip GetInitialAnimationClip();
        protected abstract AnimationClip GetIdleClip();
        protected abstract AnimationClip GetActiveIdleClip();
        

        public void Injection(in CombatEntity user)
        {
            Entity = user;
            animancer.States.DestroyAll();
        }

        public void PerformInitialCombatAnimation()
        {
            DoInitializeAnimationsStates();
        }

        private void DoInitializeAnimationsStates()
        {
            var baseLayer = IdleAnimationType;

            AnimationClip initialAnimation;
            AnimationClip idleAnimation;
            GetAnimations();

            if(!idleAnimation) return;
            
            AnimancerState idleAnimationState;
            CreateStates();
                PlayIdleAnimationDirectly();
            if(initialAnimation != null)
                PlayInitialAnimation();

            ////
            void GetAnimations()
            {
                initialAnimation = GetInitialAnimationClip();
                idleAnimation = GetIdleClip();
            }
            void CreateStates()
            {
                idleAnimationState = baseLayer.GetOrCreateState(idleAnimation);
            }

            void PlayInitialAnimation()
            {
                DoActionAnimation(initialAnimation);
            }
            void PlayIdleAnimationDirectly()
            {
                baseLayer.Play(idleAnimationState);
            }
        }


        public abstract void PerformEndCombatAnimation();


        public void OnRequestSequenceAnimation()
        {
            var baseLayer = IdleAnimationType;
            baseLayer.Play(GetActiveIdleClip(), idleLayerFade);
        }

        public void OnEndSequenceAnimation()
        {
            var baseLayer = IdleAnimationType;
            baseLayer.Play(GetIdleClip(), idleLayerFade);
        }
        protected abstract AnimationClip GetActionAnimation(ISkill skill, EnumsSkill.TeamTargeting type);
        protected abstract AnimationClip GetReceiveActionAnimation(ISkill skill, EnumsSkill.TeamTargeting type);

        private CoroutineHandle _animationCoroutineHandle;
        public void PerformActionAnimation(ISkill skill, in CombatEntity onTarget)
        {
            var type = skill.TeamTargeting;
            var clip = GetActionAnimation(skill, type);
            if (clip == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning($"Animation Null on: {skill}");
#endif
                return;
            }


            DoActionAnimation(clip);
        }

        public void ReceiveActionAnimation(ISkill fromSkill, CombatEntity fromPerformer)
        {
            var type = UtilsTarget.GetReceiveSkillType(fromSkill, fromPerformer, Entity);
            var clip = GetReceiveActionAnimation(fromSkill, type);
            if (clip == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning($"Animation Null on: {fromSkill}");
#endif
                return;
            }

            DoActionAnimation(clip);
        }

        private AnimancerState _currentActionState;
        private const float OnSameClipResetTimerTo = .2f;
        private void DoActionAnimation(AnimationClip clip)
        {
            var actionLayer = ActionAnimationType;
            var targetState = actionLayer.GetOrCreateState(clip);
            float targetInitialTime = 0;
            if (targetState == _currentActionState)
            {
                targetInitialTime = OnSameClipResetTimerTo;
            }
         
            _currentActionState = actionLayer.Play(targetState,actionLayerFade);
            _currentActionState.NormalizedTime = targetInitialTime;
            _currentActionState.Speed = speedRandomness.CalculateRandom();

            if (_animationCoroutineHandle.IsRunning) return;
            _animationCoroutineHandle = Timing.RunCoroutine(_waitUntilAnimationFinishToIdle(actionLayer).CancelWith(animancer));
        }

        private IEnumerator<float> _waitUntilAnimationFinishToIdle(AnimancerLayer layer)
        {
            var actionLayer = ActionAnimationType;

            actionLayer.StartFade(1,actionLayerFade);
            while (CalculateThreshold())
            {
                yield return Timing.WaitForOneFrame;
            }

            _currentActionState = null;
            actionLayer.StartFade(0,idleLayerFade);


            bool CalculateThreshold()
            {
                var currentAnimancerState = layer.CurrentState;
                return currentAnimancerState.RemainingDuration > idleLayerFade;
            }
        }
    }

    public sealed class ProvisionalCombatAnimator : ICombatEntityAnimator
    {
        public void Injection(in CombatEntity user)
        {
        }

        public void PerformInitialCombatAnimation()
        {
        }

        public void PerformEndCombatAnimation()
        {
        }


        internal void Injection(in Transform body)
        {
            _body = body;
        }

        private Transform _body;

        private const float AnimationDuration = .4f;

        public void OnRequestSequenceAnimation()
        {
            _body.DOPunchPosition(Vector3.up, AnimationDuration, 4);
        }

        public void PerformActionAnimation(ISkill skill, in CombatEntity onTarget)
        {
            _body.DOPunchPosition(Vector3.forward, AnimationDuration, 4);
        }

        public void ReceiveActionAnimation(ISkill fromSkill, CombatEntity fromPerformer)
        {
            _body.DOPunchScale(Vector3.one * .1f, AnimationDuration);
        }

        public void OnEndSequenceAnimation()
        {
            _body.DOPunchScale(Vector3.one * -0.1f, AnimationDuration);
        }
    }
}
