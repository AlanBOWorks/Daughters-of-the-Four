using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UFixedVitalityInfoHandler : UTeamStructureInstantiateHandler<UVitalityInfo>, IDamageDoneListener
    {
        public void OnShieldLost(in CombatEntity target, in CombatEntity performer, in float amount)
        {
            if(ActiveElementsDictionary.ContainsKey(target))
                ActiveElementsDictionary[target].UpdateToCurrentStats();
        }

        public void OnHealthLost(in CombatEntity target, in CombatEntity performer, in float amount)
        {
            if(ActiveElementsDictionary.ContainsKey(target))
                ActiveElementsDictionary[target].UpdateToCurrentStats();
        }

        public void OnMortalityLost(in CombatEntity target, in CombatEntity performer, in float amount)
        {
            if(ActiveElementsDictionary.ContainsKey(target))
                ActiveElementsDictionary[target].UpdateToCurrentStats();
        }

        public void OnKnockOut(in CombatEntity target, in CombatEntity performer)
        {
            if(ActiveElementsDictionary.ContainsKey(target))
                ActiveElementsDictionary[target].UpdateToCurrentStats();
        }

        /* UOnEntityCreatedSpawner VARIATION
         *
        protected override UVitalityInfo GenerateElement(in CombatEntity entity, in EntityElementSpawner spawner)
        {
            var generatedElement = base.GenerateElement(in entity, in spawner);
            int index = spawner.ActiveCount - 1;
            generatedElement.RepositionByIndexHeight(index);

            return generatedElement;
        }*/

        public override void OnIterationCall(in UVitalityInfo element, in CombatEntity entity, int notNullIndex)
        {
            var elementGameObject = element.gameObject;
            if(entity == null)
            {
                elementGameObject.SetActive(false);
                return;
            }

            element.EntityInjection(in entity);
            element.RepositionByIndexHeight(notNullIndex);
            elementGameObject.SetActive(true);
        }
    }
}
