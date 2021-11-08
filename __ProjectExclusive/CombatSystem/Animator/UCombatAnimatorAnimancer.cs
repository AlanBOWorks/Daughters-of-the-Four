using System;
using System.Collections.Generic;
using Animancer;
using CombatEntity;
using CombatSkills;
using MEC;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CombatSystem.Animator
{
    public class UCombatAnimatorAnimancer : UCombatAnimationsHandler
    {
        [Title("References")]
        [SerializeField] private AnimancerComponent animancer;

        [SerializeField] private SCombatStandByAnimationsHolder stabByAnimations; 
        [SerializeField] private SCombatAnimationsHolder actionAnimations;

        private CombatingEntity _user;

        public override void DoIntroductionAnimation(CombatingEntity user)
        {
            _user = user;
            var state = animancer.Play(stabByAnimations.GetStartingCombatAnimation());

            state.Events.OnEnd += DoSwitchToIdle;
            void DoSwitchToIdle() => DoSwitchToIdleState(user);
        }

        public override void DoSwitchToIdleState(CombatingEntity user)
        {
            animancer.Play(stabByAnimations.GetIdleAnimation(user));
        }

        private AnimancerState PerformSkillAnimation(SkillValuesHolders skillValues)
        {
            var actionAnimation = actionAnimations.GetPerformActionAnimation(skillValues);
            var animancerState = animancer.Play(actionAnimation);

            animancerState.Time = 0;
            animancerState.Events.OnEnd = DoSwitchToIdle;

            return animancerState;

            void DoSwitchToIdle() => DoSwitchToIdleState(_user);
        }

        public override void DoPerformSkillAnimation(SkillValuesHolders skillValues)
        {
            PerformSkillAnimation(skillValues);
        }

        public override IEnumerator<float> _DoPerformSkillAnimation(SkillValuesHolders skillValues)
        {
            var animancerState = PerformSkillAnimation(skillValues);
            float animationTime = animancerState.Duration;

            yield return Timing.WaitForSeconds(animationTime);
        }

        public override void _DoReceiveSkillAnimation(SkillValuesHolders skillValues)
        {
            var receivingAnimation = actionAnimations.GetReceiveActionAnimation(skillValues);
            animancer.Play(receivingAnimation).Time = 0;
        }
    }
}
