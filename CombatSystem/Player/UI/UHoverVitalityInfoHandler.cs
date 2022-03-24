using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UHoverVitalityInfoHandler : UCombatEventsSubscriber, IUIHoverListener,
         IDamageDoneListener, IVitalityChangeListeners
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

        public void OnElementCreated(in UUIHoverEntityHolder element, in CombatEntity entity)
        {
            var healthInfo = element.GetHealthInfo();
            _infoDictionary.Add(entity,healthInfo);

            healthInfo.Injection(entity.Stats);
        }

        public void ClearEntities()
        {
            _infoDictionary.Clear();
        }

        public void OnShieldLost(in CombatEntity target, in CombatEntity performer, in float amount)
        {
        }

        public void OnHealthLost(in CombatEntity target, in CombatEntity performer, in float amount)
        {
        }

        public void OnMortalityLost(in CombatEntity target, in CombatEntity performer, in float amount)
        {
        }

        public void OnKnockOut(in CombatEntity target, in CombatEntity performer)
        {
        }

        public void OnDamageDone(in CombatEntity target, in CombatEntity performer, in float amount)
        {
            _infoDictionary[target].UpdateToCurrentStats();
        }

        public void OnTotalDamage(in CombatEntity target, in CombatEntity performer, in float amount)
        {
        }
    }
}
