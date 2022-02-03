using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Stats
{
    [Serializable]
    public class Stats<T> : StatsBase<T>, IStats<T>
    {
        [TitleGroup("Masters"), PropertyOrder(-10)]
        [SerializeField] private T offensiveType;
        [TitleGroup("Masters")]
        [SerializeField] private T supportType;
        [TitleGroup("Masters")]
        [SerializeField] private T vitalityType;
        [TitleGroup("Masters")]
        [SerializeField] private T concentrationType;
        public T OffensiveType
        {
            get => offensiveType;
            set => offensiveType = value;
        }
        public T SupportType
        {
            get => supportType;
            set => supportType = value;
        }
        public T VitalityType
        {
            get => vitalityType;
            set => vitalityType = value;
        }
        public T ConcentrationType
        {
            get => concentrationType;
            set => concentrationType = value;
        }

        public Stats(T defaultValue) : this(defaultValue,defaultValue)
        { }

        public Stats(T masterDefaultValue, T basicsDefaultValue) : base(basicsDefaultValue)
        {
            offensiveType = masterDefaultValue;
            supportType = masterDefaultValue;
            vitalityType = masterDefaultValue;
            concentrationType = masterDefaultValue;
        }
    }

    [Serializable]
    public class SerializableStats<T> : SerializableStatsBase<T>, IStats<T>
    where T : new()
    {
        [Title("Masters")]
        [SerializeField] private T offensiveType = new T();
        [SerializeField] private T supportType = new T();
        [SerializeField] private T vitalityType = new T();
        [SerializeField] private T concentrationType = new T();
        public T OffensiveType
        {
            get => offensiveType;
            set => offensiveType = value;
        }
        public T SupportType
        {
            get => supportType;
            set => supportType = value;
        }
        public T VitalityType
        {
            get => vitalityType;
            set => vitalityType = value;
        }
        public T ConcentrationType
        {
            get => concentrationType;
            set => concentrationType = value;
        }
    }


    [Serializable]
    public class StatsBase<T> : IBasicStatsRead<T>,
        IOffensiveStats<T>, ISupportStats<T>, IVitalityStats<T>, IConcentrationStats<T>
    {
        [HorizontalGroup("Top")]
        [SerializeField, BoxGroup("Top/Offensive")] private T attackType;
        [SerializeField, BoxGroup("Top/Offensive")] private T overTimeType;
        [SerializeField, BoxGroup("Top/Offensive")] private T deBuffType;
        [SerializeField, BoxGroup("Top/Offensive")] private T followUpType;

        [SerializeField, BoxGroup("Top/Support")] private T healType;
        [SerializeField, BoxGroup("Top/Support")] private T shieldingType;
        [SerializeField, BoxGroup("Top/Support")] private T buffType;
        [SerializeField, BoxGroup("Top/Support")] private T receiveBuffType;

        [HorizontalGroup("Bottom")]
        [SerializeField, BoxGroup("Bottom/Vitality")] private T healthType;
        [SerializeField, BoxGroup("Bottom/Vitality")] private T mortalityType;
        [SerializeField, BoxGroup("Bottom/Vitality")] private T damageReductionType;
        [SerializeField, BoxGroup("Bottom/Vitality")] private T deBuffResistanceType;

        [SerializeField, BoxGroup("Bottom/Concentration")] private T actionsType;
        [SerializeField, BoxGroup("Bottom/Concentration")] private T speedType;
        [SerializeField, BoxGroup("Bottom/Concentration")] private T controlType;
        [SerializeField, BoxGroup("Bottom/Concentration")] private T criticalType;

        public StatsBase(T defaultValue)
        {
            attackType = defaultValue;
            overTimeType = defaultValue;
            deBuffType = defaultValue;
            followUpType = defaultValue;

            healType = defaultValue;
            shieldingType = defaultValue;
            buffType = defaultValue;
            receiveBuffType = defaultValue;

            healthType = defaultValue;
            mortalityType = defaultValue;
            damageReductionType = defaultValue;
            deBuffResistanceType = defaultValue;

            actionsType = defaultValue;
            speedType = defaultValue;
            controlType = defaultValue;
            criticalType = defaultValue;
        }

        public T AttackType
        {
            get => attackType;
            set => attackType = value;
        }
        public T OverTimeType
        {
            get => overTimeType;
            set => overTimeType = value;
        }
        public T DeBuffType
        {
            get => deBuffType;
            set => deBuffType = value;
        }
        public T FollowUpType
        {
            get => followUpType;
            set => followUpType = value;
        }

        
        public T HealType
        {
            get => healType;
            set => healType = value;
        }
        public T ShieldingType
        {
            get => shieldingType;
            set => shieldingType = value;
        }
        public T BuffType
        {
            get => buffType;
            set => buffType = value;
        }
        public T ReceiveBuffType
        {
            get => receiveBuffType;
            set => receiveBuffType = value;
        }

        
        public T HealthType
        {
            get => healthType;
            set => healthType = value;
        }
        public T MortalityType
        {
            get => mortalityType;
            set => mortalityType = value;
        }
        public T DamageReductionType
        {
            get => damageReductionType;
            set => damageReductionType = value;
        }
        public T DeBuffResistanceType
        {
            get => deBuffResistanceType;
            set => deBuffResistanceType = value;
        }

        
        public T ActionsType
        {
            get => actionsType;
            set => actionsType = value;
        }
        public T SpeedType
        {
            get => speedType;
            set => speedType = value;
        }
        public T ControlType
        {
            get => controlType;
            set => controlType = value;
        }
        public T CriticalType
        {
            get => criticalType;
            set => criticalType = value;
        }
    }

    [Serializable]
    public class SerializableStatsBase<T> : IOffensiveStats<T>, ISupportStats<T>, IVitalityStats<T>, IConcentrationStats<T>
        where T : new()
    {
        [Title("Offensive"), HorizontalGroup("Top")]
        [SerializeField] private T attackType = new T();
        [SerializeField] private T overTimeType = new T();
        [SerializeField] private T deBuffType = new T();
        [SerializeField] private T followUpType = new T();

        [Title("Support"), HorizontalGroup("Top")]
        [SerializeField] private T healType = new T();
        [SerializeField] private T shieldingType = new T();
        [SerializeField] private T buffType = new T();
        [SerializeField] private T receiveBuffType = new T();

        [Title("Vitality"), HorizontalGroup("Bottom")]
        [SerializeField] private T healthType = new T();
        [SerializeField] private T mortalityType = new T();
        [SerializeField] private T damageReductionType = new T();
        [SerializeField] private T deBuffResistanceType = new T();

        [Title("Concentration"), HorizontalGroup("Bottom")]
        [SerializeField] private T actionsType = new T();
        [SerializeField] private T speedType = new T();
        [SerializeField] private T controlType = new T();
        [SerializeField] private T criticalType = new T();

        public T AttackType
        {
            get => attackType;
            set => attackType = value;
        }
        public T OverTimeType
        {
            get => overTimeType;
            set => overTimeType = value;
        }
        public T DeBuffType
        {
            get => deBuffType;
            set => deBuffType = value;
        }
        public T FollowUpType
        {
            get => followUpType;
            set => followUpType = value;
        }

       
        public T HealType
        {
            get => healType;
            set => healType = value;
        }
        public T ShieldingType
        {
            get => shieldingType;
            set => shieldingType = value;
        }
        public T BuffType
        {
            get => buffType;
            set => buffType = value;
        }
        public T ReceiveBuffType
        {
            get => receiveBuffType;
            set => receiveBuffType = value;
        }

       
        public T HealthType
        {
            get => healthType;
            set => healthType = value;
        }
        public T MortalityType
        {
            get => mortalityType;
            set => mortalityType = value;
        }
        public T DamageReductionType
        {
            get => damageReductionType;
            set => damageReductionType = value;
        }
        public T DeBuffResistanceType
        {
            get => deBuffResistanceType;
            set => deBuffResistanceType = value;
        }

        
        public T ActionsType
        {
            get => actionsType;
            set => actionsType = value;
        }
        public T SpeedType
        {
            get => speedType;
            set => speedType = value;
        }
        public T ControlType
        {
            get => controlType;
            set => controlType = value;
        }
        public T CriticalType
        {
            get => criticalType;
            set => criticalType = value;
        }
    }
}
