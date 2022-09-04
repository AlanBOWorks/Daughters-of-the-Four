using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace ExplorationSystem
{

    public interface IExplorationEntitiesSpawnRateHolder
    {
        int CalculateBasicWeakEntitiesAdditionAmount { get; }
        int CalculateEliteEntitiesAdditionAmount { get; }
    }
    [Serializable]
    public sealed class RandomExplorationEntitiesSpawnRates : IExplorationEntitiesSpawnRateHolder
    {
        [SerializeField] private SRange basicWeakEntitiesAdditionChance = new SRange(0, 1);
        [SerializeField] private SRange eliteEntitiesAdditionChance = new SRange(0, 1);

        public int CalculateBasicWeakEntitiesAdditionAmount => (int) basicWeakEntitiesAdditionChance.CalculateRandom();
        public int CalculateEliteEntitiesAdditionAmount => (int) eliteEntitiesAdditionChance.CalculateRandom();
    }

    [Serializable]
    public sealed class MediaRandomExplorationEntitiesSpawnRates : IExplorationEntitiesSpawnRateHolder
    {
        [Title("Basic Rates")] 
        [SerializeField, HorizontalGroup("Basic")]
        private SRange firstWeakEntitiesChanceRange = new SRange(0,1);
        [SerializeField, HorizontalGroup("Basic")]
        private SRange secondWeakEntitiesChanceRange = new SRange(0,2);

        [Title("Elite Rates")]
        [SerializeField, HorizontalGroup("Elite")]
        private SRange firstEliteEntitiesChanceRange = new SRange(0,1);
        [SerializeField, HorizontalGroup("Elite")]
        private SRange secondaryEliteEntitiesChanceRange = new SRange(0,2);

        public int CalculateBasicWeakEntitiesAdditionAmount
            => CalculateAmount(firstWeakEntitiesChanceRange, secondWeakEntitiesChanceRange);

        public int CalculateEliteEntitiesAdditionAmount
            => CalculateAmount(firstEliteEntitiesChanceRange, secondaryEliteEntitiesChanceRange);

        private static int CalculateAmount(SRange first, SRange second)
        {
            var sum = first.CalculateRandom() + second.CalculateRandom();
            var media = sum * .5f;
            return (int) media;
        }
    }
}
