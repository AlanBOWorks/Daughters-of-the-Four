using System;
using System.Collections.Generic;
using _Team;
using Characters;
using MEC;
using Skills;
using Stats;
using UnityEngine;

namespace _CombatSystem
{
    public class TempoEventsSequencer : TempoEvents
    {
        public TempoEventsSequencer(TempoTicker tempoTicker)
        {
            _requestedEntities = tempoTicker.TempoFullEntities;
        }

        private readonly Queue<CombatingEntity> _requestedEntities;
        private PerformedSkill _performedSkill;
        private Func<bool> _checkPerformedSkillValid;


        public IEnumerator<float> _CombatSequence()
        {
            _performedSkill = CombatSystemSingleton.PerformSkillHandler.TrackedDoSkill;
            _checkPerformedSkillValid = _performedSkill.IsValid;

            while (_requestedEntities.Count > 0)
            {
                var entity = _requestedEntities.Dequeue();
                var stats = entity.CombatStats;

                CombatSystemSingleton.CurrentActingEntity = entity;

                //// FATE Skills
                var fateSkills = entity.FateSkills;
                yield return Timing.WaitUntilDone(fateSkills.InvokeFateSkills());
                AfterFateActions();

                if (!entity.CanAct())
                {
                    DoSkipActions();
                }
                else
                {
                    // MAIN/Choice Actions
                    do
                    {
                        DoMoreActions();
                        yield return Timing.WaitUntilTrue(_checkPerformedSkillValid);
                        yield return
                            Timing.WaitUntilDone(CombatSystemSingleton.PerformSkillHandler._DoPerformedSkill());

                        stats.ActionsSubtraction();
                    } while (stats.HasActionLeft());
                }
                OnFinishActions();


                void AfterFateActions()
                {
                    stats.RefillInitiativeActions();
                    entity.DelayBuffHandler.OnInitiativeTrigger(entity);
                    entity.CharacterCriticalBuff.OnInitiativeTrigger(entity);

                    CombatSystemSingleton.PerformSkillHandler.ResetOnInitiative();
                    CombatSystemSingleton.ControllersHandler.CallForControl(entity);

                    CombatSystemSingleton.CombatEventsInvoker.OnInitiativeTrigger(entity);
                
                    foreach (ITempoListener listener in TempoListeners)
                    {
                        listener.OnInitiativeTrigger(entity);
                    }
                }

                void DoMoreActions()
                {
                    

                    entity.CombatSkills.ReduceCooldown();
                    entity.DelayBuffHandler.OnDoMoreActions(entity);
                    CombatSystemSingleton.CombatEventsInvoker.OnDoMoreActions(entity);

                    foreach (ITempoListener listener in TempoListeners)
                    {
                        listener.OnDoMoreActions(entity);
                    }

                    CombatSystemSingleton.ControllersHandler.CallForControl(entity);
                }

                void OnFinishActions()
                {
                    stats.ResetInitiativePercentage();
                    stats.ResetActionsAmount();
                    entity.CombatStats.ResetBurst();
                    entity.ReceivedStats.ResetToZero();
                    entity.PassivesHolder.ResetOnFinish();

                    TempoTicker.CallUpdateOnInitiativeBar(entity);

                    CombatSystemSingleton.PerformSkillHandler.ResetOnFinish();
                    CombatSystemSingleton.CombatEventsInvoker.OnFinisAllActions(entity);

                    foreach (ITempoListener listener in TempoListeners)
                    {
                        listener.OnFinisAllActions(entity);
                    }

                    CombatSystemSingleton.CurrentActingEntity = null;
                }

                void DoSkipActions()
                {
                    foreach (ISkippedTempoListener listener in SkippedListeners)
                    {
                        listener.OnSkippedEntity(entity);
                    }
                    CombatSystemSingleton.PerformSkillHandler.ResetOnFinish();
                    CombatSystemSingleton.CombatEventsInvoker.OnFinisAllActions(entity);
                }
            }
        }


        public void OnRoundCompleteSequence(List<CombatingEntity> allEntities, CombatingEntity lastEntity)
        {
            foreach (IRoundListener listener in RoundListeners)
            {
                listener.OnRoundCompleted(allEntities, lastEntity);
            }
        }

        /// <summary>
        /// Is just [<see cref="OnDoMoreActions"/>] but just for
        /// [<seealso cref="Skills.PerformSkillHandler"/>]
        /// (so it more clear who does the step to the next Action OR calls the finish implicitly)
        /// </summary>
        /// <returns>If the Combat is finish</returns>
        public bool IsCombatFinish(CombatConditionsChecker.FinishState finishCheck, CombatingEntity lastEntity)
        {
            switch (finishCheck)
            {
                case CombatConditionsChecker.FinishState.PlayerWin:
                    CombatSystemSingleton.Invoker.OnCombatFinish(lastEntity, true);
                    return true;
                case CombatConditionsChecker.FinishState.EnemyWin:
                    CombatSystemSingleton.Invoker.OnCombatFinish(lastEntity, false);
                    return true;
            }

            return false;
        }

    }
}
