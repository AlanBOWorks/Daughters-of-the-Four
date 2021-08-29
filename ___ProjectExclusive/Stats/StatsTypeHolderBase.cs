using System;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class StatsTypeHolderBase<T> : BuffTypeHolder<T>, IStatsHolder<T>
    {
        public StatsTypeHolderBase()
        {}

        public StatsTypeHolderBase(T baseType, T buffType, T burstType)
            : base(buffType, burstType)
        {
            this.baseType = baseType;
        }

        [SerializeField] protected T baseType;
        public T GetBase() => baseType;
    }

    [Serializable]
    public class BuffTypeHolder<T> : IBuffHolder<T>
    {
        public BuffTypeHolder()
        {}

        public BuffTypeHolder(T buffType, T burstType)
        {
            this.buffType = buffType;
            this.burstType = burstType;
        }

        [SerializeField] protected T buffType;
        [SerializeField] protected T burstType;

        public T GetBuff() => buffType;
        public T GetBurst() => burstType;
    }
}
