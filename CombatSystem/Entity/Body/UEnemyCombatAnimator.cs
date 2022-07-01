using Animancer;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Skills.Effects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Animations
{
    public class UEnemyCombatAnimator : UCombatEntityAnimator
    {
        

        [SerializeField, InlineEditor()] private SCombatAnimationsBasic animations;

        private const int MainAnimationLayer = 0;

        private void Animate(AnimationClip clip, float fade)
        {
            if(!clip) return;
            var targetLayer = animancer.Layers[MainAnimationLayer];

            if(targetLayer.CurrentState != null) targetLayer.Stop();
            targetLayer.Play(clip, fade);
        }


        protected override AnimationClip GetInitialAnimationClip() => animations.InitialAnimationType;
        protected override AnimationClip GetIdleClip() => animations.GetIdleClip();
        protected override AnimationClip GetActiveIdleClip() => animations.GetActiveClip();


        public override void PerformEndCombatAnimation()
        {
        }

        protected override AnimationClip GetActionAnimation(ISkill skill, EnumsSkill.TeamTargeting type)
        {
            return UtilsSkill.GetElement(type, animations.AnimationPerformType);
        }

        protected override AnimationClip GetReceiveActionAnimation(ISkill skill, EnumsSkill.TeamTargeting type)
        {
            return UtilsSkill.GetElement(type, animations.AnimationReceiveType);
        }

        protected override AnimationClip GetReceiveActionAnimation(IEffect effect, EnumsSkill.TeamTargeting type)
        {
            return UtilsSkill.GetElement(type, animations.AnimationReceiveType);
        }
    }
}
