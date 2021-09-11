using Stats;
using UnityEngine;

namespace Characters
{
    public interface ICombatingEntityInjector
    {
        void Injection(CombatingEntity entity);
    }

    public interface ICharacterLore
    {
        string CharacterName { get; }
    }

    public interface ICharacterRanges<T> : ICharacterRangesData<T>, ICharacterRangesInjection<T>
    {
        new T MeleeRange { get; set; }
        new T RangedRange { get; set; }
    }

    public interface ICharacterRangesData<out T>
    {
        T MeleeRange { get; }
        T RangedRange { get; }
    }

    public interface ICharacterRangesInjection<in T>
    {
        T MeleeRange { set; }
        T RangedRange { set; }
    }
}
