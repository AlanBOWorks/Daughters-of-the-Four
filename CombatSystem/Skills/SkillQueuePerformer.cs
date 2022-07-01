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
        private const float SkillFinishAfter = SkillAppliesAfter + AnimationOffsetDuration;

        protected override IEnumerator<float> _DoDeQueue()
        {
            var eventsHolder = CombatSystemSingleton.EventsHolder;

            yield return Timing.WaitForOneFrame; //safeWait

            while (Queue.Count > 0)
            {

                var queueValues = Queue.Dequeue();
                eventsHolder.OnCombatSkillPerform(in queueValues);


                yield return Timing.WaitForSeconds(SkillFinishAfter);
                eventsHolder.OnCombatSkillFinish(queueValues.Performer);
                yield return Timing.WaitForOneFrame; //safeWait
            }
        }
    }
}
