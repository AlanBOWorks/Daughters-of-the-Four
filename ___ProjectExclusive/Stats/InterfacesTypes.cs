using UnityEngine;

namespace Stats
{
    public interface IStatDriven<T> : IStatDrivenData<T>, IStatDrivenInjection<T>
    {
    }

    public interface IStatDrivenFull<T> : IStatDriven<T>
    {
        T ElementHolder { get; }
    }

    public interface IStatDrivenData<out T>
    {

        T Health { get; }
        T Static { get; }
        T Buff { get; }
        T Harmony { get; }
        T Tempo { get; }
        T Control { get; }
        T Stance { get; }
        T Area { get; }
    }
    public interface IStatDrivenInjection<in T>
    {

        T Health { set; }
        T Static { set; }
        T Buff { set; }
        T Harmony { set; }
        T Tempo { set; }
        T Control { set; }
        T Stance { set; }
        T Area { set; }
    }

    /// <summary>
    /// Used normally with [<seealso cref="ITargetDriven{T}"/>], has an additional element for each type of target
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITargetDrivenExtension<T>
    {
        T SelfOnlyElement { get; set; }
        T OffensiveElement { get; set; }
        T SupportElement { get; set; }
    }

    public interface ITargetDriven<T> : ITargetDrivenData<T>, ITargetDrivenInjection<T>
    { }

    public interface ITargetDrivenData<out T>
    {
        T SelfOnly { get; }
        T Offensive { get; }
        T Support { get; }
    }
    public interface ITargetDrivenInjection<in T>
    {
        T SelfOnly { set; }
        T Offensive { set; }
        T Support { set; }
    }
}
