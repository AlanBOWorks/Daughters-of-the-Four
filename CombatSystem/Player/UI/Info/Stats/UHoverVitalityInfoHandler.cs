using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UHoverVitalityInfoHandler : MonoBehaviour,
         IDamageDoneListener, IVitalityChangeListener
    {
        [ShowInInspector,DisableInEditorMode]
        private IReadOnlyDictionary<CombatEntity, UUIBaseHoverEntityHolder> _infoDictionary;


        public void Injection(IReadOnlyDictionary<CombatEntity, UUIBaseHoverEntityHolder> dictionary)
        {
            _infoDictionary = dictionary;
        }

        public void UpdateEntityVitality(CombatEntity entity)
        {
            if (!_infoDictionary.ContainsKey(entity)) return;

            _infoDictionary[entity].GetHealthInfo().UpdateToCurrentStats();
        }

        [Button]
        public void UpdateAllStats()
        {
            foreach (var pair in _infoDictionary)
            {
                var info = pair.Value;
                info.GetHealthInfo().UpdateToCurrentStats();
            }
        }

        private void Awake()
        {
            CombatSystemSingleton.EventsHolder.Subscribe(this);
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
            UpdateEntityVitality(target);
        }

        public void OnKnockOut(CombatEntity performer, CombatEntity target)
        {
            UpdateEntityVitality(target);
        }

        public void OnDamageBeforeDone(CombatEntity performer, CombatEntity target, float amount)
        {
        }

        public void OnRevive(CombatEntity entity, bool isHealRevive)
        {
            UpdateEntityVitality(entity);
        }

    }
}
