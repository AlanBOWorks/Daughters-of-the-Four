using System.Collections.Generic;
using _CombatSystem;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Characters
{
    public class CharacterTempoHandler : ITempoTriggerHandler
    {
        [ShowInInspector]
        private readonly Queue<ITempoListener> _listeners;
        [ShowInInspector]
        private readonly Queue<IPlayerTempoListener> _playerListeners;
        public bool CanControlEnemies;

        [ShowInInspector] private readonly Queue<IRoundListener> _roundListeners;

        private const int PredictedAmountOfListeners = 4;
        public CharacterTempoHandler(bool canControlEnemies = false)
        {
            _listeners = new Queue<ITempoListener>(PredictedAmountOfListeners);
            _playerListeners = new Queue<IPlayerTempoListener>(PredictedAmountOfListeners);
            CanControlEnemies = canControlEnemies;
            _roundListeners = new Queue<IRoundListener>(PredictedAmountOfListeners);

            _onTriggerHandler = new OnTriggerHandler(_listeners,_playerListeners);
            _onActionDoneHandler = new OnActionDoneHandler(_listeners,_playerListeners);
            _onFinishAllHandler = new OnFinishAllHandler(_listeners,_playerListeners);
        }

        private readonly OnTriggerHandler _onTriggerHandler;
        public void OnInitiativeTrigger(CombatingEntity entity)
        {
            _onTriggerHandler.DoActions(entity,CanControlEnemies);
        }

        private readonly OnActionDoneHandler _onActionDoneHandler;
        public void OnDoMoreActions(CombatingEntity entity)
        {
            _onActionDoneHandler.DoActions(entity,CanControlEnemies);
        }

        private readonly OnFinishAllHandler _onFinishAllHandler;
        public void OnFinisAllActions(CombatingEntity entity)
        {
            Debug.Log("Finish Triggers");
            _onFinishAllHandler.DoActions(entity,CanControlEnemies);
        }

        public void Subscribe(ITempoListener listener)
        {
            if (listener is IPlayerTempoListener playerTempoListener)
            {
                _playerListeners.Enqueue(playerTempoListener);
            }
            else
            {
                _listeners.Enqueue(listener);
            }
        }

        public void Subscribe(IRoundListener listener)
        {
            _roundListeners.Enqueue(listener);
        }

        //This is mainly for checking if is a Player Controllable and if so call the necessary Queue
        internal abstract class HandlerBase
        {
            private readonly Queue<ITempoListener> _listeners;
            private readonly Queue<IPlayerTempoListener> _playerListeners;

            protected HandlerBase(Queue<ITempoListener> listeners, Queue<IPlayerTempoListener> playerListeners)
            {
                _listeners = listeners;
                _playerListeners = playerListeners;
            }

            public void DoActions(CombatingEntity entity, bool canControlEnemies)
            {
                if (canControlEnemies || CharacterUtils.IsAPlayerEntity(entity))
                {
                    foreach (IPlayerTempoListener playerListener in _playerListeners)
                    {
                        OnListenerAction(playerListener,entity);
                    }
                }

                foreach (ITempoListener listener in _listeners)
                { 
                    OnListenerAction(listener,entity);
                }
            }

            protected abstract void OnListenerAction(ITempoListener listener, CombatingEntity entity);
        }

        internal class OnTriggerHandler : HandlerBase
        {
            public OnTriggerHandler(Queue<ITempoListener> listeners, Queue<IPlayerTempoListener> playerListeners) : base(listeners, playerListeners)
            { }
            protected override void OnListenerAction(ITempoListener listener, CombatingEntity entity)
            {
                listener.OnInitiativeTrigger(entity);
            }
        }
        internal class OnActionDoneHandler : HandlerBase
        {
            public OnActionDoneHandler(Queue<ITempoListener> listeners, Queue<IPlayerTempoListener> playerListeners) : base(listeners, playerListeners)
            { }

            protected override void OnListenerAction(ITempoListener listener, CombatingEntity entity)
            {
                listener.OnDoMoreActions(entity);
            }
        }
        internal class OnFinishAllHandler : HandlerBase
        {
            public OnFinishAllHandler(Queue<ITempoListener> listeners, Queue<IPlayerTempoListener> playerListeners) : base(listeners, playerListeners)
            {
            }

            protected override void OnListenerAction(ITempoListener listener, CombatingEntity entity)
            {
                listener.OnFinisAllActions(entity);
            }
        }

        public void OnRoundCompleted(List<CombatingEntity> allEntities, CombatingEntity lastEntity)
        {
            foreach (IRoundListener listener in _roundListeners)
            {
                listener.OnRoundCompleted(allEntities,lastEntity);
            }
        }
    }

    public interface IPlayerTempoListener : ITempoListener { }
}
