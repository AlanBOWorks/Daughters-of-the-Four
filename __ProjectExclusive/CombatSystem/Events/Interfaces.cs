using CombatEffects;
using CombatEntity;
using CombatSkills;
using CombatTeam;
using UnityEngine;

namespace CombatSystem.Events
{
    public interface ICharactersEvents<in THolder, in TActor,in TActionReceiver, TEffect> :
        IOffensiveActionReceiverListener<THolder, TActionReceiver, TEffect>,
        ISupportActionReceiverListener<THolder, TActionReceiver, TEffect>,

        IVitalityLostListener<THolder, TActionReceiver>,
        IDamageReceiverListener<THolder,TActionReceiver>,

        ITempoListener<TActor>,
        ITempoAlternateListener<TActor>,
        IRoundListener<TActor>
    { }
    public interface ICharactersEvents : 
        ICharactersEvents<ISkillParameters, CombatingEntity, CombatingEntity, SkillComponentResolution>
    { }
    public interface ICombatSystemEvents : ICharactersEvents, ITeamStateChangeListener<CombatingTeam>, ISkillEventListener,
        IAnimationsListener<SkillValuesHolders>
    { }

    public interface IOffensiveActionReceiverListener<in THolder,in TReceiver, TEffect>
    {
        void OnReceiveOffensiveAction(THolder holder, TReceiver receiver);
        //ref because by design some listener could change the receiver (this was made so passive/reaction could be possible,
        // passives such damage reduction, counter attacks, etc)
        void OnReceiveOffensiveEffect(TReceiver receiver, ref TEffect value); 
    }
    public interface IOffensiveActionReceiverListener<in T>
    {
        void OnReceiveOffensiveAction(T element);
    }

    public interface ISupportActionReceiverListener<in THolder,in TReceiver, TEffect>
    {
        void OnReceiveSupportAction(THolder holder, TReceiver receiver);
        void OnReceiveSupportEffect(TReceiver receiver, ref TEffect value);
    }
    public interface ISupportActionReceiverListener<in T>
    {
        void OnReceiveSupportAction(T element);
    }
   

    /// <summary>
    /// Used when a [<see cref="CombatingEntity"/>] receive a change on vitality (damage, recovery, health at zero)
    /// </summary>
    public interface IVitalityLostListener<in T,in TReceiver>
    {
        /// <summary>
        /// Shields were higher thant zero but lost in the action
        /// </summary>
        void OnShieldLost(T element, TReceiver receiver);
        /// <summary>
        /// Health were higher that zero but lost in the action
        /// </summary>
        void OnHealthLost(T element, TReceiver receiver);
        /// <summary>
        /// This is summon when the mortality reach zero; this normally happens once and then
        /// entities could be revived through HP, meaning that this events will not be invoked
        /// yet [<seealso cref="ITeamStateChangeListener.OnMemberDeath"/>] can still be invoked
        /// as consequence
        /// </summary>
        void OnMortalityLost(T element, TReceiver receiver);
    }
    public interface IDamageReceiverListener<in T, in TReceiver> 
    {
        void OnShieldDamage(T element, TReceiver receiver);
        void OnHealthDamage(T element, TReceiver receiver);
        void OnMortalityDamage(T element, TReceiver receiver);
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
