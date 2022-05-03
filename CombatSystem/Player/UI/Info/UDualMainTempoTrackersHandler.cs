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
    public class UDualMainTempoTrackersHandler : UDualTeamMainStructureInstantiateHandler<UTempoTrackerHolder>,
        ITempoEntityPercentListener, ITempoDedicatedEntityStatesListener
    {
        [Title("OffRole - References")]
        [SerializeField] private RectTransform offRolesParent;

        public void OnEntityTick(in CombatEntity entity, in float currentTick, in float percentInitiative)
        {
            if (GetActiveElementsDictionary().ContainsKey(entity))
                GetActiveElementsDictionary()[entity].TickTempo(in currentTick, in percentInitiative);
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


        public void OnTrinityEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            //todo make activeAnimation 
        }

        public void OnOffEntityRequestSequence(CombatEntity entity, bool canAct)
        {
        }

        public void OnTrinityEntityFinishSequence(CombatEntity entity)
        {
            UtilsTempoInfosHandler.HandleOnFinishSequence(this, in entity);
        }

        public void OnOffEntityFinishSequence(CombatEntity entity)
        {
            
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

        public static void HandleOnFinishSequence(ITempoEntityPercentListener listener,in CombatEntity entity)
        {
            UtilsCombatStats.CalculateTempoPercent(entity.Stats, out var currentTick, out var initiativePercent);
            listener.OnEntityTick(in entity, in currentTick, in initiativePercent);
        }
    }
}
