using System;
using System.Collections.Generic;
using _Enemies;
using _Player;
using _Team;
using Characters;
using MEC;
using Sirenix.OdinInspector;
using SMaths;
using Stats;
using UnityEngine;

namespace _CombatSystem
{
    public class TempoTicker : ICombatFullListener
    { public TempoTicker()
        {
            int memoryAllocation = UtilsCharacter.PredictedAmountOfCharactersInBattle;

            CombatConditionChecker
                = new CombatConditionsChecker();
            _roundTracker
                = new List<CombatingEntity>(memoryAllocation);
            _tickers
                = new Queue<ITempoTicker>();

            _fillingEntities = new List<CombatingEntity>(memoryAllocation);
            TempoFullEntities = new Queue<CombatingEntity>(memoryAllocation);

            Sequencer = new TempoEventsSequencer(this);

            CombatSystemSingleton.CombatConditionChecker = CombatConditionChecker;
        }

        public enum TickType
        {
            OnBeforeSequence,
            OnAction,
            OnAfterSequence,
            OnRound
        }

        private CharacterArchetypesList<CombatingEntity> _characters;

        [HideInEditorMode,HideInPlayMode]
        public readonly CombatConditionsChecker CombatConditionChecker;

        [ShowInInspector, DisableInEditorMode] public readonly TempoEventsSequencer Sequencer;

        [ShowInInspector]
        private readonly Queue<ITempoTicker> _tickers;

        // This exits so not acted yet entities have higher priority in the Queue (checks from the start to end of the list)
        [ShowInInspector] 
        private readonly List<CombatingEntity> _fillingEntities;

        /// <summary>
        /// Used to track which [<see cref="CombatingEntity"/>]s are active for Actions. Once some entity reach
        /// the initiative check it will be saved in here and this ticker will be 'paused' indirectly (by WaitUntil).
        /// <br></br>
        /// This Queue is shared with [<seealso cref="TempoEventsSequencer"/>] which will perform all
        /// Actions until all entities have no longer actions to do.
        /// <br></br>
        /// <br></br>
        /// Then this ticker will resume once the [<seealso cref="TempoEventsSequencer.SequenceHandle"/>] is over.
        /// </summary>
        [ShowInInspector] 
        public readonly Queue<CombatingEntity> TempoFullEntities;

        /// <summary>
        /// A list of characters remaining of a Round to be completed; A round is considered
        /// as an unit of all [<see cref="CombatingEntity"/>]s acted at least once.<br></br>
        /// (Fast characters can act multiple times in a Round)
        /// </summary>
        private readonly List<CombatingEntity> _roundTracker;

        [Range(0, 10), SuffixLabel("deltas")]
        public float TempoStepModifier = 1f;

        public void Subscribe(ITempoTicker ticker)
        {
            _tickers.Enqueue(ticker);
        }


        public void OnBeforeStart(
            CombatingTeam playerEntities,
            CombatingTeam enemyEntities,
            CharacterArchetypesList<CombatingEntity> allEntities)
        {
            CombatConditionChecker.OnAfterPreparation(playerEntities,enemyEntities,allEntities);
            _characters = allEntities;
            AddEntitiesToFillingInitiative(allEntities);
            RefillRoundTracker();

            TempoStepModifier = CombatSystemSingleton.ParamsVariable.TempoVelocityModifier;
        }

        private const float InitiativeCheck = GlobalCombatParams.InitiativeCheck;
        private const float VelocityModifier = GlobalCombatParams.TempoVelocityModifier;
        public CoroutineHandle TickHandle;

        /// <summary>
        /// The amount of second each step waits to be done (used for waits)
        /// </summary>
        public const float DeltaStepPeriod = .2f;
        /// <summary>
        /// The amount of step increment for each [<see cref="DeltaStepPeriod"/>]
        /// </summary>
        private const float DeltaStepFrequency = 1/DeltaStepPeriod; //sec
        private IEnumerator<float> _Tick()
        {
            // >>>> PREPARATIONS
            yield return Timing.WaitForOneFrame; //To ensure that everything is initialized
            
            ForcedBarUpdate();

            // >>>> COMBAT LOOP
            while (_characters != null)
            {
                float deltaIncrement = Time.deltaTime * TempoStepModifier * DeltaStepFrequency;
                yield return Timing.WaitForSeconds(DeltaStepPeriod);

                InvokeListeners();
                TickEntitiesInitiative();

                if (HasActingEntities())
                {
                    //Wait until the sequencer is finish all entities actions to return to the ticking
                    yield return Timing.WaitUntilDone(Sequencer._CombatSequence());
                    DoCheckIfRoundPassed();
                }


                void InvokeListeners()
                {
                    foreach (ITempoTicker ticker in _tickers)
                    {
                        ticker.TempoTick(deltaIncrement);
                    }
                }

                void TickEntitiesInitiative()
                {
                    for (var i = 0; i < _fillingEntities.Count; i++)
                    {
                        CombatingEntity entity = _fillingEntities[i];
                        if(!entity.IsConscious()) continue;

                        // Increment
                        CombatStatsHolder stats = entity.CombatStats;
                        IFullStats<float> baseStats = entity.CombatStats.BaseStats;

                        float initiative = baseStats.InitiativePercentage;
                        initiative += deltaIncrement * stats.SpeedAmount * VelocityModifier;
                        baseStats.InitiativePercentage = initiative;

                        // Percentage
                        CallUpdateOnInitiativeBar(entity,stats);
                        CheckAndInjectEntityInitiative(entity);
                    }
                }
                
                
                
            }
        }

