using System.Collections.Generic;
using CombatSystem._Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player
{
    public class PlayerCombatEventsHolder : CombatEventsHolder, ITempoTickListener
    {
        public PlayerCombatEventsHolder() : base()
        {
            _tempoTickListeners = new HashSet<ITempoTickListener>();
            var combatEventsHolder = (SystemCombatEventsHolder) CombatSystemSingleton.EventsHolder;
            combatEventsHolder.SubscribeEventsHandler(this);
        }

        [ShowInInspector]
        private readonly HashSet<ITempoTickListener> _tempoTickListeners;

        protected override void SubscribeTempo(ITempoTickListener tickListener)
        {
            _tempoTickListeners.Add(tickListener);
        }

        public void OnStartTicking()
        {
            foreach (var listener in _tempoTickListeners)
            {
                listener.OnStartTicking();
            }
        }

        public void OnTick()
        {
            foreach (var listener in _tempoTickListeners)
            {
                listener.OnTick();
            }
        }

        public void OnRoundPassed()
        {
            foreach (var listener in _tempoTickListeners)
            {
                listener.OnRoundPassed();
            }
        }

        public void OnStopTicking()
        {
            foreach (var listener in _tempoTickListeners)
            {
                listener.OnStopTicking();
            }
        }
    }
}
