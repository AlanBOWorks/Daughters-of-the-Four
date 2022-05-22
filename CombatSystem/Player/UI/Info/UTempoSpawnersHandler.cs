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
    public class UTempoSpawnersHandler : UTeamColumnElementSpawner<UTempoTrackerHolder>,
        ITempoEntityPercentListener, ITempoDedicatedEntityStatesListener
    {
        protected override void OnCreateElement(CombatEntity entity, UTempoTrackerHolder element, bool isPlayerElement)
        {
            UtilsTempoInfosHandler.HandleHandler(in element, in entity, isPlayerElement);
        }

        public void OnEntityTick(in CombatEntity entity, in float currentTick, in float percentInitiative)
        {
            var dictionary = GetDictionary();
            if (dictionary.ContainsKey(entity))
                dictionary[entity].TickTempo(in currentTick, in percentInitiative);
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
            UtilsTempoInfosHandler.HandleOnFinishSequence(this, in entity);
        }

    }

    public static class UtilsTempoInfosHandler
    {
        public static void HandleHandler(in UTempoTrackerHolder element, in CombatEntity entity, bool isPlayerElement)
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
