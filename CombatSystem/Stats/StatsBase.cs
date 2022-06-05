using System;
using System.Collections;
using System.Collections.Generic;
using CombatSystem.Skills.Effects;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Stats
{
    [Serializable]
    public class Stats<T> : StatsBase<T>, IStats<T>,
        ITeamFlexStructureRead<T>, ITeamFlexPositionStructureRead<T>, 
        IEffectTypeStructureRead<T>,
        IStanceStructureRead<T>
    {
        [TitleGroup("Masters"), PropertyOrder(-10)]
        [SerializeField] private T offensiveType;
        [TitleGroup("Masters")]
        [SerializeField] private T supportType;
        [TitleGroup("Masters")]
        [SerializeField] private T vitalityType;
        [TitleGroup("Masters")]
        [SerializeField] private T concentrationType;


        public Stats(T defaultValue) : this(defaultValue, defaultValue)
        { }

        public Stats(T masterDefaultValue, T basicsDefaultValue) : base(basicsDefaultValue)
        {
            offensiveType = masterDefaultValue;
            supportType = masterDefaultValue;
            vitalityType = masterDefaultValue;
            concentrationType = masterDefaultValue;
        }


        public T OffensiveStatType
        {
            get => offensiveType;
            set => offensiveType = value;
        }
        public T SupportStatType
        {
            get => supportType;
            set => supportType = value;
        }
        public T VitalityStatType
        {
            get => vitalityType;
            set => vitalityType = value;
        }
        public T ConcentrationStatType
        {
            get => concentrationType;
            set => concentrationType = value;
        }

        public T OffensiveEffectType => offensiveType;
        public T SupportEffectType => supportType;
        public T TeamEffectType => vitalityType;

        public T VanguardType => vitalityType;
        public T AttackerType => offensiveType;
        public T SupportType => supportType;
        public T FlexType => concentrationType;

        public T FrontLineType => vitalityType;
        public T MidLineType => offensiveType;
        public T BackLineType => supportType;
        public T FlexLineType => concentrationType;

        public T AttackingStance => offensiveType;
        public T NeutralStance => supportType;
        public T DefendingStance => vitalityType;
    }

    [Serializable]
    public class SerializableStats<T> : SerializableStatsBase<T>, IStats<T>,
        ITeamFlexStructureRead<T>, ITeamFlexPositionStructureRead<T>,
        IEffectTypeStructureRead<T>,
        IStanceStructureRead<T>
    where T : new()
    {
        [Title("Masters")]
        [SerializeField] private T offensiveType = new T();
        [SerializeField] private T supportType = new T();
        [SerializeField] private T vitalityType = new T();
        [SerializeField] private T concentrationType = new T();
        public T OffensiveStatType
        {
            get => offensiveType;
            set => offensiveType = value;
        }
        public T SupportStatType
        {
            get => supportType;
            set => supportType = value;
        }
        public T VitalityStatType
        {
            get => vitalityType;
            set => vitalityType = value;
        }
        public T ConcentrationStatType
        {
            get => concentrationType;
            set => concentrationType = value;
        }


        public T OffensiveEffectType => offensiveType;
        public T SupportEffectType => supportType;
        public T TeamEffectType => vitalityType;

        public T VanguardType => vitalityType;
        public T AttackerType => offensiveType;
        public T SupportType => supportType;
        public T FlexType => concentrationType;

        public T FrontLineType => vitalityType;
        public T MidLineType => offensiveType;
        public T BackLineType => supportType;
        public T FlexLineType => concentrationType;

        public T AttackingStance => offensiveType;
        public T NeutralStance => supportType;
        public T DefendingStance => vitalityType;
    }


    [Serializable]
    public class StatsBase<T> : IBasicStats<T>
    {
        [HorizontalGroup("Top")]
        [SerializeField, BoxGroup("Top/Offensive"), LabelWidth(100)] protected T attackType;
        [SerializeField, BoxGroup("Top/Offensive"), LabelWidth(100)] protected T overTimeType;
        [SerializeField, BoxGroup("Top/Offensive"), LabelWidth(100)] protected T deBuffType;
        [SerializeField, BoxGroup("Top/Offensive"), LabelWidth(100)] protected T followUpType;

        [SerializeField, BoxGroup("Top/Support"), LabelWidth(100)] protected T healType;
        [SerializeField, BoxGroup("Top/Support"), LabelWidth(100)] protected T shieldingType;
        [SerializeField, BoxGroup("Top/Support"), LabelWidth(100)] protected T buffType;
        [SerializeField, BoxGroup("Top/Support"), LabelWidth(100)] protected T receiveBuffType;

        [HorizontalGroup("Bottom")]
        [SerializeField, BoxGroup("Bottom/Vitality"), LabelWidth(100)] protected T healthType;
        [SerializeField, BoxGroup("Bottom/Vitality"), LabelWidth(100)] protected T mortalityType;
        [SerializeField, BoxGroup("Bottom/Vitality"), LabelWidth(100)] protected T damageReductionType;
        [SerializeField, BoxGroup("Bottom/Vitality"), LabelWidth(100)] protected T deBuffResistanceType;

        [SerializeField, BoxGroup("Bottom/Concentration"), LabelWidth(100)] protected T actionsType;
        [SerializeField, BoxGroup("Bottom/Concentration"), LabelWidth(100)] protected T speedType;
        [SerializeField, BoxGroup("Bottom/Concentration"), LabelWidth(100)] protected T controlType;
        [SerializeField, BoxGroup("Bottom/Concentration"), LabelWidth(100)] protected T criticalType;


        public StatsBase()
        { }
        public StatsBase(T defaultValue)
        {
            OverrideByValue(in defaultValue);
        }

        public void OverrideByValue(in T value)
        {
            attackType = value;
            overTimeType = value;
            deBuffType = value;
            followUpType = value;

            healType = value;
            shieldingType = value;
            buffType = value;
            receiveBuffType = value;

            healthType = value;
            mortalityType = value;
            damageReductionType = value;
            deBuffResistanceType = value;

            actionsType = value;
            speedType = value;
            controlType = value;
            criticalType = value;
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
    public class SerializableStatsBase<T> : StatsBase<T>,IBasicStats<T>
        where T : new()
    {
        public SerializableStatsBase() : base()
        {
            attackType = new T();
            overTimeType = new T();
            deBuffType = new T();
            followUpType = new T();

            healType = new T();
            shieldingType = new T();
            buffType = new T();
            receiveBuffType = new T();

            healthType = new T();
            mortalityType = new T();
            damageReductionType = new T();
            deBuffResistanceType = new T();

            actionsType = new T();
            speedType = new T();
            controlType = new T();
            criticalType = new T();
        }
    }
}
