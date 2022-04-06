using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UMainVitalityInfosHandler : UTeamMainStructureInstantiateHandler<UVitalityInfo>, IDamageDoneListener
    {
        [Title("OffRole - References")]
        [SerializeField] private RectTransform offRolesParent;

        public void OnShieldLost(in CombatEntity target, in CombatEntity performer, in float amount)
        {
            if(GetActiveElementsDictionary().ContainsKey(target))
                GetActiveElementsDictionary()[target].UpdateToCurrentStats();
        }

        public void OnHealthLost(in CombatEntity target, in CombatEntity performer, in float amount)
        {
            if(GetActiveElementsDictionary().ContainsKey(target))
                GetActiveElementsDictionary()[target].UpdateToCurrentStats();
        }

        public void OnMortalityLost(in CombatEntity target, in CombatEntity performer, in float amount)
        {
            if(GetActiveElementsDictionary().ContainsKey(target))
                GetActiveElementsDictionary()[target].UpdateToCurrentStats();
        }

        public void OnKnockOut(in CombatEntity target, in CombatEntity performer)
        {
            if(GetActiveElementsDictionary().ContainsKey(target))
                GetActiveElementsDictionary()[target].UpdateToCurrentStats();
        }

        
        public override void OnIterationCall(in UVitalityInfo element, in CombatEntity entity, in TeamStructureIterationValues values)
        {
            UtilsVitalityInfosHandler.HandleHandler(in element, in entity, in values, out var repositionIndex);
            RepositionElementByIndex(in element, in repositionIndex);
        }

        public override void OnFinishPreStarts()
        {
            RepositionElementByIndex(in offRolesParent, GetEnemyHandler().activeCount);
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
