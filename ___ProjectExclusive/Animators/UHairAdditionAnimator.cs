using System;
using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ___ProjectExclusive.Animators
{
    public class UHairAdditionAnimator : MonoBehaviour
    {
        [SerializeField] private Parameters parameters = new Parameters();

        public MixerState.Transition2D HairState { get; private set; }
        private void Start()
        {
            HairState = parameters.StartAnimations();
            parameters = null;
        }

        [Serializable]
        private class Parameters
        {
            [SerializeField] private AnimancerComponent hairAnimancer;
            [SerializeField] private MixerTransition2D windAdditions;

            [SerializeField,Min(0), 
             InfoBox("This [Layer] will become additive",InfoMessageType.Warning)] 
            private int targetLayer = 1;
            public MixerState.Transition2D StartAnimations()
            {
                var animationLayer = hairAnimancer.Layers[targetLayer];
                animationLayer.IsAdditive = true;
                animationLayer.GetOrCreateState(windAdditions);

                return windAdditions.Transition;
            }
        }
    }
}
