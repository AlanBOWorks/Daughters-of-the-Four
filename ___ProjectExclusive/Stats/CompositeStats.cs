using System.Collections.Generic;
using Characters;
using Stats;
using UnityEngine;

namespace Stats
{
    public class CompositeStats : IBasicStatsData
    {
        public CompositeStats()
        {
            baseCombatStats = new CombatStatsBasic();
            Initialize();
        }
        public CompositeStats(IBasicStats stats)
        {
            baseCombatStats = new CombatStatsBasic(stats);
            Initialize();
        }

        protected readonly CombatStatsBasic baseCombatStats;
        protected List<IOffensiveStatsData> offensiveStats;
        protected List<ISupportStatsData> supportStats;
        protected List<IVitalityStatsData> vitalityStats;
        protected List<IConcentrationStatsData> specialStats;
        protected List<ICombatTemporalStatsBaseData> temporalStats;

        public void Add(IBasicStats stats)
        {
            Add(stats as IOffensiveStatsData);
            Add(stats as ISupportStatsData);
            Add(stats as IVitalityStatsData);
            Add(stats as IConcentrationStatsData);
            Add(stats as ICombatTemporalStatsBaseData);
        }

        public void Add(IOffensiveStatsData stats)
        {
            offensiveStats.Add(stats);
        }
        public void Add(ISupportStatsData stats)
        {
            supportStats.Add(stats);
        }
        public void Add(IVitalityStatsData stats)
        {
            vitalityStats.Add(stats);
        }
        public void Add(IConcentrationStatsData stats)
        {
            specialStats.Add(stats);
        }
        public void Add(ICombatTemporalStatsBaseData stats)
        {
            temporalStats.Add(stats);
        }

        private void Initialize()
        {
            offensiveStats = new List<IOffensiveStatsData>();
            supportStats = new List<ISupportStatsData>();
            vitalityStats = new List<IVitalityStatsData>();
            temporalStats = new List<ICombatTemporalStatsBaseData>();
            specialStats = new List<IConcentrationStatsData>();
        }

        public void ResetToZero()
        {
            offensiveStats.Clear();
            supportStats.Clear();
            vitalityStats.Clear();
            specialStats.Clear();
            temporalStats.Clear();
        }

        public float AttackPower
        {
            get
            {
                float calculation = baseCombatStats.AttackPower;
                foreach (IOffensiveStatsData offensiveStat in offensiveStats)
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
                float calculation = baseCombatStats.DeBuffPower;
                foreach (IOffensiveStatsData offensiveStat in offensiveStats)
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
                float calculation = baseCombatStats.StaticDamagePower;
                foreach (IOffensiveStatsData offensiveStat in offensiveStats)
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
                float calculation = baseCombatStats.HealPower;
                foreach (ISupportStatsData supportStat in supportStats)
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
                float calculation = baseCombatStats.BuffPower;
                foreach (ISupportStatsData supportStat in supportStats)
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
                float calculation = baseCombatStats.BuffReceivePower;
                foreach (ISupportStatsData supportStat in supportStats)
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
                float calculation = baseCombatStats.MaxHealth;
                foreach (IVitalityStatsData vitalityStat in vitalityStats)
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
                float calculation = baseCombatStats.MaxMortalityPoints;
                foreach (IVitalityStatsData vitalityStat in vitalityStats)
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
                float calculation = baseCombatStats.DamageReduction;
                foreach (IVitalityStatsData vitalityStat in vitalityStats)
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
                float calculation = baseCombatStats.DeBuffReduction;
                foreach (IVitalityStatsData vitalityStat in vitalityStats)
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
                float calculation = baseCombatStats.Enlightenment;
                foreach (IConcentrationStatsData specialStat in specialStats)
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
                float calculation = baseCombatStats.CriticalChance;
                foreach (IConcentrationStatsData specialStat in specialStats)
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
                float calculation = baseCombatStats.SpeedAmount;
                foreach (IConcentrationStatsData specialStat in specialStats)
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
                float calculation = baseCombatStats.InitiativePercentage;
                foreach (ICombatTemporalStatsBaseData specialStat in temporalStats)
                {
                    calculation += specialStat.InitiativePercentage;
                }

                return calculation;
            }

        }
        public int ActionsPerInitiative
        {
            get
            {
                int calculation = baseCombatStats.ActionsPerInitiative;
                foreach (ICombatTemporalStatsBaseData specialStat in temporalStats)
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
                float calculation = baseCombatStats.HarmonyAmount;
                foreach (ICombatTemporalStatsBaseData specialStat in temporalStats)
                {
                    calculation += specialStat.HarmonyAmount;
                }

                return calculation;
            }
        }

    }
}
