using CombatEffects;
using CombatEntity;
using CombatSkills;
using UnityEngine;

namespace CombatSystem.Events
{
    public sealed class DamageReceiveEventsInvoker : 
        IOffensiveActionReceiverListener<CombatEntityPairAction, SkillComponentResolution>
    {
       

        private CombatEntityPairAction _actors;
        private SkillComponentResolution _values;
        public void OnReceiveOffensiveAction(CombatEntityPairAction element, ref SkillComponentResolution value)
        {
            _actors = element;
            _values = value;

        }

        public void OnShieldLost()
        {
            var events = CombatSystemSingleton.EventsHolder;
            events.OnShieldLost(_actors, ref _values);
        }

        public void OnHealthLost()
        {
            var events = CombatSystemSingleton.EventsHolder;
            events.OnHealthLost(_actors, ref _values);
        }

        public void OnMortalityDeath()
        {
            CombatSystemSingleton.EntityDeathHandler.EnQueueEntity(_actors.Target);
        }
    }
}
