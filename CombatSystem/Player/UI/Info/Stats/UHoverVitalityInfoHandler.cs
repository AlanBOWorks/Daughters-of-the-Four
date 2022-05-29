using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UHoverVitalityInfoHandler : UCombatEventsSubscriber, ITeamElementSpawnListener<UUIHoverEntity>,
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

        public void OnAfterElementsCreated(UTeamElementSpawner<UUIHoverEntity> holder)
        {
        }

        public void OnElementCreated(UUIHoverEntity element, CombatEntity entity,
            int index)
        {
            var healthInfo = element.GetHealthInfo();
            _infoDictionary.Add(entity,healthInfo);

            healthInfo.EntityInjection(in entity);
        }

        public void OnCombatEnd()
        {
            ClearEntities();
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

        public void OnDamageBeforeDone(in CombatEntity performer, in CombatEntity target, in float amount)
        {
        }

        public void OnRevive(in CombatEntity entity, bool isHealRevive)
        {
            _infoDictionary[entity].UpdateToCurrentStats();
        }

    }
}
