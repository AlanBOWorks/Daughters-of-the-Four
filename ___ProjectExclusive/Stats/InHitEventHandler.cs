using System;
using System.Collections.Generic;
using _CombatSystem;
using Characters;
using Sirenix.OdinInspector;
using Skills;
using UnityEngine;

namespace Stats
{
    public class InHitEventHandler : ICombatHitListener, IHitEventHandler, ITempoListenerVoid
    {
        private readonly CombatingEntity _user;
        [ShowInInspector]
        private readonly StackableBuffPool _onHitInvoke;
        [ShowInInspector]
        private readonly StackableBuffPool _onNoHitInvoke;
        private List<ICombatHitListener> _onHitListeners;

        public InHitEventHandler(CombatingEntity user)
        {
            _user = user;
            _onHitInvoke = new StackableBuffPool();
            _onNoHitInvoke = new StackableBuffPool();
            _onHitListeners = new List<ICombatHitListener>();
        }

        public void AddBuff(IDelayBuff buff, CombatingEntity actor,bool isOnHit)
        {
            var buffValues = new StackableBuffValues(buff,actor);
            if (isOnHit)
            {
                _onHitInvoke.Add(buffValues);
            }
            else
            {
                _onNoHitInvoke.Add(buffValues);
            }
        }


        public void OnHit(float damage)
        {
            foreach (ICombatHitListener listener in _onHitListeners)
            {
                listener.OnHit(damage);
            }
            _onHitInvoke.InvokeAndClear(_user);
            _onNoHitInvoke.Clear();
        }

        public void OnNotBeingHitSequence()
        {
            foreach (ICombatHitListener listener in _onHitListeners)
            {
                listener.OnNotBeingHitSequence();
            }
            _onNoHitInvoke.InvokeAndClear(_user);
            _onHitInvoke.Clear();
        }


        public void SubscribeListener(ICombatHitListener listener)
        {
            if (_onHitListeners == null)
            {
                _onHitListeners = new List<ICombatHitListener> { listener };
            }
            else
            {
                _onHitListeners.Add(listener);
            }
        }
        public void UnSubscribeListener(ICombatHitListener listener)
        {
            _onHitListeners.Remove(listener);
        }

        public void OnInitiativeTrigger()
            => OnNotBeingHitSequence();
        
        public void OnDoMoreActions()
        { }

        public void OnFinisAllActions()
        { }


#if UNITY_EDITOR
        private class DebuggHit : IDelayBuff
        {
            private string _messange;
            public DebuggHit(bool isHit)
            {
                if (isHit)
                    _messange = "Hit happens";
                else
                {
                    _messange = "Reach sequence without Hits";
                }
            }
            public TempoHandler.TickType GetTickType()
            {
                return TempoHandler.TickType.OnBeforeSequence;
            }

            public void DoBuff(CombatingEntity user, CombatingEntity target, float stacks)
            {
                Debug.Log(_messange);
            }

            public int MaxStack { get; } = 1;
        } 
#endif
    }

    public interface IHitEventHandler
    {
        void SubscribeListener(ICombatHitListener listener);
        void UnSubscribeListener(ICombatHitListener listener);

    }

    public interface ICombatHitListener
    {
        void OnHit(float damage);
        void OnNotBeingHitSequence();
    }
}
