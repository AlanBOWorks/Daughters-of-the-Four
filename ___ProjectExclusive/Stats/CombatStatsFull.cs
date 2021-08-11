using System;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Stats
{
    public class SerializedCombatStatsFull : SerializedCombatStatsBasic, ICharacterFullStats
    {
        public SerializedCombatStatsFull(ICharacterFullStats serializeThis) : base(FullStatsLength)
        {
            InjectValues(serializeThis);
            Add(serializeThis.HealthPoints);
            Add(serializeThis.ShieldAmount);
            Add(serializeThis.MortalityPoints);
            
        }


        public float HealthPoints
        {
            get => this[HealthPointsIndex];
            set => this[HealthPointsIndex] = value;
        }
        public float ShieldAmount
        {
            get => this[ShieldAmountIndex];
            set => this[ShieldAmountIndex] = value;
        }
        public float MortalityPoints
        {
            get => this[MortalityPointsIndex];
            set => this[MortalityPointsIndex] = value;
        }
        

        public const int HealthPointsIndex = BasicStatsLength;
        public const int ShieldAmountIndex = HealthPointsIndex + 1;
        public const int MortalityPointsIndex = ShieldAmountIndex + 1;
        public const int FullStatsLength = MortalityPointsIndex + 1;

        public override void ResetToZero()
        {
            base.ResetToZero();
            ActionsPerInitiative = 0;
        }
    }

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

        public new void OverrideAll(float value)
        {
            base.OverrideAll(value);
            HealthPoints = value;
            ShieldAmount = value;
            MortalityPoints = value;
        }

        public void ResetToZero() => OverrideAll(0);

        public CharacterCombatStatsFull(ICharacterFullStats copyFrom)
        {
            UtilsStats.CopyStats(this, copyFrom);
        }
    }

    public class SerializedTrackedStats : SerializedCombatStatsFull, ITrackingStats
    {
        public SerializedTrackedStats(ICharacterFullStats serializeThis) : base(serializeThis)
        {
            int countLoop = TrackedStatsLength - this.Count;
            for (int i = 0; i < countLoop; i++)
            {
                Add(0);
            }
        }
        public float DamageReceived
        {
            get => this[DamageReceivedIndex];
            set => this[DamageReceivedIndex] = value;
        }
        public float HealReceived
        {
            get => this[HealReceivedIndex];
            set => this[HealReceivedIndex] = value;
        }

        public const int DamageReceivedIndex = FullStatsLength;
        public const int HealReceivedIndex = DamageReceivedIndex + 1;
        public const int TrackedStatsLength = HealReceivedIndex + 1;
    }

    public interface ITrackingStats
    {
        float DamageReceived { set; get; }
        float HealReceived { get; set; }
    }
}
