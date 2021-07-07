using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace _Player
{
    public interface IPlayerArchetypes<T> : ICharacterArchetypes<T>, 
        IPlayerArchetypesData<T>, IPlayerArchetypesInjection<T>
    { }
    public interface IPlayerArchetypesData<out T> : ICharacterArchetypesData<T>
    { }

    public interface IPlayerArchetypesInjection<in T> : ICharacterArchetypesInjection<T>
    { }

}
