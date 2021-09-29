using CombatTeam;
using UnityEngine;

namespace CombatSystem.Events
{ 
    public interface IFullEventListener<in T,in TTempo, in TValue> : IStatActionListener<T,TTempo,TValue>, IRoundListener<TTempo>
    { }

    public interface IStatActionListener<in T,in TTempo, in TStats> :
        IOffensiveActionListener<T, TStats>,
        ISupportActionListener<T, TStats>,
        IVitalityChangeListener<T, TStats>,
        ITempoListener<TTempo>
    { }

    /// <summary>
    /// Used when a [<see cref="CombatingEntity"/>] perform an offensive action
    /// </summary>
    /// <typeparam name="T">The type which is passed to all listeners</typeparam>
    /// <typeparam name="TValue">The type of value passed (generally a [<seealso cref="float"/>])</typeparam>
    public interface IOffensiveActionListener<in T, in TValue>
    {
        void OnPerformOffensiveAction(T element, TValue value);
        void OnReceiveOffensiveAction(T element, TValue value);
    }

    /// <summary>
    /// Used when a [<see cref="CombatingEntity"/>] perform a support action
    /// </summary>
    /// <typeparam name="T">The type which is passed to all listeners</typeparam>
    /// <typeparam name="TValue">The type of value passed (generally a [<seealso cref="float"/>])</typeparam>
    public interface ISupportActionListener<in T, in TValue>
    {
        void OnPerformSupportAction(T element, TValue value);
        void OnReceiveSupportAction(T element, TValue value);
    }

    /// <summary>
    /// Used when a [<see cref="CombatingEntity"/>] receive a change on vitality (damage, recovery, health at zero)
    /// </summary>
    /// <typeparam name="T">The type which is passed to all listeners</typeparam>
    /// <typeparam name="TValue">The type of value passed (generally a [<seealso cref="float"/>])</typeparam>
    public interface IVitalityChangeListener<in T, in TValue>
    {
        /// <summary>
        /// Heals, shields or any protection than avoids a possible death 
        /// </summary>
        void OnRecoveryReceiveAction(T element, TValue value);
        /// <summary>
        /// Unlike [<seealso cref="IOffensiveActionListener{T,TValue}.OnReceiveOffensiveAction"/>], this listener
        /// is just for damage (<seealso cref="EnumTeam.Role.Vanguard"/> tends to requires this instead of offensive)
        /// </summary>
        void OnDamageReceiveAction(T element, TValue value);

        /// <summary>
        /// Shields were higher thant zero but lost in the action
        /// </summary>
        void OnShieldLost(T element, TValue value);
        /// <summary>
        /// Health were higher that zero but lost in the action
        /// </summary>
        void OnHealthLost(T element, TValue value);

        /// <summary>
        /// Invoked once the character can't remain acting
        /// </summary>
        void OnMortalityDeath(T element, TValue value);
    }

    public interface ITempoListener<in T>
    {
        void OnInitiativeTrigger(T element);
        void OnDoMoreActions(T element);
        /// <summary>
        /// Invoke once at least the [<see cref="CombatingEntity"/>] has 1 action and can't no longer act (by spending all actions or waiting)
        /// </summary>
        /// <param name="element"></param>
        void OnFinishAllActions(T element);
        /// <summary>
        /// Unlike [<seealso cref="OnFinishAllActions"/>], this is invoked only when the character was forced to
        /// not acting (by stun, removing actions or some external force)
        /// </summary>
        /// <param name="element"></param>
        void OnSkipActions(T element);
    }

    public interface IRoundListener<in T>
    {
        void OnRoundFinish(T lastElement);
    }
}
