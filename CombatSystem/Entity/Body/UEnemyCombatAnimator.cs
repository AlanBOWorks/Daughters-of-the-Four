using Animancer;
using CombatSystem.Entity;
using CombatSystem.Skills;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Animations
{
    public class UEnemyCombatAnimator : UCombatEntityAnimator
    {
        [SerializeField] private AnimancerComponent animancer;
        [SerializeField, SuffixLabel("s"),Range(0,1)] private float idleFade = .2f;
        [SerializeField, SuffixLabel("s"),Range(0,1)] private float actionFade = .12f;

        [SerializeField, InlineEditor()] private SCombatAnimationsBasic animations;

        private const int MainAnimationLayer = 0;

        private void Animate(AnimationClip clip, float fade)
        {
            if(!clip) return;
            var targetLayer = animancer.Layers[MainAnimationLayer];

            if(targetLayer.CurrentState != null) targetLayer.Stop();
            targetLayer.Play(clip, fade);
        }

        public override void PerformInitialCombatAnimation()
        {
            var initialAnimation = animations.InitialAnimationType;
            if(initialAnimation == null) return;

            Animate(initialAnimation, idleFade);
        }

        public override void PerformEndCombatAnimation()
        {
        }

        public override void OnRequestSequenceAnimation()
        {
            var activeIdle = animations.GetActiveClip();
            Animate(activeIdle, idleFade);
        }
        public override void OnEndSequenceAnimation()
        {
            var idle = animations.GetIdleClip();
            Animate(idle, idleFade);
        }

        protected override void DoPerformActionAnimation(in EnumsSkill.Archetype type)
        {
            var clip = UtilsSkill.GetElement(type, animations.AnimationPerformType);
            Animate(clip, actionFade);
        }

        protected override void DoReceiveActionAnimation(in EnumsSkill.Archetype type)
        {
            var clip = UtilsSkill.GetElement(type, animations.AnimationReceiveType);
            Animate(clip, actionFade);
        }
    }
}
