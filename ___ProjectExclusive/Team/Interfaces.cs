using Characters;
using UnityEngine;

namespace _Team
{
    public interface ITeamsData<out T>
    {
        T PlayerData { get; }
        T EnemyData { get; }
    }

    public interface ITeamDataFull<T> : ITeamsData<ICharacterArchetypes<T>>
    {
        T GenerateElement();
    }
    public interface IStanceAll<T> : IStance<T>, IStanceAllData<T>, IStanceAllInjector<T>
    {
        /// <summary>
        /// This elements is shared for all [<seealso cref="EnumTeam.Stances"/>]
        /// </summary>
        new T InAllStances { get; set; }
    }

    public interface IStanceAllData<out T> : IStanceData<T>
    {
        /// <summary>
        /// <inheritdoc cref="IStanceAll{T}.InAllStances"/>
        /// </summary>
        T InAllStances { get; }
    }
    public interface IStanceAllInjector<in T> : IStanceInjector<T>
    {
        /// <summary>
        /// <inheritdoc cref="IStanceAll{T}.InAllStances"/>
        /// </summary>
        T InAllStances { set; }
    }

    public interface IStance<T> : IStanceData<T>, IStanceInjector<T>
    {
        new T AttackingStance { get; set; }
        new T NeutralStance { get; set; }
        new T DefendingStance { get; set; }
    }

    public interface IStanceData<out T>
    {
        T AttackingStance { get; }
        T NeutralStance { get; }
        T DefendingStance { get; }
    }

    public interface IStanceInjector<in T>
    {
        T AttackingStance { set; }
        T NeutralStance { set; }
        T DefendingStance { set; }
    }

    public interface IStanceElement<out T>
    {
        T GetCurrentStanceValue();
    }

    public interface IStanceProvider
    {
        EnumTeam.Stances CurrentStance { get; }
    }
}
