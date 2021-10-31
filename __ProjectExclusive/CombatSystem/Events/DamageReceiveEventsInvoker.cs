using CombatEffects;
using CombatEntity;
using CombatSkills;
using UnityEngine;

namespace CombatSystem.Events
{
    // Calling events through CombatSystemSingleton.EventsHolder was a struggle;
    // This wrapper class is meant to reduce the possible errors making the code for the Damage event's calls.
    // Just call this events and everything should be simpler
    public sealed class DamageReceiveEventsInvoker : 
        IOffensiveActionReceiverListener<CombatEntityPairAction,CombatingSkill, SkillComponentResolution>
    {
        private CombatEntityPairAction _actors;
        private CombatingSkill _usedSkill;

        public void OnShieldLost()
        {
            var events = CombatSystemSingleton.EventsHolder;
            events.OnShieldLost(_actors, _usedSkill);
        }

        public void OnHealthLost()
        {
            var events = CombatSystemSingleton.EventsHolder;
            events.OnHealthLost(_actors, _usedSkill);
        }

        public void OnMortalityDeath()
        {
            CombatSystemSingleton.EntityDeathHandler.EnQueueEntity(_actors.Target);
        }

        public void OnReceiveOffensiveAction(CombatEntityPairAction element, CombatingSkill skill)
        {
            _actors = element;
            _usedSkill = skill;
        }

        public void OnReceiveOffensiveEffect(CombatEntityPairAction element, ref SkillComponentResolution value)
        {
            
        }
    }
}
