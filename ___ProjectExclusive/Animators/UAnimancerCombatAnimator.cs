using System;
using System.Collections.Generic;
using Animancer;
using Characters;
using MEC;
using Sirenix.OdinInspector;
using Skills;
using StylizedAnimator;
using UnityEngine;

namespace ___ProjectExclusive.Animators
{
    public class UAnimancerCombatAnimator : MonoBehaviour, ICharacterCombatAnimator
    {
        [InfoBox("It's recommendable having this in the same level than UCharacterHolder")]
        [SerializeField] private AnimancerComponent animancer;
        [SerializeField] private SCombatAnimationsStates animations = null;
        private AnimancerState _currentState;
        private Func<bool> _isAnimationFinish;
        private Action _returnToIdle;
        private AnimancerTicker _stylizedAnimancerTicker;


        public void Awake()
        {
            _isAnimationFinish = IsStateFinish;
            _returnToIdle = ReturnToIdle;
            animations.Injection(animancer);

            AnimancerEvent onAnimationEnd = new AnimancerEvent(.9f, _returnToIdle);


            InjectEvent(animations.OffensiveAnimation);
            InjectEvent(animations.SupportAnimation);
            InjectEvent(animations.ReceiveOffensiveAnimation);
            InjectEvent(animations.ReceiveSupportAnimation);
            animancer.Layers[0].Weight = 1;


            void InjectEvent(AnimancerState state)
            {
                state.Events.Add(onAnimationEnd);
            }

            _stylizedAnimancerTicker = new AnimancerTicker(animancer);
            _stylizedAnimancerTicker.InjectInManager(StylizedTickManager.HigherFrameRate.Twos);
        }

        public void DoInitialAnimation()
        {
            ReturnToIdle();
        }

        private bool IsStateFinish()
        {
            return _currentState.NormalizedTime > .95f;
        }

        private void ReturnToIdle()
        {
            animancer.Play(animations.IdleAnimation);
        }


        private CoroutineHandle _currentAnimationHandle;
        public CoroutineHandle DoAnimation(CombatingEntity user, List<CombatingEntity> targets, CombatSkill skill)
        {
            Timing.KillCoroutines(_currentAnimationHandle);
            _currentAnimationHandle = Timing.RunCoroutine(_DoAnimation());

            return _currentAnimationHandle;

            IEnumerator<float> _DoAnimation()
            {
                if (UtilsSkill.GetType(skill) == SSkillPreset.SkillType.Offensive)
                {
                    _currentState = animancer.Play(animations.OffensiveAnimation);
                }
                else
                {
                    _currentState = animancer.Play(animations.SupportAnimation);
                }

                yield return Timing.WaitUntilTrue(_isAnimationFinish);
                animancer.Play(animations.IdleAnimation);
                _currentState = null;
            }
        }

        public CoroutineHandle ReceiveAction(CombatingEntity actor, CombatingEntity receiver, bool isSupport)
        {
            Timing.KillCoroutines(_currentAnimationHandle);
            _currentAnimationHandle = Timing.RunCoroutine(_DoProvisionalAnimation());

            return _currentAnimationHandle;

            IEnumerator<float> _DoProvisionalAnimation()
            {
                float targetDuration;
                if (isSupport)
                {
                    targetDuration = DoReceiveAction(animations.ReceiveSupportAnimation);
                }
                else
                {
                    targetDuration = DoReceiveAction(animations.ReceiveOffensiveAnimation);
                }

                yield return Timing.WaitForSeconds(targetDuration);
            }
        }

        private float DoReceiveAction(AnimancerState target)
        {
            animancer.Play(target);
            _currentState = null;
            return target.Duration;
        }

    }

    public abstract class SCombatAnimationsTransitionsBase : ScriptableObject,
        ICombatAnimationsHandler<ITransition>
    {
        public abstract ITransition IdleAnimation { get; }
        public abstract ITransition SelfSupportAnimation { get; }
        public abstract ITransition OffensiveAnimation { get; }
        public abstract ITransition SupportAnimation { get; }
        public abstract ITransition ReceiveSupportAnimation { get; }
        public abstract ITransition ReceiveOffensiveAnimation { get; }
        public abstract ITransition GetActionAnimationElement(CombatSkill skill);
    }

    public  abstract class SCombatAnimationsStates : ScriptableObject, ICombatAnimationsPreparation<AnimancerState>
    {
        public abstract void Injection(AnimancerComponent animancer);


        public enum AnimationType
        {
            Offensive,
            Support,
            ReceiveOffensive,
            ReceiveSupport
        }

        public abstract AnimancerState IdleAnimation { get; }
        public abstract AnimancerState SelfSupportAnimation { get; }
        public abstract AnimancerState OffensiveAnimation { get; }
        public abstract AnimancerState SupportAnimation { get; }
        public abstract AnimancerState ReceiveSupportAnimation { get; }
        public abstract AnimancerState ReceiveOffensiveAnimation { get; }
    }

    public interface ICombatAnimationsPreparation<out T> : ICombatAnimationsBasic<T>
    {
        void Injection(AnimancerComponent animancer);
    }

    public interface ICombatAnimationsHandler<out T> : ICombatAnimationsBasic<T>
    {
        T GetActionAnimationElement(CombatSkill skill);
    }

    public interface ICombatAnimationsBasic<out T> : ICombatAnimationsIdle<T>, ICombatAnimationsActions<T>,
        ICombatAnimationsReceive<T>
    {}

    public interface ICombatAnimationsIdle<out T>
    {
        T IdleAnimation { get; }
    }

    public interface ICombatAnimationsActions<out T>
    {
        T SelfSupportAnimation { get; }
        T OffensiveAnimation { get; }
        T SupportAnimation { get; }
    }

    public interface ICombatAnimationsReceive<out T>
    {
        T ReceiveSupportAnimation { get; }
        T ReceiveOffensiveAnimation { get; }
    }


    public interface ICombatAnimationListener
    {
        void OnAnimationPlay(SCombatAnimationsStates.AnimationType type);
    }
}
