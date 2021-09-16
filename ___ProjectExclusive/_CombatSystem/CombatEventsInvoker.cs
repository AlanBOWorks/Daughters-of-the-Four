using System;
using System.Collections.Generic;
using _Team;
using Characters;
using Stats;
using UnityEngine;

namespace _CombatSystem
{
    public class CombatEventsInvoker : ICombatAfterPreparationListener
    {
        public CombatEventsInvoker(CombatEvents globalEvents, PersistentElementsDictionary persistentEvents, 
            CombatControllersHandler controllersEvents)
        {
            _globalEvents = globalEvents;
            _persistentEvents = persistentEvents;

            _tempoEventsHandlers = new Queue<ITempoListener>(1);
            _tempoEventsHandlers.Enqueue(controllersEvents);
        }

        private readonly CombatEvents _globalEvents;
        private readonly PersistentElementsDictionary _persistentEvents;
        private readonly Queue<ITempoListener> _tempoEventsHandlers;

        public void Subscribe(ITempoListener listener) => _tempoEventsHandlers.Enqueue(listener);

        public void OnAfterPreparation(CombatingTeam playerEntities, CombatingTeam enemyEntities, CharacterArchetypesList<CombatingEntity> allEntities)
        {
            foreach (CombatingEntity entity in allEntities)
            {
                InvokeVitalityChange(entity);
                InvokeOnHealthChange(entity);
                InvokeTemporalStatChange(entity);
                InvokeAreaChange(entity);
            }
        }

        private static void DoEvents<T>(List<T> globalEvents, List<T> persistentEvents, List<T> entityEvents, Action<T> action)
        {
            for (var i = 0; i < globalEvents.Count; i++)
            {
                T globalEvent = globalEvents[i];
                action(globalEvent);
            }

            for (var i = 0; i < persistentEvents.Count; i++)
            {
                T persistentEvent = persistentEvents[i];
                action(persistentEvent);
            }

            for (var i = 0; i < entityEvents.Count; i++)
            {
                T entityEvent = entityEvents[i];
                action(entityEvent);
            }
        }

        private Action<ITempoListener> _tempoAction;
        private void DoTempoActions(CombatingEntity entity)
        {
            DoEvents(
                _globalEvents.onTempoListeners,
                _persistentEvents[entity].CombatEvents.onTempoListeners,
                entity.Events.onTempoListeners,
                _tempoAction
            );
            foreach (ITempoListener listener in _tempoEventsHandlers)
            {
                _tempoAction(listener);
            }
        }

        public void OnInitiativeTrigger(CombatingEntity entity)
        {
            _tempoAction = TempoAction;
            DoTempoActions(entity);
            void TempoAction(ITempoListener listener)
            {
                listener.OnInitiativeTrigger(entity);
            }
        }

        public void OnDoMoreActions(CombatingEntity entity)
        {
            _tempoAction = TempoAction;
            DoTempoActions(entity);

            void TempoAction(ITempoListener listener)
            {
                listener.OnDoMoreActions(entity);
            }
        }

        public void OnFinisAllActions(CombatingEntity entity)
        {
            _tempoAction = TempoAction;
            DoTempoActions(entity);

            void TempoAction(ITempoListener listener)
            {
                listener.OnFinisAllActions(entity);
            }
        }

        private Action<IVitalityChangeListener> _vitalityAction;
        private void DoVitalityActions(CombatingEntity entity)
        {
            DoEvents(
                _globalEvents.onVitalityChange,
                _persistentEvents[entity].CombatEvents.onVitalityChange,
                entity.Events.onVitalityChange,
                _vitalityAction
            );
        }
        public void InvokeVitalityChange(CombatingEntity entity)
        {
            IVitalityStatsData<float> vitalityStats = entity.CombatStats;
            _vitalityAction = VitalityAction;
            DoVitalityActions(entity);

            void VitalityAction(IVitalityChangeListener listener)
                => listener.OnVitalityChange(vitalityStats);
            
        }

        private Action<ITemporalStatChangeListener> _temporalStatsAction;
        private void DoTemporalStatActions(CombatingEntity entity)
        {
            DoEvents(
                _globalEvents.onTemporalStatChange,
                _persistentEvents[entity].CombatEvents.onTemporalStatChange,
                entity.Events.onTemporalStatChange,
                _temporalStatsAction
            );
        }
        public void InvokeTemporalStatChange(CombatingEntity entity)
        {
            ITemporalStatsData<float> stats = entity.CombatStats;
            _temporalStatsAction = TemporalStatsAction;
            DoTemporalStatActions(entity);

            void TemporalStatsAction(ITemporalStatChangeListener listener)
                => listener.OnConcentrationChange(stats);
        }


        private Action<IAreaStateChangeListener> _areaAction;
        private void DoAreaActions(CombatingEntity entity)
        {
            DoEvents(
                _globalEvents.onAreaChange,
                _persistentEvents[entity].CombatEvents.onAreaChange,
                entity.Events.onAreaChange,
                _areaAction
            );
        }
        public void InvokeAreaChange(CombatingEntity entity)
        {
            var areaData = entity.AreasDataTracker;
            _areaAction = AreaAction;
            DoAreaActions(entity);
            void AreaAction(IAreaStateChangeListener listener)
                => listener.OnAreaStateChange(areaData);
        }

        private Action<ICombatHealthChangeListener> _healthAction;
        private void DoHealthAction(CombatingEntity entity)
        {
            DoEvents(
                _globalEvents.onCombatHealthChange,
                _persistentEvents[entity].CombatEvents.onCombatHealthChange,
                entity.Events.onCombatHealthChange,
                _healthAction
            );
        }

        public void InvokeOnHealthChange(CombatingEntity entity)
        {
            var healthStats = entity.CombatStats;
            _healthAction = HealthAction;
            DoHealthAction(entity);
            void HealthAction(ICombatHealthChangeListener listener)
                => listener.OnTemporalStatsChange(healthStats);
        }

        private Action<IHealthZeroListener> _healthZeroAction;

        private void DoHealthZeroAction(CombatingEntity entity)
        {
            DoEvents(
                _globalEvents.onHealthZeroListeners,
                _persistentEvents[entity].CombatEvents.onHealthZeroListeners,
                entity.Events.onHealthZeroListeners,
                _healthZeroAction
            );
        }
        public void InvokeOnHealthZero(CombatingEntity entity)
        {
            _healthZeroAction = HealthAction;
            DoHealthZeroAction(entity);

            void HealthAction(IHealthZeroListener listener)
                => listener.OnHealthZero(entity);
        }

        public void InvokeOnMortalityZero(CombatingEntity entity)
        {
            _healthZeroAction = HealthAction;
            DoHealthZeroAction(entity);

            void HealthAction(IHealthZeroListener listener)
                => listener.OnMortalityZero(entity);
        }
        
        public void InvokeOnRevive(CombatingEntity entity)
        {
            _healthZeroAction = HealthAction;
            DoHealthZeroAction(entity);

            void HealthAction(IHealthZeroListener listener)
                => listener.OnMortalityZero(entity);
        }

        public void InvokeOnTeamHealthZero(CombatingTeam losingTeam)
        {
            _healthZeroAction = HealthAction;
            foreach (CombatingEntity entity in losingTeam)
            {
                DoHealthZeroAction(entity);
            }

            void HealthAction(IHealthZeroListener listener)
                => listener.OnTeamHealthZero(losingTeam);
        }

    }
}
