using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UDualMainVitalityInfosHandler : UDualTeamMainStructureInstantiateHandler<UVitalityInfo>, IDamageDoneListener
    {
        [Title("OffRole - References")]
        [SerializeField] private RectTransform offRolesParent;

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
            if (GetActiveElementsDictionary().ContainsKey(target))
                GetActiveElementsDictionary()[target].UpdateToCurrentStats();
        }


        public override void OnIterationCall(in UVitalityInfo element, in CombatEntity entity, in TeamStructureIterationValues values)
        {
            UtilsVitalityInfosHandler.HandleHandler(in element, in entity, in values, out var repositionIndex);
            RepositionElementByIndex(in element, in repositionIndex);
        }

        public override void OnFinishPreStarts()
        {
            int offRolesIndex = GetEnemyHandler().activeCount +1; //Since the last position is already used > +1 for put this below
            RepositionElementByIndex(in offRolesParent, offRolesIndex);
        }

        private const float ElementMarginTop = 8;
        private static void RepositionElementByIndex(in UVitalityInfo element, in int index)
        {
            var rectTransform = element.GetComponent<RectTransform>();
            RepositionElementByIndex(in rectTransform, in index);
        }

        private const float RectHeight = 50;
        private static void RepositionElementByIndex(in RectTransform element, in int index)
        {
            var rectTransform = element.GetComponent<RectTransform>();
            var position = rectTransform.localPosition;

            position.y = -(RectHeight + ElementMarginTop) * index;

            rectTransform.localPosition = position;
        }
    }

    public static class UtilsVitalityInfosHandler
    {
        public static void HandleHandler(in UVitalityInfo element, in CombatEntity entity, in TeamStructureIterationValues values,
            out int repositionIndex)
        {
            bool isPlayerElement = values.IsPlayerElement;

            if (isPlayerElement)
            {
                repositionIndex = values.IterationIndex;
                HandleActivePlayer(in element, in entity);
            }
            else
            {
                repositionIndex = values.NotNullIndex;
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
