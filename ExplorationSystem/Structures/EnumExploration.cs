using System.Collections.Generic;
using UnityEngine;

namespace ExplorationSystem
{
    public static class EnumExploration
    {

        public enum ExplorationTier
        {
            Undefined = 0,
            F,
            E,
            D,
            C,
            B,
            A,
            S
        }

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
