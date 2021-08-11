using System;
using System.Collections.Generic;
using ___ProjectExclusive.Animators;
using Animancer;
using Characters;
using DG.Tweening;
using MEC;
using Sirenix.OdinInspector;
using Skills;
using UnityEngine;

namespace Animators
{
    public class UStylizedCombatAnimator : MonoBehaviour, ICharacterCombatAnimator
    {
        [TitleGroup("References"),HideInPlayMode]
        [SerializeField] private AnimancerComponent animancer;
        [SerializeField] private SCombatAnimationsTransitionsBase animations = null;

        [TitleGroup("Fades")]
        [SerializeField]
        private AnimationCurve fadeCurve = new AnimationCurve(
            new Keyframe(0, 0), new Keyframe(1, 1));
        [SuffixLabel("sec")]
        public float fadeDuration = 1f;

        private const int IdleLayerIndex = 0;
        private const int DoAnimationLayerIndex = 1;

        public void DoInitialAnimation()
        {
            var idleLayer = animancer.Layers[IdleLayerIndex];
            var idleAnimation = animations.IdleAnimation;
            if(idleAnimation == null) return;
            idleLayer.Play(animations.IdleAnimation);
        }

        /// <summary>
        /// Stylized fade is not linear, is manually moved in specific point of blend
        /// that are more fitting visually to a hand draw pose (generally drastic changes
        /// on pose, thus the .8f) 
        /// </summary>
        private const float FadeInStepWeight = .8f;
        private const float FadeOutStepWeight = 1 - FadeInStepWeight;

        private IEnumerator<float> _DoActionAnimation(ITransition targetAnimation)
        {
            var actionLayer = animancer.Layers[DoAnimationLayerIndex];
            var animationState = actionLayer.Play(targetAnimation, 0);

            bool skipFrame = true; //this is for stylized purposes (draw in twos)

            animationState.IsPlaying = false;
            animationState.Time = 0;
            actionLayer.Weight = 0;

            // FADE IN the Layer
            DoFadeInTween();
            yield return Timing.WaitForSeconds(fadeDuration);
            // PLAY the animation
            actionLayer.Weight = 1;
            animationState.IsPlaying = true;


            // WAIT to finish
            while (animationState.NormalizedTime < 1)
            {
                yield return Timing.WaitForOneFrame;
            }



            // FADE OUT
            DoFadeOutTween();
            yield return Timing.WaitForSeconds(fadeDuration);

            actionLayer.Weight = 0;
            animationState.IsPlaying = false;

            void DoFadeInTween()
            {
                DOTween.To(
                    GetStateWeight,
                    SetLayerWeight,
                    1, 
                    fadeDuration);
            }
            void DoFadeOutTween()
            {
                //The inverses are for moving through the curve from 1 -> 0
                DOTween.To(
                    GetStateWeightInverse,
                    SetLayerWeightInverse,
                    1,
                    fadeDuration);
            }

            float GetStateWeight() => actionLayer.Weight;
            float GetStateWeightInverse() => 1 - actionLayer.Weight;

            void SetLayerWeight(float weight)
            {
                skipFrame = !skipFrame;
                if (skipFrame) return;

                float curvedWeight = fadeCurve.Evaluate(weight);
                actionLayer.Weight = curvedWeight;
            }

            void SetLayerWeightInverse(float weight)
            {
                skipFrame = !skipFrame;
                if (skipFrame) return;

                float curvedWeight = fadeCurve.Evaluate(weight);
                actionLayer.Weight = 1 - curvedWeight;
            }

        }

        private CoroutineHandle _currentAnimationHandle;
        private const float OnNullWait = 1;
        private CoroutineHandle TryAnimate(ITransition target)
        {
            Timing.KillCoroutines(_currentAnimationHandle);
            if(target != null)
                _currentAnimationHandle = Timing.RunCoroutine(_DoActionAnimation(target));
            else
            {
                _currentAnimationHandle = Timing.RunCoroutine(_DoNullAnimation());
            }
            return _currentAnimationHandle;

            IEnumerator<float> _DoNullAnimation()
            {
                animancer.transform.DOPunchPosition(Vector3.up, OnNullWait);
                yield return Timing.WaitForSeconds(OnNullWait);
                yield return Timing.WaitForOneFrame;
            }
        }

        public CoroutineHandle DoAnimation(CombatingEntity user, List<CombatingEntity> targets, CombatSkill skill)
        {
            var targetAnimation = animations.GetActionAnimationElement(skill);
            return TryAnimate(targetAnimation);
        }

        public CoroutineHandle ReceiveAction(CombatingEntity actor, CombatingEntity receiver, bool isSupport)
        {
            var targetAnimation = (isSupport)
                ? animations.ReceiveSupportAnimation
                : animations.ReceiveOffensiveAnimation;
            return TryAnimate(targetAnimation);
        }
    }
}
