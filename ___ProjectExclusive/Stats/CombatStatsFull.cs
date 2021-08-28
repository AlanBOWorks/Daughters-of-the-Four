using System;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class CombatStatsFull : FullStats<float>
    {
        public CombatStatsFull() : base()
        { }

        public CombatStatsFull(float value)
        {
            UtilsStats.OverrideStats(this, value);
        }
        public CombatStatsFull(IFullStatsData<float> copyFrom)
        {
            UtilsStats.CopyStats(this, copyFrom);
        }

        public void ResetToZero() => UtilsStats.OverrideStats(this, 0);
    }

    [Serializable]
    public class FullStats<T> : BasicStats<T>, IFullStats<T>
    {
        [Title("Temporal Stats")]
        [SerializeField, SuffixLabel("u|%%"), Tooltip("Base is Unit; Buff is percent")]
        private T healthPoints; // before fight this could be reduced
        [SerializeField, SuffixLabel("u|%%"), Tooltip("Base is Unit; Buff is percent")]
        private T shieldAmount; // before fight this could be increased
        [SerializeField, SuffixLabel("u|%%"), Tooltip("Base is Unit; Buff is percent")]
        private T mortalityPoints; // after fight this could be reduced
        [SerializeField, SuffixLabel("u|%%"), Tooltip("Base is Unit; Buff is percent")]
        private T accumulatedStatic;

        public T HealthPoints
        {
            get => healthPoints;
            set => healthPoints = value;
        }

        public T ShieldAmount
        {
            get => shieldAmount;
            set => shieldAmount = value;
        }

        public T MortalityPoints
        {
            get => mortalityPoints;
            set => mortalityPoints = value;
        }

        public T AccumulatedStatic
        {
            get => accumulatedStatic;
            set => accumulatedStatic = value;
        }
    }

    public class TrackingStats : CombatStatsFull, ITrackingStats
    {
        public TrackingStats() : base()
        { }
        public TrackingStats(IFullStatsData<float> serializeThis) : base(serializeThis)
        { }
        public float DamageReceived { get; set; }
        public float HealReceived { get; set; }
    }

    public interface ITrackingStats 
    {
        float DamageReceived { set; get; }
        float HealReceived { get; set; }
    }
}
