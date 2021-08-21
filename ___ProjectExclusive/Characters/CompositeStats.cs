using System.Collections.Generic;
using Stats;
using UnityEngine;

namespace Characters
{
    public class CompositeStats : ICharacterBasicStatsData
    {
        protected readonly List<IOffensiveStatsData> offensiveStats;
        protected readonly List<ISupportStatsData> supportStats;
        protected readonly List<IVitalityStatsData> vitalityStats;
        protected readonly List<ISpecialStatsData> specialStats;
        protected readonly List<ICombatTemporalStatsBaseData> temporalStats;

        public void Add(ICharacterBasicStats stats)
        {
            offensiveStats.Add(stats);
            supportStats.Add(stats);
            vitalityStats.Add(stats);
            specialStats.Add(stats);
            temporalStats.Add(stats);
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
            offensiveStats = new List<IOffensiveStatsData>();
            supportStats = new List<ISupportStatsData>();
            vitalityStats = new List<IVitalityStatsData>();
            specialStats = new List<ISpecialStatsData>();
            temporalStats = new List<ICombatTemporalStatsBaseData>();
        }

        public CompositeStats(ICharacterBasicStats stats)
        {
            offensiveStats = new List<IOffensiveStatsData>
            {
                stats
            };
            supportStats = new List<ISupportStatsData>
            {
                stats
            };
            vitalityStats = new List<IVitalityStatsData>
            {
                stats
            };
            specialStats = new List<ISpecialStatsData>
            {
                stats
            };
            temporalStats = new List<ICombatTemporalStatsBaseData>
            {
                stats
            };
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
                float calculation = 0;
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
                float calculation = 0;
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
                float calculation = 0;
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
                float calculation = 0;
                foreach (ISupportStatsData supportStats in supportStats)
                {
                    calculation += supportStats.GetHealPower();
                }

                return calculation;
            }

        }
        public float BuffPower
        {
            get
            {
                float calculation = 0;
                foreach (ISupportStatsData supportStats in supportStats)
                {
                    calculation += supportStats.GetBuffPower();
                }

                return calculation;
            }

        }

        public float BuffReceivePower
        {
            get
            {
                float calculation = 0;
                foreach (ISupportStatsData supportStats in supportStats)
                {
                    calculation += supportStats.GetBuffReceivePower();
                }

                return calculation;
            }

        }

        public float MaxHealth
        {
            get
            {
                float calculation = 0;
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
                float calculation = 0;
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
                float calculation = 0;
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
                float calculation = 0;
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
                float calculation = 0;
                foreach (ISpecialStatsData specialStats in specialStats)
                {
                    calculation += specialStats.GetEnlightenment();
                }

                return calculation;
            }

        }
        public float CriticalChance
        {
            get
            {
                float calculation = 0;
                foreach (ISpecialStatsData specialStats in specialStats)
                {
                    calculation += specialStats.GetCriticalChance();
                }

                return calculation;
            }

        }
        public float SpeedAmount
        {
            get
            {
                float calculation = 0;
                foreach (ISpecialStatsData specialStats in specialStats)
                {
                    calculation += specialStats.GetSpeedAmount();
                }

                return calculation;
            }

        }
        public float InitiativePercentage
        {
            get
            {
                float calculation = 0;
                foreach (ICombatTemporalStatsBaseData specialStats in temporalStats)
                {
                    calculation += specialStats.GetInitiativePercentage();
                }

                return calculation;
            }

        }
        public int ActionsPerInitiative
        {
            get
            {
                int calculation = 0;
                foreach (ICombatTemporalStatsBaseData specialStats in temporalStats)
                {
                    calculation += specialStats.GetActionsPerInitiative();
                }

                return calculation;
            }

        }
        public float HarmonyAmount
        {
            get
            {
                float calculation = 0;
                foreach (ICombatTemporalStatsBaseData specialStats in temporalStats)
                {
                    calculation += specialStats.GetHarmonyAmount();
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
