using CombatTeam;
using UnityEngine;

namespace CombatSystem.Events
{
    public interface IOffensiveActionReceiverListener<in T,in TAction, TEffect>
    {
        void OnReceiveOffensiveAction(T element, TAction value);
        //ref because by design some listener could change the value (this was made so passive/reaction could be possible,
        // passives such damage reduction, counter attacks, etc)
        void OnReceiveOffensiveEffect(T element, ref TEffect value); 
    }
    public interface IOffensiveActionReceiverListener<in T>
    {
        void OnReceiveOffensiveAction(T element);
    }

    public interface ISupportActionReceiverListener<in T,in TAction, TEffect>
    {
        void OnReceiveSupportAction(T element, TAction value);
        void OnReceiveSupportEffect(T element, ref TEffect value);
    }
    public interface ISupportActionReceiverListener<in T>
    {
        void OnReceiveSupportAction(T element);
    }
   

    /// <summary>
    /// Used when a [<see cref="CombatingEntity"/>] receive a change on vitality (damage, recovery, health at zero)
    /// </summary>
    /// <typeparam name="T">The type which is passed to all listeners</typeparam>
    /// <typeparam name="TAction">The type of value passed (generally a [<seealso cref="float"/>])</typeparam>
    public interface IVitalityChangeListener<in T,in TAction>
    {
        /// <summary>
        /// Shields were higher thant zero but lost in the action
        /// </summary>
        void OnShieldLost(T element, TAction value);
        /// <summary>
        /// Health were higher that zero but lost in the action
        /// </summary>
        void OnHealthLost(T element, TAction value);
    }

    public interface ITempoListener<in T>
    {
        void OnFirstAction(T element);
        void OnFinishAction(T element);
        void OnFinishAllActions(T element);
    }

    public interface ITempoAlternateListener<in T>
    {
        void OnCantAct(T element);
    }


    public interface IRoundListener<in T>
    {
        void OnRoundFinish(T lastElement);
    }
}
