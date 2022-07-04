using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Skills.Effects;
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
        /// This depends on the attacker's behalf; for knowing if damage
        /// had happen check: [<seealso cref="IDamageDoneListener.OnDamageReceive"/>]
        /// </summary>
        void OnDamageBeforeDone(CombatEntity performer, CombatEntity target, float amount);

        void OnRevive(CombatEntity entity, bool isHealRevive);

    }

    public interface IDamageDoneListener : ICombatEventListener
    {
        void OnShieldLost(CombatEntity performer, CombatEntity target, float amount);
        void OnHealthLost(CombatEntity performer, CombatEntity target, float amount);
        void OnMortalityLost(CombatEntity performer, CombatEntity target, float amount);
        /// <summary>
        /// After call of the concrete events calls:
        /// <br></br>- <see cref="OnShieldLost"/>
        /// <br></br>- <see cref="OnHealthLost"/>
        /// <br></br>- <see cref="OnMortalityLost"/>
        /// <br></br><br></br>
        /// Note:<br></br>
        /// It the receiving end of the event [<seealso cref="IVitalityChangeListener.OnDamageBeforeDone"/>], a.k.a: depends if the
        /// damage had happen.
        /// </summary>
        void OnDamageReceive(CombatEntity performer, CombatEntity target);
        /// <summary>
        /// Event raised only if all Vitality values becomes zero
        /// </summary>
        void OnKnockOut(CombatEntity performer, CombatEntity target);
    }

    public interface IRecoveryDoneListener : ICombatEventListener
    {
        void OnShieldGain(CombatEntity performer, CombatEntity target, float amount);
        void OnHealthGain(CombatEntity performer, CombatEntity target, float amount);
        void OnMortalityGain(CombatEntity performer, CombatEntity target, float amount);
        /// <summary>
        /// After call of the concrete events calls:
        /// <br></br>- <see cref="OnShieldGain"/>
        /// <br></br>- <see cref="OnHealthGain"/>
        /// <br></br>- <see cref="OnMortalityGain"/>
        /// </summary>
        void OnRecoveryReceive(CombatEntity performer, CombatEntity target);
        void OnKnockHeal(EntityPairInteraction entities, int currentTick, int amount);
    }

    public interface IStatsChangeListener
    {
        void OnBuffDone(EntityPairInteraction entities, IBuffEffect buff, float effectValue);
        void OnDeBuffDone(EntityPairInteraction entities, IDeBuffEffect deBuff, float effectValue);
    }
}
