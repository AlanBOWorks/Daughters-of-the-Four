using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UMainTempoTrackersHandler : UTeamMainStructureInstantiateHandler<UTempoTrackerHolder>, ITempoEntityPercentListener
    {
        public void OnEntityTick(in CombatEntity entity, in float currentInitiative, in float percentInitiative)
        {
            if(GetActiveElementsDictionary().ContainsKey(entity))
                GetActiveElementsDictionary()[entity].TickTempo(in currentInitiative, in percentInitiative);
        }

        public override void OnIterationCall(in UTempoTrackerHolder element, in CombatEntity entity, in TeamStructureIterationValues values)
        {
            int heightIndex;
            bool isPlayerElement = values.IsPlayerElement;

            if (isPlayerElement)
            {
                heightIndex = values.IterationIndex;
                HandleActivePlayer(in element, in entity);
            }
            else
            {
                heightIndex = values.NotNullIndex;
                HandleActiveEnemy(in element, in entity);
            }
            element.RepositionLocalHeight(heightIndex);

        }

        private static void HandleActivePlayer(in UTempoTrackerHolder element, in CombatEntity entity)
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

        private static void HandleActiveEnemy(in UTempoTrackerHolder element, in CombatEntity entity)
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

    /*public class UMainTempoTrackersHandler : UOnEntityCreatedSpawner<UTempoTrackerHolder>, ITempoEntityPercentListener
    {
        public void OnEntityTick(in CombatEntity entity, in float currentInitiative, in float percentInitiative)
        {
            ActiveElementsDictionary[entity].TickTempo(in currentInitiative, in percentInitiative);
        }

        protected override UTempoTrackerHolder GenerateElement(in CombatEntity entity, in EntityElementSpawner spawner)
        {
            var generatedElement = base.GenerateElement(in entity, in spawner);
            generatedElement.RepositionLocalHeight(spawner.ActiveCount);

            return generatedElement;
        }
    }*/
}
