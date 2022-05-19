using CombatSystem._Core;
using CombatSystem.Entity;
using UnityEngine;

namespace CombatSystem.Stats
{
    /// <summary>
    /// Higher level of events for changes on the [<seealso cref="IVitalityStats{T}"/>] values
    /// </summary>
    public interface IVitalityChangeListener : ICombatEventListener
    {
        /// <summary>
        /// Event raised every time a damage is perform; this ignores if its a shield, health or mortality damage;
        /// It also triggers before of all events of [<see cref="IDamageDoneListener"/>]'s.
        /// <br></br><br></br>
        /// Note:<br></br>
        /// This depends on the attacker's behalf; for knowing if damage had happen check: [<seealso cref="IDamageDoneListener.OnDamageReceive"/>]
        /// </summary>
        void OnDamageDone(in CombatEntity performer, in CombatEntity target, in float amount);

        void OnRevive(in CombatEntity entity, bool isHealRevive);

    }

    public interface IDamageDoneListener : ICombatEventListener
    {
        void OnShieldLost(in CombatEntity performer, in CombatEntity target, in float amount);
        void OnHealthLost(in CombatEntity performer, in CombatEntity target, in float amount);
        void OnMortalityLost(in CombatEntity performer, in CombatEntity target, in float amount);
        /// <summary>
        /// After call of the concrete events calls:
        /// <br></br>- <see cref="OnShieldLost"/>
        /// <br></br>- <see cref="OnHealthLost"/>
        /// <br></br>- <see cref="OnMortalityLost"/>
        /// <br></br><br></br>
        /// Note:<br></br>
        /// It the receiving end of the event [<seealso cref="IVitalityChangeListener.OnDamageDone"/>], a.k.a: depends if the
        /// damage had happen.
        /// </summary>
        void OnDamageReceive(in CombatEntity performer, in CombatEntity target);
        /// <summary>
        /// Event raised only if all Vitality values becomes zero
        /// </summary>
        void OnKnockOut(in CombatEntity performer, in CombatEntity target);
    }
}
