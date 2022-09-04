using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

namespace Utils.Maths
{
    public static class UtilsRandom
    {
        public static readonly INonConsecutiveRandomNumbersGenerator EightSizeNonConsecutiveRandomNumbersGenerator =
            new DualNonConsecutiveRandomsGenerators(8);
        public static readonly INonConsecutiveRandomNumbersGenerator FourSizeNonConsecutiveRandomNumbersGenerator =
            new DualNonConsecutiveRandomsGenerators(4);
    }

    public interface INonConsecutiveRandomNumbersGenerator
    {
        IReadOnlyList<int> CalculateNonConsecutiveRandoms(int lengthUpToSize);
        IReadOnlyList<int> CalculateNonConsecutiveRandoms();
    }

    internal sealed class DualNonConsecutiveRandomsGenerators : INonConsecutiveRandomNumbersGenerator
    {
        private readonly FreeSizeNonConsecutiveRandomNumbersGenerator _freeGenerator;
        private readonly FixedSizeNonConsecutiveRandomNumbersGenerator _fixedGenerator;

        public DualNonConsecutiveRandomsGenerators(int length)
        {
            _freeGenerator = new FreeSizeNonConsecutiveRandomNumbersGenerator(length);
            _fixedGenerator = new FixedSizeNonConsecutiveRandomNumbersGenerator(length);
        }

        public IReadOnlyList<int> CalculateNonConsecutiveRandoms(int lengthUpToSize)
        {
            return _freeGenerator.GetNonConsecutiveNumbers(lengthUpToSize);
        }

        public IReadOnlyList<int> CalculateNonConsecutiveRandoms()
        {
            return _fixedGenerator.GetNonConsecutiveNumbers();
        }
    }

    internal sealed class FreeSizeNonConsecutiveRandomNumbersGenerator
    {
        private readonly List<int> _numbersHolder;

        public FreeSizeNonConsecutiveRandomNumbersGenerator(int collectionLength)
        {
            _numbersHolder = new List<int>(collectionLength);
        }


        public IReadOnlyList<int> GetNonConsecutiveNumbers(int lengthUpToCollectionSize)
        {
            int capacity = _numbersHolder.Capacity;
            if (lengthUpToCollectionSize > capacity) lengthUpToCollectionSize = capacity;

            GenerateNonConsecutiveValues(lengthUpToCollectionSize);
            return _numbersHolder;
        }

        private void GenerateNonConsecutiveValues(int length)
        {
            //Fisher-Yates Shuffle Algorithm - Variation (List)
            _numbersHolder.Clear();
            int i = 0;
            for(; i < length; i++)
            {
                _numbersHolder.Add(i);
            }

            
            for(--i;i > 1;i--)
            {
                int randomValue = Random.Range(0, i);
                var swapValue = _numbersHolder[randomValue];
                _numbersHolder[randomValue] = _numbersHolder[i];
                _numbersHolder[i] = swapValue;
            }
        }
    }

    internal sealed class FixedSizeNonConsecutiveRandomNumbersGenerator
    {
        private readonly int[] _numbersHolder;

        public FixedSizeNonConsecutiveRandomNumbersGenerator(int arrayLength)
        {
            _numbersHolder = new int[arrayLength];
            for (int i = 0; i < arrayLength; i++)
            {
                _numbersHolder[i] = i;
            }
        }

        public IReadOnlyList<int> GetNonConsecutiveNumbers()
        {
            Shuffle();
            return _numbersHolder;
        }

        private void Shuffle()
        {
            int length = _numbersHolder.Length;
            for (int i = 0; i < length; i++)
            {
                int randomValue = Random.Range(i, length);
                var swapValue = _numbersHolder[i];
                _numbersHolder[i] = _numbersHolder[randomValue];
                _numbersHolder[randomValue] = swapValue;
            }
        }
    }
}
