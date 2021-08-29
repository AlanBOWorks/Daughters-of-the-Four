using System;
using Sirenix.OdinInspector;
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

        [SerializeField, ShowInInspector,PropertyOrder(-10)] protected T baseType;
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

        [SerializeField, ShowInInspector] protected T buffType;
        [SerializeField, ShowInInspector] protected T burstType;

        public T GetBuff() => buffType;
        public T GetBurst() => burstType;
    }
}
