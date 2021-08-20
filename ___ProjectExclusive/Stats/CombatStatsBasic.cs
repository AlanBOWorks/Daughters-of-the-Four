using System;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Stats
{
    [Serializable]
    public class CharacterCombatStatsBasic : ICharacterBasicStats
    {
        [Title("Offensive")]
        [SerializeField, SuffixLabel("u|%%"),Tooltip("Base is Unit; Buff is percent")]
        private float attackPower = 10;
        [SerializeField, SuffixLabel("%00")]
        private float deBuffPower = 0.1f;
        
        [SerializeField, SuffixLabel("Units")]
        private float staticDamagePower = 10;


        [Title("Support")]
        [SerializeField, SuffixLabel("u|%%"),Tooltip("Base is Unit; Buff is percent")]
        private float healPower = 10;
        [SerializeField, SuffixLabel("%00")]
        private float buffPower = 1;

        [SerializeField, SuffixLabel("%00(Add)")]
        private float buffReceivePower = 0;

        [Title("Vitality")]
        [SerializeField, SuffixLabel("Units")]
        private float maxHealth = 100;
        [SerializeField, SuffixLabel("Units")]
        private float maxMortalityPoints = 1000;

        [SerializeField, SuffixLabel("u|%%"),Tooltip("Base is Unit; Buff is percent")]
        private float damageReduction = 0;
        [SerializeField, SuffixLabel("%00"), Tooltip("Counters DeBuffPower")]
        private float deBuffReduction = 0;


        [Title("Enlightenment")]
        [SerializeField, SuffixLabel("%00"), Tooltip("Affects Harmony gain")]
        private float enlightenment = 1; // before fight this could be modified
        [SerializeField, SuffixLabel("%00")]
        private float criticalChance = 0;
        [SerializeField, SuffixLabel("u|%%"), Tooltip("[100] is the default value")]
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

        public float StaticDamagePower
        {
            get => staticDamagePower;
            set => staticDamagePower = value;
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

        public float BuffReceivePower
        {
            get => buffReceivePower;
            set => buffReceivePower = value;
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

        public CharacterCombatStatsBasic(float value)
        {
            UtilsStats.OverrideStats(this, value);
        }

        [Button]
        public virtual void OverrideAll(float value) => UtilsStats.OverrideStats(this, value);
        public virtual void ResetToZero() => UtilsStats.OverrideStats(this);

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