        private bool HasActingEntities() => TempoFullEntities.Count > 0;
        private void AddEntitiesToFillingInitiative(List<CombatingEntity> entities)
        {
            if (_fillingEntities.Count > 0) _fillingEntities.Clear();
            _fillingEntities.AddRange(entities);
        }


        public void CheckAndInjectEntityInitiative(CombatingEntity entity)
        {
            if (TempoFullEntities.Contains(entity)) return;

            IFullStatsData<float> stats = entity.CombatStats;
            float initiative = stats.InitiativePercentage;

            if (initiative < InitiativeCheck) return;

            // Add to to acting Queue
            TempoFullEntities.Enqueue(entity);

            //Remove at the position and adds in the end
            _fillingEntities.Remove(entity);
            _fillingEntities.Add(entity);

            //Remove from the round tracker
            _roundTracker.Remove(entity);
        }

        private void DoCheckIfRoundPassed()
        {
            if (_roundTracker.Count <= 1) //this means is the last one
            {
                var entity = _roundTracker[0];
                RefillRoundTracker();
                Sequencer.OnRoundCompleteSequence(_characters, entity);
            }
            
        }

        public static void CallUpdateOnInitiativeBar(CombatingEntity entity)
        {
            var stats = entity.CombatStats;
            CallUpdateOnInitiativeBar(entity,stats);
        }

        private static void CallUpdateOnInitiativeBar(CombatingEntity entity, ITemporalStatsData<float> stats)
        {
            float initiativePercentage = stats.InitiativePercentage;
            var fillers = CombatSystemSingleton.TeamsPersistentElements[entity].TempoFillers;
            UpdateFillers(fillers,initiativePercentage);
        }
        private static void UpdateFillers(List<ITempoFiller> fillers, float percentage)
        {
            for (var i = 0; i < fillers.Count; i++)
            {
                ITempoFiller filler = fillers[i];
                filler.FillBar(percentage);
            }
        }
        private static void ForcedBarUpdate()
        {
            var fillers = CombatSystemSingleton.TeamsPersistentElements;
            foreach (var element in fillers)
            {
                float percentage = element.Key.CombatStats.InitiativePercentage;
                UpdateFillers(element.Value.TempoFillers, percentage);
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
            Timing.KillCoroutines(TickHandle);
            TickHandle = Timing.RunCoroutine(_Tick());
        }

        public void OnCombatFinish(CombatingEntity lastEntity, bool isPlayerWin)
        {
            Timing.KillCoroutines(TickHandle);
            TempoFullEntities.Clear();
            _fillingEntities.Clear();
        }

        public void DoPause() => OnCombatPause();
        public void OnCombatPause()
        {
            if(HasActingEntities()) return;
            Timing.PauseCoroutines(TickHandle);
        }

        public void DoResume() => OnCombatResume();
        public void OnCombatResume()
        {
            if(HasActingEntities()) return;
            Timing.ResumeCoroutines(TickHandle);
        }
    }


    

    public interface ITempoTypes<out T> : ICharacterEventListener
    {
        T OnBeforeSequence { get; }
        T OnAction { get; }
        T OnSequence { get; }
        T OnRound { get; }
    }

    public interface ITempoTriggerHandler : ITempoFullListener, ISkippedTempoListener
    {
        List<ITempoListener> TempoListeners { get; }
        List<IRoundListener> RoundListeners { get; }
        List<ISkippedTempoListener> SkippedListeners { get; }
    }
    public interface ITempoFullListener : ITempoListener, IRoundListener
    { }

    public interface ISkippedTempoListener : ICharacterEventListener
    {
        void OnSkippedEntity(CombatingEntity entity);
    }

    public interface ITempoListener : ICharacterEventListener
    {
        void OnInitiativeTrigger(CombatingEntity entity);
        void OnDoMoreActions(CombatingEntity entity);
        void OnFinisAllActions(CombatingEntity entity);
    }
    

    public interface ITempoTicker 
    {
        void TempoTick(float deltaVariation);
    }

    public interface ITempoFillerFull : ITempoFiller
    {
        void OnFullBar();
    }
    public interface ITempoFiller
    {
        void FillBar(float percentage);
    }

    public interface IRoundListener : ICharacterEventListener
    {
        void OnRoundCompleted(List<CombatingEntity> allEntities, CombatingEntity lastEntity);
    }
}
