using System;
using System.Collections;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Skills;
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
        [SerializeField, BoxGroup("Top/Offensive"), LabelWidth(50)] protected T attackType;
        [SerializeField, BoxGroup("Top/Offensive"), LabelWidth(50)] protected T overTimeType;
        [SerializeField, BoxGroup("Top/Offensive"), LabelWidth(50)] protected T deBuffType;
        [SerializeField, BoxGroup("Top/Offensive"), LabelWidth(50)] protected T followUpType;

        [SerializeField, BoxGroup("Top/Support"), LabelWidth(50)] protected T healType;
        [SerializeField, BoxGroup("Top/Support"), LabelWidth(50)] protected T shieldingType;
        [SerializeField, BoxGroup("Top/Support"), LabelWidth(50)] protected T buffType;
        [SerializeField, BoxGroup("Top/Support"), LabelWidth(50)] protected T receiveBuffType;

        [HorizontalGroup("Bottom")]
        [SerializeField, BoxGroup("Bottom/Vitality"), LabelWidth(50)] protected T healthType;
        [SerializeField, BoxGroup("Bottom/Vitality"), LabelWidth(50)] protected T mortalityType;
        [SerializeField, BoxGroup("Bottom/Vitality"), LabelWidth(50)] protected T damageReductionType;
        [SerializeField, BoxGroup("Bottom/Vitality"), LabelWidth(50)] protected T deBuffResistanceType;

        [SerializeField, BoxGroup("Bottom/Concentration"), LabelWidth(50)] protected T actionsType;
        [SerializeField, BoxGroup("Bottom/Concentration"), LabelWidth(50)] protected T speedType;
        [SerializeField, BoxGroup("Bottom/Concentration"), LabelWidth(50)] protected T controlType;
        [SerializeField, BoxGroup("Bottom/Concentration"), LabelWidth(50)] protected T luckType;

        public StatsBase()
        { }
        public StatsBase(IBasicStatsRead<T> stats)
        {
            UtilsStats.DoCopyBasics(this, stats);
        }
        public StatsBase(T defaultValue) 
            : this(defaultValue,defaultValue,defaultValue,defaultValue)
        {
        }

        public StatsBase
            (T defaultOffensiveValue, T defaultSupportValue, T defaultVitalityValue, T defaultConcentrationValue)
        {
            attackType = defaultOffensiveValue;
            overTimeType = defaultOffensiveValue;
            deBuffType = defaultOffensiveValue;
            followUpType = defaultOffensiveValue;

            healType = defaultSupportValue;
            shieldingType = defaultSupportValue;
            buffType = defaultSupportValue;
            receiveBuffType = defaultSupportValue;

            healthType = defaultVitalityValue;
            mortalityType = defaultVitalityValue;
            damageReductionType = defaultVitalityValue;
            deBuffResistanceType = defaultVitalityValue;

            actionsType = defaultConcentrationValue;
            speedType = defaultConcentrationValue;
            controlType = defaultConcentrationValue;
            luckType = defaultConcentrationValue;
        }

        public void OverrideByValue(T value) 
            => OverrideByValue(value, value, value, value);

        public void OverrideByValue
            (T defaultOffensiveValue, T defaultSupportValue, T defaultVitalityValue, T defaultConcentrationValue)
        {
            attackType = defaultOffensiveValue;
            overTimeType = defaultOffensiveValue;
            deBuffType = defaultOffensiveValue;
            followUpType = defaultOffensiveValue;

            healType = defaultSupportValue;
            shieldingType = defaultSupportValue;
            buffType = defaultSupportValue;
            receiveBuffType = defaultSupportValue;

            healthType = defaultVitalityValue;
            mortalityType = defaultVitalityValue;
            damageReductionType = defaultVitalityValue;
            deBuffResistanceType = defaultVitalityValue;

            actionsType = defaultConcentrationValue;
            speedType = defaultConcentrationValue;
            controlType = defaultConcentrationValue;
            luckType = defaultConcentrationValue;
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
        [SerializeReference, HorizontalGroup("Top"), LabelWidth(100),
         InfoBox("Using Static.One", "OffensiveStatsNull")]
        private IOffensiveStats offensiveStats;
        [SerializeReference, HorizontalGroup("Top"), LabelWidth(100),
         InfoBox("Using Static.One", "SupportStatsNull")]
        private ISupportStats supportStats;
        [SerializeReference, HorizontalGroup("Bottom"), LabelWidth(100),
         InfoBox("Using Static.OneThousand", "VitalityStatsNull")]
        private IVitalityStats vitalityStats;
        [SerializeReference, HorizontalGroup("Bottom"), LabelWidth(100),
         InfoBox("Using Static.One", "ConcentrationStatsNull")]
        private IConcentrationStats concentrationStats;

#if UNITY_EDITOR
        private bool OffensiveStatsNull() => offensiveStats == null;
        private bool SupportStatsNull() => supportStats == null;
        private bool VitalityStatsNull() => vitalityStats == null;
        private bool ConcentrationStatsNull() => concentrationStats == null;
#endif

        public ReferencedMainStatsBase()
        { }

        public ReferencedMainStatsBase(IOffensiveStats offensive, ISupportStats support, 
            IVitalityStats vitality, IConcentrationStats concentration)
        {
            offensiveStats = offensive;
            supportStats = support;
            vitalityStats = vitality;
            concentrationStats = concentration;
        }

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

        public float AttackType { get => offensiveStats?.AttackType ?? Stats.OffensiveStats.One.AttackType; set => offensiveStats.AttackType = value; }
        public float OverTimeType { get => offensiveStats?.OverTimeType ?? Stats.OffensiveStats.One.OverTimeType; set => offensiveStats.OverTimeType = value; }
        public float DeBuffType { get => offensiveStats?.DeBuffType ?? Stats.OffensiveStats.One.DeBuffType; set => offensiveStats.DeBuffType = value; }
        public float FollowUpType { get => offensiveStats?.FollowUpType ?? Stats.OffensiveStats.One.FollowUpType; set => offensiveStats.FollowUpType = value; }

        public float HealType { get => supportStats?.HealType ?? Stats.SupportStats.One.HealType; set => supportStats.HealType = value; }
        public float ShieldingType { get => supportStats?.ShieldingType ?? Stats.SupportStats.One.ShieldingType; set => supportStats.ShieldingType = value; }
        public float BuffType { get => supportStats?.BuffType ?? Stats.SupportStats.One.BuffType; set => supportStats.BuffType = value; }
        public float ReceiveBuffType { get => supportStats?.ReceiveBuffType ?? Stats.SupportStats.One.ReceiveBuffType; set => supportStats.ReceiveBuffType = value; }

        public float HealthType { get => vitalityStats?.HealthType ?? Stats.VitalityStats.OneThousand.HealthType; set => vitalityStats.HealthType = value; }
        public float MortalityType { get => vitalityStats?.MortalityType ?? Stats.VitalityStats.OneThousand.MortalityType; set => vitalityStats.MortalityType = value; }
        public float DamageReductionType { get => vitalityStats?.DamageReductionType ?? Stats.VitalityStats.OneThousand.DamageReductionType; set => vitalityStats.DamageReductionType = value; }
        public float DeBuffResistanceType { get => vitalityStats?.DeBuffResistanceType ?? Stats.VitalityStats.OneThousand.DeBuffResistanceType; set => vitalityStats.DeBuffResistanceType = value; }

        public float ActionsType { get => concentrationStats?.ActionsType ?? Stats.ConcentrationStats.One.ActionsType; set => concentrationStats.ActionsType = value; }
        public float SpeedType { get => concentrationStats?.SpeedType ?? Stats.ConcentrationStats.One.SpeedType; set => concentrationStats.SpeedType = value; }
        public float ControlType { get => concentrationStats?.ControlType ?? Stats.ConcentrationStats.One.ControlType; set => concentrationStats.ControlType = value; }
        public float CriticalType { get => concentrationStats?.CriticalType ?? Stats.ConcentrationStats.One.CriticalType; set => concentrationStats.CriticalType = value; }
    }

    [Serializable]
    public class ReferencedPresetStats : IBasicStats
    {
        [SerializeField] private SPreparationEntityBase referencedEntity;

        [SerializeReference, 
         InfoBox("Using Preset reference", "OffensiveStatsNull")]
        private IOffensiveStats offensiveStats;
        [SerializeReference, 
         InfoBox("Using Preset reference", "SupportStatsNull")]
        private ISupportStats supportStats;
        [SerializeReference, 
         InfoBox("Using Preset reference", "VitalityStatsNull")]
        private IVitalityStats vitalityStats;
        [SerializeReference, 
         InfoBox("Using Preset reference", "ConcentrationStatsNull")]
        private IConcentrationStats concentrationStats;



#if UNITY_EDITOR
        private bool OffensiveStatsNull() => offensiveStats == null;
        private bool SupportStatsNull() => supportStats == null;
        private bool VitalityStatsNull() => vitalityStats == null;
        private bool ConcentrationStatsNull() => concentrationStats == null;
#endif

        public float AttackType { get => offensiveStats?.AttackType ?? referencedEntity.GetBaseStats().AttackType; set => offensiveStats.AttackType = value; }
        public float OverTimeType { get => offensiveStats?.OverTimeType ?? referencedEntity.GetBaseStats().OverTimeType; set => offensiveStats.OverTimeType = value; }
        public float DeBuffType { get => offensiveStats?.DeBuffType ?? referencedEntity.GetBaseStats().DeBuffType; set => offensiveStats.DeBuffType = value; }
        public float FollowUpType { get => offensiveStats?.FollowUpType ?? referencedEntity.GetBaseStats().FollowUpType; set => offensiveStats.FollowUpType = value; }

        public float HealType { get => supportStats?.HealType ?? referencedEntity.GetBaseStats().HealType; set => supportStats.HealType = value; }
        public float ShieldingType { get => supportStats?.ShieldingType ?? referencedEntity.GetBaseStats().ShieldingType; set => supportStats.ShieldingType = value; }
        public float BuffType { get => supportStats?.BuffType ?? referencedEntity.GetBaseStats().BuffType; set => supportStats.BuffType = value; }
        public float ReceiveBuffType { get => supportStats?.ReceiveBuffType ?? referencedEntity.GetBaseStats().ReceiveBuffType; set => supportStats.ReceiveBuffType = value; }

        public float HealthType { get => vitalityStats?.HealthType ?? referencedEntity.GetBaseStats().HealthType; set => vitalityStats.HealthType = value; }
        public float MortalityType { get => vitalityStats?.MortalityType ?? referencedEntity.GetBaseStats().MortalityType; set => vitalityStats.MortalityType = value; }
        public float DamageReductionType { get => vitalityStats?.DamageReductionType ?? referencedEntity.GetBaseStats().DamageReductionType; set => vitalityStats.DamageReductionType = value; }
        public float DeBuffResistanceType { get => vitalityStats?.DeBuffResistanceType ?? referencedEntity.GetBaseStats().DeBuffResistanceType; set => vitalityStats.DeBuffResistanceType = value; }

        public float ActionsType { get => concentrationStats?.ActionsType ?? referencedEntity.GetBaseStats().ActionsType; set => concentrationStats.ActionsType = value; }
        public float SpeedType { get => concentrationStats?.SpeedType ?? referencedEntity.GetBaseStats().SpeedType; set => concentrationStats.SpeedType = value; }
        public float ControlType { get => concentrationStats?.ControlType ?? referencedEntity.GetBaseStats().ControlType; set => concentrationStats.ControlType = value; }
        public float CriticalType { get => concentrationStats?.CriticalType ?? referencedEntity.GetBaseStats().CriticalType; set => concentrationStats.CriticalType = value; }
    }

    [Serializable]
    public class BaseStats : StatsBase<float>, IBasicStats
    {
        public BaseStats() : base(1,1,0,1)
        { }
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

        public OffensiveStats()
        { }

        public OffensiveStats(T defaultValue)
        {
            attackType = defaultValue;
            overTimeType = defaultValue;
            deBuffType = defaultValue;
            followUpType = defaultValue;
        }

    }

    [Serializable]
    public class OffensiveStats : OffensiveStats<float>, IOffensiveStats
    {
        public static readonly OffensiveStats One = new OffensiveStats(1);
        public OffensiveStats(float defaultValue) : base(defaultValue)
        { }

        public OffensiveStats() : this(1)
        {
            
        }
    }

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

        public SupportStats()
        {
            
        }

        public SupportStats(T defaultValue)
        {
            healType = defaultValue;
            shieldingType = defaultValue;
            buffType = defaultValue;
            receiveBuffType = defaultValue;
        }
    }

    [Serializable]
    public class SupportStats : SupportStats<float>, ISupportStats
    {
        public static readonly SupportStats One = new SupportStats(1);

        public SupportStats(float defaultValue) : base(defaultValue)
        { }
        public SupportStats() : this(1)
        { }
    }


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


        public VitalityStats(T defaultValue) : this(defaultValue,defaultValue,defaultValue)
        { }

        public VitalityStats(T healthValue, T mortalityValue, T percentValue)
        {
            healthType = healthValue;
            mortalityType = mortalityValue;
            damageReductionType = percentValue;
            deBuffResistanceType = percentValue;
        }
    }

    [Serializable]
    public class VitalityStats : VitalityStats<float>, IVitalityStats
    {
        public static readonly VitalityStats One = new VitalityStats(1);
        /// <summary>
        /// Similar than [<seealso cref="One"/>] but with (100HP / 1000 Mortality)
        /// </summary>
        public static readonly VitalityStats OneThousand = new VitalityStats(100,1000,1);

        public VitalityStats(float defaultValue) : base(defaultValue)
        { }

        public VitalityStats(float healthValue, float mortalityValue, float percentValue) 
            : base(healthValue,mortalityValue, percentValue)
        { }

        public VitalityStats() : this(100,1000,1)
        { }
    }

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

        public ConcentrationStats()
        {
            
        }

        public ConcentrationStats(T defaultValue)
        {
            actionsType = defaultValue;
            speedType = defaultValue;
            controlType = defaultValue;
            luckType = defaultValue;
        }

    }

    [Serializable]
    public class ConcentrationStats : ConcentrationStats<float>, IConcentrationStats
    {
        public static readonly ConcentrationStats One = new ConcentrationStats(1);

        public ConcentrationStats(float defaultValue) : base(defaultValue)
        { }

        public ConcentrationStats() : this(1)
        { }
    }
}
