using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UVitalitySpawnerHandler : UTeamColumnElementSpawner<UVitalityInfo>,
        IVitalityChangeListener,IDamageDoneListener, IRecoveryDoneListener
    {

        public void OnDamageBeforeDone(in CombatEntity performer, in CombatEntity target, in float amount)
        {

        }

        public void OnRevive(in CombatEntity entity, bool isHealRevive)
        {
            var dictionary = GetDictionary();
            dictionary[entity].HideKnockOut();
        }

        public void OnShieldLost(in CombatEntity performer, in CombatEntity target, in float amount)
        { }

        public void OnHealthLost(in CombatEntity performer, in CombatEntity target, in float amount)
        { }

        public void OnMortalityLost(in CombatEntity performer, in CombatEntity target, in float amount)
        { }

        public void OnDamageReceive(in CombatEntity performer, in CombatEntity target)
        {
            UpdateTargetVitality(target);
        }

        public void OnKnockOut(in CombatEntity performer, in CombatEntity target)
        {
            var dictionary = GetDictionary();
            dictionary[target].ShowKnockOut();
        }


        public void OnShieldGain(in CombatEntity performer, in CombatEntity target, in float amount)
        {
        }

        public void OnHealthGain(in CombatEntity performer, in CombatEntity target, in float amount)
        {
        }

        public void OnMortalityGain(in CombatEntity performer, in CombatEntity target, in float amount)
        {
        }

        public void OnRecoveryReceive(in CombatEntity performer, in CombatEntity target)
        {
            UpdateTargetVitality(target);
        }

        public void OnKnockHeal(in CombatEntity performer, in CombatEntity target, in int currentTick, in int amount)
        {
            TickKnockOut(target, currentTick);
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
            element.EntityInjection(in entity);
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
            element.EntityInjection(in entity);
        }
    }
}
