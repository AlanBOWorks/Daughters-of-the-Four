using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Animations;
using CombatSystem.Entity;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Skills
{
    public sealed class SkillQueuePerformer : ISkillUsageListener, ICombatTerminationListener
    {
        public SkillQueuePerformer()
        {
            _usedSkillsQueue = new Queue<SkillUsageValues>();
        }
        [ShowInInspector]
        private readonly Queue<SkillUsageValues> _usedSkillsQueue;


        public void EnQueueValue(in SkillUsageValues values)
        {
            _usedSkillsQueue.Enqueue(values);

            if (_queueHandle.IsRunning) return;
            _queueHandle = Timing.RunCoroutine(_DoQueue());
        }

        public void OnCombatEnd()
        {
            Timing.KillCoroutines(_queueHandle);
            _usedSkillsQueue.Clear();
        }

        public void OnCombatFinish(bool isPlayerWin)
        {
        }

        public void OnCombatQuit()
        {
        }




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


        public bool IsActing() => _queueHandle.IsRunning;
       

        private const float SkillAppliesAfter = CombatControllerAnimationHandler.PerformToReceiveTimeOffset;
        private const float AnimationOffsetDuration = CombatControllerAnimationHandler.FromReceiveToFinishTimeOffset;

        private CoroutineHandle _queueHandle;
        private IEnumerator<float> _DoQueue()
        {
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            var animator = CombatSystemSingleton.CombatControllerAnimationHandler;

            yield return Timing.WaitForOneFrame; //safeWait

            while (_usedSkillsQueue.Count > 0)
            {

                var queueValues = _usedSkillsQueue.Dequeue();
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
