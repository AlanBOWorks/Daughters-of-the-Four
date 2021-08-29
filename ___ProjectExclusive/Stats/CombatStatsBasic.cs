using System;
using ___ProjectExclusive;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Stats
{
    [Serializable]
    public class CombatStatsBasic : BasicStats<float>
    {
        public CombatStatsBasic()
        { }

        public CombatStatsBasic(float value)
        {
            UtilsStats.OverrideStats(this, value);
        }

        [Button]
        public virtual void OverrideAll(float value) => UtilsStats.OverrideStats(this, value);
        public virtual void ResetToZero() => UtilsStats.OverrideStats(this);

        public CombatStatsBasic(IBasicStatsData<float> copyFrom)
        {
            UtilsStats.CopyStats(this, copyFrom);
        }
    }

    [Serializable]
    public abstract class BasicStats<T> : IBasicStats<T>
    {

        [Title("Offensive")]
        [SerializeField, SuffixLabel("u|%%"), Tooltip("Base is Unit; Buff is percent")]
        private T attackPower;
        [SerializeField, SuffixLabel("%00")]
        private T deBuffPower;

        [SerializeField, SuffixLabel("Units")]
        private T staticDamagePower;


        [Title("Support")]
        [SerializeField, SuffixLabel("u|%%"), Tooltip("Base is Unit; Buff is percent")]
        private T healPower;
        [SerializeField, SuffixLabel("%00")]
        private T buffPower;

        [SerializeField, SuffixLabel("%00(Add)")]
        private T buffReceivePower;

        [Title("Vitality")]
        [SerializeField, SuffixLabel("Units")]
        private T maxHealth;
        [SerializeField, SuffixLabel("Units")]
        private T maxMortalityPoints;

        [SerializeField, SuffixLabel("u|%%"), Tooltip("Base is Unit; Buff is percent")]
        private T damageReduction;
        [SerializeField, SuffixLabel("%00"), Tooltip("Counters DeBuffPower")]
        private T deBuffReduction;


        [Title("Concentration")]
        [SerializeField, SuffixLabel("%00"), Tooltip("Affects Harmony gain")]
        private T enlightenment; // before fight this could be modified
        [SerializeField, SuffixLabel("%00")]
        private T criticalChance;
        [SerializeField, SuffixLabel("u|%%"), Tooltip("[100] is the default value")]
        private T speedAmount;

        [SerializeField, SuffixLabel("%00")]
        private T initiativePercentage;
        [SerializeField, SuffixLabel("Units")]
        private T actionsPerInitiative;
        [SerializeField, SuffixLabel("%00")]
        private T harmonyAmount;



        public T AttackPower
        {
            get => attackPower;
            set => attackPower = value;
        }

        public T DeBuffPower
        {
            get => deBuffPower;
            set => deBuffPower = value;
        }

        public T StaticDamagePower
        {
            get => staticDamagePower;
            set => staticDamagePower = value;
        }

        public T HealPower
        {
            get => healPower;
            set => healPower = value;
        }

        public T BuffPower
        {
            get => buffPower;
            set => buffPower = value;
        }

        public T BuffReceivePower
        {
            get => buffReceivePower;
            set => buffReceivePower = value;
        }

        public T MaxHealth
        {
            get => maxHealth;
            set => maxHealth = value;
        }

        public T MaxMortalityPoints
        {
            get => maxMortalityPoints;
            set => maxMortalityPoints = value;
        }

        public T DamageReduction
        {
            get => damageReduction;
            set => damageReduction = value;
        }

        public T DeBuffReduction
        {
            get => deBuffReduction;
            set => deBuffReduction = value;
        }

        public T Enlightenment
        {
            get => enlightenment;
            set => enlightenment = value;
        }

        public T CriticalChance
        {
            get => criticalChance;
            set => criticalChance = value;
        }

        public T SpeedAmount
        {
            get => speedAmount;
            set => speedAmount = value;
        }

        public T InitiativePercentage
        {
            get => initiativePercentage;
            set => initiativePercentage = value;
        }

        public T ActionsPerInitiative
        {
            get => actionsPerInitiative;
            set => actionsPerInitiative = value;
        }

        public T HarmonyAmount
        {
            get => harmonyAmount;
            set => harmonyAmount = value;
        }
    }

    /// <summary>
    /// Mean to be used for [<see cref="SerializeField"/>] in [<seealso cref="UnityEngine.Object"/>]'s serialization
    /// as a Base. It doesn't have any functionality beside that
    /// </summary>
    public abstract class SStatsBase : ScriptableObject, IStatsData
    {
        [SerializeField] private string statName = "NULL";
        public string StatName => statName;

        public abstract void DoInjection(IBasicStats<float> stats);

        protected virtual string AssetPrefix() => "STAT - ";

        [Button(ButtonSizes.Large)]
        private void UpdateAssetName()
        {
            name = AssetPrefix() + $"{statName} [Stats]";
            UtilsGame.UpdateAssetName(this);
        }
    }
}
