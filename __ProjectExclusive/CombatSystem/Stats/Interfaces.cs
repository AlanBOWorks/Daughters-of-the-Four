

namespace Stats
{

    // This structures uses <T> because not only will be used for float/int (calculations), but also
    // for animations, sprites, strings, color, etc. (so everything has the same structure)

    public interface IMasterStatsRead<out T>
    {
        T Offensive { get; }
        T Support { get; }
        T Vitality { get; }
        T Concentration { get; }
    }
    public interface IMasterStatsInject<in T>
    {
        T Offensive { set; }
        T Support { set; }
        T Vitality { set; }
        T Concentration { set; }
    }

    public interface IMasterStats<T> : IMasterStatsInject<T>, IMasterStatsRead<T>
    {
        new T Offensive { get; set; }
        new T Support { get; set; }
        new T Vitality { get; set; }
        new T Concentration { get; set; }
    }


    public interface ICondensedOffensiveStat<TMaster,TElement> : IOffensiveStatsInject<TElement>, IOffensiveStatsRead<TElement>
    {
        TMaster Offensive { get; set; }
        new TElement Attack { get; set; }
        new TElement Persistent { get; set; }
        new TElement Debuff { get; set; }
        new TElement FollowUp { get; set; }
    }

    public interface ICondensedSupportStat<TMaster,TElement> : ISupportStatsRead<TElement>, ISupportStatsInject<TElement>
    {
        TMaster Support { get; set; }
        new TElement Heal { get; set; }
        new TElement Shielding { get; set; }
        new TElement Buff { get; set; }
        new TElement ReceiveBuff { get; set; }
    }


    public interface IOffensiveStatsRead<out T>
    {
        T Attack { get; }
        T Persistent { get; } //In game it's call static damage, but here is Persistent for naming convenience
        T Debuff { get; }
        T FollowUp { get; }
    }

    public interface ISupportStatsRead<out T>
    {
        T Heal { get; }
        T Shielding { get; }
        T Buff { get; }
        T ReceiveBuff { get; }
    }

    public interface IVitalityStatsRead<out T>
    {
        T MaxHealth { get; }
        T MaxMortality { get; }
        T DebuffResistance { get; }
        T DamageResistance { get; }
    }

    public interface IConcentrationStatsRead<out T>
    {
        T InitiativeSpeed { get; }
        T InitialInitiative { get; }
        //this could be a float since the sum of all stats could be something like (1.5f + 2.5f = 4 instead of 3)
        T ActionsPerSequence { get; }
        T Critical { get; }
    }


    /// <summary>
    /// Stats that only exist in combat and generally percentage related
    /// </summary>
    public interface ICombatPercentStatsRead<out T>
    {
        T CurrentShields { get; }
        T CurrentHealth { get; }
        T CurrentMortality { get; }
        T TickingInitiative { get; }
    }

    /// <summary>
    /// Stats that only exist in combat and generally unit related
    /// </summary>
    public interface ICombatUnitStatsRead<out T>
    {
        T CurrentActions { get; }
    }



    public interface IOffensiveStatsInject<in T>
    {
        T Attack { set; }
        T Persistent { set; } //In game it's call statsic damage, but here is Persistent for naming convenience
        T Debuff { set; }
        T FollowUp { set; }

    }

    public interface ISupportStatsInject<in T>
    {
        T Heal { set; }
        T Shielding { set; }
        T Buff { set; }
        T ReceiveBuff { set; }
    }

    public interface IVitalityStatsInject<in T>
    {
        T MaxHealth { set; }
        T MaxMortality { set; }
        T DebuffResistance { set; }
        T DamageResistance { set; }
    }

    public interface IConcentrationStatsInject<in T>
    {
        T InitiativeSpeed{ set; }
        T InitialInitiative { set; }
        //this could be a float since the sum of all stats could be something like (1.5f + 2.5f = 4 instead of 3)
        T ActionsPerSequence { set; }
        T Critical { set; }
    }


    /// <summary>
    /// <inheritdoc cref="ICombatPercentStatsRead{T}"/>
    /// </summary>
    public interface ICombatPercentStatsInject<in T>
    {
        T CurrentShields { set; }
        T CurrentHealth { set; }
        T CurrentMortality { set; }
        T TickingInitiative { set; }
    }

    public interface ICombatHealth<T> : ICombatHealthRead<T>, ICombatHealthInject<T>
    {
        new T MaxHealth { get; set; }
        new T MaxMortality { get; set; }

        new T CurrentHealth { get; set; }
        new T CurrentMortality { get; set; }

        new T CurrentShields { get; set; }
    }
    public interface ICombatHealthRead<out T>
    {
        T MaxHealth { get; }
        T MaxMortality { get; }

        T CurrentHealth { get; }
        T CurrentMortality { get; }

        T CurrentShields { get; }
    }
    public interface ICombatHealthInject<in T>
    {
        T MaxHealth { set; }
        T MaxMortality { set; }

        T CurrentHealth { set; }
        T CurrentMortality { set; }

        T CurrentShields { set; }
    }



    /// <summary>
    /// Stats that only exist in combat and generally unit related
    /// </summary>
    public interface ICombatUnitStatsInject<in T>
    {
        T CurrentActions { set; }
    }

    public interface IBaseStatsRead<out T> :
        IOffensiveStatsRead<T>, ISupportStatsRead<T>, IVitalityStatsRead<T>, IConcentrationStatsRead<T>
    { }

    public interface IBaseStatsInject<in T> :
        IOffensiveStatsInject<T>, ISupportStatsInject<T>, IVitalityStatsInject<T>, IConcentrationStatsInject<T>
    {}

    public interface IBaseStats<T> : IBaseStatsRead<T>, IBaseStatsInject<T>
    {
        new T Attack { get; set; }
        new T Persistent { get; set; }
        new T Debuff { get; set; }
        new T FollowUp { get; set; }

        new T Heal { get; set; }
        new T Buff { get; set; }
        new T ReceiveBuff { get; set; }
        new T Shielding { get; set; }


        new T MaxHealth { get; set; }
        new T MaxMortality { get; set; }
        new T DebuffResistance { get; set; }
        new T DamageResistance { get; set; }

        new T InitiativeSpeed { get; set; }
        new T Critical { get; set; }
        new T InitialInitiative { get; set; }
        new T ActionsPerSequence { get; set; }
    }
    public interface ICombatPercentStats<T> : ICombatPercentStatsRead<T>, ICombatPercentStatsInject<T>
    {
        new T CurrentShields { get; set; }
        new T CurrentHealth { get; set; }
        new T CurrentMortality { get; set; }
        new T TickingInitiative { get; set; }
    }

    public interface ICombatUnitStats<T> : ICombatUnitStatsRead<T>, ICombatUnitStatsInject<T>
    {
        new T CurrentActions { get; set; }
    }



    public interface IBehaviourStats<T> : IBehaviourStatsRead<T>, IBehaviourStatsInject<T>
    {
        new T BaseStats { get; set; }
        new T BuffStats { get; set; }
        new T BurstStats { get; set; }
    }
    public interface IBehaviourStatsRead<out T>
    {
        T BaseStats { get; }
        T BuffStats { get; }
        T BurstStats { get; }
    }

    public interface IBehaviourStatsInject<in T>
    {
        T BaseStats { set; }
        T BuffStats { set; }
        T BurstStats { set; }
    }
}
