using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UHoverVitalityInfoHandler : UCombatEventsSubscriber, IUIHoverListener,
         IDamageDoneListener, IVitalityChangeListener
    {
        private Dictionary<CombatEntity, UVitalityInfo> _infoDictionary;

        protected override ICombatEventsHolder GetEventsHolder()
        {
            return PlayerCombatSingleton.PlayerCombatEvents;
        }

        private void Awake()
        {
            _infoDictionary = new Dictionary<CombatEntity, UVitalityInfo>();
        }

        public void OnElementCreated(in UUIHoverEntity element, in CombatEntity entity)
        {
            var healthInfo = element.GetHealthInfo();
            _infoDictionary.Add(entity,healthInfo);

            healthInfo.EntityInjection(in entity);
        }

        public void ClearEntities()
        {
            _infoDictionary.Clear();
        }

        public void OnShieldLost(in CombatEntity performer, in CombatEntity target, in float amount)
        {
        }

        public void OnHealthLost(in CombatEntity performer, in CombatEntity target, in float amount)
        {
        }

        public void OnMortalityLost(in CombatEntity performer, in CombatEntity target, in float amount)
        {
        }

        public void OnDamageReceive(in CombatEntity performer, in CombatEntity target)
        {
            _infoDictionary[target].UpdateToCurrentStats();
        }

        public void OnKnockOut(in CombatEntity performer, in CombatEntity target)
        {
            _infoDictionary[target].UpdateToCurrentStats();
        }

        public void OnDamageDone(in CombatEntity performer, in CombatEntity target, in float amount)
        {
        }

        public void OnTotalDamage(in CombatEntity target, in CombatEntity performer, in float amount)
        {
        }
    }
}
