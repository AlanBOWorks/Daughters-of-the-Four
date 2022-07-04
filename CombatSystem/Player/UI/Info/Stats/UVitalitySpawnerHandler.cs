using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UVitalitySpawnerHandler : UTeamColumnElementSpawner<UVitalityInfo>,
        IVitalityChangeListener,IDamageDoneListener, IRecoveryDoneListener
    {

        public void OnDamageBeforeDone(CombatEntity performer, CombatEntity target, float amount)
        {

        }

        public void OnRevive(CombatEntity entity, bool isHealRevive)
        {
            var dictionary = GetDictionary();
            dictionary[entity].HideKnockOut();
        }

        public void OnShieldLost(CombatEntity performer, CombatEntity target, float amount)
        { }

        public void OnHealthLost(CombatEntity performer, CombatEntity target, float amount)
        { }

        public void OnMortalityLost(CombatEntity performer, CombatEntity target, float amount)
        { }

        public void OnDamageReceive(CombatEntity performer, CombatEntity target)
        {
            UpdateTargetVitality(target);
        }

        public void OnKnockOut(CombatEntity performer, CombatEntity target)
        {
            var dictionary = GetDictionary();
            dictionary[target].ShowKnockOut();
        }


        public void OnShieldGain(CombatEntity performer, CombatEntity target, float amount)
        {
        }

        public void OnHealthGain(CombatEntity performer, CombatEntity target, float amount)
        {
        }

        public void OnMortalityGain(CombatEntity performer, CombatEntity target, float amount)
        {
        }

        public void OnRecoveryReceive(CombatEntity performer, CombatEntity target)
        {
            UpdateTargetVitality(target);
        }

        public void OnKnockHeal(EntityPairInteraction entities, int currentTick, int amount)
        {
            TickKnockOut(entities.Target, currentTick);
        }


        private void UpdateTargetVitality(CombatEntity target)
        {
            var dictionary = GetDictionary();
            dictionary[target].UpdateToCurrentStats();
        }

        private void TickKnockOut(CombatEntity target,int tick)
        {
            var dictionary = GetDictionary();
            dictionary[target].UpdateKnockOut(tick);
        }

        protected override void OnCreateElement(CombatEntity entity, UVitalityInfo element, bool isPlayerElement)
        {
            UtilsVitalityInfosHandler.HandleHandler(in element, in entity, isPlayerElement);
        }

    }

    public static class UtilsVitalityInfosHandler
    {
        public static void HandleHandler(in UVitalityInfo element, in CombatEntity entity, bool isPlayerElement)
        {
            if (isPlayerElement)
            {
                HandleActivePlayer(in element, in entity);
            }
            else
            {
                HandleActiveEnemy(in element, in entity);
            }
        }
        private static void HandleActivePlayer(in UVitalityInfo element, in CombatEntity entity)
        {
            if(element==null) return;

            if (entity == null)
            {
                element.DisableElement();
            }
            else
            {
                element.ShowElement();
            }
            element.EntityInjection(entity);
        }

        private static void HandleActiveEnemy(in UVitalityInfo element, in CombatEntity entity)
        {
            if(element==null) return;

            if (entity == null)
            {
                element.HideElement();
                return;
            }

            element.ShowElement();
            element.EntityInjection(entity);
        }
    }
}
