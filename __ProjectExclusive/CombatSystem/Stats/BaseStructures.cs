
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class BaseStats<T> : IBaseStats<T>
    {
        [Title("Offensive")]
        [SerializeField, SuffixLabel("u"), LabelWidth(150)] private T attack;
        [SerializeField, SuffixLabel("u"), LabelWidth(150)] private T persistent;
        [SerializeField, SuffixLabel("%"), LabelWidth(150)] private T debuff;
        [SerializeField, SuffixLabel("%"), LabelWidth(150)] private T followUp;
        [Title("Support")]
        [SerializeField, SuffixLabel("u"), LabelWidth(150)] private T heal;
        [SerializeField, SuffixLabel("%"), LabelWidth(150)] private T buff;
        [SerializeField, SuffixLabel("%"), LabelWidth(150)] private T receiveBuff;
        [SerializeField, SuffixLabel("d"), LabelWidth(150)] private T shielding;
        [Title("Vitality")]
        [SerializeField, SuffixLabel("u"), LabelWidth(150)] private T maxHealth;
        [SerializeField, SuffixLabel("u"), LabelWidth(150)] private T maxMortality;
        [SerializeField, SuffixLabel("%"), LabelWidth(150)] private T debuffResistance;
        [SerializeField, SuffixLabel("u"), LabelWidth(150)] private T damageResistance;
        [Title("Concentration")]
        [SerializeField, SuffixLabel("d"), LabelWidth(150)] private T velocity;
        [SerializeField, SuffixLabel("%"), LabelWidth(150)] private T critical;
        [SerializeField, SuffixLabel("%"), LabelWidth(150)] private T initialInitiative;
        [SerializeField, SuffixLabel("d"), LabelWidth(150)] private T actionsPerSequence;

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

        public T InitiativeSpeed
        {
            get => velocity;
            set => velocity = value;
        }

        public T Critical
        {
            get => critical;
            set => critical = value;
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
    }

    /// <typeparam name="TPercent">Generally a float</typeparam>
    /// <typeparam name="TUnit">Generally an integer</typeparam>
    [Serializable]
    public class CombatStats<TPercent,TUnit> :
        IOffensiveStatsRead<TPercent>, ISupportStatsRead<TPercent>, IVitalityStatsRead<TPercent>, IConcentrationStatsRead<TPercent>,
        ICombatPercentStats<TPercent>, ICombatUnitStats<TUnit>
    {
        public CombatStats(IBaseStatsRead<TPercent> mainStatsHolder)
        {
            MainStats = mainStatsHolder;
        }
        protected CombatStats()
        {}
        /// <summary>
        /// It's the stats that gives the data to external entities (through <seealso cref="IBaseStats{T}"/>)
        /// </summary>
        [ShowInInspector]
        protected IBaseStatsRead<TPercent> MainStats { get; set; }

        [ShowInInspector]
        public TPercent CurrentShields { get; set; }
        [ShowInInspector]
        public TPercent CurrentHealth { get; set; }
        [ShowInInspector]
        public TPercent CurrentMortality { get; set; }
        [ShowInInspector]
        public TPercent TickingInitiative { get; set; }
        [ShowInInspector]
        public TUnit CurrentActions { get; set; }



        public TPercent Attack => MainStats.Attack;
        public TPercent Persistent => MainStats.Persistent;
        public TPercent Debuff => MainStats.Debuff;
        public TPercent FollowUp => MainStats.FollowUp;
        public TPercent Heal => MainStats.Heal;
        public TPercent Buff => MainStats.Buff;
        public TPercent ReceiveBuff => MainStats.ReceiveBuff;
        public TPercent Shielding => MainStats.Shielding;
        public TPercent MaxHealth => MainStats.MaxHealth;
        public TPercent MaxMortality => MainStats.MaxMortality;
        public TPercent DebuffResistance => MainStats.DebuffResistance;
        public TPercent DamageResistance => MainStats.DamageResistance;
        public TPercent Critical => MainStats.Critical;
        public TPercent InitiativeSpeed => MainStats.InitiativeSpeed;
        public TPercent InitialInitiative => MainStats.InitialInitiative;
        public TPercent ActionsPerSequence => MainStats.ActionsPerSequence;
    }
    

    public class ListStats<T> : List<IBaseStatsRead<T>>, IBaseStatsRead<T>
    {
        public ListStats(IBaseStatsRead<T> baseStats, ListAction listOperation, int length = 1) : base(length)
        {
            Add(baseStats);
            _listAction = listOperation;
        }
        public delegate T ListAction(T recursionValue, T stat);
        private readonly ListAction _listAction;


        public T Attack
        {
            get
            {
                T amount = this[0].Attack;
                for (var i = 1; i < this.Count; i++)
                {
                    IBaseStatsRead<T> stat = this[i];
                    amount = _listAction(amount, stat.Attack);
                }
                return amount;
            }
        }
        public T Persistent
        {
            get
            {
                T amount = this[0].Persistent;
                for (var i = 1; i < this.Count; i++)
                {
                    IBaseStatsRead<T> stat = this[i];
                    amount = _listAction(amount, stat.Persistent);
                }
                return amount;
            }
        }
        public T Debuff
        {
            get
            {
                T amount = this[0].Debuff;
                for (var i = 1; i < this.Count; i++)
                {
                    IBaseStatsRead<T> stat = this[i];
                    amount = _listAction(amount, stat.Debuff);
                }
                return amount;
            }
        }

        public T FollowUp
        {
            get
            {
                T amount = this[0].FollowUp;
                for (var i = 1; i < this.Count; i++)
                {
                    IBaseStatsRead<T> stat = this[i];
                    amount = _listAction(amount, stat.FollowUp);
                }
                return amount;
            }
        }

        public T Heal
        {
            get
            {
                T amount = this[0].Heal;
                for (var i = 1; i < this.Count; i++)
                {
                    IBaseStatsRead<T> stat = this[i];
                    amount = _listAction(amount, stat.Heal);
                }
                return amount;
            }
        }
        public T Buff
        {
            get
            {
                T amount = this[0].Buff;
                for (var i = 1; i < this.Count; i++)
                {
                    IBaseStatsRead<T> stat = this[i];
                    amount = _listAction(amount, stat.Buff);
                }
                return amount;
            }
        }
        public T ReceiveBuff
        {
            get
            {
                T amount = this[0].ReceiveBuff;
                for (var i = 1; i < this.Count; i++)
                {
                    IBaseStatsRead<T> stat = this[i];
                    amount = _listAction(amount, stat.ReceiveBuff);
                }
                return amount;
            }
        }

        public T Shielding
        {
            get
            {
                T amount = this[0].Shielding;
                for (var i = 1; i < this.Count; i++)
                {
                    IBaseStatsRead<T> stat = this[i];
                    amount = _listAction(amount, stat.Shielding);
                }
                return amount;
            }
        }

        public T MaxHealth
        {
            get
            {
                T amount = this[0].MaxHealth;
                for (var i = 1; i < this.Count; i++)
                {
                    IBaseStatsRead<T> stat = this[i];
                    amount = _listAction(amount, stat.MaxHealth);
                }
                return amount;
            }
        }
        public T MaxMortality
        {
            get
            {
                T amount = this[0].MaxMortality;
                for (var i = 1; i < this.Count; i++)
                {
                    IBaseStatsRead<T> stat = this[i];
                    amount = _listAction(amount, stat.MaxMortality);
                }
                return amount;
            }
        }
        public T DebuffResistance
        {
            get
            {
                T amount = this[0].DebuffResistance;
                for (var i = 1; i < this.Count; i++)
                {
                    IBaseStatsRead<T> stat = this[i];
                    amount = _listAction(amount, stat.DebuffResistance);
                }
                return amount;
            }
        }
        public T DamageResistance
        {
            get
            {
                T amount = this[0].DamageResistance;
                for (var i = 1; i < this.Count; i++)
                {
                    IBaseStatsRead<T> stat = this[i];
                    amount = _listAction(amount, stat.DamageResistance);
                }
                return amount;
            }
        }
        public T InitiativeSpeed
        {
            get
            {
                T amount = this[0].InitiativeSpeed;
                for (var i = 1; i < this.Count; i++)
                {
                    IBaseStatsRead<T> stat = this[i];
                    amount = _listAction(amount, stat.InitiativeSpeed);
                }
                return amount;
            }
        }
        public T Critical
        {
            get
            {
                T amount = this[0].Critical;
                for (var i = 1; i < this.Count; i++)
                {
                    IBaseStatsRead<T> stat = this[i];
                    amount = _listAction(amount, stat.Critical);
                }
                return amount;
            }
        }
        public T InitialInitiative
        {
            get
            {
                T amount = this[0].InitialInitiative;
                for (var i = 1; i < this.Count; i++)
                {
                    IBaseStatsRead<T> stat = this[i];
                    amount = _listAction(amount, stat.InitialInitiative);
                }
                return amount;
            }
        }
        public T ActionsPerSequence
        {
            get
            {
                T amount = this[0].ActionsPerSequence;
                for (var i = 1; i < this.Count; i++)
                {
                    IBaseStatsRead<T> stat = this[i];
                    amount = _listAction(amount, stat.ActionsPerSequence);
                }
                return amount;
            }
        }
    }

    [Serializable]
    public class MasterStats<T> : IMasterStats<T>
    {
        public MasterStats() {}

        public MasterStats(T overrideBy)
        {
            offensive = overrideBy;
            support = overrideBy;
            vitality = overrideBy;
            concentration = overrideBy;
        }

        public MasterStats(IMasterStatsRead<T> injection)
        {
            offensive = injection.Offensive;
            support = injection.Support;
            vitality = injection.Vitality;
            concentration = injection.Concentration;
        }

        [SerializeField] private T offensive;
        [SerializeField] private T support;
        [SerializeField] private T vitality;
        [SerializeField] private T concentration;


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
}
