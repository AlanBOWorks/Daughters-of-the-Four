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
    public class TempoTicker : ICombatPreparationListener
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

        public TempoTicker()
        {
            _tickingEntities = new HashSet<CombatingEntity>();
            _activeEntities = new Queue<CombatingEntity>(GameParams.DefaultMemberPerCombat);
            _roundTracker = new HashSet<CombatingEntity>();

            _tempoListeners = new HashSet<ITempoListener<CombatingEntity>>();
            _roundListeners = new HashSet<IRoundListener<CombatingEntity>>();
            _entityTickListeners = new List<IEntityTickListener>();
        }

        [HorizontalGroup("Entities", Title = "Entities"), ShowInInspector,HideInEditorMode]
        private readonly HashSet<CombatingEntity> _tickingEntities;
        [HorizontalGroup("Entities", Title = "Entities"), ShowInInspector,HideInEditorMode]
        private readonly Queue<CombatingEntity> _activeEntities;
        [HorizontalGroup("Entities", Title = "Entities"), ShowInInspector,HideInEditorMode]
        private readonly HashSet<CombatingEntity> _roundTracker;

        [HorizontalGroup("Events", Title = "Events"), ShowInInspector, DisableIf("_tempoListeners")]
        private readonly HashSet<ITempoListener<CombatingEntity>> _tempoListeners;
        [HorizontalGroup("Events", Title = "Events"), ShowInInspector, DisableIf("_roundListeners")]
        private readonly HashSet<IRoundListener<CombatingEntity>> _roundListeners;

        [HorizontalGroup("Events", Title = "Events"), ShowInInspector, DisableIf("_tempoListeners")]
        private readonly List<IEntityTickListener> _entityTickListeners; 

        [Title("Condition")] 
        [ShowInInspector]
        private ICombatEndConditionProvider _conditionProvider;
        private static readonly ICombatEndConditionProvider ProvisionalConditionProvider = new GenericWinCondition(); 

        public void InjectCondition(ICombatEndConditionProvider conditionProvider)
        {
            _conditionProvider = conditionProvider;
        }

        public void Subscribe(ITempoListener<CombatingEntity> listener)
        {
            _tempoListeners.Add(listener);
        }

        public void Subscribe(IRoundListener<CombatingEntity> listener)
        {
            _roundListeners.Add(listener);
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
            Timing.RunCoroutine(_TickingLoop());
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
                    //Removes (and adds later) so acted entities has fewer priority that those how didn't 
                    _tickingEntities.Remove(actingEntity);


                    var stats = actingEntity.CombatStats;
                    UtilsCombatStats.RefillActions(stats);
                    UtilsCombatStats.InitiativeResetOnTrigger(stats);


                    yield return Timing.WaitUntilDone(_DoEntitySequence(actingEntity));

                    //Finish the acting
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

        private IEnumerator<float> _DoEntitySequence(CombatingEntity actingEntity)
        {
            var entityTempoHandler = CombatSystemSingleton.EntityActionRequestHandler;

            yield return Timing.WaitForOneFrame;
            var stats = actingEntity.CombatStats;

            if (stats.CurrentActions <= 0)
            {
                CombatSystemSingleton.EventsHolder.OnSkipActions(actingEntity);
                yield break;
            }

            // The EntityActionRequestHandler deals with the event of OnInitiativeTrigger(Entity)
            yield return Timing.WaitUntilDone(entityTempoHandler._RequestFinishActions(actingEntity));

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
