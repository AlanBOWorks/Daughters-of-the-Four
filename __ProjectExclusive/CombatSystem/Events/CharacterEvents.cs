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
            _tempoDisruptionListeners = new List<ITempoDisruptionListener<TTempo>>();
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
        private readonly List<ITempoDisruptionListener<TTempo>> _tempoDisruptionListeners;

        [ShowInInspector, HorizontalGroup("Tempo Events")]
        private readonly List<IRoundListener<TTempo>> _roundListeners;

        public virtual void SubscribeListener(object listener)
        {
            if(listener is IOffensiveActionReceiverListener<T,TStat> offensiveListener)
                Subscribe(offensiveListener);
            if(listener is ISupportActionReceiverListener<T,TStat> supportListener)
                Subscribe(supportListener);
            if(listener is IVitalityChangeListener<T,TStat> vitalityChangeListener)
                Subscribe(vitalityChangeListener);
            if(listener is ITempoListener<TTempo> tempoListener)
                Subscribe(tempoListener);
            if(listener is IRoundListener<TTempo> roundListener)
                Subscribe(roundListener);
        }


        private void Subscribe(CharacterEventsHolder<T, TTempo, TStat> slaveEventsHolder)
        {
            Subscribe(slaveEventsHolder as IStatActionListener<T, TTempo, TStat>);
            Subscribe(slaveEventsHolder as IRoundListener<TTempo>);
        }


        private void Subscribe(IFullEventListener<T, TTempo, TStat> listener)
        {
            Subscribe(listener as IStatActionListener<T, TTempo, TStat>);
            Subscribe(listener as IRoundListener<TTempo>);
        }

        private void Subscribe(IEventListenerHandler<T, TTempo, TStat> listener)
        {
            Subscribe(listener as IOffensiveActionReceiverListener<T,TStat>);
            Subscribe(listener as ISupportActionReceiverListener<T,TStat>);
            Subscribe(listener as IVitalityChangeListener<T,TStat>);
            Subscribe(listener as ITempoListener<TTempo>);
            Subscribe(listener as IRoundListener<TTempo>);
        }

        private void Subscribe(IStatActionListener<T,TTempo,TStat> listener)
        {
            Subscribe(listener as IOffensiveActionReceiverListener<T, TStat>);
            Subscribe(listener as ISupportActionReceiverListener<T, TStat>);
            Subscribe(listener as IVitalityChangeListener<T, TStat>);
            Subscribe(listener as ITempoListener<TTempo>);
        }


        private void Subscribe(IOffensiveActionReceiverListener<T, TStat> listener)
        {
            _offensiveReceiveListeners.Add(listener);
            if(listener is IOffensiveActionListener<T,TStat> offensiveActionListener)
                _offensiveListeners.Add(offensiveActionListener);
        }

        private void Subscribe(ISupportActionReceiverListener<T, TStat> listener)
        {
            _supportReceiveListeners.Add(listener);
            if(listener is ISupportActionListener<T,TStat> supportActionListener)
                _supportListeners.Add(supportActionListener);
        }

        private void Subscribe(IVitalityChangeListener<T, TStat> listener) => _vitalityChangeListeners.Add(listener);
        private void Subscribe(ITempoListener<TTempo> listener) => _tempoListeners.Add(listener);
        private void Subscribe(IRoundListener<TTempo> listener) => _roundListeners.Add(listener);


        public void OnPerformOffensiveAction(T element,ref TStat value)
        {
            foreach (var listener in _offensiveListeners)
            {
                listener.OnPerformOffensiveAction(element,ref value);
            }
        }

        public void OnReceiveOffensiveAction(T element,ref TStat value)
        {
            foreach (var listener in _offensiveReceiveListeners)
            {
                listener.OnReceiveOffensiveAction(element,ref value);
            }
        }

        public void OnPerformSupportAction(T element,ref TStat value)
        {
            foreach (var listener in _supportListeners)
            {
                listener.OnPerformSupportAction(element,ref value);
            }
        }
        public void OnReceiveSupportAction(T element,ref TStat value)
        {
            foreach (var listener in _supportReceiveListeners)
            {
                listener.OnReceiveSupportAction(element,ref value);
            }
        }

        public void OnShieldLost(T element,ref TStat value)
        {
            foreach (var listener in _vitalityChangeListeners)
            {
                listener.OnShieldLost(element,ref value);
            }
        }

        public void OnHealthLost(T element,ref TStat value)
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
