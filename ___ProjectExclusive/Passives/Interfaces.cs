using System.Collections.Generic;
using Characters;
using Stats;
using UnityEngine;

namespace Passives
{
    public interface IPassiveInjector
    {
        void InjectPassive(CombatingEntity entity);
    }

    public interface IPassiveHolder 
    { }

    public interface IHarmonyHolderBase : IBasicStatsData<float>
    {
        float HarmonyLossOnDeath { get; }
    }

}
