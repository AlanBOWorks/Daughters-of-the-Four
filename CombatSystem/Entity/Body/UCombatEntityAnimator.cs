using System;
using System.Collections.Generic;
using Animancer;
using CombatSystem.Animations;
using CombatSystem.Skills;
using DG.Tweening;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

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
            
            AnimancerState initialAnimationState;
            AnimancerState idleAnimationState;
            CreateStates();
            if(initialAnimationState != null)
                PlayInitialAnimation();
            else
                PlayIdleAnimationDirectly();

            ////
            void GetAnimations()
            {
                initialAnimation = GetInitialAnimationClip();
                idleAnimation = GetIdleClip();
            }
            void CreateStates()
            {
                initialAnimationState = (initialAnimation != null) 
                    ? baseLayer.CreateState(initialAnimation)
                    : null;
                idleAnimationState = baseLayer.CreateState(idleAnimation);
            }

            void PlayInitialAnimation()
            {
                float normalizedTriggerTime = 1 - idleLayerFade;
                initialAnimationState.Events.Add(normalizedTriggerTime, FadeToInitialState);
                baseLayer.Play(initialAnimationState);

                void FadeToInitialState() => baseLayer.Play(idleAnimationState, idleLayerFade, FadeMode.FixedDuration);
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
        protected abstract AnimationClip GetActionAnimation(CombatSkill skill, EnumsSkill.Archetype type);
        protected abstract AnimationClip GetReceiveActionAnimation(CombatSkill skill, EnumsSkill.Archetype type);

        private CoroutineHandle _animationCoroutineHandle;
        public void PerformActionAnimation(CombatSkill skill, in CombatEntity onTarget)
        {
            var type = skill.Archetype;
            var clip = GetActionAnimation(skill, type);
            if (clip == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning($"Animation Null on: {skill.Preset}");
#endif
                return;
            }


            DoActionAnimation(clip);
        }

        public void ReceiveActionAnimation(CombatSkill fromSkill, in CombatEntity fromPerformer)
        {
            var type = UtilsTarget.GetReceiveSkillType(in fromSkill, in fromPerformer, Entity);
            var clip = GetReceiveActionAnimation(fromSkill, type);
            if (clip == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning($"Animation Null on: {fromSkill.Preset}");
#endif
                return;
            }

            DoActionAnimation(clip);
        }

        private AnimationClip _currentAnimationClip;
        private const float OnSameClipRewindToTime = .2f;
        private void DoActionAnimation(AnimationClip clip)
        {
            var actionLayer = ActionAnimationType;
            if (clip == _currentAnimationClip)
            {
                actionLayer.CurrentState.NormalizedTime = OnSameClipRewindToTime;
                return;
            }

            _currentAnimationClip = clip;
            actionLayer.Play(clip,actionLayerFade);

            if(_animationCoroutineHandle.IsRunning) return;;
            actionLayer.StartFade(1,actionLayerFade);
            _animationCoroutineHandle = Timing.RunCoroutine(_waitUntilAnimationFinishToIdle(actionLayer).CancelWith(animancer));
        }

        private IEnumerator<float> _waitUntilAnimationFinishToIdle(AnimancerLayer layer)
        {
            while (CalculateThreshold())
            {
                yield return Timing.WaitForOneFrame;
            }

            var actionLayer = ActionAnimationType;
            DoLayerFade(actionLayer,0);

            bool CalculateThreshold()
            {
                var currentAnimancerState = layer.CurrentState;
                return currentAnimancerState.RemainingDuration > idleLayerFade;
            }

            void DoLayerFade(AnimancerLayer onLayer, float targetWeight)
            {
                onLayer.StartFade(targetWeight,idleLayerFade);
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

        public void PerformActionAnimation(CombatSkill skill, in CombatEntity onTarget)
        {
            _body.DOPunchPosition(Vector3.forward, AnimationDuration, 4);
        }

        public void ReceiveActionAnimation(CombatSkill fromSkill, in CombatEntity fromPerformer)
        {
            _body.DOPunchScale(Vector3.one * .1f, AnimationDuration);
        }

        public void OnEndSequenceAnimation()
        {
            _body.DOPunchScale(Vector3.one * -0.1f, AnimationDuration);
        }
    }
}
