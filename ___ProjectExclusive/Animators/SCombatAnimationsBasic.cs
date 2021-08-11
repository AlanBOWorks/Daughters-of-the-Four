using System;
using System.Collections.Generic;
using Animancer;
using Characters;
using MEC;
using Skills;
using UnityEngine;

namespace ___ProjectExclusive.Animators
{
    [CreateAssetMenu(fileName = "CombatAnimations Basic - N [Variable]",
        menuName = "Variable/Combat Animations/Basic")]
    public class SCombatAnimationsBasic : SCombatAnimationsStates
    {
        [SerializeField] private AnimationClip combatIdleClip;
        [SerializeField] private AnimationClip selfSupportClip;
        [SerializeField] private AnimationClip offensiveClip;
        [SerializeField] private AnimationClip supportClip;
        [SerializeField] private AnimationClip receiveSupportClip;
        [SerializeField] private AnimationClip receivingOffensiveClip;

        private AnimancerState _idleState;
        public override AnimancerState IdleAnimation => _idleState;

        private AnimancerState _selfSupportState;
        public override AnimancerState SelfSupportAnimation => _selfSupportState;

        private AnimancerState _offensiveState;
        public override AnimancerState OffensiveAnimation => _offensiveState;

        private AnimancerState _supportState;
        public override AnimancerState SupportAnimation => _supportState;

        private AnimancerState _receiveSupportState;
        public override AnimancerState ReceiveSupportAnimation => _receiveSupportState;

        private AnimancerState _receiveOffensiveState;
        public override AnimancerState ReceiveOffensiveAnimation => _receiveOffensiveState;

        public override void Injection(AnimancerComponent animancer)
        {
            _idleState = GenerateState(combatIdleClip);
            _selfSupportState = GenerateState(selfSupportClip);
            _offensiveState = GenerateState(offensiveClip);
            _supportState = GenerateState(supportClip);
            _receiveSupportState = GenerateState(receiveSupportClip);
            _receiveOffensiveState = GenerateState(receivingOffensiveClip);

            AnimancerState GenerateState(AnimationClip clip)
            {
                if (clip == null) return null;
                return animancer.States.Create(clip);
            }
        }
    }
}
