using System.Collections.Generic;
using CombatEntity;
using CombatSkills;
using UnityEngine;

namespace CombatSystem.Events
{
    public class CombatingEntityEvents : 
        IOffensiveActionReceiverListener<CombatingEntity>, // ISkill because events could be invoked by Skills
        ISupportActionReceiverListener<CombatingEntity>
    {

        private List<IOffensiveActionReceiverListener<CombatingEntity>> _offensiveReceiverListeners;
        private List<ISupportActionReceiverListener<CombatingEntity>> _supportReceiverListeners;

        public void Clear()
        {
            _offensiveReceiverListeners.Clear();
            _supportReceiverListeners.Clear();
        }

        public void Subscribe(IOffensiveActionReceiverListener<CombatingEntity> listener)
        {
            if(_offensiveReceiverListeners == null) 
                _offensiveReceiverListeners = new List<IOffensiveActionReceiverListener<CombatingEntity>>();
            _offensiveReceiverListeners.Add(listener);
        }

        public void Subscribe(ISupportActionReceiverListener<CombatingEntity> listener)
        {
            if(_supportReceiverListeners == null)
                _supportReceiverListeners = new List<ISupportActionReceiverListener<CombatingEntity>>();
            _supportReceiverListeners.Add(listener);
        }
        public void OnReceiveOffensiveAction(CombatingEntity offensivePerformer)
        {
            if(_offensiveReceiverListeners == null) return;

            foreach (var listener in _offensiveReceiverListeners)
            {
                listener.OnReceiveOffensiveAction(offensivePerformer);
            }
        }

        public void OnReceiveSupportAction(CombatingEntity supportPerformer)
        {
            if(_supportReceiverListeners == null) return;

            foreach (var listener in _supportReceiverListeners)
            {
                listener.OnReceiveSupportAction(supportPerformer);
            }
        }
    }
}
