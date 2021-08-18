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
        private readonly StackableBuffPool _onHitIncrease;
        [ShowInInspector]
        private readonly StackableBuffPool _onHitRemove;
        private List<ICombatHitListener> _onHitListeners;

        public InHitEventHandler(CombatingEntity user)
        {
            _user = user;
            _onHitInvoke = new StackableBuffPool();
            _onHitIncrease = new StackableBuffPool();
            _onHitRemove = new StackableBuffPool();
            _onHitListeners = new List<ICombatHitListener>();
        }

        public void AddBuff(IDelayBuff buff, CombatingEntity actor,EnumSkills.HitType hitType)
        {
            var buffValues = new StackableBuffValues(buff,actor);
            switch (hitType)
            {
                case EnumSkills.HitType.DirectHit:
                    _onHitInvoke.Add(buffValues);
                    break;
                case EnumSkills.HitType.OnHitIncrease:
                    _onHitIncrease.Add(buffValues);
                    break;
                case EnumSkills.HitType.OnHitCancel:
                    _onHitRemove.Add(buffValues);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Hit type [{hitType}] is not implemented");
            }
        }


        public void OnHit(float damage)
        {
            foreach (ICombatHitListener listener in _onHitListeners)
            {
                listener.OnHit(damage);
            }
            _onHitInvoke.Invoke(_user);
            _onHitIncrease.IncreaseStacks();
            _onHitRemove.Clear();
        }

        public void OnNotBeingHitSequence()
        {
            foreach (ICombatHitListener listener in _onHitListeners)
            {
                listener.OnNotBeingHitSequence();
            }
            _onHitRemove.InvokeAndClear(_user);
            _onHitIncrease.InvokeAndClear(_user);
            _onHitInvoke.Clear();
        }


        public void Subscribe(ICombatHitListener listener)
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
        public void UnSubscribe(ICombatHitListener listener)
        {
            _onHitListeners.Remove(listener);
        }

        public void OnInitiativeTrigger()
        {
            OnNotBeingHitSequence();

        }

        public void OnDoMoreActions()
        { }

        public void OnFinisAllActions()
        { }


#if UNITY_EDITOR
        private class DebuggHit : IDelayBuff
        {
            private readonly string _messange;
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
        void Subscribe(ICombatHitListener listener);
        void UnSubscribe(ICombatHitListener listener);

    }

    public interface ICombatHitListener
    {
        void OnHit(float damage);
        void OnNotBeingHitSequence();
    }
}
