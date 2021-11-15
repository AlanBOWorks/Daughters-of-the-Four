using System.Collections.Generic;
using CombatEntity;
using CombatTeam;
using MEC;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace CombatSystem
{
    public sealed class CombatingEntitiesTicker : ITempoTickListener, ICombatPreparationListener
    {
        public CombatingEntitiesTicker()
        {
            EntityTickListeners = new HashSet<IEntityTickListener>();
            _tickingEntities = new HashSet<CombatingEntity>();
            _activeEntities = new Queue<CombatingEntity>();

        }

        public void OnPreparationCombat(CombatingTeam playerTeam, CombatingTeam enemyTeam)
        {
            AddEntities(playerTeam);
            AddEntities(enemyTeam);
            InjectTickAmountCheck();

            void AddEntities(CombatingTeam team)
            {
                foreach (CombatingEntity entity in team)
                {
                    _tickingEntities.Add(entity);
                }
            }

            void InjectTickAmountCheck()
            {
                foreach (var listener in EntityTickListeners)
                {
                    listener.RoundAmountInjection(FullTickAmount);
                }
            }
        }

        public void OnAfterLoadsCombat()
        {
        }


        [HorizontalGroup("Events", Title = "Events"), ShowInInspector]
        public readonly HashSet<IEntityTickListener> EntityTickListeners;

        [HorizontalGroup("Entities", Title = "Entities"), ShowInInspector, HideInEditorMode]
        private readonly HashSet<CombatingEntity> _tickingEntities;
        [HorizontalGroup("Entities", Title = "Entities"), ShowInInspector, HideInEditorMode]
        private readonly Queue<CombatingEntity> _activeEntities;
        public CombatingEntity LastActingEntity { get; private set; }

        public bool HasActingEntity() => _activeEntities.Count > 0;


        private const int FullTickAmount = 24;//todo make it a param
        private int _roundTickAmount;
        public void OnTickStep(float seconds)
        {
            foreach (var entity in _tickingEntities)
            {
                var statsHolder = entity.CombatStats;
                if (!UtilsCombatStats.IsTickingValid(statsHolder))
                    continue; ///// >>>>>

                float tickIncrement = statsHolder.InitiativeSpeed;
                statsHolder.TickingInitiative += tickIncrement;

                float tickCheck = FullTickAmount;

                float currentInitiative = statsHolder.TickingInitiative;
                InvokeTickingEvents(entity, currentInitiative);

                if (currentInitiative < tickCheck)
                    continue; ///// >>>>>

                _activeEntities.Enqueue(entity);
            }

            if(_activeEntities.Count > 0)
                CombatSystemSingleton.TempoTicker.PauseTickingUntil(_DoEntitiesActions());
            HandleRound();
        }

        private void HandleRound()
        {
            _roundTickAmount++;
            InvokeRoundTickEvents();
            if(_roundTickAmount < FullTickAmount) return;

            _roundTickAmount = 0;
            CombatSystemSingleton.EventsHolder.OnRoundFinish(LastActingEntity);
        }

        private IEnumerator<float> _DoEntitiesActions()
        {
            yield return Timing.WaitForOneFrame;
            while (_activeEntities.Count > 0)
            {
                var actingEntity = _activeEntities.Dequeue();
                LastActingEntity = actingEntity;
                //Removes (and adds later) so acted entities has fewer priority that those how didn't 
                _tickingEntities.Remove(actingEntity);


                var stats = actingEntity.CombatStats;
                UtilsCombatStats.RefillActions(stats);
                UtilsCombatStats.InitiativeResetOnTrigger(stats);


                yield return Timing.WaitUntilDone(_DoEntitySequence(actingEntity));

                //Finish the acting
                _tickingEntities.Add(actingEntity);
            }

            yield return Timing.WaitForOneFrame;
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
                = Timing.RunCoroutine(CombatSystemSingleton.EntityActionRequestHandler._RequestEntityActions(actingEntity));
            yield return Timing.WaitUntilDone(_requestActionCoroutineHandle);

        }


        private void InvokeTickingEvents(CombatingEntity entity, float currentInitiativeTick)
        {
            foreach (var tickListener in EntityTickListeners)
            {
                tickListener.OnTickEntity(entity, currentInitiativeTick);
            }
        }
        private void InvokeRoundTickEvents()
        {
            foreach (var listener in EntityTickListeners)
            {
                listener.OnRoundTick(_roundTickAmount);
            }
        }

    }
}
