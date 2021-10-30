using System.Collections.Generic;
using CombatEntity;
using CombatSkills;
using CombatSystem.Events;
using UnityEngine;

namespace CombatEffects
{
    public sealed class ProvokeHandler : IOffensiveActionReceiverListener<CombatingEntity>
    {
        public CombatingEntity CurrentBait { get; private set; }


        public void SwitchBait(CombatingEntity bait) => CurrentBait = bait;

        public void CheckForReset(CombatingEntity initiativeEntity)
        {
            if (CurrentBait == initiativeEntity)
                CurrentBait = null;
        }
        public void OnReceiveOffensiveAction(CombatingEntity offensiveReceiver)
        {
            if(CurrentBait == null) return;
            if (CurrentBait != offensiveReceiver)
            {
                CurrentBait.ProvokeEffects.Invoke();
            }
        }
    }

    public sealed class ProvokeEffectsHolder : Queue<EffectParameter>
    {
        public ProvokeEffectsHolder(CombatingEntity user)
        {
            _user = user;
        }
        private readonly CombatingEntity _user;

        public void Invoke()
        {
            //It targets it self while the EffectParameter.TargetingType will handle how receives the effect
            var entities = new CombatEntityPairAction(_user,_user); 
            while (Count > 0)
            {
                var effect = Dequeue();
                effect.DoEffect(entities);
            }
        }
    }
}
