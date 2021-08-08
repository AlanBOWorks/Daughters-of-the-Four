using System;
using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ___ProjectExclusive.Animators
{
    public class UHairAdditionAnimator : MonoBehaviour
    {
        [SerializeField,HideInPlayMode] private Parameters parameters = new Parameters();

        [ShowInInspector,HideInEditorMode]
        public MixerState.Transition2D HairState { get; private set; }
        private void Start()
        {
            HairState = parameters.StartAnimations();
            parameters = null;
        }

        [Serializable]
        internal class Parameters
        {
            [SerializeField] private AnimancerComponent hairAnimancer;
            [SerializeField] private MixerTransition2D windAdditions;

            [SerializeField,Min(0), 
             InfoBox("This [Layer] will become additive",InfoMessageType.Warning)] 
            private int targetLayer = 1;
            public MixerState.Transition2D StartAnimations()
            {
                var animationLayer = hairAnimancer.Layers[targetLayer];
                animationLayer.Play(windAdditions);
                animationLayer.IsAdditive = true;
                animationLayer.SetWeight(1);
                return windAdditions.Transition;
            }
        }
    }
}
