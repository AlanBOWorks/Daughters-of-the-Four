using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Utils.Collections
{
    public class ConcatDualList<T> : IReadOnlyList<T>
    {
        public ConcatDualList()
        {
            _firstList = new List<T>();
            _secondList = new List<T>();
        }

        public ConcatDualList(int capacity)
        {
            _firstList = new List<T>(capacity);
            _secondList = new List<T>(capacity);
        }

        [ShowInInspector,HorizontalGroup()]
        private readonly List<T> _firstList;
        [ShowInInspector,HorizontalGroup()]
        private readonly List<T> _secondList;

        public IList<T> GetFirstList() => _firstList;
        public IList<T> GetSecondList() => _secondList;
        public IEnumerable<T> GetFirstEnumerable() => _firstList;
        public IEnumerable<T> GetSecondEnumerable() => _secondList;

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var entity in _firstList)
            {
                yield return entity;
            }

            foreach (var entity in _secondList)
            {
                yield return entity;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _firstList.Count + _secondList.Count;

        public T this[int index]
        {
            get
            {
                int secondaryCount = _firstList.Count;
                if (index > secondaryCount)
                {
                    return _secondList[index - secondaryCount];
                }

                return _firstList[index];
            }
        }
    }
}
