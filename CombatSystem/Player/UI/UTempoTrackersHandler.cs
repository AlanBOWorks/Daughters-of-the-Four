using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UTempoTrackersHandler : UOnEntityCreatedSpawner<UTempoTrackerHolder>, ITempoEntityPercentListener
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
    }
}
