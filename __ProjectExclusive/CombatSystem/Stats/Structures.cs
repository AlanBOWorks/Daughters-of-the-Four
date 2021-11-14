using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class MasterStatStructure<T> : IMasterStats<T>
    {
        [SerializeField]
        private T offensive;
        [SerializeField]
        private T support;
        [SerializeField]
        private T vitality;
        [SerializeField]
        private T concentration;

        public T Offensive
        {
            get => offensive;
            set => offensive = value;
        }

        public T Support
        {
            get => support;
            set => support = value;
        }

        public T Vitality
        {
            get => vitality;
            set => vitality = value;
        }

        public T Concentration
        {
            get => concentration;
            set => concentration = value;
        }
    }

    [Serializable]
    public class CondensedMasterStructure<TMaster,TElement> : IMasterStats<TMaster>, IBaseStats<TElement>,
        ICondensedOffensiveStat<TMaster,TElement>,
        ICondensedSupportStat<TMaster,TElement>
    {
        [SerializeField, HorizontalGroup("External Action")]
        private OffensiveWrapper offensiveValues = new OffensiveWrapper();
        [SerializeField,HorizontalGroup("External Action")]
        private SupportWrapper supportValues = new SupportWrapper();
        [SerializeField, HorizontalGroup("Self Action")]
        private VitalityWrapper vitalityValues = new VitalityWrapper();
        [SerializeField, HorizontalGroup("Self Action")]
        private ConcentrationWrapper concentrationValues = new ConcentrationWrapper();

        public TMaster GetElement(EnumStats.MasterStatType type) => UtilStats.GetElement(type, this);
        public TElement GetElement(EnumStats.OffensiveStatType type) => UtilStats.GetElement(type, this);
        public TElement GetElement(EnumStats.SupportStatType type) => UtilStats.GetElement(type, this);
        public TElement GetElement(EnumStats.VitalityStatType type) => UtilStats.GetElement(type, this);
        public TElement GetElement(EnumStats.ConcentrationStatType type) => UtilStats.GetElement(type, this);
        public TElement GetElement(EnumStats.BaseStatType type) => UtilStats.GetElement(type, this);

        public OffensiveWrapper<TMaster, TElement> GetOffensiveWrapper() => offensiveValues;
        public SupportWrapper<TMaster, TElement> GetSupportWrapper() => supportValues;
        public VitalityWrapper<TMaster, TElement> GetVitalityWrapper() => vitalityValues;
        public ConcentrationWrapper<TMaster, TElement> GetConcentrationWrapper() => concentrationValues;


        #region Interface Implementation

        // MASTER
        public TMaster Offensive
        {
            get => offensiveValues.offensiveElement;
            set => offensiveValues.offensiveElement = value;
        }
        public TMaster Support
        {
            get => supportValues.supportElement;
            set => supportValues.supportElement = value;
        }
        public TMaster Vitality
        {
            get => vitalityValues.vitalityElement;
            set => vitalityValues.vitalityElement = value;
        }
        public TMaster Concentration
        {
            get => concentrationValues.concentrationElement;
            set => concentrationValues.concentrationElement = value;
        }

        // OFFENSIVE
        public TElement Attack
        {
            get => offensiveValues.attack;
            set => offensiveValues.attack = value;
        }
        public TElement Persistent
        {
            get => offensiveValues.persistent;
            set => offensiveValues.persistent = value;
        }
        public TElement Debuff
        {
            get => offensiveValues.debuff;
            set => offensiveValues.debuff = value;
        }
        public TElement FollowUp
        {
            get => offensiveValues.followUp;
            set => offensiveValues.followUp = value;
        }

        // SUPPORT

        public TElement Heal
        {
            get => supportValues.heal;
            set => supportValues.heal = value;
        }
        public TElement Buff
        {
            get => supportValues.buff;
            set => supportValues.buff = value;
        }
        public TElement ReceiveBuff
        {
            get => supportValues.receiveBuff;
            set => supportValues.receiveBuff = value;
        }
        public TElement Shielding
        {
            get => supportValues.shielding;
            set => supportValues.shielding = value;
        }

        // VITALITY
        public TElement MaxHealth
        {
            get => vitalityValues.maxHealth;
            set => vitalityValues.maxHealth = value;
        }
        public TElement MaxMortality
        {
            get => vitalityValues.maxMortality;
            set => vitalityValues.maxMortality = value;
        }
        public TElement DebuffResistance
        {
            get => vitalityValues.debuffResistance;
            set => vitalityValues.debuffResistance = value;
        }
        public TElement DamageResistance
        {
            get => vitalityValues.damageResistance;
            set => vitalityValues.damageResistance = value;
        }

        // CONCENTRATION
        public TElement InitiativeSpeed
        {
            get => concentrationValues.initiativeSpeed;
            set => concentrationValues.initiativeSpeed = value;
        }
        public TElement InitialInitiative
        {
            get => concentrationValues.initialInitiative;
            set => concentrationValues.initiativeSpeed = value;
        }
        public TElement ActionsPerSequence
        {
            get => concentrationValues.actionsPerSequence;
            set => concentrationValues.actionsPerSequence = value;
        }
        public TElement Critical
        {
            get => concentrationValues.critical;
            set => concentrationValues.critical = value;
        }


        [Serializable]
        private class OffensiveWrapper : OffensiveWrapper<TMaster, TElement>
        { }
        [Serializable]
        private class SupportWrapper : SupportWrapper<TMaster, TElement>
        { }
        [Serializable]
        private class VitalityWrapper : VitalityWrapper<TMaster, TElement>
        { }
        [Serializable]
        private class ConcentrationWrapper : ConcentrationWrapper<TMaster, TElement>
        { } 
        #endregion
    }


    [Serializable]
    public class SerializableCondensedMasterStructure<TMaster, TElement> : IMasterStats<TMaster>, IBaseStats<TElement>,
        ICondensedOffensiveStat<TMaster, TElement>,
        ICondensedSupportStat<TMaster, TElement>
    where TMaster : new() where TElement : new()
    {
        [SerializeField, HorizontalGroup("External Action")]
        private OffensiveWrapper offensiveValues = new OffensiveWrapper();
        [SerializeField, HorizontalGroup("External Action")]
        private SupportWrapper supportValues = new SupportWrapper();
        [SerializeField, HorizontalGroup("Self Action")]
        private VitalityWrapper vitalityValues = new VitalityWrapper();
        [SerializeField, HorizontalGroup("Self Action")]
        private ConcentrationWrapper concentrationValues = new ConcentrationWrapper();

        public TMaster GetElement(EnumStats.MasterStatType type) => UtilStats.GetElement(type, this);
        public TElement GetElement(EnumStats.OffensiveStatType type) => UtilStats.GetElement(type, this);
        public TElement GetElement(EnumStats.SupportStatType type) => UtilStats.GetElement(type, this);
        public TElement GetElement(EnumStats.VitalityStatType type) => UtilStats.GetElement(type, this);
        public TElement GetElement(EnumStats.ConcentrationStatType type) => UtilStats.GetElement(type, this);
        public TElement GetElement(EnumStats.BaseStatType type) => UtilStats.GetElement(type, this);

        public SerializableOffensiveWrapper<TMaster, TElement> GetOffensiveWrapper() => offensiveValues;
        public SerializableSupportWrapper<TMaster, TElement> GetSupportWrapper() => supportValues;
        public SerializableVitalityWrapper<TMaster, TElement> GetVitalityWrapper() => vitalityValues;
        public SerializableConcentrationWrapper<TMaster, TElement> GetConcentrationWrapper() => concentrationValues;


        #region Interface Implementation

        // MASTER
        public TMaster Offensive
        {
            get => offensiveValues.offensiveElement;
            set => offensiveValues.offensiveElement = value;
        }
        public TMaster Support
        {
            get => supportValues.supportElement;
            set => supportValues.supportElement = value;
        }
        public TMaster Vitality
        {
            get => vitalityValues.vitalityElement;
            set => vitalityValues.vitalityElement = value;
        }
        public TMaster Concentration
        {
            get => concentrationValues.concentrationElement;
            set => concentrationValues.concentrationElement = value;
        }

        // OFFENSIVE
        public TElement Attack
        {
            get => offensiveValues.attack;
            set => offensiveValues.attack = value;
        }
        public TElement Persistent
        {
            get => offensiveValues.persistent;
            set => offensiveValues.persistent = value;
        }
        public TElement Debuff
        {
            get => offensiveValues.debuff;
            set => offensiveValues.debuff = value;
        }
        public TElement FollowUp
        {
            get => offensiveValues.followUp;
            set => offensiveValues.followUp = value;
        }

        // SUPPORT
        public TElement Heal
        {
            get => supportValues.heal;
            set => supportValues.heal = value;
        }
        public TElement Buff
        {
            get => supportValues.buff;
            set => supportValues.buff = value;
        }
        public TElement ReceiveBuff
        {
            get => supportValues.receiveBuff;
            set => supportValues.receiveBuff = value;
        }
        public TElement Shielding
        {
            get => supportValues.shielding;
            set => supportValues.shielding = value;
        }

        // VITALITY
        public TElement MaxHealth
        {
            get => vitalityValues.maxHealth;
            set => vitalityValues.maxHealth = value;
        }
        public TElement MaxMortality
        {
            get => vitalityValues.maxMortality;
            set => vitalityValues.maxMortality = value;
        }
        public TElement DebuffResistance
        {
            get => vitalityValues.debuffResistance;
            set => vitalityValues.debuffResistance = value;
        }
        public TElement DamageResistance
        {
            get => vitalityValues.damageResistance;
            set => vitalityValues.damageResistance = value;
        }

        // CONCENTRATION
        public TElement InitiativeSpeed
        {
            get => concentrationValues.initiativeSpeed;
            set => concentrationValues.initiativeSpeed = value;
        }
        public TElement InitialInitiative
        {
            get => concentrationValues.initialInitiative;
            set => concentrationValues.initiativeSpeed = value;
        }
        public TElement ActionsPerSequence
        {
            get => concentrationValues.actionsPerSequence;
            set => concentrationValues.actionsPerSequence = value;
        }
        public TElement Critical
        {
            get => concentrationValues.critical;
            set => concentrationValues.critical = value;
        }


        [Serializable]
        private class OffensiveWrapper : SerializableOffensiveWrapper<TMaster, TElement>
        { }
        [Serializable]
        private class SupportWrapper : SerializableSupportWrapper<TMaster, TElement>
        { }
        [Serializable]
        private class VitalityWrapper : SerializableVitalityWrapper<TMaster, TElement>
        { }
        [Serializable]
        private class ConcentrationWrapper : SerializableConcentrationWrapper<TMaster, TElement>
        { }
        #endregion
    }


    /// <summary>
    /// Structure that contains a master element [<seealso cref="IMasterStats{T}"/>] and its relative
    /// sub-elements [<seealso cref="IBaseStats{T}"/>]
    /// </summary>
    [Serializable]
    public class OffensiveWrapper<TMaster, TElement> : ICondensedOffensiveStat<TMaster,TElement>
    {
        [Title("Master")]
        public TMaster offensiveElement;

        [Title("SubElement")]
        public TElement attack;
        public TElement persistent;
        public TElement debuff;
        public TElement followUp;

        public TMaster Offensive
        {
            get => offensiveElement;
            set => offensiveElement = value;
        }

        public TElement Attack
        {
            get => attack;
            set => attack = value;
        }

        public TElement Persistent
        {
            get => persistent;
            set => persistent = value;
        }

        public TElement Debuff
        {
            get => debuff;
            set => debuff = value;
        }

        public TElement FollowUp
        {
            get => followUp;
            set => followUp = value;
        }
    }
    /// <summary>
    /// Serializable version for [<seealso cref="object"/>] initializations; <br></br>
    /// <inheritdoc cref="OffensiveWrapper{TMaster,TElement}"/>
    /// </summary>
    [Serializable]
    public class SerializableOffensiveWrapper<TMaster, TElement> : ICondensedOffensiveStat<TMaster, TElement>
     where TMaster : new() where TElement : new()
    {
        [Title("Master")]
        public TMaster offensiveElement = new TMaster();

        [Title("SubElement")]
        public TElement attack = new TElement();
        public TElement persistent = new TElement();
        public TElement debuff = new TElement();
        public TElement followUp = new TElement();

        public TMaster Offensive
        {
            get => offensiveElement;
            set => offensiveElement = value;
        }
        public TElement Attack
        {
            get => attack;
            set => attack = value;
        }

        public TElement Persistent
        {
            get => persistent;
            set => persistent = value;
        }

        public TElement Debuff
        {
            get => debuff;
            set => debuff = value;
        }

        public TElement FollowUp
        {
            get => followUp;
            set => followUp = value;
        }
    }

    /// <summary>
    /// <inheritdoc cref="OffensiveWrapper{TMaster,TElement}"/>
    /// </summary>
    [Serializable]
    public class SupportWrapper<TMaster, TElement> : ICondensedSupportStat<TMaster,TElement>
    {
        [Title("Master")]
        public TMaster supportElement;
        [Title("SubElement")]
        public TElement heal;
        public TElement shielding;
        public TElement buff;
        public TElement receiveBuff;

        public TMaster Support
        {
            get => supportElement;
            set => supportElement = value;
        }

        public TElement Heal
        {
            get => heal;
            set => heal = value;
        }

        public TElement Buff
        {
            get => buff;
            set => buff = value;
        }

        public TElement ReceiveBuff
        {
            get => receiveBuff;
            set => receiveBuff = value;
        }

        public TElement Shielding
        {
            get => shielding;
            set => shielding = value;
        }
    }
    /// <summary>
    /// <inheritdoc cref="SerializableOffensiveWrapper{TMaster,TElement}"/>
    /// </summary>
    [Serializable]
    public class SerializableSupportWrapper<TMaster, TElement> : ICondensedSupportStat<TMaster,TElement>
     where TMaster : new() where TElement : new()
    {
        [Title("Master")]
        public TMaster supportElement = new TMaster();
        [Title("SubElement")]
        public TElement heal = new TElement();
        public TElement shielding = new TElement();
        public TElement buff = new TElement();
        public TElement receiveBuff = new TElement();


        public TMaster Support
        {
            get => supportElement;
            set => supportElement = value;
        }
        public TElement Heal
        {
            get => heal;
            set => heal = value;
        }

        public TElement Buff
        {
            get => buff;
            set => buff = value;
        }

        public TElement ReceiveBuff
        {
            get => receiveBuff;
            set => receiveBuff = value;
        }

        public TElement Shielding
        {
            get => shielding;
            set => shielding = value;
        }
    }

    /// <summary>
    /// <inheritdoc cref="OffensiveWrapper{TMaster,TElement}"/>
    /// </summary>
    [Serializable]
    public class VitalityWrapper<TMaster, TElement> : IVitalityStatsRead<TElement>, IVitalityStatsInject<TElement>
    {
        [Title("Master")]
        public TMaster vitalityElement;

        [Title("SubElements")]
        public TElement maxHealth;
        public TElement maxMortality;
        public TElement debuffResistance;
        public TElement damageResistance;

        public TElement MaxHealth
        {
            get => maxHealth;
            set => maxHealth = value;
        }
        public TElement MaxMortality
        {
            get => maxMortality;
            set => maxMortality = value;
        }
        public TElement DebuffResistance
        {
            get => debuffResistance;
            set => debuffResistance = value;
        }
        public TElement DamageResistance
        {
            get => damageResistance;
            set => damageResistance = value;
        }
    }

    /// <summary>
    /// Serializable version for [<seealso cref="object"/>] initializations; <br></br>
    /// <inheritdoc cref="OffensiveWrapper{TMaster,TElement}"/>
    /// </summary>
    [Serializable]
    public class SerializableVitalityWrapper<TMaster, TElement> : IVitalityStatsRead<TElement>, IVitalityStatsInject<TElement>
     where TMaster : new() where TElement : new()
    {
        [Title("Master")] 
        public TMaster vitalityElement = new TMaster();

        [Title("SubElements")]
        public TElement maxHealth = new TElement();
        public TElement maxMortality = new TElement();
        public TElement debuffResistance = new TElement();
        public TElement damageResistance = new TElement();

        public TElement MaxHealth
        {
            get => maxHealth;
            set => maxHealth = value;
        }
        public TElement MaxMortality
        {
            get => maxMortality;
            set => maxMortality = value;
        }
        public TElement DebuffResistance
        {
            get => debuffResistance;
            set => debuffResistance = value;
        }
        public TElement DamageResistance
        {
            get => damageResistance;
            set => damageResistance = value;
        }
    }


    /// <summary>
    /// <inheritdoc cref="OffensiveWrapper{TMaster,TElement}"/>
    /// </summary>
    [Serializable]
    public class ConcentrationWrapper<TMaster, TElement> : IConcentrationStatsRead<TElement>, IConcentrationStatsInject<TElement>
    {
        [Title("Master")]
        public TMaster concentrationElement;

        [Title("SubElements")]
        public TElement initiativeSpeed;
        public TElement initialInitiative;
        public TElement actionsPerSequence;
        public TElement critical;

        public TElement InitiativeSpeed
        {
            get => initiativeSpeed;
            set => initiativeSpeed = value;
        }

        public TElement InitialInitiative
        {
            get => initialInitiative;
            set => initialInitiative = value;
        }

        public TElement ActionsPerSequence
        {
            get => actionsPerSequence;
            set => actionsPerSequence = value;
        }

        public TElement Critical
        {
            get => critical;
            set => critical = value;
        }
    }

    /// <summary>
    /// Serializable version for [<seealso cref="object"/>] initializations; <br></br>
    /// <inheritdoc cref="OffensiveWrapper{TMaster,TElement}"/>
    /// </summary>
    [Serializable]
    public class SerializableConcentrationWrapper<TMaster, TElement> : IConcentrationStatsRead<TElement>, IConcentrationStatsInject<TElement>
    where TMaster : new() where TElement : new()
    {
        [Title("Master")]
        public TMaster concentrationElement = new TMaster();

        [Title("SubElements")]
        public TElement initiativeSpeed = new TElement();
        public TElement initialInitiative = new TElement();
        public TElement actionsPerSequence = new TElement();
        public TElement critical = new TElement();

        public TElement InitiativeSpeed
        {
            get => initiativeSpeed;
            set => initiativeSpeed = value; 
        }

        public TElement InitialInitiative
        {
            get => initialInitiative;
            set => initialInitiative = value;
        }

        public TElement ActionsPerSequence
        {
            get => actionsPerSequence;
            set => actionsPerSequence = value;
        }

        public TElement Critical
        {
            get => critical;
            set => critical = value;
        }
    }
}
