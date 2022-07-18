using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    public class ConcatDualDictionary<TKey,TValue> : IReadOnlyDictionary<TKey,TValue>
    {
        [ShowInInspector]
        public IReadOnlyDictionary<TKey, TValue> FirstDictionary { get; protected set; }
        [ShowInInspector]
        public IReadOnlyDictionary<TKey, TValue> SecondDictionary { get; protected set; }

        public ConcatDualDictionary(
            IReadOnlyDictionary<TKey, TValue> firstDictionary, 
            IReadOnlyDictionary<TKey, TValue> secondDictionary)
        {
            FirstDictionary = firstDictionary;
            SecondDictionary = secondDictionary;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            var firstEnumerator = FirstDictionary.GetEnumerator();
            var secondEnumerator = SecondDictionary.GetEnumerator();

            while (firstEnumerator.MoveNext())
            {
                yield return firstEnumerator.Current;
            }

            while (secondEnumerator.MoveNext())
            {
                yield return secondEnumerator.Current;
            }

            firstEnumerator.Dispose();
            secondEnumerator.Dispose();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => FirstDictionary.Count + SecondDictionary.Count;
        public bool ContainsKey(TKey key)
        {
            return FirstDictionary.ContainsKey(key) || SecondDictionary.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
           bool gotValue = FirstDictionary.TryGetValue(key, out value);
           return gotValue || SecondDictionary.TryGetValue(key, out value);
        }

        public TValue this[TKey key] => FirstDictionary.ContainsKey(key) 
            ? FirstDictionary[key] 
            : SecondDictionary[key];

        public IEnumerable<TKey> Keys => FirstDictionary.Keys.Concat(SecondDictionary.Keys);
        public IEnumerable<TValue> Values => FirstDictionary.Values.Concat(SecondDictionary.Values);

        public static IReadOnlyDictionary<TKey,TValue> GetFirstOrNewConcatDictionary(
            IReadOnlyDictionary<TKey, TValue> first,
            IReadOnlyDictionary<TKey, TValue> second)
        {
            return (second == null || first == second) ? first : new ConcatDualDictionary<TKey, TValue>(first,second);
        }
    }
}
