using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

namespace CombatSystem.Animations
{
    public static class UtilsAnimations
    {
        public const int IdleAnimationLayer = 0;
        public const int ActionAnimationLayer = 1;

        public static AnimancerLayer GetIdleLayer(AnimancerComponent animancer) 
            => animancer.Layers[IdleAnimationLayer];
        public static AnimancerLayer GetActionLayer(AnimancerComponent animancer) 
            => animancer.Layers[ActionAnimationLayer];

        public static AnimancerState GetIdleState(AnimancerComponent animancer,AnimationClip clip)
        {
            return animancer.Layers[IdleAnimationLayer].GetOrCreateState(clip);
        }

        public static AnimancerState GetActionState(AnimancerComponent animancer, AnimationClip clip)
        {
            return animancer.Layers[ActionAnimationLayer].GetOrCreateState(clip);
        }
    }
}

