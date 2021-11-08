using System;
using System.Collections.Generic;
using CombatSkills;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace CombatSystem.Animator
{

    [CreateAssetMenu(fileName = "N [Combat Single Animations]",
        menuName = "Combat/Animations/Single Animations")]
    public class SCombatAnimationsSingle : SCombatAnimationsHolder
    {
        [SerializeField, HorizontalGroup()]
        private AnimationsHolder performActions = new AnimationsHolder();
        [SerializeField, HorizontalGroup()]
        private AnimationsHolder receiveActions = new AnimationsHolder();

        public override AnimationClip GetPerformActionAnimation(SkillValuesHolders skillValues)
        {
            return UtilSkills.GetElementSafe(performActions, skillValues);
        }

        public override AnimationClip GetReceiveActionAnimation(SkillValuesHolders skillValues)
        {
            return UtilSkills.GetElementSafe(receiveActions, skillValues);
        }



        [Serializable]
        private class AnimationsHolder : SkillInteractionsStructure<AnimationClip,AnimationClip>
        {
            
        }

    }
}
