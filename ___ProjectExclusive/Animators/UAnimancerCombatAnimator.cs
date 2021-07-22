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


            InjectEvent(animations.GetOffensive());
            InjectEvent(animations.GetSupport());
            InjectEvent(animations.GetReceiveOffensive());
            InjectEvent(animations.GetReceiveSupport());
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
            animancer.Play(animations.GetIdle());
        }

        public IEnumerator<float> _DoAnimation(CombatingEntity user, List<CombatingEntity> targets, CombatSkill skill)
        {
            if (UtilsSkill.GetType(skill) == SSkillPreset.SkillType.Offensive)
            {
                _currentState = animancer.Play(animations.GetOffensive());
            }
            else
            {
                _currentState = animancer.Play(animations.GetSupport());
            }

            CharacterAnimatorHandler.DoReactAnimation(user,targets,skill);
            yield return Timing.WaitUntilTrue(_isAnimationFinish);
            animancer.Play(animations.GetIdle());
            _currentState = null;
        }

        public void ReceiveSupport(CombatingEntity actor, CombatingEntity receiver, CombatSkill skill)
        {
            animancer.Play(animations.GetReceiveSupport());
            _currentState = null;
        }

        public void ReceiveAttack(CombatingEntity actor, CombatingEntity receiver, CombatSkill skill)
        {
            animancer.Play(animations.GetReceiveOffensive());
            _currentState = null;
        }
    }

    public  abstract class SCombatAnimationsStates : ScriptableObject, CombatAnimationsBasic<AnimancerState>
    {
        public abstract AnimancerState GetIdle();
        public abstract AnimancerState GetOffensive();
        public abstract AnimancerState GetSupport();
        public abstract AnimancerState GetReceiveSupport();
        public abstract AnimancerState GetReceiveOffensive();
        public abstract void Injection(AnimancerComponent animancer);
    }

    public interface CombatAnimationsBasic<out T>
    {
        T GetIdle();
        T GetOffensive();
        T GetSupport();
        T GetReceiveSupport();
        T GetReceiveOffensive();

        void Injection(AnimancerComponent animancer);
    }
}
