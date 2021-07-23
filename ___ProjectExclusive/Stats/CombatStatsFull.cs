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
            Add(serializeThis.HarmonyAmount);
            Add(serializeThis.InitiativePercentage);
            ActionsPerInitiative = serializeThis.ActionsPerInitiative;
        }

        public SerializedCombatStatsFull(SerializedCombatStatsFull serializeThis) : base(serializeThis)
        {
            ActionsPerInitiative = serializeThis.ActionsPerInitiative;
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
        public float HarmonyAmount
        {
            get => this[HarmonyAmountIndex];
            set => this[HarmonyAmountIndex] = value;
        }
        public float InitiativePercentage
        {
            get => this[InitiativePercentageIndex];
            set => this[InitiativePercentageIndex] = value;
        }
        public int ActionsPerInitiative { get; set; }

        public const int HealthPointsIndex = BasicStatsLength;
        public const int ShieldAmountIndex = HealthPointsIndex + 1;
        public const int MortalityPointsIndex = ShieldAmountIndex + 1;
        public const int HarmonyAmountIndex = MortalityPointsIndex + 1;
        public const int InitiativePercentageIndex = HarmonyAmountIndex + 1;
        public const int FullStatsLength = InitiativePercentageIndex + 1;

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
        [SerializeField, SuffixLabel("units")]
        private float harmonyAmount; // before fight this could be modified
        [SerializeField, SuffixLabel("%00"), Tooltip("Guaranteed amount of initiative on [Fight Starts]")]
        private float initiativePercentage;
        [SerializeField, SuffixLabel("units")]
        private int actionsPerInitiative = 0;


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

        public float HarmonyAmount
        {
            get => harmonyAmount;
            set => harmonyAmount = value;
        }

        public float InitiativePercentage
        {
            get => initiativePercentage;
            set => initiativePercentage = value;
        }

        public int ActionsPerInitiative
        {
            get => actionsPerInitiative;
            set => actionsPerInitiative = value;
        }


        public CharacterCombatStatsFull() : base()
        { }

        public CharacterCombatStatsFull(int overrideByDefault) : base(overrideByDefault)
        {
            float value = overrideByDefault;
            HealthPoints = value;
            ShieldAmount = value;
            MortalityPoints = value;
            HarmonyAmount = value;
            InitiativePercentage = value;
            ActionsPerInitiative = (int)value;
        }

        public CharacterCombatStatsFull(ICharacterFullStats copyFrom)
        {
            UtilsStats.CopyStats(this, copyFrom);
        }

        private const float MaxInitialInitiative = .8f;
        private const float DefaultInitialInitiative = .6f;
        public void AddInitialInitiative(float randomMax = DefaultInitialInitiative)
        {
            float initiativeAddition = Random.value * randomMax;
            InitiativePercentage += initiativeAddition;
            if (InitiativePercentage > MaxInitialInitiative)
                InitiativePercentage = MaxInitialInitiative;
        }


    }
}
