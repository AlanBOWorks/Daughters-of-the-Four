using System;
using System.Collections.Generic;
using _CombatSystem;
using Skills;
using UnityEngine;

namespace Characters
{
    public class CharacterCriticalActionHandler : List<IOnCriticalListener>, ITempoListenerVoid
    {
        private readonly CombatingEntity _user;
        public SDelayBuffPreset CriticalBuff { set; private get; }

        public CharacterCriticalActionHandler(CombatingEntity user, int memorySize = 0) : base(memorySize)
        {
            _user = user;
        }



        private bool _isFirstCritical;
        private void OnFirstCritical()
        {
            if(!_isFirstCritical) return;
            
            foreach (IOnCriticalListener listener in this)
            {
                listener.OnFirstCritical();
            }
            _isFirstCritical = true;

            if (CriticalBuff != null)
                _user.DelayBuffHandler.EnqueueBuff(CriticalBuff,_user);
        }

        public void OnCriticalAction()
        {
            foreach (IOnCriticalListener listener in this)
            {
                listener.OnCriticalAction();
            }
            OnFirstCritical();
        }

        public void OnInitiativeTrigger()
        {
            _isFirstCritical = false;
        }
        public void OnFinisAllActions()
        {
            _isFirstCritical = false;
        }
        // There's to [_isFirstCritical = false] because counter attacks can crit as well and trigger
        // this effect once more; In other words => 2 Critical Triggers per Initiative

        public void OnDoMoreActions()
        { }
    }

    public interface IOnCriticalListener
    {
        void OnFirstCritical();
        void OnCriticalAction();
    }
}
