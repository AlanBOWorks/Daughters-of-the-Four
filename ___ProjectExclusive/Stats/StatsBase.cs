using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class MasterStats<T> : IMasterStats<T>
    {
        [SerializeField, TitleGroup("Offensive")]
        private T offensivePower;
        [SerializeField, TitleGroup("Support")]
        private T supportPower;
        [SerializeField, TitleGroup("Vitality")]
        private T vitalityAmount;
        [SerializeField, TitleGroup("Concentration")]
        private T concentrationAmount;

        public T OffensivePower
        {
            get => offensivePower;
            set => offensivePower = value;
        }

        public T SupportPower
        {
            get => supportPower;
            set => supportPower = value;
        }

        public T VitalityAmount
        {
            get => vitalityAmount;
            set => vitalityAmount = value;
        }

        public T ConcentrationAmount
        {
            get => concentrationAmount;
            set => concentrationAmount = value;
        }
    }

    [Serializable]
    public class OffensiveStats<T> : IOffensiveStats<T>
    {
        [SerializeField, SuffixLabel("u|%%"), Tooltip("Base is Unit; Buff is percent")]
        private T attackPower;
        [SerializeField, SuffixLabel("%00")]
        private T deBuffPower;

        [SerializeField, SuffixLabel("Units")]
        private T staticDamagePower;
        public T AttackPower
        {
            get => attackPower;
            set => attackPower = value;
        }

        public T DeBuffPower
        {
            get => deBuffPower;
            set => deBuffPower = value;
        }

        public T StaticDamagePower
        {
            get => staticDamagePower;
            set => staticDamagePower = value;
        }

    }
    [Serializable]
    public class SupportStats<T> : ISupportStats<T>
    {
        [SerializeField, SuffixLabel("u|%%"), Tooltip("Base is Unit; Buff is percent")]
        private T healPower;
        [SerializeField, SuffixLabel("%00")]
        private T buffPower;

        [SerializeField, SuffixLabel("%00(Add)")]
        private T buffReceivePower;

        public T HealPower
        {
            get => healPower;
            set => healPower = value;
        }

        public T BuffPower
        {
            get => buffPower;
            set => buffPower = value;
        }

        public T BuffReceivePower
        {
            get => buffReceivePower;
            set => buffReceivePower = value;
        }
    }
    [Serializable]
    public class VitalityStats<T> : IVitalityStats<T>
    {
        [SerializeField, SuffixLabel("Units")]
        private T maxHealth;
        [SerializeField, SuffixLabel("Units")]
        private T maxMortalityPoints;

        [SerializeField, SuffixLabel("u|%%"), Tooltip("Base is Unit; Buff is percent")]
        private T damageReduction;
        [SerializeField, SuffixLabel("%00"), Tooltip("Counters DeBuffPower")]
        private T deBuffReduction;

        public T MaxHealth
        {
            get => maxHealth;
            set => maxHealth = value;
        }

        public T MaxMortalityPoints
        {
            get => maxMortalityPoints;
            set => maxMortalityPoints = value;
        }

        public T DamageReduction
        {
            get => damageReduction;
            set => damageReduction = value;
        }

        public T DeBuffReduction
        {
            get => deBuffReduction;
            set => deBuffReduction = value;
        }
    }
    [Serializable]
    public class ConcentrationStats<T> : IConcentrationStats<T>
    {
        [SerializeField, SuffixLabel("%00"), Tooltip("Affects Harmony gain")]
        private T enlightenment; // before fight this could be modified
        [SerializeField, SuffixLabel("%00")]
        private T criticalChance;
        [SerializeField, SuffixLabel("u|%%"), Tooltip("[100] is the default value")]
        private T speedAmount;
        public T Enlightenment
        {
            get => enlightenment;
            set => enlightenment = value;
        }

        public T CriticalChance
        {
            get => criticalChance;
            set => criticalChance = value;
        }

        public T SpeedAmount
        {
            get => speedAmount;
            set => speedAmount = value;
        }
    }

    [Serializable]
    public class TemporalStats<T> : ITemporalStats<T>
    {

        [SerializeField, SuffixLabel("%00")]
        private T initiativePercentage;
        [SerializeField, SuffixLabel("Units")]
        private T actionsPerInitiative;
        [SerializeField, SuffixLabel("%00")]
        private T harmonyAmount;
        public T InitiativePercentage
        {
            get => initiativePercentage;
            set => initiativePercentage = value;
        }

        public T ActionsPerInitiative
        {
            get => actionsPerInitiative;
            set => actionsPerInitiative = value;
        }

        public T HarmonyAmount
        {
            get => harmonyAmount;
            set => harmonyAmount = value;
        }
    }

    [Serializable]
    public abstract class SerializableHolderStats<T> : MasterStats<T>, IBasicStatsHolderData<T>
    {
        [SerializeField, TitleGroup("Offensive")] 
        private OffensiveStats<T> offensiveStats = new OffensiveStats<T>();
        [SerializeField, TitleGroup("Support")]
        private SupportStats<T> supportStats = new SupportStats<T>();
        [SerializeField, TitleGroup("Vitality")]
        private VitalityStats<T> vitalityStats = new VitalityStats<T>();
        [SerializeField, TitleGroup("Concentration")]
        private ConcentrationStats<T> concentrationStats = new ConcentrationStats<T>();
        [SerializeField, TitleGroup("Temporal")]
        private TemporalStats<T> temporalStats = new TemporalStats<T>();

        public IOffensiveStatsData<T> OffensiveStats => offensiveStats;
        public ISupportStatsData<T> SupportStats => supportStats;
        public IVitalityStatsData<T> VitalityStats => vitalityStats;
        public IConcentrationStatsData<T> ConcentrationStats => concentrationStats;
        public ITemporalStatsData<T> TemporalStats => temporalStats;
    }


    public abstract class CompositeBasicStats<T> : IBasicStatsHolder<T>, IBasicStatsData<T>
    {
        public abstract IOffensiveStats<T> OffensiveStats { get; set; }
        public abstract ISupportStats<T> SupportStats { get; set; }
        public abstract IVitalityStats<T> VitalityStats { get; set; }
        public abstract IConcentrationStats<T> ConcentrationStats { get; set; }
        public abstract ITemporalStats<T> TemporalStats { get; set; }


        public T AttackPower => OffensiveStats.AttackPower;
        public T DeBuffPower => OffensiveStats.DeBuffPower;
        public T StaticDamagePower => OffensiveStats.StaticDamagePower;

        public T HealPower => SupportStats.HealPower;
        public T BuffPower => SupportStats.BuffPower;
        public T BuffReceivePower => SupportStats.BuffReceivePower;

        public T MaxHealth => VitalityStats.MaxHealth;
        public T MaxMortalityPoints => VitalityStats.MaxMortalityPoints;
        public T DamageReduction => VitalityStats.DamageReduction;
        public T DeBuffReduction => VitalityStats.DeBuffReduction;

        public T Enlightenment => ConcentrationStats.Enlightenment;
        public T CriticalChance => ConcentrationStats.CriticalChance;
        public T SpeedAmount => ConcentrationStats.SpeedAmount;

        public T InitiativePercentage => TemporalStats.InitiativePercentage;
        public T ActionsPerInitiative => TemporalStats.ActionsPerInitiative;
        public T HarmonyAmount => TemporalStats.HarmonyAmount;
    }



    public abstract class CollectionBasicStats<T>
    {
        public abstract ICollection<IOffensiveStatsData<T>> OffensiveStats { get; protected set; }
        public abstract ICollection<ISupportStatsData<T>> SupportStats { get; protected set; }
        public abstract ICollection<IVitalityStatsData<T>> VitalityStats { get; protected set; }
        public abstract ICollection<IConcentrationStatsData<T>> SpecialStats { get; protected set; }
        public abstract ICollection<ITemporalStatsData<T>> TemporalStats { get; protected set; }
    }

    public abstract class CollectionBasicStats : CollectionBasicStats<float>
    {
        public void Add(IBasicStats<float> stats)
        {
            Add(stats as IOffensiveStatsData<float>);
            Add(stats as ISupportStatsData<float>);
            Add(stats as IVitalityStatsData<float>);
            Add(stats as IConcentrationStatsData<float>);
            Add(stats as ITemporalStatsData<float>);
        }

        public void Add(IOffensiveStatsData<float> stats)
        {
            OffensiveStats.Add(stats);
        }
        public void Add(ISupportStatsData<float> stats)
        {
            SupportStats.Add(stats);
        }
        public void Add(IVitalityStatsData<float> stats)
        {
            VitalityStats.Add(stats);
        }
        public void Add(IConcentrationStatsData<float> stats)
        {
            SpecialStats.Add(stats);
        }
        public void Add(ITemporalStatsData<float> stats)
        {
            TemporalStats.Add(stats);
        }


        public void ResetToZero()
        {
            OffensiveStats.Clear();
            SupportStats.Clear();
            VitalityStats.Clear();
            SpecialStats.Clear();
            TemporalStats.Clear();
        }

        public float AttackPower
        {
            get
            {
                float calculation = 0;
                foreach (IOffensiveStatsData<float> offensiveStat in OffensiveStats)
                {
                    calculation += offensiveStat.AttackPower;
                }

                return calculation;
            }

        }
        public float DeBuffPower
        {
            get
            {
                float calculation = 0;
                foreach (IOffensiveStatsData<float> offensiveStat in OffensiveStats)
                {
                    calculation += offensiveStat.DeBuffPower;
                }

                return calculation;
            }

        }

        public float StaticDamagePower
        {
            get
            {
                float calculation = 0;
                foreach (IOffensiveStatsData<float> offensiveStat in OffensiveStats)
                {
                    calculation += offensiveStat.StaticDamagePower;
                }

                return calculation;
            }

        }

        public float HealPower
        {
            get
            {
                float calculation = 0;
                foreach (ISupportStatsData<float> supportStat in SupportStats)
                {
                    calculation += supportStat.HealPower;
                }

                return calculation;
            }

        }
        public float BuffPower
        {
            get
            {
                float calculation = 0;
                foreach (ISupportStatsData<float> supportStat in SupportStats)
                {
                    calculation += supportStat.BuffPower;
                }

                return calculation;
            }

        }

        public float BuffReceivePower
        {
            get
            {
                float calculation = 0;
                foreach (ISupportStatsData<float> supportStat in SupportStats)
                {
                    calculation += supportStat.BuffReceivePower;
                }

                return calculation;
            }

        }

        public float MaxHealth
        {
            get
            {
                float calculation = 0;
                foreach (IVitalityStatsData<float> vitalityStat in VitalityStats)
                {
                    calculation += vitalityStat.MaxHealth;
                }

                return calculation;
            }

        }
        public float MaxMortalityPoints
        {
            get
            {
                float calculation = 0;
                foreach (IVitalityStatsData<float> vitalityStat in VitalityStats)
                {
                    calculation += vitalityStat.MaxMortalityPoints;
                }

                return calculation;
            }

        }
        public float DamageReduction
        {
            get
            {
                float calculation = 0;
                foreach (IVitalityStatsData<float> vitalityStat in VitalityStats)
                {
                    calculation += vitalityStat.DamageReduction;
                }

                return calculation;
            }

        }
        public float DeBuffReduction
        {
            get
            {
                float calculation = 0;
                foreach (IVitalityStatsData<float> vitalityStat in VitalityStats)
                {
                    calculation += vitalityStat.DeBuffReduction;
                }

                return calculation;
            }

        }


        public float Enlightenment
        {
            get
            {
                float calculation = 0;
                foreach (IConcentrationStatsData<float> specialStat in SpecialStats)
                {
                    calculation += specialStat.Enlightenment;
                }

                return calculation;
            }

        }
        public float CriticalChance
        {
            get
            {
                float calculation = 0;
                foreach (IConcentrationStatsData<float> specialStat in SpecialStats)
                {
                    calculation += specialStat.CriticalChance;
                }

                return calculation;
            }

        }
        public float SpeedAmount
        {
            get
            {
                float calculation = 0;
                foreach (IConcentrationStatsData<float> specialStat in SpecialStats)
                {
                    calculation += specialStat.SpeedAmount;
                }

                return calculation;
            }

        }
        public float InitiativePercentage
        {
            get
            {
                float calculation = 0;
                foreach (ITemporalStatsData<float> specialStat in TemporalStats)
                {
                    calculation += specialStat.InitiativePercentage;
                }

                return calculation;
            }

        }
        public float ActionsPerInitiative
        {
            get
            {
                float calculation = 0;
                foreach (ITemporalStatsData<float> specialStat in TemporalStats)
                {
                    calculation += specialStat.ActionsPerInitiative;
                }

                return calculation;
            }

        }
        public float HarmonyAmount
        {
            get
            {
                float calculation = 0;
                foreach (ITemporalStatsData<float> specialStat in TemporalStats)
                {
                    calculation += specialStat.HarmonyAmount;
                }

                return calculation;
            }
        }
    }
}
