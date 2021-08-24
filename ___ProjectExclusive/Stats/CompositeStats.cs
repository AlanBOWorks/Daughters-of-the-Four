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
                    calculation += offensiveStat.GetAttackPower();
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
                    calculation += offensiveStat.GetDeBuffPower();
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
                    calculation += offensiveStat.GetStaticDamagePower();
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
                    calculation += supportStat.GetHealPower();
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
                    calculation += supportStat.GetBuffPower();
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
                    calculation += supportStat.GetBuffReceivePower();
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
                    calculation += vitalityStat.GetMaxHealth();
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
                    calculation += vitalityStat.GetMaxMortalityPoints();
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
                    calculation += vitalityStat.GetDamageReduction();
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
                    calculation += vitalityStat.GetDeBuffReduction();
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
                    calculation += specialStat.GetEnlightenment();
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
                    calculation += specialStat.GetCriticalChance();
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
                    calculation += specialStat.GetSpeedAmount();
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
                    calculation += specialStat.GetInitiativePercentage();
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
                    calculation += specialStat.GetActionsPerInitiative();
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
                    calculation += specialStat.GetHarmonyAmount();
                }

                return calculation;
            }

        }
        public float GetAttackPower() => AttackPower;

        public float GetDeBuffPower() => DeBuffPower;

        public float GetStaticDamagePower() => StaticDamagePower;

        public float GetHealPower() => HealPower;

        public float GetBuffPower() => BuffPower;

        public float GetBuffReceivePower() => BuffReceivePower;

        public float GetMaxHealth() => MaxHealth;

        public float GetMaxMortalityPoints() => MaxMortalityPoints;

        public float GetDamageReduction() => DamageReduction;

        public float GetDeBuffReduction() => DeBuffReduction;

        public float GetEnlightenment() => Enlightenment;

        public float GetCriticalChance() => CriticalChance;

        public float GetSpeedAmount() => SpeedAmount;

        public float GetInitiativePercentage() => InitiativePercentage;

        public int GetActionsPerInitiative() => ActionsPerInitiative;

        public float GetHarmonyAmount() => HarmonyAmount;
    }
}
