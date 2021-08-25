using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Passives
{
    public interface IPassiveBase
    {
        void InjectPassive(CombatingEntity entity);
    }

    public interface IPassiveHolder : IPassiveBase
    { }
}
