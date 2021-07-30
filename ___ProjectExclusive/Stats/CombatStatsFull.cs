using System;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

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
        [SerializeField, SuffixLabel("units")]
        private float healthPoints; // before fight this could be reduced
        [SerializeField, SuffixLabel("units")]
        private float shieldAmount; // before fight this could be increased
        [SerializeField, SuffixLabel("units")]
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

        public CharacterCombatStatsFull(int overrideByDefault) : base(overrideByDefault)
        {
            float value = overrideByDefault;
            HealthPoints = value;
            ShieldAmount = value;
            MortalityPoints = value;
            
        }

        public override void OverrideAll(float value)
        {
            base.OverrideAll(value);
            HealthPoints = value;
            ShieldAmount = value;
            MortalityPoints = value;
        }

        public CharacterCombatStatsFull(ICharacterFullStats copyFrom)
        {
            UtilsStats.CopyStats(this, copyFrom);
        }

        


    }
}
