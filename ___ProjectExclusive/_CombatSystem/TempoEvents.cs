using System.Collections.Generic;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CombatSystem
{
    public class TempoEvents : ITempoTriggerHandler
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
            foreach (ITempoListener listener in TempoListeners)
            {
                listener.OnFinisAllActions(entity);
            }

        }
        public void OnRoundCompleted(List<CombatingEntity> allEntities, CombatingEntity lastEntity)
        {
            foreach (IRoundListener listener in RoundListeners)
            {
                listener.OnRoundCompleted(allEntities, lastEntity);
            }

            foreach (CombatingEntity entity in allEntities)
            {
                entity.DelayBuffHandler.OnRoundCompleted();
            }
        }

        public void OnSkippedEntity(CombatingEntity entity)
        {
            //TODO make something more concrete about this like UI pop up
#if UNITY_EDITOR
            Debug.LogWarning($"Skipped character: {entity.CharacterName}");
#endif
            
            entity.Events.OnFinisAllActions();
            foreach (var listener in SkippedListeners)
            {
                listener.OnSkippedEntity(entity);
            }
        }
    }

}
