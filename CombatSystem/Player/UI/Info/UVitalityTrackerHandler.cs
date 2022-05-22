using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UVitalityTrackerHandler : UTeamColumnElementSpawner<UVitalityInfo>, IDamageDoneListener
    {
        public void OnShieldLost(in CombatEntity performer, in CombatEntity target, in float amount)
        { }

        public void OnHealthLost(in CombatEntity performer, in CombatEntity target, in float amount)
        { }

        public void OnMortalityLost(in CombatEntity performer, in CombatEntity target, in float amount)
        { }

        public void OnDamageReceive(in CombatEntity performer, in CombatEntity target)
        {
            UpdateTarget(in target);
        }

        public void OnKnockOut(in CombatEntity performer, in CombatEntity target)
        {
            UpdateTarget(in target);
        }

        private void UpdateTarget(in CombatEntity target)
        {
            var dictionary = GetDictionary();
            if (dictionary.ContainsKey(target))
                dictionary[target].UpdateToCurrentStats();
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
        public static void HandleHandler(in UVitalityInfo element, in CombatEntity entity, in TeamStructureIterationValues values)
        {
            bool isPlayerElement = values.IsPlayerElement;

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
