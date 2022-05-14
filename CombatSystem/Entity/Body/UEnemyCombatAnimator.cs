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

        private void Animate(in AnimationClip clip, in float fade)
        {
            if(!clip) return;
            animancer.Play(clip, fade);
        }

        public override void OnRequestSequenceAnimation()
        {
            var activeIdle = animations.GetActiveClip();
            Animate(in activeIdle, in idleFade);
        }
        public override void OnEndSequenceAnimation()
        {
            var idle = animations.GetIdleClip();
            Animate(in idle, in idleFade);
        }

        protected override void DoPerformActionAnimation(in EnumsSkill.Archetype type)
        {
            var clip = UtilsSkill.GetElement(type, animations.AnimationPerformType);
            Animate(in clip, in actionFade);
        }

        protected override void DoReceiveActionAnimation(in EnumsSkill.Archetype type)
        {
            var clip = UtilsSkill.GetElement(type, animations.AnimationReceiveType);
            Animate(in clip, in actionFade);
        }
    }
}
