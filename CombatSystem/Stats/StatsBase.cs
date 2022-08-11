using System;
using System.Collections;
using System.Collections.Generic;
using CombatSystem.Skills.Effects;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

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

        public Stats(IStatsRead<T> stats) : base()
        {
            UtilsStats.DoCopyFull(this, stats);
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
        public T VanguardEffectType => vitalityType;
        public T FlexibleEffectType => concentrationType;

        public T VanguardType => vitalityType;
        public T AttackerType => offensiveType;
        public T SupportType => supportType;
        public T FlexType => concentrationType;

        public T FrontLineType => vitalityType;
        public T MidLineType => offensiveType;
        public T BackLineType => supportType;
        public T FlexLineType => concentrationType;

        public T AttackingStance => offensiveType;
        public T SupportingStance => supportType;
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
        public T VanguardEffectType => vitalityType;
        public T FlexibleEffectType => concentrationType;

        public T VanguardType => vitalityType;
        public T AttackerType => offensiveType;
        public T SupportType => supportType;
        public T FlexType => concentrationType;

        public T FrontLineType => vitalityType;
        public T MidLineType => offensiveType;
        public T BackLineType => supportType;
        public T FlexLineType => concentrationType;

        public T AttackingStance => offensiveType;
        public T SupportingStance => supportType;
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
        [SerializeField, BoxGroup("Bottom/Concentration"), LabelWidth(100)] protected T luckType;

        public StatsBase()
        { }
        public StatsBase(IBasicStatsRead<T> stats)
        {
            UtilsStats.DoCopyBasics(this, stats);
        }
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
            luckType = value;
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
            get => luckType;
            set => luckType = value;
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
            luckType = new T();
        }
    }

    [Serializable]
    public class ReferencedMainStatsBase : IBasicStats
    {
        [SerializeReference, HorizontalGroup("Top"), LabelWidth(100)] private IOffensiveStats offensiveStats;
        [SerializeReference, HorizontalGroup("Top"), LabelWidth(100)] private ISupportStats supportStats;
        [SerializeReference, HorizontalGroup("Bottom"), LabelWidth(100)] private IVitalityStats vitalityStats;
        [SerializeReference, HorizontalGroup("Bottom"), LabelWidth(100)] private IConcentrationStats concentrationStats;

        public IOffensiveStats OffensiveStats
        {
            get => offensiveStats;
            set => offensiveStats = value;
        }

        public ISupportStats SupportStats
        {
            get => supportStats;
            set => supportStats = value;
        }

        public IVitalityStats VitalityStats
        {
            get => vitalityStats;
            set => vitalityStats = value;
        }

        public IConcentrationStats ConcentrationStats
        {
            get => concentrationStats;
            set => concentrationStats = value;
        }

        public float AttackType { get => offensiveStats.AttackType; set => offensiveStats.AttackType = value; }
        public float OverTimeType { get => offensiveStats.OverTimeType; set => offensiveStats.OverTimeType = value; }
        public float DeBuffType { get => offensiveStats.DeBuffType; set => offensiveStats.DeBuffType = value; }
        public float FollowUpType { get => offensiveStats.FollowUpType; set => offensiveStats.FollowUpType = value; }

        public float HealType { get => supportStats.HealType; set => supportStats.HealType = value; }
        public float ShieldingType { get => supportStats.ShieldingType; set => supportStats.ShieldingType = value; }
        public float BuffType { get => supportStats.BuffType; set => supportStats.BuffType = value; }
        public float ReceiveBuffType { get => supportStats.ReceiveBuffType; set => supportStats.ReceiveBuffType = value; }

        public float HealthType { get => vitalityStats.HealthType; set => vitalityStats.HealthType = value; }
        public float MortalityType { get => vitalityStats.MortalityType; set => vitalityStats.MortalityType = value; }
        public float DamageReductionType { get => vitalityStats.DamageReductionType; set => vitalityStats.DamageReductionType = value; }
        public float DeBuffResistanceType { get => vitalityStats.DeBuffResistanceType; set => vitalityStats.DeBuffResistanceType = value; }

        public float ActionsType { get => concentrationStats.ActionsType; set => concentrationStats.ActionsType= value; }
        public float SpeedType { get => concentrationStats.SpeedType; set => concentrationStats.SpeedType = value; }
        public float ControlType { get => concentrationStats.ControlType; set => concentrationStats.ControlType = value; }
        public float CriticalType { get => concentrationStats.CriticalType; set => concentrationStats.CriticalType = value; }
    }

    [Serializable]
    public class BaseStats : StatsBase<float>, IBasicStats
    {
        public BaseStats() : base(0)
        {

        }
    }

    public class OffensiveStats<T> : IOffensiveStats<T>
    {
        [SerializeField, LabelWidth(100)] protected T attackType;
        [SerializeField, LabelWidth(100)] protected T overTimeType;
        [SerializeField, LabelWidth(100)] protected T deBuffType;
        [SerializeField, LabelWidth(100)] protected T followUpType;

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
    }
    [Serializable]
    public class OffensiveStats : OffensiveStats<float>, IOffensiveStats { }

    public class SupportStats<T> : ISupportStats<T>
    {
        [SerializeField, LabelWidth(100)] protected T healType;
        [SerializeField, LabelWidth(100)] protected T shieldingType;
        [SerializeField, LabelWidth(100)] protected T buffType;
        [SerializeField, LabelWidth(100)] protected T receiveBuffType;

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
    }
    [Serializable]
    public class SupportStats : SupportStats<float>, ISupportStats { }


    public class VitalityStats<T> : IVitalityStats<T>
    {
        [SerializeField, LabelWidth(100)] protected T healthType;
        [SerializeField, LabelWidth(100)] protected T mortalityType;
        [SerializeField, LabelWidth(100)] protected T damageReductionType;
        [SerializeField, LabelWidth(100)] protected T deBuffResistanceType;
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
    }
    [Serializable]
    public class VitalityStats : VitalityStats<float>, IVitalityStats { }

    public class ConcentrationStats<T> : IConcentrationStats<T>
    {

        [SerializeField, LabelWidth(100)] protected T actionsType;
        [SerializeField, LabelWidth(100)] protected T speedType;
        [SerializeField, LabelWidth(100)] protected T controlType;
        [SerializeField, LabelWidth(100)] protected T luckType;
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
            get => luckType;
            set => luckType = value;
        }
    }
    [Serializable]
    public class ConcentrationStats : ConcentrationStats<float>, IConcentrationStats { }
}
