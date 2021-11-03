using System.Collections.Generic;
using CombatEntity;
using CombatSystem.Events;
using CombatTeam;
using MEC;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace CombatSystem
{
    public class TempoTicker : ICombatPreparationListener, ICombatDisruptionListener
    {
#if UNITY_EDITOR
        [ShowInInspector, TextArea] private const string BehaviourExplanation =
            "TempoTicker increase in ticksSteps the [CombatingEntities] initiative.\n" +
            "Once an entity reachs 100% triggers save it in a Queue and then" +
            "invokes the initiative trigger events for each one.\n" +
            "(It just simulates time progression and turn order)";

        private bool RemoveWarningFromUnityConsole(string nonUsedStringInReal)
            => nonUsedStringInReal == BehaviourExplanation;
#endif

        public TempoTicker(IEntityTempoHandler requestHandler)
        {
            _requestActionHandler = requestHandler;
            _tickingEntities = new HashSet<CombatingEntity>();
            _activeEntities = new Queue<CombatingEntity>(GameParams.DefaultMemberPerCombat);
            _roundTracker = new HashSet<CombatingEntity>();

            _entityTickListeners = new List<IEntityTickListener>();
        }

        private readonly IEntityTempoHandler _requestActionHandler;

        [HorizontalGroup("Entities", Title = "Entities"), ShowInInspector,HideInEditorMode]
        private readonly HashSet<CombatingEntity> _tickingEntities;
        [HorizontalGroup("Entities", Title = "Entities"), ShowInInspector,HideInEditorMode]
        private readonly Queue<CombatingEntity> _activeEntities;
        [HorizontalGroup("Entities", Title = "Entities"), ShowInInspector,HideInEditorMode]
        private readonly HashSet<CombatingEntity> _roundTracker;


        [HorizontalGroup("Events", Title = "Events"), ShowInInspector]
        private readonly List<IEntityTickListener> _entityTickListeners; 

        [Title("Condition")] 
        [ShowInInspector]
        private ICombatEndConditionProvider _conditionProvider;
        private static readonly ICombatEndConditionProvider ProvisionalConditionProvider = new GenericWinCondition();

        public CombatingEntity CurrentActingEntity { get; private set; }

        public bool IsRunning() => _coroutineHandle.IsRunning;
        public bool IsPaused() => _coroutineHandle.IsAliveAndPaused;
        public bool IsFinish() => !_coroutineHandle.IsRunning && !_requestActionCoroutineHandle.IsRunning;
        public bool HasActingEntity() => CurrentActingEntity != null;


        private CoroutineHandle _coroutineHandle;
        public void InjectCondition(ICombatEndConditionProvider conditionProvider)
        {
            _conditionProvider = conditionProvider;
        }

        public void Subscribe(IEntityTickListener listener)
        {
            _entityTickListeners.Add(listener);
        }

        public void OnPreparationCombat(CombatingTeam playerTeam, CombatingTeam enemyTeam)
        {
            AddEntities(playerTeam);
            AddEntities(enemyTeam);

            void AddEntities(CombatingTeam team)
            {
                foreach (CombatingEntity entity in team)
                {
                    _tickingEntities.Add(entity);
                    _roundTracker.Add(entity);
                }
            }
        }

        public void OnAfterLoads()
        {
            _coroutineHandle = Timing.RunCoroutine(_TickingLoop());
        }


        private const float TickPeriodSeconds = .2f;
        private const int EntityTickTriggerCheck = 24;//todo make it a param
        private IEnumerator<float> _TickingLoop()
        {
#if UNITY_EDITOR
            Debug.Log("Starting (TICKING)");
#endif
            InjectThreshold(EntityTickTriggerCheck);

            //Just to give a space to breath before tempo's ticking
            yield return Timing.WaitForSeconds(1);

            if (_conditionProvider == null) 
                _conditionProvider = ProvisionalConditionProvider;

            while (!_conditionProvider.IsCombatFinish())
            {
#if UNITY_EDITOR
                Debug.Log("TICKING...");
#endif

                // TICKING
                foreach (var entity in _tickingEntities)
                {
                    var statsHolder = entity.CombatStats;
                    if(!UtilsCombatStats.IsTickingValid(statsHolder)) 
                        continue; ///// >>>>>

                    float tickIncrement = statsHolder.InitiativeSpeed;
                    statsHolder.TickingInitiative += tickIncrement;

                    float tickCheck = EntityTickTriggerCheck;

                    float currentInitiative = statsHolder.TickingInitiative;
                    InvokeTickingEvents(entity,currentInitiative);

                    if(currentInitiative < tickCheck) 
                        continue; ///// >>>>>

                    _activeEntities.Enqueue(entity);
                }

                yield return Timing.WaitForSeconds(TickPeriodSeconds);

                // Wait for emptying actives;
                while (_activeEntities.Count > 0)
                {
                    var actingEntity = _activeEntities.Dequeue();
                    CurrentActingEntity = actingEntity;
                    //Removes (and adds later) so acted entities has fewer priority that those how didn't 
                    _tickingEntities.Remove(actingEntity);


                    var stats = actingEntity.CombatStats;
                    UtilsCombatStats.RefillActions(stats);
                    UtilsCombatStats.InitiativeResetOnTrigger(stats);


                    yield return Timing.WaitUntilDone(_DoEntitySequence(actingEntity));

                    //Finish the acting
                    CurrentActingEntity = null;
                    _tickingEntities.Add(actingEntity);
                    _DoRoundEndCheck(actingEntity);
                }

                yield return Timing.WaitForOneFrame;

            }

            void InjectThreshold(float initiativeCheckAmount)
            {
                foreach (var listener in _entityTickListeners)
                {
                    listener.TickThresholdInjection(initiativeCheckAmount);
                }
            }
            void InvokeTickingEvents(CombatingEntity entity, float currentInitiativeTick)
            {
                foreach (var tickListener in _entityTickListeners)
                {
                    tickListener.OnTickEntity(entity,currentInitiativeTick);
                }
            }
        }

        private CoroutineHandle _requestActionCoroutineHandle;
        private IEnumerator<float> _DoEntitySequence(CombatingEntity actingEntity)
        {

            yield return Timing.WaitForOneFrame;

            if (!actingEntity.CanAct())
            {
                CombatSystemSingleton.EventsHolder.OnCantAct(actingEntity);
                yield break;
            }

            // The EntityActionRequestHandler deals with the event of OnFirstAction(Entity)
            _requestActionCoroutineHandle 
                = Timing.RunCoroutine(_requestActionHandler._RequestFinishActions(actingEntity));
            yield return Timing.WaitUntilDone(_requestActionCoroutineHandle);

        }

        //TODO make it coroutine when applies
        private void _DoRoundEndCheck(CombatingEntity checkingEntity)
        {
            if (! _roundTracker.Contains(checkingEntity)) return;

            if (_roundTracker.Count == 1) //This means is the las entity
            {
                CombatSystemSingleton.EventsHolder.OnRoundFinish(checkingEntity);
                foreach (CombatingEntity entity in _tickingEntities)
                {
                    _roundTracker.Add(entity);
                }
            }
            else
            {
                _roundTracker.Remove(checkingEntity);
            }
        }

        public void OnCombatPause()
        {
            if (_requestActionCoroutineHandle.IsRunning)
            {
                _requestActionCoroutineHandle.IsAliveAndPaused = true;
            }
            else
            {
                _coroutineHandle.IsAliveAndPaused = true;
            }
        }
        public void OnCombatResume()
        {
            if (_requestActionCoroutineHandle.IsRunning)
            {
                _requestActionCoroutineHandle.IsAliveAndPaused = false;
            }
            else
            {
                _coroutineHandle.IsAliveAndPaused = false;
            }
        }
        public void OnCombatExit()
        {
            _coroutineHandle.IsRunning = false;
            _requestActionCoroutineHandle.IsRunning = false;
        }
    }

    
    public interface IEntityTempoHandler
    {
        IEnumerator<float> _RequestFinishActions(CombatingEntity entity);
    }

    public interface IEntityTickListener
    {
        void TickThresholdInjection(float initiativeCheckAmount);
        void OnTickEntity(CombatingEntity entity, float currentTickAmount);
    }
}
