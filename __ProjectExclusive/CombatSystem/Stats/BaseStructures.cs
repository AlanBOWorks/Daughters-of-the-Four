
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
        [SerializeField] private T attack;
        [SerializeField] private T persistent;
        [SerializeField] private T debuff;
        [Title("Support")]
        [SerializeField] private T heal;
        [SerializeField] private T buff;
        [SerializeField] private T receiveBuff;
        [Title("Vitality")]
        [SerializeField] private T maxHealth;
        [SerializeField] private T maxMortality;
        [SerializeField] private T debuffResistance;
        [SerializeField] private T damageResistance;
        [Title("Concentration")]
        [SerializeField] private T velocity;
        [SerializeField] private T critical;
        [SerializeField] private T initialInitiative;
        [SerializeField] private T actionsPerSequence;

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
        public TPercent Heal => MainStats.Heal;
        public TPercent Buff => MainStats.Buff;
        public TPercent ReceiveBuff => MainStats.ReceiveBuff;
        public TPercent MaxHealth => MainStats.MaxHealth;
        public TPercent MaxMortality => MainStats.MaxMortality;
        public TPercent DebuffResistance => MainStats.DebuffResistance;
        public TPercent DamageResistance => MainStats.DamageResistance;
        public TPercent Critical => MainStats.Critical;
        public TPercent InitiativeSpeed => MainStats.InitiativeSpeed;
        public TPercent InitialInitiative => MainStats.InitialInitiative;
        public TPercent ActionsPerSequence => MainStats.ActionsPerSequence;
    }

    public class ReflectionBaseStat<T> : IBaseStats<T>
    {
        public ReflectionBaseStat(IBaseStats<T> referencedStats)
        {
            ReferencedStats = referencedStats;
        }
        public readonly IBaseStats<T> ReferencedStats;

        public T Attack
        {
            get => ReferencedStats.Attack;
            set => ReferencedStats.Attack = value;
        }
        public T Persistent
        {
            get => ReferencedStats.Persistent;
            set => ReferencedStats.Persistent = value;
        }
        public T Debuff
        {
            get => ReferencedStats.Debuff;
            set => ReferencedStats.Debuff = value;
        }
        public T Heal
        {
            get => ReferencedStats.Heal;
            set => ReferencedStats.Heal = value;
        }
        public T Buff
        {
            get => ReferencedStats.Buff;
            set => ReferencedStats.Buff = value;
        }
        public T ReceiveBuff
        {
            get => ReferencedStats.ReceiveBuff;
            set => ReferencedStats.ReceiveBuff = value;
        }
        public T MaxHealth
        {
            get => ReferencedStats.MaxHealth;
            set => ReferencedStats.MaxHealth = value;
        }
        public T MaxMortality
        {
            get => ReferencedStats.MaxMortality;
            set => ReferencedStats.MaxMortality = value;
        }
        public T DebuffResistance
        {
            get => ReferencedStats.DebuffResistance;
            set => ReferencedStats.DebuffResistance = value;
        }
        public T DamageResistance
        {
            get => ReferencedStats.DamageResistance;
            set => ReferencedStats.DamageResistance = value;
        }
        public T InitiativeSpeed
        {
            get => ReferencedStats.InitiativeSpeed;
            set => ReferencedStats.InitiativeSpeed = value;
        }
        public T Critical
        {
            get => ReferencedStats.Critical;
            set => ReferencedStats.Critical = value;
        }
        public T InitialInitiative
        {
            get => ReferencedStats.InitialInitiative;
            set => ReferencedStats.InitialInitiative = value;
        }
        public T ActionsPerSequence
        {
            get => ReferencedStats.ActionsPerSequence;
            set => ReferencedStats.ActionsPerSequence = value;
        }
    }

    public class StatBehaviourStructure<T> : IBehaviourStatsRead<T>
    {
        public StatBehaviourStructure(T baseStats, T buffStats, T burstStats)
        {
            BaseStats = baseStats;
            BuffStats = buffStats;
            BurstStats = burstStats;
        }

        public T BaseStats { get; }
        public T BuffStats { get; }
        public T BurstStats { get; }
    }

    public class BehaviourCombinedStats<T> : IBaseStatsRead<T>, IBehaviourStatsRead<IBaseStatsRead<T>>
    {
        public BehaviourCombinedStats(IBaseStatsRead<T> baseStats, IBaseStatsRead<T> buffStats, IBaseStatsRead<T> burstStats,
            CompositeAction compositeAction)
        {
            BaseStats = baseStats;
            BuffStats = buffStats;
            BurstStats = burstStats;
            _compositeAction = compositeAction;
        }

        public delegate T CompositeAction(T baseStat, T buffStat, T burstStat);

        private readonly CompositeAction _compositeAction;

        public IBaseStatsRead<T> BaseStats { get; }
        public IBaseStatsRead<T> BuffStats { get; }
        public IBaseStatsRead<T> BurstStats { get; }

        public T Attack => _compositeAction(BaseStats.Attack, BuffStats.Attack, BurstStats.Attack);
        public T Persistent => _compositeAction(BaseStats.Persistent, BuffStats.Persistent, BurstStats.Persistent);
        public T Debuff => _compositeAction(BaseStats.Debuff, BuffStats.Debuff, BurstStats.Debuff);
        public T Heal => _compositeAction(BaseStats.Heal, BuffStats.Heal, BurstStats.Heal);
        public T Buff => _compositeAction(BaseStats.Buff, BuffStats.Buff, BurstStats.Buff);
        public T ReceiveBuff => _compositeAction(BaseStats.ReceiveBuff, BuffStats.ReceiveBuff, BurstStats.ReceiveBuff);
        public T MaxHealth => _compositeAction(BaseStats.MaxHealth, BuffStats.MaxHealth, BurstStats.MaxHealth);
        public T MaxMortality => _compositeAction(BaseStats.MaxMortality, BuffStats.MaxMortality, BurstStats.MaxMortality);
        public T DebuffResistance => _compositeAction(BaseStats.DebuffResistance, BuffStats.DebuffResistance, BurstStats.DebuffResistance);
        public T DamageResistance => _compositeAction(BaseStats.DamageResistance, BuffStats.DamageResistance, BurstStats.DamageResistance);
        public T InitiativeSpeed => _compositeAction(BaseStats.InitiativeSpeed, BuffStats.InitiativeSpeed, BurstStats.InitiativeSpeed);
        public T Critical => _compositeAction(BaseStats.Critical, BuffStats.Critical, BurstStats.Critical);
        public T InitialInitiative => _compositeAction(BaseStats.InitialInitiative, BuffStats.InitialInitiative, BurstStats.InitialInitiative);
        public T ActionsPerSequence => _compositeAction(BaseStats.ActionsPerSequence, BuffStats.ActionsPerSequence, BurstStats.ActionsPerSequence);

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
    public class MasterStats<T> : IMasterStatsInject<T>, IMasterStatsRead<T>
    {
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
