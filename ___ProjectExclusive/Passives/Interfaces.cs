using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Passives
{
    public interface IPassiveInjector
    {
        void InjectPassive(CombatingEntity entity);
    }

    public interface IPassiveHolder 
    { }
}
