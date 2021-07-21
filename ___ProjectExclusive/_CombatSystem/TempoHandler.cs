using System;
using System.Collections.Generic;
using _Enemies;
using _Player;
using Characters;
using MEC;
using Sirenix.OdinInspector;
using SMaths;
using UnityEngine;

namespace _CombatSystem
{
    public class TempoHandler : ICombatFullListener, ITempoFullListener, ISkippedTempoListener
    {
        public enum TickType
        {
            OnBeforeSequence,
            OnAction,
            OnSequence,
            OnRound
        }

        private CharacterArchetypesList<CombatingEntity> _characters;
        [Range(0,10),SuffixLabel("deltas")]
        public float TempoModifier = 1f;

        [HideInEditorMode,HideInPlayMode]
        public readonly CombatConditionsChecker CombatConditionChecker;

        [ShowInInspector, DisableInPlayMode] 
        public readonly TempoEvents TriggerBasicHandler;
        [ShowInInspector, DisableInPlayMode] 
        public ITempoTriggerHandler PlayerTempoHandler { get; private set; }
        public ICombatEnemyController EnemyController { get; private set; }

        public readonly Dictionary<CombatingEntity, ITempoFiller> EntitiesBar;

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
            TriggerBasicHandler  = new TempoEvents();
            int memoryAllocation = UtilsCharacter.PredictedAmountOfCharactersInBattle;
            EntitiesBar = new Dictionary<CombatingEntity, ITempoFiller>(
                memoryAllocation);
            _roundTracker = new List<CombatingEntity>(memoryAllocation);
            CombatConditionChecker = new CombatConditionsChecker();
            CombatSystemSingleton.CombatConditionChecker = CombatConditionChecker;
        }

        public void Inject(ITempoTriggerHandler playerTriggerHandler)
        {
            PlayerTempoHandler = playerTriggerHandler;
        }

        public void Inject(ICombatEnemyController enemyController)
        {
            if (enemyController == null)
                EnemyController = CombatEnemyControllerRandom.GenericEnemyController;
            else
            {
                EnemyController = enemyController;
            }
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


        public void OnBeforeStart(
            CombatingTeam playerEntities,
            CombatingTeam enemyEntities,
            CharacterArchetypesList<CombatingEntity> allEntities)
        {
            CombatConditionChecker.OnAfterPreparation(playerEntities,enemyEntities,allEntities);
            _characters = allEntities;
            RefillRoundTracker();
        }


        

        private void ForcedBarUpdate()
        {
            foreach (KeyValuePair<CombatingEntity, ITempoFiller> pair in EntitiesBar)
            {
                pair.Value.FillBar(pair.Key.CombatStats.InitiativePercentage);
            }
        }

        private CoroutineHandle _loopHandle;
        private IEnumerator<float> _Tick()
        {
            // >>>> PREPARATIONS
            yield return Timing.WaitForOneFrame; //To ensure that everything is initialized
            const float initiativeCheck = GlobalCombatParams.InitiativeCheck;
            const float speedModifier = GlobalCombatParams.SpeedStatModifier;
            _entityTriggers = false;
            ForcedBarUpdate();

            // >>>> COMBAT LOOP
            while (_characters != null)
            {
                float deltaIncrement = Time.deltaTime * TempoModifier;
                foreach (CombatingEntity entity in _characters)
                {
                    CharacterCombatData stats = entity.CombatStats;

                    //Increase Initiative
                    {
                        stats.InitiativePercentage += deltaIncrement * stats.SpeedAmount * speedModifier;

                        float initiativePercentage = SRange.Percentage(
                            stats.InitiativePercentage,
                            0, initiativeCheck);
                        EntitiesBar[entity].FillBar(initiativePercentage);
                    }

                    if (stats.InitiativePercentage < initiativeCheck) continue;
                    yield return Timing.WaitForOneFrame;
                    //Stats refill
                    stats.InitiativePercentage = 0;
                    stats.RefillInitiativeActions();

                    if (!entity.CanAct())
                    {
                        OnSkippedEntity(entity);
                        continue;
                    }

                    // Invoke Triggers and LOOP
                    StartUsingActions(entity);

                    _entityTriggers = true;
                    Timing.PauseCoroutines(_loopHandle);
                    yield return Timing.WaitForOneFrame;

                    // Finish LOOP
                    DoCheckIfRoundPassed(); void DoCheckIfRoundPassed()
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
                }
                yield return Timing.WaitForSeconds(deltaIncrement);
            }
        }

        public static void CallUpdateOnInitiativeBar(CombatingEntity entity)
        {
            var entitiesBar 
                = CombatSystemSingleton.TempoHandler.EntitiesBar;
            entitiesBar[entity].FillBar(entity.CombatStats.InitiativePercentage);
        }

        private void StartUsingActions(CombatingEntity entity)
        {
            var stats = entity.CombatStats;
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
            EnemyController = null;
        }

        public void OnCombatPause()
        {
            if(_entityTriggers) return;
            Timing.PauseCoroutines(_loopHandle);
        }

        public void OnCombatResume()
        {
            if(_entityTriggers) return;
            Timing.ResumeCoroutines(_loopHandle);
        }

        private bool IsForPlayer(CombatingEntity entity)
        {
            return _canControlAll || UtilsCharacter.IsAPlayerEntity(entity);
        }

        public void OnInitiativeTrigger(CombatingEntity entity)
        {
            TriggerBasicHandler.OnInitiativeTrigger(entity);
            if (IsForPlayer(entity))
            {
                PlayerTempoHandler.OnInitiativeTrigger(entity);
            }
            else
            {
                EnemyController.DoControlOn(entity);
            }
        }
        /// <summary>
        /// Is just <see cref="OnDoMoreActions"/> but just for <seealso cref="Skills.PerformSkillHandler"/>
        /// (so it more clear who does the step to the next Action OR calls the finish implicitly)
        /// </summary>
        public void OnSkillActionFinish(CombatingEntity entity)
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
                if (IsForPlayer(entity))
                    PlayerTempoHandler.OnDoMoreActions(entity);
                else
                {
                    EnemyController.DoControlOn(entity);
                }
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
                ResumeFromTempoTrigger();
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

        private bool _entityTriggers;
        /// <summary>
        /// Used to resume [<see cref="_Tick"/>] (which was paused by an [<seealso cref="CombatingEntity"/>] when
        /// it reaches its top [<seealso cref="ICombatTemporalStats.InitiativePercentage"/>])
        /// </summary>
        private void ResumeFromTempoTrigger()
        {
            _entityTriggers = false;
            ForcedBarUpdate();
            Timing.ResumeCoroutines(_loopHandle);
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
