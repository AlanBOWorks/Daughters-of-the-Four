using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;
using UnityEngine;

namespace CombatSystem.Animations
{
    public sealed class CombatAnimationHandler : ITempoEntityStatesListener
    {
        private static ICombatEntityAnimator GetAnimator(in CombatEntity entity) => entity.Body.GetAnimator();

        public void PerformActionAnimation(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
            var animator = GetAnimator(in performer);
            animator.PerformActionAnimation(in usedSkill, in target);
        }


        public void PerformReceiveAnimations(in CombatSkill usedSkill, in CombatEntity performer)
        {
            var interactions = CombatSystemSingleton.SkillTargetingHandler.GetInteractions();
            foreach (var entity in interactions)
            {
                PerformReceiveAnimation(in entity, in usedSkill, in performer);
            }
        }
        private void PerformReceiveAnimation(in CombatEntity target, in CombatSkill usedSkill, in CombatEntity performer)
        {
            var targetAnimator = GetAnimator(in target);
            targetAnimator.ReceiveActionAnimation(in usedSkill, in performer);
        }


        public void OnEntityRequestSequence(CombatEntity entity, bool canControl)
        {
            var animator = GetAnimator(in entity);
            animator.OnRequestSequenceAnimation();
        }

        public void OnEntityRequestAction(CombatEntity entity)
        {
            
        }

        public void OnEntityBeforeSkill(CombatEntity entity)
        {
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
           
        }

        public void OnEntityEmptyActions(CombatEntity entity)
        {
        }

        public void OnEntityFinishSequence(CombatEntity entity, in bool isForcedByController)
        {
            var animator = GetAnimator(in entity);
            animator.OnEndSequenceAnimation();
        }
    }
}
