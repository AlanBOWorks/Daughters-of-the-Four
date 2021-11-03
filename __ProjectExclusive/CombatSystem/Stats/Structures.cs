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
    public class CondensedMasterStructure<TMaster,TElement> : IMasterStats<TMaster>, IBaseStats<TElement>
    {
        [SerializeField, HorizontalGroup("External Action")]
        private OffensiveWrapper offensiveValues = new OffensiveWrapper();
        [SerializeField,HorizontalGroup("External Action")]
        private SupportWrapper supportValues = new SupportWrapper();
        [SerializeField, HorizontalGroup("Self Action")]
        private VitalityWrapper vitalityValues = new VitalityWrapper();
        [SerializeField, HorizontalGroup("Self Action")]
        private ConcentrationWrapper concentrationValues = new ConcentrationWrapper();

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
        private class OffensiveWrapper : IOffensiveStatsInject<TElement>, IOffensiveStatsRead<TElement>
        {
            [Title("Master")]
            public TMaster offensiveElement;

            [Title("SubElement")]
            public TElement attack;
            public TElement persistent;
            public TElement debuff;
            public TElement followUp;

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

        [Serializable]
        private class SupportWrapper : ISupportStatsRead<TElement>, ISupportStatsInject<TElement>
        {
            [Title("Master")]
            public TMaster supportElement;
            [Title("SubElement")]
            public TElement heal;
            public TElement buff;
            public TElement receiveBuff;
            public TElement shielding;

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

        [Serializable]
        private class VitalityWrapper : IVitalityStatsRead<TElement>, IVitalityStatsInject<TElement>
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

        [Serializable]
        private class ConcentrationWrapper : IConcentrationStatsRead<TElement>, IConcentrationStatsInject<TElement>
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
    }


}
