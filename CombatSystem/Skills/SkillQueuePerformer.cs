using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Animations;
using CombatSystem.Entity;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Skills
{
    public sealed class SkillQueuePerformer : TempoQueuePerformer<SkillUsageValues>, ISkillUsageListener
    {
        public void OnCombatSkillSubmit(in SkillUsageValues values)
        {
            EnQueueValue(in values);
        }

        public void OnCombatSkillPerform(in SkillUsageValues values)
        {
        }
        public void OnCombatSkillFinish(CombatEntity performer)
        {
            
        }


       

        private const float SkillAppliesAfter = CombatControllerAnimationHandler.PerformToReceiveTimeOffset;
        private const float AnimationOffsetDuration = CombatControllerAnimationHandler.FromReceiveToFinishTimeOffset;

        protected override IEnumerator<float> _DoDeQueue()
        {
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            var animator = CombatSystemSingleton.CombatControllerAnimationHandler;

            yield return Timing.WaitForOneFrame; //safeWait

            while (Queue.Count > 0)
            {

                var queueValues = Queue.Dequeue();
                queueValues.Extract(out var performer,out var target,out var usedSkill);
                eventsHolder.OnCombatSkillPerform(in queueValues);


                animator.PerformActionAnimation(usedSkill, performer, target);
                yield return Timing.WaitForSeconds(SkillAppliesAfter);


                animator.PerformReceiveAnimations(usedSkill, performer);
                yield return Timing.WaitForSeconds(AnimationOffsetDuration);
              
                eventsHolder.OnCombatSkillFinish(performer);
                yield return Timing.WaitForOneFrame; //safeWait
            }
        }
    }
}
