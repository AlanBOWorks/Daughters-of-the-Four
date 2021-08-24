using System;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class CombatStatsFull : CombatStatsBasic, IFullStats
    {

        [Title("Temporal Stats")]
        [SerializeField, SuffixLabel("u|%%"),Tooltip("Base is Unit; Buff is percent")]
        private float healthPoints; // before fight this could be reduced
        [SerializeField, SuffixLabel("u|%%"),Tooltip("Base is Unit; Buff is percent")]
        private float shieldAmount; // before fight this could be increased
        [SerializeField, SuffixLabel("u|%%"),Tooltip("Base is Unit; Buff is percent")]
        private float mortalityPoints; // after fight this could be reduced
        [SerializeField, SuffixLabel("u|%%"), Tooltip("Base is Unit; Buff is percent")]
        private float accumulatedStatic;

        public float HealthPoints
        {
            get => healthPoints;
            set => healthPoints = value;
        }

        public float ShieldAmount
        {
            get => shieldAmount;
            set => shieldAmount = value;
        }

        public float MortalityPoints
        {
            get => mortalityPoints;
            set => mortalityPoints = value;
        }

        public float AccumulatedStatic
        {
            get => accumulatedStatic;
            set => accumulatedStatic = value;
        }


        public CombatStatsFull() : base()
        { }

        public CombatStatsFull(float value) : base(value)
        {
            HealthPoints = value;
            ShieldAmount = value;
            MortalityPoints = value;
        }
        public CombatStatsFull(IFullStatsData copyFrom)
        {
            UtilsStats.CopyStats(this, copyFrom);
        }

        public override void ResetToZero() => UtilsStats.OverrideStats(this as IFullStatsData, 0);

    }

    public class TrackingStats : CombatStatsFull, ITrackingStats
    {
        public TrackingStats() : base()
        { }
        public TrackingStats(IFullStatsData serializeThis) : base(serializeThis)
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
