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
        public static IEnumerable<KeyValuePair<TKey,TValue>> GetEnumerable<TKey,TValue>
            (IExplorationTypesStructureRead<TKey> keys, IExplorationTypesStructureRead<TValue> values)
        {
            yield return new KeyValuePair<TKey, TValue>(keys.BasicThreatType, values.BasicThreatType);
            yield return new KeyValuePair<TKey, TValue>(keys.EliteThreatType, values.EliteThreatType);
            yield return new KeyValuePair<TKey, TValue>(keys.BossThreatType, values.BossThreatType);

            yield return new KeyValuePair<TKey, TValue>(keys.CombinationType, values.CombinationType);
            yield return new KeyValuePair<TKey, TValue>(keys.AwakeningType, values.AwakeningType);

            yield return new KeyValuePair<TKey, TValue>(keys.TreasureType, values.TreasureType);
            yield return new KeyValuePair<TKey, TValue>(keys.ShopType, values.ShopType);
        }

    }

    public static class EnumExploration
    {
        public const int BasicThreatIndex = 4;
        public const int EliteThreatIndex = BasicThreatIndex + 1;
        public const int BossThreatIndex = EliteThreatIndex + 1;

        public const int CombinationIndex = 10;
        public const int AwakeningIndex = CombinationIndex + 1;

        public const int TreasureIndex = 20;
        public const int ShopIndex = TreasureIndex + 1;

        public enum ExplorationType
        {
            Undefined,
            BasicThreat = BasicThreatIndex,
            EliteThreat = EliteThreatIndex,
            BossThreat = BossThreatIndex,

            Combination = CombinationIndex,
            Awakening = AwakeningIndex,

            Treasure = TreasureIndex,
            Shop = ShopIndex
        }

        public static readonly IExplorationTypesStructureRead<ExplorationType> StaticEnums 
            = new StaticExplorationEnumHolder();

        private sealed class StaticExplorationEnumHolder : IExplorationTypesStructureRead<ExplorationType>
        {
            public ExplorationType BasicThreatType => ExplorationType.BasicThreat;
            public ExplorationType EliteThreatType => ExplorationType.EliteThreat;
            public ExplorationType BossThreatType => ExplorationType.BossThreat;
            public ExplorationType CombinationType => ExplorationType.Combination;
            public ExplorationType AwakeningType => ExplorationType.Awakening;
            public ExplorationType TreasureType => ExplorationType.Treasure;
            public ExplorationType ShopType => ExplorationType.Shop;
        }
    }
}
