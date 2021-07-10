using System;
using System.Collections.Generic;
using Characters;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CombatSystem
{
    public class TempoHandler : ICombatFullListener
    {
        private CharacterArchetypesList<CombatingEntity> _characters;
        [Range(0,10),SuffixLabel("deltas")]
        public float TempoModifier = 1f;

        [ShowInInspector, DisableInPlayMode] private ITempoTriggerHandler _triggerHandler;

        public readonly Dictionary<CombatingEntity, ITempoFiller> EntitiesBar;

        /// <summary>
        /// A list of characters remaining of a Round to be completed; A round is considered
        /// as an unit of all [<see cref="CombatingEntity"/>]s acted at least once.<br></br>
        /// (Fast characters can act multiple times in a Round)
        /// </summary>
        private readonly List<CombatingEntity> _roundTracker;

        public void Subscribe(ITempoListener listener)
        {
            _triggerHandler.Subscribe(listener);
        }

        public void Subscribe(IRoundListener listener)
        {
            _triggerHandler.Subscribe(listener);
        }

        public TempoHandler(ITempoTriggerHandler handler)
        {
            _triggerHandler = handler;
            int memoryAllocation = CharacterUtils.PredictedAmountOfCharactersInBattle;
            EntitiesBar = new Dictionary<CombatingEntity, ITempoFiller>(
                memoryAllocation);
            _roundTracker = new List<CombatingEntity>(memoryAllocation);
        }
        public void OnBeforeStart(
            CombatingTeam playerEntities,
            CombatingTeam enemyEntities,
            CharacterArchetypesList<CombatingEntity> allEntities)
        {
            _characters = allEntities;
            RefillRoundTracker();
        }


        private bool _entityPaused;
        private void OnEntityPause()
        {
            _entityPaused = true;
            Timing.PauseCoroutines(_loopHandle);
        }
        /// <summary>
        /// Used to resume [<see cref="_Tick"/>] (which was paused by an [<seealso cref="CombatingEntity"/>] when
        /// it reaches its top [<seealso cref="ICombatTemporalStats.InitiativePercentage"/>])
        /// </summary>
        public void ResumeFromTempoTrigger()
        {
            _entityPaused = false;
            Timing.ResumeCoroutines(_loopHandle);
        }


        private CoroutineHandle _loopHandle;
        private IEnumerator<float> _Tick()
        {
            yield return Timing.WaitForOneFrame; //To ensure that everything is initialized
            while (_characters != null)
            {
                float initiativeCheck = GlobalCombatParams.InitiativeCheck;
                float deltaIncrement = Time.deltaTime * TempoModifier;
                foreach (CombatingEntity entity in _characters)
                {
                    CharacterCombatData stats = entity.CombatStats;

                    IncreaseInitiative(); void IncreaseInitiative()
                    {
                        stats.InitiativePercentage += deltaIncrement * stats.SpeedAmount;

                        float initiativePercentage = SMaths.SRange.Percentage(
                            stats.InitiativePercentage,
                            0, initiativeCheck);
                        EntitiesBar[entity].FillBar(initiativePercentage);
                    }

                    if (stats.InitiativePercentage < initiativeCheck) continue;

                    // Invoke Triggers and LOOP
                    OnInitiativeTrigger(entity);
                    //TODO make it as a dedicated class/handler (CharacterActionsHandler)
                    LoopAllActions(); void LoopAllActions()
                    {
                        stats.RefillInitiativeActions();
                        while (stats.ActionsLefts > 0)
                        {
                            OnEntityPause();
                            OnActionDone(entity);
                            stats.ActionsLefts--;
                        }
                        stats.ActionsLefts = 0;
                        stats.InitiativePercentage = 0;
                    }
                    
                    // Finish LOOP
                    OnFinishTrigger(entity);
                    DoCheckIfRoundPassed(); void DoCheckIfRoundPassed()
                    {
                        if (!_roundTracker.Contains(entity)) return;
                        if (_roundTracker.Count <= 1) //this means is the last one
                        {
                            RefillRoundTracker();
                            _triggerHandler.OnRoundCompleted(_characters, entity);
                        }
                        else
                        {
                            _roundTracker.Remove(entity);
                        }
                    }
                }

                yield return Timing.WaitForSeconds(deltaIncrement);
            }

            

            void OnInitiativeTrigger(CombatingEntity entity)
            {
                _triggerHandler.OnInitiativeTrigger(entity);

#if UNITY_EDITOR
                Debug.Log($"Character TEMPO - {entity.CharacterName}"); 
#endif
            }

            void OnActionDone(CombatingEntity entity)
            {
                _triggerHandler.OnActionDone(entity);
            }

            void OnFinishTrigger(CombatingEntity entity)
            {
                _triggerHandler.OnFinisAllActions(entity);
            }

            
        }

        private void RefillRoundTracker()
        {
            _roundTracker.Clear();
            foreach (CombatingEntity entity in _characters)
            {
                _roundTracker.Add(entity);
            }
        }

        public void OnCombatStart()
        {
            Timing.KillCoroutines(_loopHandle);
            _loopHandle = Timing.RunCoroutine(_Tick());
        }

        public void OnCombatFinish(CombatingTeam removeEnemies)
        {
            Timing.KillCoroutines(_loopHandle);
            EntitiesBar.Clear();
        }

        public void OnCombatPause()
        {
            if(_entityPaused) return;
            Timing.PauseCoroutines(_loopHandle);
        }

        public void OnCombatResume()
        {
            if(_entityPaused) return;
            Timing.ResumeCoroutines(_loopHandle);
        }
    }

    public interface ITempoTriggerHandler : ITempoListener, IRoundListener
    {
        void Subscribe(ITempoListener listener);
        void Subscribe(IRoundListener listener);

    }

    public interface ITempoListener
    {
        void OnInitiativeTrigger(CombatingEntity entity);
        void OnActionDone(CombatingEntity entity);
        void OnFinisAllActions(CombatingEntity entity);
    }

    public interface ITempoFiller
    {
        void FillBar(float percentage);
    }

    public interface IRoundListener
    {
        void OnRoundCompleted(List<CombatingEntity> allEntities, CombatingEntity lastEntity);
    }
}
