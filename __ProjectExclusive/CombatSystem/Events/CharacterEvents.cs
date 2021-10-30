using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace CombatSystem.Events
{
    public class CharacterEventsHolder<THolder, TTempo, TValue> :
        IOffensiveActionReceiverListener<THolder, TValue>,
        ISupportActionReceiverListener<THolder, TValue>,
        IVitalityChangeListener<THolder, TValue>,
        ITempoListener<TTempo>
    {
        public CharacterEventsHolder()
        {
            _offensiveReceiveListeners = new List<IOffensiveActionReceiverListener<THolder, TValue>>();

            _supportReceiveListeners = new List<ISupportActionReceiverListener<THolder, TValue>>();

            _vitalityChangeListeners = new List<IVitalityChangeListener<THolder,TValue>>();

            _tempoListeners = new List<ITempoListener<TTempo>>();
            _tempoDisruptionListeners = new List<ITempoAlternateListener<TTempo>>();
            _roundListeners = new List<IRoundListener<TTempo>>();
        }

        [ShowInInspector,HorizontalGroup("Offensive Events", 
             Title = "________________________ Offensive Events ________________________")]
        private readonly List<IOffensiveActionReceiverListener<THolder,TValue>> _offensiveReceiveListeners;
        [ShowInInspector, HorizontalGroup("Support Events", 
             Title = "________________________ Support Events ________________________")]
        private readonly List<ISupportActionReceiverListener<THolder,TValue>> _supportReceiveListeners;

        [ShowInInspector, HorizontalGroup("Reaction stats", 
             Title = "________________________ Reaction Events ________________________")]
        private readonly List<IVitalityChangeListener<THolder,TValue>> _vitalityChangeListeners;

        [ShowInInspector, HorizontalGroup("Tempo Events", 
             Title = "________________________ Tempo Events ________________________")]
        private readonly List<ITempoListener<TTempo>> _tempoListeners;
        private readonly List<ITempoAlternateListener<TTempo>> _tempoDisruptionListeners;

        [ShowInInspector, HorizontalGroup("Tempo Events")]
        private readonly List<IRoundListener<TTempo>> _roundListeners;

        public void Subscribe(IOffensiveActionReceiverListener<THolder, TValue> listener)
        {
            _offensiveReceiveListeners.Add(listener);
        }

        public void Subscribe(ISupportActionReceiverListener<THolder, TValue> listener)
        {
            _supportReceiveListeners.Add(listener);
        }

        public void Subscribe(IVitalityChangeListener<THolder, TValue> listener) => _vitalityChangeListeners.Add(listener);
        public void Subscribe(ITempoListener<TTempo> listener) => _tempoListeners.Add(listener);
        public void Subscribe(ITempoAlternateListener<TTempo> listener) => _tempoDisruptionListeners.Add(listener);
        public void Subscribe(IRoundListener<TTempo> listener) => _roundListeners.Add(listener);


        public void UnSubscribe(ITempoListener<TTempo> listener) => _tempoListeners.Remove(listener);

        public void OnReceiveOffensiveAction(THolder element,ref TValue value)
        {
            foreach (var listener in _offensiveReceiveListeners)
            {
                listener.OnReceiveOffensiveAction(element,ref value);
            }
        }
        public void OnReceiveSupportAction(THolder element,ref TValue value)
        {
            foreach (var listener in _supportReceiveListeners)
            {
                listener.OnReceiveSupportAction(element,ref value);
            }
        }

        public void OnShieldLost(THolder element,ref TValue value)
        {
            foreach (var listener in _vitalityChangeListeners)
            {
                listener.OnShieldLost(element,ref value);
            }
        }

        public void OnHealthLost(THolder element,ref TValue value)
        {
            foreach (var listener in _vitalityChangeListeners)
            {
                listener.OnHealthLost(element,ref value);
            }
        }

        public void OnFirstAction(TTempo element)
        {
            foreach (var listener in _tempoListeners)
            {
                listener.OnFirstAction(element);
            }
        }

        public void OnFinishAction(TTempo element)
        {
            foreach (var listener in _tempoListeners)
            {
                listener.OnFinishAction(element);
            }
        }

        public void OnFinishAllActions(TTempo element)
        {
            foreach (var listener in _tempoListeners)
            {
                listener.OnFinishAllActions(element);
            }
        }

        public void OnCantAct(TTempo element)
        {
            foreach (var listener in _tempoDisruptionListeners)
            {
                listener.OnCantAct(element);
            }
        }

        public void OnRoundFinish(TTempo lastElement)
        {
            foreach (var listener in _roundListeners)
            {
                listener.OnRoundFinish(lastElement);
            }
        }
    }

}
