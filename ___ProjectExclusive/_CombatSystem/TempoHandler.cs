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
    public class TempoHandler : ICombatFullListener, ITempoFullListener, ISkippedTempoListener
    {
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
        public readonly TempoEvents TriggerBasicHandler;
        [ShowInInspector, DisableInEditorMode] 
        public ITempoTriggerHandler PlayerTempoHandler { get; private set; }

        // I know this could be in another class container, but from what
        // was tried it's just a lot easier and direct having this here
        [ShowInInspector, DisableInEditorMode] 
        public ICombatEnemyController EnemyController { get; private set; }

        public readonly Dictionary<CombatingEntity, ITempoFiller> EntitiesBar;
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

        [ShowInInspector]
        private bool _canControlAll;
        public TempoHandler()
        {
            int memoryAllocation = UtilsCharacter.PredictedAmountOfCharactersInBattle;

            TriggerBasicHandler  
                = new TempoEvents();
            EntitiesBar 
                = new Dictionary<CombatingEntity, ITempoFiller>(memoryAllocation);
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

        public void InjectPlayerEvents(ITempoTriggerHandler playerTriggerHandler)
        {
            PlayerTempoHandler = playerTriggerHandler;
        }

        public void Inject(ICombatEnemyController enemyController)
        {
            if (enemyController == null)
                EnemyController = CombatEnemyControllerRandom.GenericEnemyController;
            else
                EnemyController = enemyController;
        }
        public void Subscribe(ITempoListener listener)
        {
            if (PlayerTempoHandler != null && listener is IPlayerTempoListener)
            {
                PlayerTempoHandler.TempoListeners.Add(listener);
                return;
            }
            TriggerBasicHandler.TempoListeners.Add(listener);
        }

        public void Subscribe(IRoundListener listener)
        {
            if (PlayerTempoHandler != null && listener is IPlayerRoundListener)
            {
                PlayerTempoHandler.RoundListeners.Add(listener);
                return;
            }
            TriggerBasicHandler.RoundListeners.Add(listener);
        }

        public void Subscribe(ISkippedTempoListener listener)
        {
            if (PlayerTempoHandler != null && listener is IPlayerRoundListener)
            {
                PlayerTempoHandler.SkippedListeners.Add(listener);
                return;
            }
            TriggerBasicHandler.SkippedListeners.Add(listener);
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

        private void AddEntitiesToFillingInitiative(List<CombatingEntity> entities)
        {
            if(_fillingEntities.Count > 0) _fillingEntities.Clear();
            _fillingEntities.AddRange(entities);
        }


        private bool HasActingEntities() => _actingQueue.Count > 0;
        private void ForcedBarUpdate()
        {
            foreach (KeyValuePair<CombatingEntity, ITempoFiller> pair in EntitiesBar)
            {
                pair.Value.FillBar(pair.Key.CombatStats.InitiativePercentage);
            }
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
        private IEnumerator<float> _Tick()
        {
            // >>>> PREPARATIONS
            yield return Timing.WaitForOneFrame; //To ensure that everything is initialized
            
            ForcedBarUpdate();

            // >>>> COMBAT LOOP
            while (_characters != null)
            {
                float deltaIncrement = Time.deltaTime * TempoStepModifier;
                yield return Timing.WaitForOneFrame;

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
                        IFullStatsData<float> stats = entity.CombatStats;
                        IFullStatsInjection<float> injection = entity.CombatStats.BaseStats;
                        float initiative = stats.InitiativePercentage;
                        initiative += deltaIncrement * stats.SpeedAmount * VelocityModifier;
                        injection.InitiativePercentage = initiative;

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
                OnSkippedEntity(entity);
            else
                StartUsingActions(entity);
        }

        private void DoCheckIfRoundPassed(CombatingEntity entity)
        {
            if (!_roundTracker.Contains(entity)) return;
            if (_roundTracker.Count <= 1) //this means is the last one
            {
                RefillRoundTracker();
                TriggerBasicHandler.OnRoundCompleted(_characters, entity);
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

        private void CallUpdateOnInitiativeBar(CombatingEntity entity, ITemporalStatsData<float> stats)
        {
            float initiativePercentage = stats.InitiativePercentage;
            EntitiesBar[entity].FillBar(initiativePercentage);
        }


        private void StartUsingActions(CombatingEntity entity)
        {
            OnInitiativeTrigger(entity);
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
            OnFinisAllActions(lastEntity);
            Timing.KillCoroutines(_loopHandle);

            EntitiesBar.Clear();
            _actingQueue.Clear();
            _fillingEntities.Clear();
            EnemyController = null;
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

        private bool IsForPlayer(CombatingEntity entity)
        {
            return _canControlAll || UtilsCharacter.IsAPlayerEntity(entity);
        }

        private void CallForControl(CombatingEntity entity)
        {
            if (IsForPlayer(entity))
            {
                PlayerTempoHandler.OnDoMoreActions(entity);
            }
            else
            {
                EnemyController.DoControlOn(entity);
            }

        }

        public void OnInitiativeTrigger(CombatingEntity entity)
        {
            TriggerBasicHandler.OnInitiativeTrigger(entity);
            CallForControl(entity);
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
                    OnDoMoreActions(entity);
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
        public void OnDoMoreActions(CombatingEntity entity)
        {
            var currentStats = entity.CombatStats;
            currentStats.ActionsLefts--;
            if (entity.CanAct())
            {
                TriggerBasicHandler.OnDoMoreActions(entity);
                CallForControl(entity);
            }
            else
            {
                currentStats.ActionsLefts = 0;
                OnFinisAllActions(entity);
            }
        }

        public void OnFinisAllActions(CombatingEntity entity)
        {
            TriggerBasicHandler.OnFinisAllActions(entity);
            if (IsForPlayer(entity))
                PlayerTempoHandler.OnFinisAllActions(entity);

            if(_loopHandle.IsRunning)
                StepNextActingEntity();

            // Do Harmony 
            // TODO entity.HarmonyBuffInvoker?.InvokeBurstStats();

            // End round?
            DoCheckIfRoundPassed(entity);
        }

        public void OnRoundCompleted(List<CombatingEntity> allEntities, CombatingEntity lastEntity)
        {
            TriggerBasicHandler.OnRoundCompleted(allEntities,lastEntity);
            PlayerTempoHandler.OnRoundCompleted(allEntities,lastEntity);
        }

        public void OnSkippedEntity(CombatingEntity entity)
        {
            TriggerBasicHandler.OnSkippedEntity(entity);
            if (IsForPlayer(entity))
                PlayerTempoHandler.OnSkippedEntity(entity);
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

    public interface ITempoTypes<out T>
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

    public interface ISkippedTempoListener
    {
        void OnSkippedEntity(CombatingEntity entity);
    }

    public interface ITempoListener
    {
        void OnInitiativeTrigger(CombatingEntity entity);
        void OnDoMoreActions(CombatingEntity entity);
        void OnFinisAllActions(CombatingEntity entity);
    }
    public interface ITempoListenerVoid
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

    public interface IRoundListener
    {
        void OnRoundCompleted(List<CombatingEntity> allEntities, CombatingEntity lastEntity);
    }
    public interface IRoundListenerVoid
    {
        void OnRoundCompleted();
    }
}
