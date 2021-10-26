using CombatEffects;
using CombatEntity;
using CombatSkills;
using UnityEngine;

namespace CombatSystem.Events
{
    public sealed class DamageReceiveEventsInvoker : 
        IOffensiveActionReceiverListener<SkillValuesHolders,SkillComponentResolution>
    {
       

        private SkillValuesHolders _skillsHolder;
        private SkillComponentResolution _values;
        public void OnReceiveOffensiveAction(SkillValuesHolders element, ref SkillComponentResolution value)
        {
            _skillsHolder = element;
            _values = value;

        }

        public void OnShieldLost()
        {
            var events = CombatSystemSingleton.EventsHolder;
            events.OnShieldLost(_skillsHolder, ref _values);
        }

        public void OnHealthLost()
        {
            var events = CombatSystemSingleton.EventsHolder;
            events.OnHealthLost(_skillsHolder, ref _values);
        }

        public void OnMortalityDeath()
        {
            CombatSystemSingleton.EntityDeathHandler.EnQueueEntity(_skillsHolder.Target);
        }
    }
}
