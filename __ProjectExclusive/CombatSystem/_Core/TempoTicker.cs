using System.Collections.Generic;
using CombatEntity;
using CombatSystem.Events;
using CombatTeam;
using MEC;
using Sirenix.OdinInspector;
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

        [Title("Condition")] 
        [ShowInInspector]
        private ICombatEndConditionProvider _conditionProvider;
        private static readonly ICombatEndConditionProvider _provisionalConditionProvider = new GenericWinCondition(); 

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

        public void OnStartAndBeforeFirstTick()
        {
            Timing.RunCoroutine(_TickingLoop());
        }


        // Twelve frames (about half second) between each tick; This is similar to the Character animation frequency of 12/24
        // frames update per game cycle.
        // This period is also better for performance since it calculates 12 times less per tick (but it could be possible no use it)
        private const float DeltaTickPeriod = 12;
        // Entities requires to reach 1f(100%) initiative and at speed 1 it will require: 12 / 0.01 = 120 ticks
        // 120 / 20 (20 ticks = 1second aprox.) = 6 seconds
        private const float TickSpeedModifier = 0.1f;
        private IEnumerator<float> _TickingLoop()
        {
#if UNITY_EDITOR
            Debug.Log("Starting (TICKING)");
#endif

            //Just to give a space to breath before tempo's ticking
            yield return Timing.WaitForSeconds(1);

            if (_conditionProvider == null) 
                _conditionProvider = _provisionalConditionProvider;

            while (!_conditionProvider.IsCombatFinish())
            {
                float deltaVariation = DeltaTickPeriod * Timing.DeltaTime;

                // TICKING
                foreach (var entity in _tickingEntities)
                {
                    var statsHolder = entity.CombatStats;
                    float tickIncrement = deltaVariation * statsHolder.InitiativeSpeed * TickSpeedModifier;
                    statsHolder.TickingInitiative += tickIncrement;
                    if(statsHolder.TickingInitiative < 1) continue;

                    _activeEntities.Enqueue(entity);
                }


                // Wait for emptying actives;
                while (_activeEntities.Count > 0)
                {
                    var actingEntity = _activeEntities.Dequeue();
                    //Removes (and adds later) so acted entities has fewer priority that those how didn't 
                    _tickingEntities.Remove(actingEntity);

                    yield return Timing.WaitUntilDone(_DoEntitySequence(actingEntity));

                    //Finish the acting
                    _tickingEntities.Add(actingEntity);
                    _DoRoundEndCheck(actingEntity);

                }


                // Check (and refill) the end of the Round
               
            }
        }

        private IEnumerator<float> _DoEntitySequence(CombatingEntity actingEntity)
        {
            Debug.Log($"Trigger entity: {actingEntity}");
            yield return Timing.WaitForOneFrame;
            var stats = actingEntity.CombatStats;

            if (stats.CurrentActions <= 0)
            {
                CombatSystemSingleton.EventsHolder.OnSkipActions(actingEntity);
                yield break;
            }

            var entityTempoHandler = CombatSystemSingleton.EntityITempoHandler;
            var eventsHolder = CombatSystemSingleton.EventsHolder;

            eventsHolder.OnInitiativeTrigger(actingEntity);
            yield return Timing.WaitUntilDone(entityTempoHandler._RequestFinishAction(actingEntity));
            while (stats.CurrentActions > 1)
            {
                eventsHolder.OnDoMoreActions(actingEntity);
                yield return Timing.WaitUntilDone(entityTempoHandler._RequestFinishAction(actingEntity));
            }

            eventsHolder.OnFinishAllActions(actingEntity);
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

    
    public interface IEntityITempoHandler
    {
        IEnumerator<float> _RequestFinishAction(CombatingEntity entity);
    }

}
