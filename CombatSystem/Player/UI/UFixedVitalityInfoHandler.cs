using CombatSystem.Entity;
using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UFixedVitalityInfoHandler : UOnEntityCreatedSpawner<UVitalityInfo>, IDamageDoneListener
    {
        public void OnShieldLost(in CombatEntity target, in CombatEntity performer, in float amount)
        {
            ActiveElementsDictionary[target].UpdateToCurrentStats();
        }

        public void OnHealthLost(in CombatEntity target, in CombatEntity performer, in float amount)
        {
            ActiveElementsDictionary[target].UpdateToCurrentStats();
        }

        public void OnMortalityLost(in CombatEntity target, in CombatEntity performer, in float amount)
        {
            ActiveElementsDictionary[target].UpdateToCurrentStats();
        }

        public void OnKnockOut(in CombatEntity target, in CombatEntity performer)
        {
            ActiveElementsDictionary[target].UpdateToCurrentStats();
        }
    }
}
