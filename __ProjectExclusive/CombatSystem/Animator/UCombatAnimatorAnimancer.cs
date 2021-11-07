using System;
using System.Collections.Generic;
using Animancer;
using CombatSkills;
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

        [SerializeField] private SCombatAnimationsHolder actionAnimations;



        public override IEnumerator<float> _DoPerformSkillAnimation(SkillValuesHolders skillValues)
        {
            throw new System.NotImplementedException();
        }

        public override void _DoReceiveSkillAnimation(SkillValuesHolders skillValues)
        {
            throw new System.NotImplementedException();
        }
    }
}
