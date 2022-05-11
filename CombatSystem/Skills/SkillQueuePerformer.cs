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
            _usedSkillsQueue = new Queue<QueueValues>();
        }
        [ShowInInspector]
        private readonly Queue<QueueValues> _usedSkillsQueue;

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




        public void OnSkillSubmit(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
            QueueValues submit = new QueueValues(in performer, in usedSkill, in target);
            _usedSkillsQueue.Enqueue(submit);

            if(_queueHandle.IsRunning) return;
            _queueHandle = Timing.RunCoroutine(_DoQueue());
        }

        public void OnSkillPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
        }

        public void OnEffectPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target, in IEffect effect)
        {
        }

        public void OnSkillFinish(in CombatEntity performer)
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
                queueValues.Extract(out var performer, out var usedSkill, out var target);
                eventsHolder.OnSkillPerform(in performer, in usedSkill, in target);


                animator.PerformActionAnimation(in performer,in usedSkill, in target);
                yield return Timing.WaitForSeconds(SkillAppliesAfter);
                animator.PerformReceiveAnimation(in target, in usedSkill, in performer);
                yield return Timing.WaitForSeconds(AnimationOffsetDuration);
              
                eventsHolder.OnSkillFinish(in performer);
                yield return Timing.WaitForOneFrame; //safeWait
            }
        }


        private struct QueueValues
        {
            private readonly CombatEntity _performer;
            private readonly CombatEntity _target;
            private readonly CombatSkill _skill;

            public QueueValues(in CombatEntity performer, in CombatSkill combatSkill, in CombatEntity target)
            {
                _performer = performer;
                _target = target;
                _skill = combatSkill;
            }

            public readonly void Extract(out CombatEntity performer, out CombatSkill usedSkill, out CombatEntity target)
            {
                performer = _performer;
                usedSkill = _skill;
                target = _target;
            }
        }

    }
}
