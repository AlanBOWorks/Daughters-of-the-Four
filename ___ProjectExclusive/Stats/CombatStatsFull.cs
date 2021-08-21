using System;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class CharacterCombatStatsFull : CharacterCombatStatsBasic, ICharacterFullStats
    {

        [Title("Temporal Stats")]
        [SerializeField, SuffixLabel("u|%%"),Tooltip("Base is Unit; Buff is percent")]
        private float healthPoints; // before fight this could be reduced
        [SerializeField, SuffixLabel("u|%%"),Tooltip("Base is Unit; Buff is percent")]
        private float shieldAmount; // before fight this could be increased
        [SerializeField, SuffixLabel("u|%%"),Tooltip("Base is Unit; Buff is percent")]
        private float mortalityPoints; // after fight this could be reduced


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


        public CharacterCombatStatsFull() : base()
        { }

        public CharacterCombatStatsFull(float value) : base(value)
        {
            HealthPoints = value;
            ShieldAmount = value;
            MortalityPoints = value;
        }
        public CharacterCombatStatsFull(ICharacterFullStats copyFrom)
        {
            UtilsStats.CopyStats(this, copyFrom);
        }

        public override void ResetToZero() => UtilsStats.OverrideStats(this as ICharacterFullStats, 0);

    }

    public class SerializedTrackedStats : CharacterCombatStatsFull, ITrackingStats
    {
        public SerializedTrackedStats(ICharacterFullStats serializeThis) : base(serializeThis)
        {}
        public float DamageReceived { get; set; }
        public float HealReceived { get; set; }
    }

    public interface ITrackingStats
    {
        float DamageReceived { set; get; }
        float HealReceived { get; set; }
    }
}
