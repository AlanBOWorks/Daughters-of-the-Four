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
    public class CondensedMasterStructure<T> : IMasterStats<T>, IBaseStats<T>
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
        public T Offensive
        {
            get => offensiveValues.offensiveElement;
            set => offensiveValues.offensiveElement = value;
        }
        public T Support
        {
            get => supportValues.supportElement;
            set => supportValues.supportElement = value;
        }
        public T Vitality
        {
            get => vitalityValues.vitalityElement;
            set => vitalityValues.vitalityElement = value;
        }
        public T Concentration
        {
            get => concentrationValues.concentrationElement;
            set => concentrationValues.concentrationElement = value;
        }

        // OFFENSIVE
        public T Attack
        {
            get => offensiveValues.attack ?? offensiveValues.offensiveElement;
            set => offensiveValues.attack = value;
        }
        public T Persistent
        {
            get => offensiveValues.persistent ?? offensiveValues.offensiveElement;
            set => offensiveValues.persistent = value;
        }
        public T Debuff
        {
            get => offensiveValues.debuff ?? offensiveValues.offensiveElement;
            set => offensiveValues.debuff = value;
        }
        public T FollowUp
        {
            get => offensiveValues.followUp ?? offensiveValues.offensiveElement;
            set => offensiveValues.followUp = value;
        }

        // SUPPORT
        public T Heal
        {
            get => supportValues.heal ?? supportValues.supportElement;
            set => supportValues.heal = value;
        }
        public T Buff
        {
            get => supportValues.buff ?? supportValues.supportElement;
            set => supportValues.buff = value;
        }
        public T ReceiveBuff
        {
            get => supportValues.receiveBuff ?? supportValues.supportElement;
            set => supportValues.receiveBuff = value;
        }
        public T Shielding
        {
            get => supportValues.shielding ?? supportValues.supportElement;
            set => supportValues.shielding = value;
        }

        // VITALITY
        public T MaxHealth
        {
            get => vitalityValues.maxHealth ?? vitalityValues.vitalityElement;
            set => vitalityValues.maxHealth = value;
        }
        public T MaxMortality
        {
            get => vitalityValues.maxMortality ?? vitalityValues.vitalityElement;
            set => vitalityValues.maxMortality = value;
        }
        public T DebuffResistance
        {
            get => vitalityValues.debuffResistance ?? vitalityValues.vitalityElement;
            set => vitalityValues.debuffResistance = value;
        }
        public T DamageResistance
        {
            get => vitalityValues.damageResistance ?? vitalityValues.vitalityElement;
            set => vitalityValues.damageResistance = value;
        }

        // CONCENTRATION
        public T InitiativeSpeed
        {
            get => concentrationValues.initiativeSpeed ?? concentrationValues.concentrationElement;
            set => concentrationValues.initiativeSpeed = value;
        }
        public T InitialInitiative
        {
            get => concentrationValues.initialInitiative ?? concentrationValues.concentrationElement;
            set => concentrationValues.initiativeSpeed = value;
        }
        public T ActionsPerSequence
        {
            get => concentrationValues.actionsPerSequence ?? concentrationValues.concentrationElement;
            set => concentrationValues.actionsPerSequence = value;
        }
        public T Critical
        {
            get => concentrationValues.critical ?? concentrationValues.concentrationElement;
            set => concentrationValues.critical = value;
        }

        [Serializable]
        private class OffensiveWrapper : IOffensiveStatsInject<T>, IOffensiveStatsRead<T>
        {
            [Title("Master")]
            public T offensiveElement;

            [Title("SubElement")]
            public T attack;
            public T persistent;
            public T debuff;
            public T followUp;

            public T Attack
            {
                get => attack;
                set => attack = value;
            }

            public T Persistent
            {
                get => persistent;
                set => persistent = value;
            }

            public T Debuff
            {
                get => debuff;
                set => debuff = value;
            }

            public T FollowUp
            {
                get => followUp;
                set => followUp = value;
            }
        }

        [Serializable]
        private class SupportWrapper : ISupportStatsRead<T>, ISupportStatsInject<T>
        {
            [Title("Master")]
            public T supportElement;
            [Title("SubElement")]
            public T heal;
            public T buff;
            public T receiveBuff;
            public T shielding;

            public T Heal
            {
                get => heal;
                set => heal = value;
            }

            public T Buff
            {
                get => buff;
                set => buff = value;
            }

            public T ReceiveBuff
            {
                get => receiveBuff;
                set => receiveBuff = value;
            }

            public T Shielding
            {
                get => shielding;
                set => shielding = value;
            }
        }

        [Serializable]
        private class VitalityWrapper : IVitalityStatsRead<T>, IVitalityStatsInject<T>
        {
            [Title("Master")] 
            public T vitalityElement;

            [Title("SubElements")] 
            public T maxHealth;
            public T maxMortality;
            public T debuffResistance;
            public T damageResistance;

            public T MaxHealth
            {
                get => maxHealth;
                set => maxHealth = value;
            }
            public T MaxMortality
            {
                get => maxMortality;
                set => maxMortality = value;
            }
            public T DebuffResistance
            {
                get => debuffResistance;
                set => debuffResistance = value;
            }
            public T DamageResistance
            {
                get => damageResistance;
                set => damageResistance = value;
            }
        }

        [Serializable]
        private class ConcentrationWrapper : IConcentrationStatsRead<T>, IConcentrationStatsInject<T>
        {
            [Title("Master")] 
            public T concentrationElement;

            [Title("SubElements")] 
            public T initiativeSpeed;
            public T initialInitiative;
            public T actionsPerSequence;
            public T critical;

            public T InitiativeSpeed
            {
                get => initiativeSpeed;
                set => initiativeSpeed = value;
            }

            public T InitialInitiative
            {
                get => initialInitiative;
                set => initialInitiative = value;
            }

            public T ActionsPerSequence
            {
                get => actionsPerSequence;
                set => actionsPerSequence = value;
            }

            public T Critical
            {
                get => critical;
                set => critical = value;
            }
        }
    }
}
