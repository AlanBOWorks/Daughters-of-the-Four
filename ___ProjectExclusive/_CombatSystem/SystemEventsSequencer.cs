using System;
using System.Collections.Generic;
using Characters;
using MEC;
using Skills;
using Stats;
using UnityEngine;

namespace _CombatSystem
{
    internal class CombatInvokeSequencer : TempoEvents, ITempoHandlerSequencer
    {
        private CoroutineHandle _currentSequence;


        public void StartSequence(CombatingEntity entity)
        {
            //these are invoked here because eventHolder doesn't caches the [CombatEntity]
            _currentSequence = Timing.RunCoroutineSingleton(
                _StartingSequence(), _currentSequence, SingletonBehavior.Wait);

            IEnumerator<float> _StartingSequence()
            {
                //// FATE Skills
                var fateSkills = entity.FateSkills;
                yield return Timing.WaitUntilDone(fateSkills.InvokeFateSkills());

                SystemActions();
                EntityActions();
                EventActions();
            }

            void SystemActions()
            {
                CombatSystemSingleton.PerformSkillHandler.ResetOnInitiative();
                CombatSystemSingleton.ControllersHandler.CallForControl(entity);
            }

            void EntityActions()
            {
                entity.DelayBuffHandler.OnInitiativeTrigger();
                entity.CharacterCriticalBuff.OnInitiativeTrigger();

                CombatSystemSingleton.CombatEventsInvoker.OnInitiativeTrigger(entity);
            }

            void EventActions()
            {
                OnInitiativeTrigger(entity);
            }
        }

        public void DoMoreActionsSequence(CombatingEntity entity)
        {
            EntityActions();
            EventActions();
            SystemActions();


            void EntityActions()
            {
                entity.CombatSkills.ReduceCooldown();
                entity.DelayBuffHandler.OnDoMoreActions();
                CombatSystemSingleton.CombatEventsInvoker.OnDoMoreActions(entity);
            }
            void EventActions()
            {
                OnDoMoreActions(entity);
            }

            void SystemActions()
            {
                CombatSystemSingleton.ControllersHandler.CallForControl(entity);
            }
        }

        public void FinishSequence(CombatingEntity entity)
        {
            EntityActions();
            SystemActions();
            EventActions();


            void EntityActions()
            {
                entity.CombatStats.ResetBurst();
                entity.ReceivedStats.ResetToZero();
                entity.PassivesHolder.ResetOnFinish();
            }
            void SystemActions()
            {
                CombatSystemSingleton.PerformSkillHandler.ResetOnFinish();
            }
            void EventActions()
            {
                OnFinisAllActions(entity);
            }

        }

        public void SkipSequence(CombatingEntity entity)
        {
            SystemActions();
            EventActions();


            void EventActions()
            {
                OnSkippedEntity(entity);
            }

            void SystemActions()
            {
                CombatSystemSingleton.PerformSkillHandler.ResetOnFinish();
            }
        }

        public void OnRoundCompleteSequence(List<CombatingEntity> allEntities, CombatingEntity lastEntity)
        {
            OnRoundCompleted(allEntities,lastEntity);
        }

    }
}
