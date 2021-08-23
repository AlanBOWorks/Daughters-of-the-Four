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
        [SerializeField, SuffixLabel("u|%%"), Tooltip("Base is Unit; Buff is percent")]
        private float attackPower;
        [SerializeField, SuffixLabel("%00")]
        private float deBuffPower;

        [SerializeField, SuffixLabel("Units")]
        private float staticDamagePower;


        [Title("Support")]
        [SerializeField, SuffixLabel("u|%%"), Tooltip("Base is Unit; Buff is percent")]
        private float healPower;
        [SerializeField, SuffixLabel("%00")]
        private float buffPower;

        [SerializeField, SuffixLabel("%00(Add)")]
        private float buffReceivePower;

        [Title("Vitality")]
        [SerializeField, SuffixLabel("Units")]
        private float maxHealth;
        [SerializeField, SuffixLabel("Units")]
        private float maxMortalityPoints;

        [SerializeField, SuffixLabel("u|%%"), Tooltip("Base is Unit; Buff is percent")]
        private float damageReduction;
        [SerializeField, SuffixLabel("%00"), Tooltip("Counters DeBuffPower")]
        private float deBuffReduction;


        [Title("Concentration")]
        [SerializeField, SuffixLabel("%00"), Tooltip("Affects Harmony gain")]
        private float enlightenment; // before fight this could be modified
        [SerializeField, SuffixLabel("%00")]
        private float criticalChance;
        [SerializeField, SuffixLabel("u|%%"), Tooltip("[100] is the default value")]
        private float speedAmount;

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

        public CharacterCombatStatsBasic(ICharacterBasicStatsData copyFrom)
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

        public void SetAttackPower(float value)
        {
            attackPower = value;
        }

        public void SetDeBuffPower(float value)
        {
            deBuffPower = value;
        }

        public void SetStaticDamagePower(float value)
        {
            staticDamagePower = value;
        }

        public void SetHealPower(float value)
        {
            healPower = value;
        }

        public void SetBuffPower(float value)
        {
            buffPower = value;
        }

        public void SetBuffReceivePower(float value)
        {
            buffReceivePower = value;
        }

        public void SetMaxHealth(float value)
        {
            MaxHealth = value;
        }

        public void SetMaxMortalityPoints(float value)
        {
            MaxMortalityPoints = value;
        }

        public void SetDamageReduction(float value)
        {
            damageReduction = value;
        }

        public void SetDeBuffReduction(float value)
        {
            deBuffReduction = value;
        }

        public void SetEnlightenment(float value)
        {
            enlightenment = value;
        }

        public void SetCriticalChance(float value)
        {
            criticalChance = value;
        }

        public void SetSpeedAmount(float value)
        {
            speedAmount = value;
        }

        public void SetInitiativePercentage(float value)
        {
            initiativePercentage = value;
        }

        public void SetActionsPerInitiative(int value)
        {
            actionsPerInitiative = value;
        }

        public void SetHarmonyAmount(float value)
        {
            harmonyAmount = value;
        }
        public float GetAttackPower() => AttackPower;

        public float GetDeBuffPower() => DeBuffPower;

        public float GetStaticDamagePower() => StaticDamagePower;

        public float GetHealPower() => HealPower;

        public float GetBuffPower() => BuffPower;

        public float GetBuffReceivePower() => BuffReceivePower;

        public float GetMaxHealth() => MaxHealth;

        public float GetMaxMortalityPoints() => MaxMortalityPoints;

        public float GetDamageReduction() => DamageReduction;

        public float GetDeBuffReduction() => DeBuffReduction;

        public float GetEnlightenment() => Enlightenment;

        public float GetCriticalChance() => CriticalChance;

        public float GetSpeedAmount() => SpeedAmount;

        public float GetInitiativePercentage() => InitiativePercentage;

        public int GetActionsPerInitiative() => ActionsPerInitiative;

        public float GetHarmonyAmount() => HarmonyAmount;
    }

}
