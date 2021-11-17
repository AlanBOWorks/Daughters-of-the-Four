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
        IOffensiveActionReceiverListener<ISkillParameters, CombatingEntity, SkillComponentResolution>
    {
        private ISkillParameters _skillParameters;
        private CombatingEntity _receiver;

        public void OnShieldLost()
        {
            var events = CombatSystemSingleton.EventsHolder;
            events.OnShieldLost(_skillParameters, _receiver);
        }

        public void OnHealthLost()
        {
            var events = CombatSystemSingleton.EventsHolder;
            events.OnHealthLost(_skillParameters, _receiver);
            HandlePossibleDeath();
        }
        public void OnMortalityLost()
        {
            var events = CombatSystemSingleton.EventsHolder;
            events.OnMortalityLost(_skillParameters,_receiver);
            HandlePossibleDeath();
        }

        private void HandlePossibleDeath()
        {
            var stats = _receiver.CombatStats;
            if(stats.CurrentMortality > 0 || stats.CurrentHealth > 0) return;

            OnMortalityDeath();
        }

        private void OnMortalityDeath()
        {
            CombatSystemSingleton.EntityDeathHandler.EnQueueEntity(_skillParameters.Target);
        }

        public void OnReceiveOffensiveAction(ISkillParameters holder, CombatingEntity receiver)
        {
            _skillParameters = holder;
            _receiver = receiver;
        }

        public void OnReceiveOffensiveEffect(CombatingEntity receiver, ref SkillComponentResolution value)
        {

        }

        public void OnShieldDamage()
        {
            var events = CombatSystemSingleton.EventsHolder;
            events.OnShieldDamage(_skillParameters, _receiver);

        }

        public void OnHealthDamage()
        {
            var events = CombatSystemSingleton.EventsHolder;
            events.OnHealthDamage(_skillParameters, _receiver);
        }

        public void OnMortalityDamage()
        {
            var events = CombatSystemSingleton.EventsHolder;
            events.OnMortalityDamage(_skillParameters, _receiver);
        }
    }
}
