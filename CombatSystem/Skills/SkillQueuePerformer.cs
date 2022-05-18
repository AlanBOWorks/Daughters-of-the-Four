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

        private CoroutineHandle _queueHandle;


        public void OnCombatEnd()
        {
            Timing.KillCoroutines(_queueHandle);
        }

        public void OnCombatFinish(bool isPlayerWin)
        {
        }

        public void OnCombatQuit()
        {
        }




        public void OnCombatSkillSubmit(in SkillUsageValues values)
        {
            _usedSkillsQueue.Enqueue(values);

            if(_queueHandle.IsRunning) return;
            _queueHandle = Timing.RunCoroutine(_DoQueue());
        }

        public void OnCombatSkillPerform(in SkillUsageValues values)
        {
        }

        public void OnCombatEffectPerform(in CombatEntity performer, in CombatEntity target, in PerformEffectValues values)
        {
        }

        public void OnCombatSkillFinish(in CombatEntity performer)
        {
            
        }


       

        [ShowInInspector] 
        public float DebugTimingOffset = .02f;

        private const float SkillAppliesAfter = .7f;
        private const float AnimationOffsetDuration = .3f;
        public const float MaxAnimationDuration = AnimationOffsetDuration + .3f;
        private IEnumerator<float> _DoQueue()
        {
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            var animator = CombatSystemSingleton.CombatAnimationHandler;
            while (_usedSkillsQueue.Count > 0)
            {
#if UNITY_EDITOR
                yield return Timing.WaitForSeconds(DebugTimingOffset);
#endif
                var queueValues = _usedSkillsQueue.Dequeue();
                queueValues.Extract(out var performer,out var target,out var usedSkill);
                eventsHolder.OnCombatSkillPerform(in queueValues);


                animator.PerformActionAnimation(in performer,in usedSkill, in target);
                yield return Timing.WaitForSeconds(SkillAppliesAfter);
                animator.PerformReceiveAnimations(in usedSkill, in performer);
                yield return Timing.WaitForSeconds(AnimationOffsetDuration);
              
                eventsHolder.OnCombatSkillFinish(in performer);
                yield return Timing.WaitForOneFrame; //safeWait
            }
        }
    }
}
