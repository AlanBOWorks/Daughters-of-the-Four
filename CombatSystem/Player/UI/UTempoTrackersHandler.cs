using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UTempoTrackersHandler : UTeamStructureInstantiateHandler<UTempoTrackerHolder>, ITempoEntityPercentListener
    {
        public void OnEntityTick(in CombatEntity entity, in float currentInitiative, in float percentInitiative)
        {
            if(ActiveElementsDictionary.ContainsKey(entity))
                ActiveElementsDictionary[entity].TickTempo(in currentInitiative, in percentInitiative);
        }

        public override void OnIterationCall(in UTempoTrackerHolder element, in CombatEntity entity, int notNullIndex)
        {
            element.RepositionLocalHeight(notNullIndex);
            HandleActive(in element, in entity);
        }

        private static void HandleActive(in UTempoTrackerHolder element, in CombatEntity entity)
        {
            if (entity == null)
            {
                element.gameObject.SetActive(false);
                return;
            }

            element.EntityInjection(in entity);
            element.ShowElement();
        }

    }

    /*public class UTempoTrackersHandler : UOnEntityCreatedSpawner<UTempoTrackerHolder>, ITempoEntityPercentListener
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
