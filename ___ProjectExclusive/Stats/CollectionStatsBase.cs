using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Stats
{
    public class CollectionBasicStatsList<T> : CollectionBasicStats<T>
    {
        public CollectionBasicStatsList()
        {
            _offensiveStats = new List<IOffensiveStatsData<T>>();
            _supportStats = new List<ISupportStatsData<T>>();
            _vitalityStats = new List<IVitalityStatsData<T>>();
            _specialStats = new List<IConcentrationStatsData<T>>();
            _temporalStats = new List<ITemporalStatsData<T>>();
        }

        private readonly List<IOffensiveStatsData<T>> _offensiveStats;
        private readonly List<ISupportStatsData<T>> _supportStats;
        private readonly List<IVitalityStatsData<T>> _vitalityStats;
        private readonly List<IConcentrationStatsData<T>> _specialStats;
        private readonly List<ITemporalStatsData<T>> _temporalStats;


        public override ICollection<IOffensiveStatsData<T>> OffensiveStats => _offensiveStats;
        public override ICollection<ISupportStatsData<T>> SupportStats => _supportStats;
        public override ICollection<IVitalityStatsData<T>> VitalityStats => _vitalityStats;
        public override ICollection<IConcentrationStatsData<T>> ConcentrationStats => _specialStats;
        public override ICollection<ITemporalStatsData<T>> TemporalStats => _temporalStats;
    }
    public class CollectionBasicStatsHashSet<T> : CollectionBasicStats<T>
    {
        public CollectionBasicStatsHashSet()
        {
            offensiveStats = new HashSet<IOffensiveStatsData<T>>();
            supportStats = new HashSet<ISupportStatsData<T>>();
            vitalityStats = new HashSet<IVitalityStatsData<T>>();
            specialStats = new HashSet<IConcentrationStatsData<T>>();
            temporalStats = new HashSet<ITemporalStatsData<T>>();
        }

        protected readonly HashSet<IOffensiveStatsData<T>> offensiveStats;
        protected readonly HashSet<ISupportStatsData<T>> supportStats;
        protected readonly HashSet<IVitalityStatsData<T>> vitalityStats;
        protected readonly HashSet<IConcentrationStatsData<T>> specialStats;
        protected readonly HashSet<ITemporalStatsData<T>> temporalStats;


        public override ICollection<IOffensiveStatsData<T>> OffensiveStats => offensiveStats;
        public override ICollection<ISupportStatsData<T>> SupportStats => supportStats;
        public override ICollection<IVitalityStatsData<T>> VitalityStats => vitalityStats;
        public override ICollection<IConcentrationStatsData<T>> ConcentrationStats => specialStats;
        public override ICollection<ITemporalStatsData<T>> TemporalStats => temporalStats;
    }


    public abstract class CollectionBasicStats<T> : ICollectionStats<T>
    {
        public abstract ICollection<IOffensiveStatsData<T>> OffensiveStats { get; }
        public abstract ICollection<ISupportStatsData<T>> SupportStats { get; }
        public abstract ICollection<IVitalityStatsData<T>> VitalityStats { get; }
        public abstract ICollection<IConcentrationStatsData<T>> ConcentrationStats { get; }
        public abstract ICollection<ITemporalStatsData<T>> TemporalStats { get; }

        public void Add(IBasicStats<T> stats)
            => UtilsStatsCollection.Add(this, stats);

        public void Add(IOffensiveStatsData<T> stats)
        {
            OffensiveStats.Add(stats);
        }
        public void Add(ISupportStatsData<T> stats)
        {
            SupportStats.Add(stats);
        }
        public void Add(IVitalityStatsData<T> stats)
        {
            VitalityStats.Add(stats);
        }
        public void Add(IConcentrationStatsData<T> stats)
        {
            ConcentrationStats.Add(stats);
        }
        public void Add(ITemporalStatsData<T> stats)
        {
            TemporalStats.Add(stats);
        }

        public void Clear()
        {
            OffensiveStats.Clear();
            SupportStats.Clear();
            VitalityStats.Clear();
            ConcentrationStats.Clear();
            TemporalStats.Clear();
        }
    }

    public class DictionaryBasicStats<TKey, TValue> : IDictionaryStats<TKey, TValue>
    {
        public DictionaryBasicStats()
        {
            OffensiveStats = new Dictionary<IOffensiveStatsData<TKey>, TValue>();
            SupportStats = new Dictionary<ISupportStatsData<TKey>, TValue>();
            VitalityStats = new Dictionary<IVitalityStatsData<TKey>, TValue>();
            ConcentrationStats = new Dictionary<IConcentrationStatsData<TKey>, TValue>();
            TemporalStats = new Dictionary<ITemporalStatsData<TKey>, TValue>();
        }

        [ShowInInspector]
        public Dictionary<IOffensiveStatsData<TKey>, TValue> OffensiveStats { get; }
        [ShowInInspector]
        public Dictionary<ISupportStatsData<TKey>, TValue> SupportStats { get; }
        [ShowInInspector]
        public Dictionary<IVitalityStatsData<TKey>, TValue> VitalityStats { get; }
        [ShowInInspector]
        public Dictionary<IConcentrationStatsData<TKey>, TValue> ConcentrationStats { get; }
        [ShowInInspector]
        public Dictionary<ITemporalStatsData<TKey>, TValue> TemporalStats { get; }


        public void Clear()
        {
            OffensiveStats.Clear();
            SupportStats.Clear();
            VitalityStats.Clear();
            ConcentrationStats.Clear();
            TemporalStats.Clear();
        }

        public void Add(IStatsData stats,TValue value)
        {
            switch (stats)
            {
                case IBasicStatsData<TKey> basicStats:
                    Add(basicStats, value);
                    break;
                case IOffensiveStatsData<TKey> offensiveStats:
                    Add(offensiveStats, value);
                    break;
                case ISupportStatsData<TKey> supportStats:
                    Add(supportStats, value);
                    break;
                case IVitalityStatsData<TKey> vitalityStats:
                    Add(vitalityStats, value);
                    break;
                case IConcentrationStatsData<TKey> concentrationStats:
                    Add(concentrationStats, value);
                    break;
                case ITemporalStatsData<TKey> temporalStats:
                    Add(temporalStats, value);
                    break;
            }
        }
        public void Add(IBasicStatsData<TKey> stats, TValue value)
        {
            OffensiveStats.Add(stats, value);
            SupportStats.Add(stats, value);
            VitalityStats.Add(stats, value);
            ConcentrationStats.Add(stats, value);
            TemporalStats.Add(stats, value);
        }

        public void Add(TValue value)
        {
            if (value is IStatsData stats)
                Add(stats,value);
        }

        public void Add(IOffensiveStatsData<TKey> stats, TValue value)
            => OffensiveStats.Add(stats, value);
        public void Add(ISupportStatsData<TKey> stats, TValue value)
            => SupportStats.Add(stats, value);
        public void Add(IVitalityStatsData<TKey> stats, TValue value)
            => VitalityStats.Add(stats, value);
        public void Add(IConcentrationStatsData<TKey> stats, TValue value)
            => ConcentrationStats.Add(stats, value);
        public void Add(ITemporalStatsData<TKey> stats, TValue value)
            => TemporalStats.Add(stats, value);
        public void Remove(IOffensiveStatsData<TKey> stats)
            => OffensiveStats.Remove(stats);
        public void Remove(ISupportStatsData<TKey> stats)
            => SupportStats.Remove(stats);
        public void Remove(IVitalityStatsData<TKey> stats)
            => VitalityStats.Remove(stats);
        public void Remove(IConcentrationStatsData<TKey> stats)
            => ConcentrationStats.Remove(stats);
        public void Remove(ITemporalStatsData<TKey> stats)
            => TemporalStats.Remove(stats);
    }

    public class SerializedHashsetStats<T> : HashSet<IBasicStatsData<T>>
    {

    }
    public class SerializedHashsetStats : SerializedHashsetStats<float>, IBasicStatsData<float>
    {
        public SerializedHashsetStats() : base()
        {}

        public SerializedHashsetStats(IBasicStatsData<float> stats)
            : base()
        {
            Add(stats);
        }
        public float AttackPower
        {
            get
            {
                float value = 0;
                foreach (var stat in this)
                {
                    value += stat.AttackPower;
                }
                return value;
            }
        }
        public float DeBuffPower
        {
            get
            {
                float value = 0;
                foreach (var stat in this)
                {
                    value += stat.DeBuffPower;
                }
                return value;
            }
        }
        public float StaticDamagePower
        {
            get
            {
                float value = 0;
                foreach (var stat in this)
                {
                    value += stat.StaticDamagePower;
                }
                return value;
            }
        }

        public float HealPower
        {
            get
            {
                float value = 0;
                foreach (var stat in this)
                {
                    value += stat.HealPower;
                }
                return value;
            }
        }
        public float BuffPower
        {
            get
            {
                float value = 0;
                foreach (var stat in this)
                {
                    value += stat.BuffPower;
                }
                return value;
            }
        }
        public float BuffReceivePower
        {
            get
            {
                float value = 0;
                foreach (var stat in this)
                {
                    value += stat.BuffReceivePower;
                }
                return value;
            }
        }
        public float MaxHealth
        {
            get
            {
                float value = 0;
                foreach (var stat in this)
                {
                    value += stat.MaxHealth;
                }
                return value;
            }
        }
        public float MaxMortalityPoints
        {
            get
            {
                float value = 0;
                foreach (var stat in this)
                {
                    value += stat.MaxMortalityPoints;
                }
                return value;
            }
        }
        public float DamageReduction
        {
            get
            {
                float value = 0;
                foreach (var stat in this)
                {
                    value += stat.DamageReduction;
                }
                return value;
            }
        }
        public float DeBuffReduction
        {
            get
            {
                float value = 0;
                foreach (var stat in this)
                {
                    value += stat.DeBuffReduction;
                }
                return value;
            }
        }
        public float Enlightenment
        {
            get
            {
                float value = 0;
                foreach (var stat in this)
                {
                    value += stat.Enlightenment;
                }
                return value;
            }
        }
        public float CriticalChance
        {
            get
            {
                float value = 0;
                foreach (var stat in this)
                {
                    value += stat.CriticalChance;
                }
                return value;
            }
        }
        public float SpeedAmount
        {
            get
            {
                float value = 0;
                foreach (var stat in this)
                {
                    value += stat.SpeedAmount;
                }
                return value;
            }
        }
        public float InitiativePercentage
        {
            get
            {
                float value = 0;
                foreach (var stat in this)
                {
                    value += stat.InitiativePercentage;
                }
                return value;
            }
        }
        public float ActionsPerInitiative
        {
            get
            {
                float value = 0;
                foreach (var stat in this)
                {
                    value += stat.ActionsPerInitiative;
                }
                return value;
            }
        }
        public float HarmonyAmount
        {
            get
            {
                float value = 0;
                foreach (var stat in this)
                {
                    value += stat.HarmonyAmount;
                }
                return value;
            }
        }
    }


    public class HashsetStatsHolder : StatsTypeHolderBase<SerializedHashsetStats>, IBasicStatsData<float>
    {
        public HashsetStatsHolder(IStatsHolder<IBasicStats<float>> holder)
            : this(holder.GetBase(), holder.GetBuff(), holder.GetBurst())
        {

        }

        public HashsetStatsHolder(IBasicStatsData<float> baseStats, IBasicStatsData<float> buff, IBasicStatsData<float> burst)
        {
            baseType = new SerializedHashsetStats(baseStats);
            buffType = new SerializedHashsetStats(buff);
            burstType = new SerializedHashsetStats(burst);
        }

        public void Add(IBasicStatsData<float> add, EnumStats.StatsType statsType)
        {
            var holder = UtilsEnumStats.GetStatsHolder(this, statsType);
            holder.Add(add);
        }

        public void Add<T>(IBuffHolder<T> holder) where T : IBasicStatsData<float>
        {
            buffType.Add(holder.GetBuff());
            burstType.Add(holder.GetBurst());
        }


        public float AttackPower =>
            UtilsStats.StatsFormula(
                baseType.AttackPower,
                buffType.AttackPower,
                burstType.AttackPower);

        public float DeBuffPower =>
            UtilsStats.StatsFormula(
                baseType.DeBuffPower,
                buffType.DeBuffPower,
                burstType.DeBuffPower);


        public float StaticDamagePower =>
            UtilsStats.StatsFormula(
                baseType.StaticDamagePower,
                buffType.StaticDamagePower,
                burstType.StaticDamagePower);

        public float HealPower =>
            UtilsStats.StatsFormula(
                baseType.HealPower,
                buffType.HealPower,
                burstType.HealPower);

        public float BuffPower =>
            UtilsStats.StatsFormula(
                baseType.BuffPower,
                buffType.BuffPower,
                burstType.BuffPower);

        public float BuffReceivePower =>
            UtilsStats.StatsFormula(
                baseType.BuffReceivePower,
                buffType.BuffReceivePower,
                burstType.BuffReceivePower);

        public float HarmonyAmount => baseType.HarmonyAmount + burstType.HarmonyAmount;

        // Summatory because initiative percentage will be changed over time only in base, while buff and burst
        // will provide a variation to that stat (eg: a character could have a permanent +20% of initiative each sequence
        // so during the initiative reset the base will be 0% while the buff remains in 20%, thus only needing 80% to act)
        public float InitiativePercentage =>
            baseType.InitiativePercentage + buffType.InitiativePercentage + burstType.InitiativePercentage;

        // Summatory because actions (buff and burst) are added in units to base, percentage doesn't make much difference
        public float ActionsPerInitiative
        {
            get
            {
                Debug.Log($"Base {baseType.ActionsPerInitiative} / Buff {buffType.ActionsPerInitiative} / " +
                          $"Burst {burstType.ActionsPerInitiative}");
                return baseType.ActionsPerInitiative + buffType.ActionsPerInitiative + burstType.ActionsPerInitiative;
            }
        }

        public float Enlightenment =>
            UtilsStats.StatsFormula(
                baseType.Enlightenment,
                buffType.Enlightenment,
                burstType.Enlightenment);

        public float CriticalChance =>
            UtilsStats.StatsFormula(
                baseType.CriticalChance,
                buffType.CriticalChance,
                burstType.CriticalChance);

        public float SpeedAmount =>
            UtilsStats.StatsFormula(
                baseType.SpeedAmount,
                buffType.SpeedAmount,
                burstType.SpeedAmount);

        public float MaxHealth 
            => baseType.MaxHealth + buffType.MaxHealth + burstType.MaxHealth;

        public float MaxMortalityPoints 
            => baseType.MaxMortalityPoints + buffType.MaxHealth + burstType.MaxHealth;


        public float DamageReduction =>
            UtilsStats.StatsFormula(
                baseType.DamageReduction,
                buffType.DamageReduction,
                burstType.DamageReduction);

        public float DeBuffReduction =>
            UtilsStats.StatsFormula(
                baseType.DeBuffReduction,
                buffType.DeBuffReduction,
                burstType.DeBuffReduction);
    }

}
