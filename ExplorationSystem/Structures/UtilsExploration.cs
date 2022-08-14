using System.Collections.Generic;
using UnityEngine;

namespace ExplorationSystem
{
    public static class UtilsExploration 
    {
        public static IEnumerable<T> GetEnumerable<T>(IExplorationTypesStructureRead<T> structure)
        {
            yield return structure.BasicThreatType;
            yield return structure.EliteThreatType;
            yield return structure.BossThreatType;

            yield return structure.CombinationType;
            yield return structure.AwakeningType;

            yield return structure.TreasureType;
            yield return structure.ShopType;
        }
    }
}
