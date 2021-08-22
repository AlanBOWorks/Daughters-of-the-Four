using System.Collections.Generic;
using Stats;
using UnityEngine;

namespace Characters
{
    public class CompositeStats : ICharacterBasicStatsData
    {
        protected readonly CharacterCombatStatsBasic BaseStats;
        protected List<IOffensiveStatsData> offensiveStats;
        protected List<ISupportStatsData> supportStats;
        protected List<IVitalityStatsData> vitalityStats;
        protected List<ISpecialStatsData> specialStats;
        protected List<ICombatTemporalStatsBaseData> temporalStats;

        public void Add(ICharacterBasicStats stats)
        {
            Add(stats as IOffensiveStatsData);
            Add(stats as ISupportStatsData);
            Add(stats as IVitalityStatsData);
            Add(stats as ISpecialStatsData);
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
        public void Add(ISpecialStatsData stats)
        {
            specialStats.Add(stats);
        }
        public void Add(ICombatTemporalStatsBaseData stats)
        {
            temporalStats.Add(stats);
        }

        public CompositeStats()
        {
            BaseStats = new CharacterCombatStatsBasic();
            Initialize();

        }
        public CompositeStats(ICharacterBasicStats stats)
        {
            BaseStats = new CharacterCombatStatsBasic(stats);
            Initialize();
        }

        private void Initialize()
        {
            offensiveStats = new List<IOffensiveStatsData>();
            supportStats = new List<ISupportStatsData>();
            vitalityStats = new List<IVitalityStatsData>();
            temporalStats = new List<ICombatTemporalStatsBaseData>();
            specialStats = new List<ISpecialStatsData>();
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
                float calculation = BaseStats.AttackPower;
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
                float calculation = BaseStats.DeBuffPower;
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
                float calculation = BaseStats.StaticDamagePower;
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
                float calculation = BaseStats.HealPower;
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
                float calculation = BaseStats.BuffPower;
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
                float calculation = BaseStats.BuffReceivePower;
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
                float calculation = BaseStats.MaxHealth;
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
                float calculation = BaseStats.MaxMortalityPoints;
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
                float calculation = BaseStats.DamageReduction;
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
                float calculation = BaseStats.DeBuffReduction;
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
                float calculation = BaseStats.Enlightenment;
                foreach (ISpecialStatsData specialStat in specialStats)
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
                float calculation = BaseStats.CriticalChance;
                foreach (ISpecialStatsData specialStat in specialStats)
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
                float calculation = BaseStats.SpeedAmount;
                foreach (ISpecialStatsData specialStat in specialStats)
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
                float calculation = BaseStats.InitiativePercentage;
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
                int calculation = BaseStats.ActionsPerInitiative;
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
                float calculation = BaseStats.HarmonyAmount;
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
