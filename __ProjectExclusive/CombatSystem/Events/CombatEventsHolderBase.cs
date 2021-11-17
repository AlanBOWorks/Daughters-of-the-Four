using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace CombatSystem.Events
{
    public class CombatEventsHolderBase<THolder, TActor, TActionReceiver, TEffect> :
        IOffensiveActionReceiverListener<THolder, TActionReceiver, TEffect>,
        ISupportActionReceiverListener<THolder, TActionReceiver, TEffect>,

        IVitalityLostListener<THolder, TActionReceiver>,
        IDamageReceiverListener<THolder,TActionReceiver>,

        ITempoListener<TActor>,
        ITempoAlternateListener<TActor>,
        IRoundListener<TActor>
    {
        public CombatEventsHolderBase()
        {
            _offensiveReceiveListeners = new HashSet<IOffensiveActionReceiverListener<THolder, TActionReceiver, TEffect>>();

            _supportReceiveListeners = new HashSet<ISupportActionReceiverListener<THolder, TActionReceiver, TEffect>>();


            _vitalityLostListeners = new HashSet<IVitalityLostListener<THolder, TActionReceiver>>();
            _damageReceiverListeners = new HashSet<IDamageReceiverListener<THolder, TActionReceiver>>();

            _tempoListeners = new HashSet<ITempoListener<TActor>>();
            _tempoDisruptionListeners = new HashSet<ITempoAlternateListener<TActor>>();
            _roundListeners = new HashSet<IRoundListener<TActor>>();
        }

        [ShowInInspector, HorizontalGroup("Actions Events",
             Title = "________________________ Actions Events ________________________")]
        private readonly HashSet<IOffensiveActionReceiverListener<THolder, TActionReceiver, TEffect>> _offensiveReceiveListeners;
        [ShowInInspector, HorizontalGroup("Actions Events")]
        private readonly HashSet<ISupportActionReceiverListener<THolder, TActionReceiver, TEffect>> _supportReceiveListeners;

        [ShowInInspector, HorizontalGroup("Vitality Events",
             Title = "________________________ Vitality Events ________________________")]
        private readonly HashSet<IVitalityLostListener<THolder, TActionReceiver>> _vitalityLostListeners;
        [ShowInInspector, HorizontalGroup("Vitality Events")]
        private readonly HashSet<IDamageReceiverListener<THolder, TActionReceiver>> _damageReceiverListeners;

        [ShowInInspector, HorizontalGroup("Tempo Events",
             Title = "________________________ Tempo Events ________________________")]
        private readonly HashSet<ITempoListener<TActor>> _tempoListeners;
        private readonly HashSet<ITempoAlternateListener<TActor>> _tempoDisruptionListeners;

        [ShowInInspector, HorizontalGroup("Tempo Events")]
        private readonly HashSet<IRoundListener<TActor>> _roundListeners;

        protected void Subscribe(CombatEventsHolderBase<THolder, TActor, TActionReceiver, TEffect> listener)
        {
            _offensiveReceiveListeners.Add(listener);
            _supportReceiveListeners.Add(listener);

            _vitalityLostListeners.Add(listener);
            _damageReceiverListeners.Add(listener);

            _tempoListeners.Add(listener);
            _tempoDisruptionListeners.Add(listener);
            _roundListeners.Add(listener);
            _tempoListeners.Remove(listener);
        }

        public void Subscribe(IOffensiveActionReceiverListener<THolder, TActionReceiver, TEffect> listener) => 
            _offensiveReceiveListeners.Add(listener);
        public void Subscribe(ISupportActionReceiverListener<THolder, TActionReceiver, TEffect> listener) => 
            _supportReceiveListeners.Add(listener);

        public void Subscribe(IVitalityLostListener<THolder, TActionReceiver> listener) => 
            _vitalityLostListeners.Add(listener);
        public void Subscribe(IDamageReceiverListener<THolder, TActionReceiver> listener) =>
            _damageReceiverListeners.Add(listener);

        public void Subscribe(ITempoListener<TActor> listener) => 
            _tempoListeners.Add(listener);
        public void Subscribe(ITempoAlternateListener<TActor> listener) =>
            _tempoDisruptionListeners.Add(listener);
        public void Subscribe(IRoundListener<TActor> listener) => 
            _roundListeners.Add(listener);
        public void UnSubscribe(ITempoListener<TActor> listener) =>
            _tempoListeners.Remove(listener);



        // ----- ACTION EVENTS -------
        public void OnReceiveOffensiveAction(THolder holder, TActionReceiver receiver)
        {
            foreach (var listener in _offensiveReceiveListeners)
            {
                listener.OnReceiveOffensiveAction(holder, receiver);
            }
        }

        public void OnReceiveOffensiveEffect(TActionReceiver receiver, ref TEffect effectValue)
        {
            foreach (var listener in _offensiveReceiveListeners)
            {
                listener.OnReceiveOffensiveEffect(receiver,ref effectValue);
            }
        }

        public void OnReceiveSupportAction(THolder holder, TActionReceiver receiver)
        {
            foreach (var listener in _supportReceiveListeners)
            {
                listener.OnReceiveSupportAction(holder, receiver);
            }
        }

        public void OnReceiveSupportEffect(TActionReceiver receiver, ref TEffect effectValue)
        {
            foreach (var listener in _supportReceiveListeners)
            {
                listener.OnReceiveSupportEffect(receiver, ref effectValue);
            }
        }


        // ----- VITALITY EVENTS -------

        public void OnShieldLost(THolder element, TActionReceiver receiver)
        {
            foreach (var listener in _vitalityLostListeners)
            {
                listener.OnShieldLost(element, receiver);
            }
        }

        public void OnHealthLost(THolder element, TActionReceiver receiver)
        {
            foreach (var listener in _vitalityLostListeners)
            {
                listener.OnHealthLost(element, receiver);
            }
        }

        public void OnMortalityLost(THolder element, TActionReceiver receiver)
        {
            foreach (var listener in _vitalityLostListeners)
            {
                listener.OnMortalityLost(element,receiver);
            }
        }
        public void OnShieldDamage(THolder element, TActionReceiver receiver)
        {
            foreach (var listener in _damageReceiverListeners)
            {
                listener.OnShieldDamage(element,receiver);
            }
        }

        public void OnHealthDamage(THolder element, TActionReceiver receiver)
        {
            foreach (var listener in _damageReceiverListeners)
            {
                listener.OnHealthDamage(element, receiver);
            }
        }

        public void OnMortalityDamage(THolder element, TActionReceiver receiver)
        {
            foreach (var listener in _damageReceiverListeners)
            {
                listener.OnMortalityDamage(element, receiver);
            }
        }



        // ----- TEMPO EVENTS -------
        public void OnFirstAction(TActor element)
        {
            foreach (var listener in _tempoListeners)
            {
                listener.OnFirstAction(element);
            }
        }

        public void OnFinishAction(TActor element)
        {
            foreach (var listener in _tempoListeners)
            {
                listener.OnFinishAction(element);
            }
        }

        public void OnFinishAllActions(TActor element)
        {
            foreach (var listener in _tempoListeners)
            {
                listener.OnFinishAllActions(element);
            }
        }

        public void OnCantAct(TActor element)
        {
            foreach (var listener in _tempoDisruptionListeners)
            {
                listener.OnCantAct(element);
            }
        }

        public void OnRoundFinish(TActor lastElement)
        {
            foreach (var listener in _roundListeners)
            {
                listener.OnRoundFinish(lastElement);
            }
        }

    }

}
