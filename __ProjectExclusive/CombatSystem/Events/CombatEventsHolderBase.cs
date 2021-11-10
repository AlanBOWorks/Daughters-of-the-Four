using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace CombatSystem.Events
{
    public class CombatEventsHolderBase<THolder, TTempo, TSkill, TEffect> :
        IOffensiveActionReceiverListener<THolder, TSkill, TEffect>,
        ISupportActionReceiverListener<THolder, TSkill, TEffect>,
        IVitalityChangeListener<THolder, TSkill>,
        ITempoListener<TTempo>,
        ITempoAlternateListener<TTempo>,
        IRoundListener<TTempo>
    {
        public CombatEventsHolderBase()
        {
            _offensiveReceiveListeners = new HashSet<IOffensiveActionReceiverListener<THolder, TSkill, TEffect>>();

            _supportReceiveListeners = new HashSet<ISupportActionReceiverListener<THolder, TSkill, TEffect>>();

            _vitalityChangeListeners = new HashSet<IVitalityChangeListener<THolder, TSkill>>();

            _tempoListeners = new HashSet<ITempoListener<TTempo>>();
            _tempoDisruptionListeners = new HashSet<ITempoAlternateListener<TTempo>>();
            _roundListeners = new HashSet<IRoundListener<TTempo>>();
        }

        [ShowInInspector, HorizontalGroup("Actions Events",
             Title = "________________________ Actions Events ________________________")]
        private readonly HashSet<IOffensiveActionReceiverListener<THolder, TSkill, TEffect>> _offensiveReceiveListeners;
        [ShowInInspector, HorizontalGroup("Actions Events")]
        private readonly HashSet<ISupportActionReceiverListener<THolder, TSkill, TEffect>> _supportReceiveListeners;

        [ShowInInspector, HorizontalGroup("Reaction stats",
             Title = "________________________ Reaction Events ________________________")]
        private readonly HashSet<IVitalityChangeListener<THolder, TSkill>> _vitalityChangeListeners;

        [ShowInInspector, HorizontalGroup("Tempo Events",
             Title = "________________________ Tempo Events ________________________")]
        private readonly HashSet<ITempoListener<TTempo>> _tempoListeners;
        private readonly HashSet<ITempoAlternateListener<TTempo>> _tempoDisruptionListeners;

        [ShowInInspector, HorizontalGroup("Tempo Events")]
        private readonly HashSet<IRoundListener<TTempo>> _roundListeners;

        protected void Subscribe(CombatEventsHolderBase<THolder, TTempo, TSkill, TEffect> listener)
        {
            _offensiveReceiveListeners.Add(listener);
            _supportReceiveListeners.Add(listener);
            _vitalityChangeListeners.Add(listener);
            _tempoListeners.Add(listener);
            _tempoDisruptionListeners.Add(listener);
            _roundListeners.Add(listener);
            _tempoListeners.Remove(listener);
        }

        public void Subscribe(IOffensiveActionReceiverListener<THolder, TSkill, TEffect> listener) => 
            _offensiveReceiveListeners.Add(listener);
        public void Subscribe(ISupportActionReceiverListener<THolder, TSkill, TEffect> listener) => 
            _supportReceiveListeners.Add(listener);
        public void Subscribe(IVitalityChangeListener<THolder, TSkill> listener) => 
            _vitalityChangeListeners.Add(listener);
        public void Subscribe(ITempoListener<TTempo> listener) => 
            _tempoListeners.Add(listener);
        public void Subscribe(ITempoAlternateListener<TTempo> listener) =>
            _tempoDisruptionListeners.Add(listener);
        public void Subscribe(IRoundListener<TTempo> listener) => 
            _roundListeners.Add(listener);
        public void UnSubscribe(ITempoListener<TTempo> listener) =>
            _tempoListeners.Remove(listener);

        public void OnReceiveOffensiveAction(THolder element, TSkill skillValue)
        {
            foreach (var listener in _offensiveReceiveListeners)
            {
                listener.OnReceiveOffensiveAction(element, skillValue);
            }
        }

        public void OnReceiveOffensiveEffect(THolder element, ref TEffect effectValue)
        {
            foreach (var listener in _offensiveReceiveListeners)
            {
                listener.OnReceiveOffensiveEffect(element,ref effectValue);
            }
        }

        public void OnReceiveSupportAction(THolder element, TSkill skillValue)
        {
            foreach (var listener in _supportReceiveListeners)
            {
                listener.OnReceiveSupportAction(element, skillValue);
            }
        }

        public void OnReceiveSupportEffect(THolder element, ref TEffect effectValue)
        {
            foreach (var listener in _supportReceiveListeners)
            {
                listener.OnReceiveSupportEffect(element, ref effectValue);
            }
        }

        public void OnShieldLost(THolder element, TSkill value)
        {
            foreach (var listener in _vitalityChangeListeners)
            {
                listener.OnShieldLost(element, value);
            }
        }

        public void OnHealthLost(THolder element, TSkill value)
        {
            foreach (var listener in _vitalityChangeListeners)
            {
                listener.OnHealthLost(element, value);
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
