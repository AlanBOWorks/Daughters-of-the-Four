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
        [SerializeField] private AnimationClip offensiveClip;
        [SerializeField] private AnimationClip supportClip;
        [SerializeField] private AnimationClip receiveSupportClip;
        [SerializeField] private AnimationClip receivingOffensiveClip;

        private AnimancerState _idleState;
        public override AnimancerState GetIdle() => _idleState;

        private AnimancerState _offensiveState;
        public override AnimancerState GetOffensive() => _offensiveState;

        private AnimancerState _supportState;
        public override AnimancerState GetSupport() => _supportState;

        private AnimancerState _receiveSupportState;
        public override AnimancerState GetReceiveSupport() => _receiveSupportState;

        private AnimancerState _receiveOffensiveState;
        public override AnimancerState GetReceiveOffensive() => _receiveOffensiveState;

        public override void Injection(AnimancerComponent animancer)
        {
            _idleState = GenerateState(combatIdleClip);
            _offensiveState = GenerateState(offensiveClip);
            _supportState = GenerateState(supportClip);
            _receiveSupportState = GenerateState(receiveSupportClip);
            _receiveOffensiveState = GenerateState(receivingOffensiveClip);

            AnimancerState GenerateState(AnimationClip clip)
            {
                return animancer.States.Create(clip);
            }
        }
    }
}
