using System.Collections.Generic;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CombatSystem
{
    public class TempoEvents
    {
        [ShowInInspector]
        public List<ITempoListener> TempoListeners { get; }
        [ShowInInspector]
        public List<IRoundListener> RoundListeners { get; }

        [ShowInInspector]
        public List<ISkippedTempoListener> SkippedListeners { get; }


        public TempoEvents()
        {
            TempoListeners = new List<ITempoListener>();
            RoundListeners = new List<IRoundListener>();
            SkippedListeners = new List<ISkippedTempoListener>();
        }

        public void Subscribe(ITempoListener listener)
        {
            TempoListeners.Add(listener);
        }

        public void Subscribe(IRoundListener listener)
        {
            RoundListeners.Add(listener);
        }

        public void UnSubscribe(ITempoListener listener)
        {
            TempoListeners.Remove(listener);
        }

        public void UnSubscribe(IRoundListener listener)
        {
            RoundListeners.Remove(listener);
        }
    }

}
