using System;
using System.Collections.Generic;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Stats
{

    public class SerializedCombatStatsBasic : List<float>, ICharacterBasicStats
    {
        public SerializedCombatStatsBasic(int memoryAllocation) : base(memoryAllocation)
        { }

        public SerializedCombatStatsBasic(ICharacterBasicStats serializeThis) : base(BasicStatsLength)
        {
            InjectValues(serializeThis);
        }

        public SerializedCombatStatsBasic(SerializedCombatStatsBasic serializeThis) : base(serializeThis)
        {
        }

        protected void InjectValues(ICharacterBasicStats serializeThis)
        {
            Add(serializeThis.AttackPower);
            Add(serializeThis.DeBuffPower);
            Add(serializeThis.HealPower);
            Add(serializeThis.BuffPower);
            Add(serializeThis.MaxHealth);
            Add(serializeThis.MaxMortalityPoints);
            Add(serializeThis.DamageReduction);
            Add(serializeThis.DeBuffReduction);
            Add(serializeThis.Enlightenment);
            Add(serializeThis.CriticalChance);
            Add(serializeThis.SpeedAmount);
            Add(serializeThis.HarmonyAmount);
            Add(serializeThis.InitiativePercentage);
            ActionsPerInitiative = serializeThis.ActionsPerInitiative;
        }
        public int ActionsPerInitiative { get; set; }


        public float AttackPower
        {
            get => this[AttackPowerIndex];
            set => this[AttackPowerIndex] = value;
        }
        public float DeBuffPower
        {
            get => this[DeBuffPowerIndex];
            set => this[DeBuffPowerIndex] = value;
        }
        public float HealPower
        {
            get => this[HealPowerIndex];
            set => this[HealPowerIndex] = value;
        }
        public float BuffPower
        {
            get => this[BuffPowerIndex];
            set => this[BuffPowerIndex] = value;
        }
        public float MaxHealth
        {
            get => this[MaxHealthIndex];
            set => this[MaxHealthIndex] = value;
        }
        public float MaxMortalityPoints
        {
            get => this[MaxMortalityPointsIndex];
            set => this[MaxMortalityPointsIndex] = value;
        }
        public float DamageReduction
        {
            get => this[DamageReductionIndex];
            set => this[DamageReductionIndex] = value;
        }
        public float DeBuffReduction
        {
            get => this[DeBuffReductionIndex];
            set => this[DeBuffReductionIndex] = value;
        }
        public float Enlightenment
        {
            get => this[EnlightenmentIndex];
            set => this[EnlightenmentIndex] = value;
        }
        public float CriticalChance
        {
            get => this[CriticalChanceIndex];
            set => this[CriticalChanceIndex] = value;
        }
        public float SpeedAmount
        {
            get => this[SpeedAmountIndex];
            set => this[SpeedAmountIndex] = value;
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


        public const int AttackPowerIndex = 0;
        public const int DeBuffPowerIndex = AttackPowerIndex + 1;
        public const int HealPowerIndex = DeBuffPowerIndex + 1;
        public const int BuffPowerIndex = HealPowerIndex + 1;
        public const int MaxHealthIndex = BuffPowerIndex + 1;
        public const int MaxMortalityPointsIndex = MaxHealthIndex + 1;
        public const int DamageReductionIndex = MaxMortalityPointsIndex + 1;
        public const int DeBuffReductionIndex = DamageReductionIndex + 1;
        public const int EnlightenmentIndex = DeBuffReductionIndex + 1;
        public const int CriticalChanceIndex = EnlightenmentIndex + 1;
        public const int SpeedAmountIndex = CriticalChanceIndex + 1;
        public const int HarmonyAmountIndex = SpeedAmountIndex + 1;
        public const int InitiativePercentageIndex = HarmonyAmountIndex + 1;
        public const int BasicStatsLength = InitiativePercentageIndex + 1;

        public virtual void ResetToZero()
        {
            for (int i = 0; i < Count; i++)
            {
                this[i] = 0;
            }
        }
    }

    [Serializable]
    public class CharacterCombatStatsBasic : ICharacterBasicStats
    {
        [Title("Offensive")]
        [SerializeField, SuffixLabel("Units")]
        private float attackPower = 10;
        [SerializeField, SuffixLabel("%00")]
        private float deBuffPower = 0.1f;
        [Title("Support")]
        [SerializeField, SuffixLabel("Units")]
        private float healPower = 10;
        [SerializeField, SuffixLabel("%00")]
        private float buffPower = 1;
        [Title("Vitality")]
        [SerializeField, SuffixLabel("Units")]
        private float maxHealth = 100;
        [SerializeField, SuffixLabel("Units")]
        private float maxMortalityPoints = 1000;

        [SerializeField, SuffixLabel("%00")]
        private float damageReduction = 0;
        [SerializeField, SuffixLabel("%00"), Tooltip("Counters DeBuffPower")]
        private float deBuffReduction = 0;


        [Title("Enlightenment")]
        [SerializeField, SuffixLabel("%00"), Tooltip("Affects Harmony gain")]
        private float enlightenment = 1; // before fight this could be modified
        [SerializeField, SuffixLabel("%00")]
        private float criticalChance = 0;
        [SerializeField, SuffixLabel("Units"), Tooltip("[100] is the default value")]
        private float speedAmount = 100;

        [SerializeField, SuffixLabel("%00")]
        private float initiativePercentage;
        [SerializeField, SuffixLabel("Units")]
        private int actionsPerInitiative;
        [SerializeField, SuffixLabel("%00")]
        private float harmonyAmount;

        public float AttackPower
        {
            get => attackPower;
            set => attackPower = value;
        }

        public float DeBuffPower
        {
            get => deBuffPower;
            set => deBuffPower = value;
        }

        public float HealPower
        {
            get => healPower;
            set => healPower = value;
        }

        public float BuffPower
        {
            get => buffPower;
            set => buffPower = value;
        }

        public float MaxHealth
        {
            get => maxHealth;
            set => maxHealth = value;
        }

        public float MaxMortalityPoints
        {
            get => maxMortalityPoints;
            set => maxMortalityPoints = value;
        }

        public float DamageReduction
        {
            get => damageReduction;
            set => damageReduction = value;
        }

        public float DeBuffReduction
        {
            get => deBuffReduction;
            set => deBuffReduction = value;
        }

        public float Enlightenment
        {
            get => enlightenment;
            set => enlightenment = value;
        }

        public float CriticalChance
        {
            get => criticalChance;
            set => criticalChance = value;
        }

        public float SpeedAmount
        {
            get => speedAmount;
            set => speedAmount = value;
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

        public float HarmonyAmount
        {
            get => harmonyAmount;
            set => harmonyAmount = value;
        }

        public CharacterCombatStatsBasic()
        { }

        public CharacterCombatStatsBasic(int overrideByDefault)
        {
            float value = overrideByDefault;
            AttackPower = value;
            DeBuffPower = value;

            HealPower = value;
            BuffPower = value;

            MaxHealth = value;
            MaxMortalityPoints = value;
            DamageReduction = value;

            Enlightenment = value;
            CriticalChance = value;
            SpeedAmount = value;

            HarmonyAmount = value;
            InitiativePercentage = value;
            ActionsPerInitiative = overrideByDefault;
        }

        public virtual void OverrideAll(float value)
        {
            AttackPower = value;
            DeBuffPower = value;

            HealPower = value;
            BuffPower = value;

            MaxHealth = value;
            MaxMortalityPoints = value;
            DamageReduction = value;

            Enlightenment = value;
            CriticalChance = value;
            SpeedAmount = value;

            HarmonyAmount = value;
            InitiativePercentage = value;
            ActionsPerInitiative = (int)value;
        }

        public CharacterCombatStatsBasic(ICharacterBasicStats copyFrom)
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
