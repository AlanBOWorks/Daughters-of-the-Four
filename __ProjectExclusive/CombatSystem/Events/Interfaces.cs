using CombatEffects;
using CombatEntity;
using CombatSkills;
using CombatTeam;
using UnityEngine;

namespace CombatSystem.Events
{
    public interface ICharactersEvents<in THolder, in TTempo,in TSkill, TEffect> :
        IOffensiveActionReceiverListener<THolder, TSkill, TEffect>,
        ISupportActionReceiverListener<THolder, TSkill, TEffect>,
        IVitalityChangeListener<THolder, TSkill>,
        ITempoListener<TTempo>,
        ITempoAlternateListener<TTempo>,
        IRoundListener<TTempo>
    { }
    public interface ICharactersEvents : 
        ICharactersEvents<CombatEntityPairAction, CombatingEntity, CombatingSkill, SkillComponentResolution>
    { }
    public interface ICombatSystemEvents : ICharactersEvents, ITeamStateChangeListener<CombatingTeam>, ISkillEventListener,
        IAnimationsListener<SkillValuesHolders>
    { }

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


    public interface ISkillEventListener
    {
        /// <summary>
        /// After the skill is used (internally) but before to [<seealso cref="OnSkillCostIncreases"/>]
        /// </summary>
        /// <param name="values"></param>
        void OnSkillUse(SkillValuesHolders values);
        /// <summary>
        /// Events happens after the skill cost of the skill is increased
        /// </summary>
        /// <param name="values"></param>
        void OnSkillCostIncreases(SkillValuesHolders values);
    }

    public interface IAnimationsListener<in T>
    {
        void OnBeforeAnimation(T element);
        /// <summary>
        /// Event that is invoked by the animation itself (normally the 'hit' in an attacking animation)
        /// </summary>
        void OnAnimationClimax(T element);
        /// <summary>
        /// Once the animation can be considered 'finished' as it reaches its end or the time limit was reached
        /// (the animation could be still played until the next animation is requested) 
        /// </summary>
        void OnAnimationHaltFinish(T element);
    }
}
