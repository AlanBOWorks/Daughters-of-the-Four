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
    public class UMainTempoTrackersHandler : UTeamMainStructureInstantiateHandler<UTempoTrackerHolder>,
        ITempoEntityPercentListener
    {
        [Title("OffRole - References")]
        [SerializeField] private RectTransform offRolesParent;

        public void OnEntityTick(in CombatEntity entity, in float currentInitiative, in float percentInitiative)
        {
            if (GetActiveElementsDictionary().ContainsKey(entity))
                GetActiveElementsDictionary()[entity].TickTempo(in currentInitiative, in percentInitiative);
        }

        public override void OnIterationCall(in UTempoTrackerHolder element, in CombatEntity entity,
            in TeamStructureIterationValues values)
        {
            UtilsTempoInfosHandler.HandleHandler(in element, in entity, in values, out var repositionIndex);
            RepositionElementByIndex(in element, in repositionIndex);
        }

        public override void OnFinishPreStarts()
        {
            int offRolesIndex = GetEnemyHandler().activeCount + 1; //Since the last position is already used > +1 for put this below
            RepositionElementByIndex(in offRolesParent, offRolesIndex);
        }

        public const float HeightElementSeparation = 16 * 2 + 10;
        public static void RepositionElementByIndex(in UTempoTrackerHolder element, in int index)
        {
            var rectTransform = element.GetComponent<RectTransform>();
            RepositionElementByIndex(in rectTransform,in index);
        }
        public static void RepositionElementByIndex(in RectTransform rectTransform, in int index)
        {
            const float transformHeight = HeightElementSeparation;

            Vector3 localPosition = rectTransform.localPosition;
            localPosition.y = -transformHeight * index;
            rectTransform.localPosition = localPosition;
        }

    }

    public static class UtilsTempoInfosHandler
    {
        public static void HandleHandler(in UTempoTrackerHolder element, in CombatEntity entity,
            in TeamStructureIterationValues values,
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

        public static void HandleHandler(in UTempoTrackerHolder element, in CombatEntity entity,
            in TeamStructureIterationValues values)
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

        private static void HandleActivePlayer(in UTempoTrackerHolder element, in CombatEntity entity)
        {
            if (element == null) return;

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

        private static void HandleActiveEnemy(in UTempoTrackerHolder element, in CombatEntity entity)
        {
            if (element == null) return;

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
