using CombatSystem._Core;
using CombatSystem.Entity;
using UnityEngine;

namespace CombatSystem.Stats
{
    /// <summary>
    /// Higher level of events for changes on the [<seealso cref="IVitalityStats{T}"/>] values
    /// </summary>
    public interface IVitalityChangeListeners : ICombatEventListener
    {
        /// <summary>
        /// Event raised every time a damage is perform; this ignores if its a shield, health or mortality damage;
        /// It also triggers before of all events of [<see cref="IDamageDoneListener"/>]'s
        /// </summary>
        void OnDamageDone(in CombatEntity target, in CombatEntity performer, in float amount);

    }

    public interface IDamageDoneListener : ICombatEventListener
    {
        
        void OnShieldLost(in CombatEntity target, in CombatEntity performer, in float amount);
        void OnHealthLost(in CombatEntity target, in CombatEntity performer, in float amount);
        void OnMortalityLost(in CombatEntity target, in CombatEntity performer, in float amount);
        /// <summary>
        /// Event raised only if all Vitality values becomes zero
        /// </summary>
        void OnKnockOut(in CombatEntity target, in CombatEntity performer);
    }
}
