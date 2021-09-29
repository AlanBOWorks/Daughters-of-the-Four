using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace CombatSystem.Events
{
    public class CharacterEventsHolder<T, TTempo, TStat> : 
        IStatActionListener<T,TTempo,TStat>, IRoundListener<TTempo>
    {
        public CharacterEventsHolder()
        {
            _offensiveListeners = new List<IOffensiveActionListener<T,TStat>>();
            _supportListeners = new List<ISupportActionListener<T,TStat>>();
            _vitalityChangeListeners = new List<IVitalityChangeListener<T,TStat>>();
            _tempoListeners = new List<ITempoListener<TTempo>>();
            _roundListeners = new List<IRoundListener<TTempo>>();
        }

        [ShowInInspector,HorizontalGroup("Action stats",Title = "Frequent Events ________________________")]
        private readonly List<IOffensiveActionListener<T,TStat>> _offensiveListeners;
        [ShowInInspector, HorizontalGroup("Action stats", Title = "Frequent Events ________________________")]
        private readonly List<ISupportActionListener<T,TStat>> _supportListeners;
        [ShowInInspector, HorizontalGroup("Reaction stats")]
        private readonly List<IVitalityChangeListener<T,TStat>> _vitalityChangeListeners;
        [ShowInInspector, HorizontalGroup("Reaction stats")]
        private readonly List<ITempoListener<TTempo>> _tempoListeners;

        [Title("Round event")]
        [ShowInInspector] 
        private readonly List<IRoundListener<TTempo>> _roundListeners;


        public void Subscribe(IFullEventListener<T, TTempo, TStat> listener)
        {
            Subscribe(listener as IStatActionListener<T, TTempo, TStat>);
            Subscribe(listener as IRoundListener<TTempo>);
        }

        public void Subscribe(IStatActionListener<T,TTempo,TStat> listener)
        {
            _offensiveListeners.Add(listener);
            _supportListeners.Add(listener);
            _vitalityChangeListeners.Add(listener);
            _tempoListeners.Add(listener);
        }


        public void Subscribe(IOffensiveActionListener<T, TStat> listener) => _offensiveListeners.Add(listener);
        public void Subscribe(ISupportActionListener<T, TStat> listener) => _supportListeners.Add(listener);
        public void Subscribe(IVitalityChangeListener<T, TStat> listener) => _vitalityChangeListeners.Add(listener);
        public void Subscribe(ITempoListener<TTempo> listener) => _tempoListeners.Add(listener);
        public void Subscribe(IRoundListener<TTempo> listener) => _roundListeners.Add(listener);

        public void OnPerformOffensiveAction(T element, TStat value)
        {
            foreach (var listener in _offensiveListeners)
            {
                listener.OnPerformOffensiveAction(element, value);
            }
        }

        public void OnReceiveOffensiveAction(T element, TStat value)
        {
            foreach (var listener in _offensiveListeners)
            {
                listener.OnReceiveOffensiveAction(element, value);
            }
        }

        public void OnPerformSupportAction(T element, TStat value)
        {
            foreach (var listener in _supportListeners)
            {
                listener.OnPerformSupportAction(element, value);
            }
        }
        public void OnReceiveSupportAction(T element, TStat value)
        {
            foreach (var listener in _supportListeners)
            {
                listener.OnReceiveSupportAction(element, value);
            }
        }

        public void OnRecoveryReceiveAction(T element, TStat value)
        {
            foreach (var listener in _vitalityChangeListeners)
            {
                listener.OnRecoveryReceiveAction(element, value);
            }
        }

        public void OnDamageReceiveAction(T element, TStat value)
        {
            foreach (var listener in _vitalityChangeListeners)
            {
                listener.OnDamageReceiveAction(element, value);
            }
        }

        public void OnShieldLost(T element, TStat value)
        {
            foreach (var listener in _vitalityChangeListeners)
            {
                listener.OnShieldLost(element, value);
            }
        }

        public void OnHealthLost(T element, TStat value)
        {
            foreach (var listener in _vitalityChangeListeners)
            {
                listener.OnHealthLost(element, value);
            }
        }

        public void OnMortalityDeath(T element, TStat value)
        {
            foreach (var listener in _vitalityChangeListeners)
            {
                listener.OnMortalityDeath(element, value);
            }
        }

        public void OnInitiativeTrigger(TTempo element)
        {
            foreach (var listener in _tempoListeners)
            {
                listener.OnInitiativeTrigger(element);
            }
        }

        public void OnDoMoreActions(TTempo element)
        {
            foreach (var listener in _tempoListeners)
            {
                listener.OnDoMoreActions(element);
            }
        }

        public void OnFinishAllActions(TTempo element)
        {
            foreach (var listener in _tempoListeners)
            {
                listener.OnFinishAllActions(element);
            }
        }

        public void OnSkipActions(TTempo element)
        {
            foreach (var listener in _tempoListeners)
            {
                listener.OnSkipActions(element);
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
