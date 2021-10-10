using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace CombatSystem.Events
{
    public class CharacterEventsHolder<T, TTempo, TStat> : 
        IStatActionListener<T,TTempo,TStat>, IEventListenerHandler<T,TTempo,TStat>
    {
        public CharacterEventsHolder()
        {
            _offensiveListeners = new List<IOffensiveActionListener<T,TStat>>();
            _offensiveReceiveListeners = new List<IOffensiveActionReceiverListener<T, TStat>>();

            _supportListeners = new List<ISupportActionListener<T,TStat>>();
            _supportReceiveListeners = new List<ISupportActionReceiverListener<T, TStat>>();

            _vitalityChangeListeners = new List<IVitalityChangeListener<T,TStat>>();

            _tempoListeners = new List<ITempoListener<TTempo>>();
            _roundListeners = new List<IRoundListener<TTempo>>();
        }

        [ShowInInspector,HorizontalGroup("Offensive Events", 
             Title = "________________________ Offensive Events ________________________")]
        private readonly List<IOffensiveActionListener<T,TStat>> _offensiveListeners;
        [ShowInInspector,HorizontalGroup("Offensive Events")]
        private readonly List<IOffensiveActionReceiverListener<T,TStat>> _offensiveReceiveListeners;
        [ShowInInspector, HorizontalGroup("Support Events", 
             Title = "________________________ Support Events ________________________")]
        private readonly List<ISupportActionListener<T,TStat>> _supportListeners;
        [ShowInInspector, HorizontalGroup("Support Events")]
        private readonly List<ISupportActionReceiverListener<T,TStat>> _supportReceiveListeners;

        [ShowInInspector, HorizontalGroup("Reaction stats", 
             Title = "________________________ Reaction Events ________________________")]
        private readonly List<IVitalityChangeListener<T,TStat>> _vitalityChangeListeners;

        [ShowInInspector, HorizontalGroup("Tempo Events", 
             Title = "________________________ Tempo Events ________________________")]
        private readonly List<ITempoListener<TTempo>> _tempoListeners;

        [ShowInInspector, HorizontalGroup("Tempo Events")]
        private readonly List<IRoundListener<TTempo>> _roundListeners;

        public void Subscribe(CharacterEventsHolder<T, TTempo, TStat> slaveEventsHolder)
        {
            Subscribe(slaveEventsHolder as IStatActionListener<T, TTempo, TStat>);
            Subscribe(slaveEventsHolder as IRoundListener<TTempo>);
        }

        public void Subscribe(IFullEventListener<T, TTempo, TStat> listener)
        {
            Subscribe(listener as IStatActionListener<T, TTempo, TStat>);
            Subscribe(listener as IRoundListener<TTempo>);
        }

        public void Subscribe(IEventListenerHandler<T, TTempo, TStat> listener)
        {
            Subscribe(listener as IOffensiveActionReceiverListener<T,TStat>);
            Subscribe(listener as ISupportActionReceiverListener<T,TStat>);
            Subscribe(listener as IVitalityChangeListener<T,TStat>);
            Subscribe(listener as ITempoListener<TTempo>);
            Subscribe(listener as IRoundListener<TTempo>);
        }

        public void Subscribe(IStatActionListener<T,TTempo,TStat> listener)
        {
            Subscribe(listener as IOffensiveActionReceiverListener<T, TStat>);
            Subscribe(listener as ISupportActionReceiverListener<T, TStat>);
            Subscribe(listener as IVitalityChangeListener<T, TStat>);
            Subscribe(listener as ITempoListener<TTempo>);
        }


        public void Subscribe(IOffensiveActionReceiverListener<T, TStat> listener)
        {
            _offensiveReceiveListeners.Add(listener);
            if(listener is IOffensiveActionListener<T,TStat> offensiveActionListener)
                _offensiveListeners.Add(offensiveActionListener);
        }

        public void Subscribe(ISupportActionReceiverListener<T, TStat> listener)
        {
            _supportReceiveListeners.Add(listener);
            if(listener is ISupportActionListener<T,TStat> supportActionListener)
                _supportListeners.Add(supportActionListener);
        }
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
            foreach (var listener in _offensiveReceiveListeners)
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
            foreach (var listener in _supportReceiveListeners)
            {
                listener.OnReceiveSupportAction(element, value);
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
