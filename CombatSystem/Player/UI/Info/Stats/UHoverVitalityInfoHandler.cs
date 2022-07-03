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

            healthInfo.EntityInjection(entity);
        }

        public void OnCombatEnd()
        {
            ClearEntities();
        }

        public void ClearEntities()
        {
            _infoDictionary.Clear();
        }

        public void OnShieldLost(CombatEntity performer, CombatEntity target, float amount)
        {
        }

        public void OnHealthLost(CombatEntity performer, CombatEntity target, float amount)
        {
        }

        public void OnMortalityLost(CombatEntity performer, CombatEntity target, float amount)
        {
        }

        public void OnDamageReceive(CombatEntity performer, CombatEntity target)
        {
            _infoDictionary[target].UpdateToCurrentStats();
        }

        public void OnKnockOut(CombatEntity performer, CombatEntity target)
        {
            _infoDictionary[target].UpdateToCurrentStats();
        }

        public void OnDamageBeforeDone(CombatEntity performer, CombatEntity target, float amount)
        {
        }

        public void OnRevive(CombatEntity entity, bool isHealRevive)
        {
            _infoDictionary[entity].UpdateToCurrentStats();
        }

    }
}
