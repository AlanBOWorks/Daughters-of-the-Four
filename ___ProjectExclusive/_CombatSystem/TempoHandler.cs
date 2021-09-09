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
    public class TempoHandler : ICombatFullListener, ITempoHandlerSequencer
    { public TempoHandler()
        {
            int memoryAllocation = UtilsCharacter.PredictedAmountOfCharactersInBattle;
            _events 
                = new CombatEventsSequencer();
            CombatEventsSequencer = _events;

            CombatConditionChecker
                = new CombatConditionsChecker();
            _roundTracker
                = new List<CombatingEntity>(memoryAllocation);
            _tickers
                = new Queue<ITempoTicker>();

            _fillingEntities = new List<CombatingEntity>(memoryAllocation);
            _actingQueue = new Queue<CombatingEntity>(memoryAllocation);


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
        [Range(0,10),SuffixLabel("deltas")]
        public float TempoStepModifier = 1f;

        [HideInEditorMode,HideInPlayMode]
        public readonly CombatConditionsChecker CombatConditionChecker;

        [ShowInInspector, DisableInEditorMode] 
        private readonly CombatEventsSequencer _events;
        public readonly ITempoHandlerSequencer CombatEventsSequencer;

        [ShowInInspector]
        private readonly Queue<ITempoTicker> _tickers;

        [ShowInInspector] 
        private readonly List<CombatingEntity> _fillingEntities;

        [ShowInInspector] 
        private readonly Queue<CombatingEntity> _actingQueue;

        /// <summary>
        /// A list of characters remaining of a Round to be completed; A round is considered
        /// as an unit of all [<see cref="CombatingEntity"/>]s acted at least once.<br></br>
        /// (Fast characters can act multiple times in a Round)
        /// </summary>
        private readonly List<CombatingEntity> _roundTracker;


        public void Subscribe(ITempoListener listener)
        {
            _events.TempoListeners.Add(listener);
        }

        public void Subscribe(IRoundListener listener)
        {
            _events.RoundListeners.Add(listener);
        }

        public void Subscribe(ISkippedTempoListener listener)
        {
            _events.SkippedListeners.Add(listener);
        }

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
        public void StartSequence(CombatingEntity entity)
        {
            CombatEventsSequencer.StartSequence(entity);
        }
        public void DoMoreActionsSequence(CombatingEntity entity)
        {
            var currentStats = entity.CombatStats;
            currentStats.ActionsSubtraction();
            if (entity.CanAct())
            {
                CombatEventsSequencer.DoMoreActionsSequence(entity);
            }
            else
            {
                currentStats.ResetActionsAmount();
                FinishSequence(entity);
            }
        }
        public void FinishSequence(CombatingEntity entity)
        {
            CombatEventsSequencer.FinishSequence(entity);
            if (_loopHandle.IsRunning)
                StepNextActingEntity();

            // Do Harmony 
            // TODO entity.HarmonyBuffInvoker?.InvokeBurstStats();

            // End round?
            DoCheckIfRoundPassed(entity);
        }
        public void SkipSequence(CombatingEntity entity)
        {
            CombatEventsSequencer.SkipSequence(entity);
            
        }
        public void OnRoundCompleteSequence(List<CombatingEntity> allEntities, CombatingEntity lastEntity)
        {
            CombatEventsSequencer.OnRoundCompleteSequence(allEntities, lastEntity);
        }


        private bool HasActingEntities() => _actingQueue.Count > 0;
        private void AddEntitiesToFillingInitiative(List<CombatingEntity> entities)
        {
            if(_fillingEntities.Count > 0) _fillingEntities.Clear();
            _fillingEntities.AddRange(entities);
        }
        

        public void CheckAndInjectEntityInitiative(CombatingEntity entity)
        {
            IFullStatsData<float> stats = entity.CombatStats;
            float initiative = stats.InitiativePercentage;

            if (initiative < InitiativeCheck) return;

            // Add to to acting Queue
            _actingQueue.Enqueue(entity);
            _fillingEntities.Remove(entity);
        }

        private const float InitiativeCheck = GlobalCombatParams.InitiativeCheck;
        private const float VelocityModifier = GlobalCombatParams.TempoVelocityModifier;
        private CoroutineHandle _loopHandle;

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
                DoInitiativeTick();
                if (HasActingEntities())
                {
                    DequeueInvokeNextActor();
                    Timing.PauseCoroutines(_loopHandle);
                }


                void InvokeListeners()
                {
                    foreach (ITempoTicker ticker in _tickers)
                    {
                        ticker.TempoTick(deltaIncrement);
                    }
                }

                void DoInitiativeTick()
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

                        // Check
                        if (initiative < InitiativeCheck) continue;

                        // Add to to acting Queue
                        _actingQueue.Enqueue(entity);
                        _fillingEntities.RemoveAt(i);
                    }
                }
                
                
                
            }
        }


        private void DequeueInvokeNextActor()
        {
            var entity = _actingQueue.Dequeue();
            _fillingEntities.Add(entity);
            CombatStatsHolder stats = entity.CombatStats;

            stats.ResetInitiativePercentage();
            stats.RefillInitiativeActions();
            CallUpdateOnInitiativeBar(entity);


            if (!entity.CanUseSkills() || !entity.HasActions())
                SkipSequence(entity);
            else
                StartSequence(entity);
        }

        private void DoCheckIfRoundPassed(CombatingEntity entity)
        {
            if (!_roundTracker.Contains(entity)) return;
            if (_roundTracker.Count <= 1) //this means is the last one
            {
                RefillRoundTracker();
                OnRoundCompleteSequence(_characters, entity);
            }
            else
            {
                _roundTracker.Remove(entity);
            }
        }

        public void CallUpdateOnInitiativeBar(CombatingEntity entity)
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
            Timing.KillCoroutines(_loopHandle);
            _loopHandle = Timing.RunCoroutine(_Tick());
        }

        public void OnCombatFinish(CombatingEntity lastEntity, bool isPlayerWin)
        {
            FinishSequence(lastEntity);
            Timing.KillCoroutines(_loopHandle);
            _actingQueue.Clear();
            _fillingEntities.Clear();
        }

        public void DoPause() => OnCombatPause();
        public void OnCombatPause()
        {
            if(HasActingEntities()) return;
            Timing.PauseCoroutines(_loopHandle);
        }

        public void DoResume() => OnCombatResume();
        public void OnCombatResume()
        {
            if(HasActingEntities()) return;
            Timing.ResumeCoroutines(_loopHandle);
        }

        


        /// <summary>
        /// Is just [<see cref="OnDoMoreActions"/>] but just for
        /// [<seealso cref="Skills.PerformSkillHandler"/>]
        /// (so it more clear who does the step to the next Action OR calls the finish implicitly)
        /// </summary>
        public void DoSkillCheckFinish(CombatingEntity entity)
        {
            CombatConditionsChecker.FinishState finishCheck =
                CombatConditionChecker.HandleFinish();
            switch (finishCheck)
            {
                case CombatConditionsChecker.FinishState.StillInCombat:
                    DoMoreActionsSequence(entity);
                    break;
                case CombatConditionsChecker.FinishState.PlayerWin:
                    CombatSystemSingleton.Invoker.OnCombatFinish(entity, true);
                    break;
                case CombatConditionsChecker.FinishState.EnemyWin:
                    CombatSystemSingleton.Invoker.OnCombatFinish(entity, false);
                    break;
                default:
                    throw new ArgumentException($"An invalid type of finish state was invoked: {finishCheck}");
            }
        }

        /// <summary>
        /// Used to resume [<see cref="_Tick"/>] (which was paused by an [<seealso cref="CombatingEntity"/>] when
        /// it reaches its top [<seealso cref="ICombatHealthStats.InitiativePercentage"/>])
        /// </summary>
        private void StepNextActingEntity()
        {
            if (HasActingEntities())
            {
                DequeueInvokeNextActor();
            }
            else
            {
                ForcedBarUpdate();
                Timing.ResumeCoroutines(_loopHandle);
            }
            
        }

    }

    internal class TeamFillerHandlers : ICharacterArchetypesData<EntityFillerHandler>
    {
        public TeamFillerHandlers(ICharacterArchetypesData<EntityPersistentElements> elements)
        {
            vanguard = new EntityFillerHandler(elements.Vanguard.TempoFillers);
            attacker = new EntityFillerHandler(elements.Attacker.TempoFillers);
            support = new EntityFillerHandler(elements.Support.TempoFillers);
        }

        internal readonly EntityFillerHandler vanguard;
        internal readonly EntityFillerHandler attacker;
        internal readonly EntityFillerHandler support;

        public EntityFillerHandler Vanguard => vanguard;
        public EntityFillerHandler Attacker => attacker;
        public EntityFillerHandler Support => support;
    }

    internal class EntityFillerHandler : ITempoFiller
    {
        public EntityFillerHandler(List<ITempoFiller> fillers)
        {
            _fillers = fillers;
        }

        private readonly List<ITempoFiller> _fillers;

        public void FillBar(float percentage)
        {
            for (int i = 0; i < _fillers.Count; i++)
            {
                _fillers[i].FillBar(percentage);
            }
        }
    }


    public interface ITempoHandlerSequencer
    {
        void StartSequence(CombatingEntity entity);
        void DoMoreActionsSequence(CombatingEntity entity);
        void FinishSequence(CombatingEntity entity);
        void SkipSequence(CombatingEntity entity);
        void OnRoundCompleteSequence(List<CombatingEntity> allEntities, CombatingEntity lastEntity);
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
    public interface ITempoListenerVoid : ICharacterEventListener
    {
        void OnInitiativeTrigger();
        void OnDoMoreActions();
        void OnFinisAllActions();
    }

    public interface ITempoTicker 
    {
        void TempoTick(float deltaVariation);
    }

    public interface ITempoFiller
    {
        void FillBar(float percentage);
    }

    public interface IRoundListener : ICharacterEventListener
    {
        void OnRoundCompleted(List<CombatingEntity> allEntities, CombatingEntity lastEntity);
    }
    public interface IRoundListenerVoid : ICharacterEventListener
    {
        void OnRoundCompleted();
    }
}
