using System.Collections.Generic;
using _CombatSystem;
using _Player;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Characters
{
    public class TempoHandlerBase : ITempoTriggerHandler
    {
        [field: ShowInInspector]
        public List<ITempoListener> TempoListeners { get; }
        [field: ShowInInspector]
        public List<IRoundListener> RoundListeners { get; }

        private const int PredictedAmountOfListeners = 4;

        public TempoHandlerBase()
        {
            TempoListeners = new List<ITempoListener>(PredictedAmountOfListeners);
            RoundListeners = new List<IRoundListener>(PredictedAmountOfListeners);
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

        public void OnInitiativeTrigger(CombatingEntity entity)
        {
            foreach (ITempoListener listener in TempoListeners)
            {
                listener.OnInitiativeTrigger(entity);
            }
        }

        public void OnDoMoreActions(CombatingEntity entity)
        {
            foreach (ITempoListener listener in TempoListeners)
            {
                listener.OnDoMoreActions(entity);
            }
        }

        public void OnFinisAllActions(CombatingEntity entity)
        {
            Debug.Log("Finish Triggers");
            foreach (ITempoListener listener in TempoListeners)
            {
                listener.OnFinisAllActions(entity);
            }
        }
        public void OnRoundCompleted(List<CombatingEntity> allEntities, CombatingEntity lastEntity)
        {
            foreach (IRoundListener listener in RoundListeners)
            {
                listener.OnRoundCompleted(allEntities,lastEntity);
            }
        }

    }

}
