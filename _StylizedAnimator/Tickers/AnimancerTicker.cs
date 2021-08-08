using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace StylizedAnimator
{
    public class AnimancerTicker : TickerBase
    {
        private bool _enabled = true;
        [ShowInInspector]
        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                Animancer.Playable.Speed = value ? 0 : 1;
            }
        }

        protected readonly AnimancerComponent Animancer;
        public AnimancerTicker(AnimancerComponent animancer)
        {
            Animancer = animancer;
            Enabled = true;
        }

        public override void DoTick(float deltaVariation)
        {
            if(!_enabled) return;
            for (var i = 0; i < Animancer.Layers.Count; i++)
            {
                AnimancerLayer layer = Animancer.Layers[i];
                var state = layer.CurrentState;
                state.Time += deltaVariation;
            }

        }
    }
}
