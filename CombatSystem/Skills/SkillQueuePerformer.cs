using System.Collections.Generic;
using CombatSystem._Core;
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
       

        [ShowInInspector] 
        public float DebugTimingOffset = .02f;

        private const float SkillAppliesAfter = .5f;
        private const float ShortWaitDuration = .2f;
        private const float AnimationOffsetDuration = .3f;
        public const float MaxAnimationDuration = AnimationOffsetDuration + .3f;

        private CoroutineHandle _queueHandle;
        private IEnumerator<float> _DoQueue()
        {
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            var animator = CombatSystemSingleton.CombatControllerAnimationHandler;

            yield return Timing.WaitForOneFrame; //safeWait

            while (_usedSkillsQueue.Count > 0)
            {
#if UNITY_EDITOR
                yield return Timing.WaitForSeconds(DebugTimingOffset);
#endif

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
