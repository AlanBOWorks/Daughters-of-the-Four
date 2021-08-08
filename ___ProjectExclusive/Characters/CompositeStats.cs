using System.Collections.Generic;
using Stats;
using UnityEngine;

namespace Characters
{
    public class CompositeStats : ICharacterBasicStats
    {
        public readonly List<IOffensiveStats> OffensiveStats;
        public readonly List<ISupportStats> SupportStats;
        public readonly List<IVitalityStats> VitalityStats;
        public readonly List<ISpecialStats> SpecialStats;
        public readonly List<ICombatTemporalStatsBase> TemporalStats;

        public void Add(ICharacterBasicStats stats)
        {
            OffensiveStats.Add(stats);
            SupportStats.Add(stats);
            VitalityStats.Add(stats);
            SpecialStats.Add(stats);
            TemporalStats.Add(stats);
        }

        public CompositeStats()
        {
            OffensiveStats = new List<IOffensiveStats>();
            SupportStats = new List<ISupportStats>();
            VitalityStats = new List<IVitalityStats>();
            SpecialStats = new List<ISpecialStats>();
            TemporalStats = new List<ICombatTemporalStatsBase>();
        }

        public CompositeStats(ICharacterBasicStats stats)
        {
            OffensiveStats = new List<IOffensiveStats>
            {
                stats
            };
            SupportStats = new List<ISupportStats>
            {
                stats
            };
            VitalityStats = new List<IVitalityStats>
            {
                stats
            };
            SpecialStats = new List<ISpecialStats>
            {
                stats
            };
            TemporalStats = new List<ICombatTemporalStatsBase>
            {
                stats
            };
        }



        public float AttackPower
        {
            get
            {
                float calculation = 0;
                foreach (IOffensiveStats offensiveStat in OffensiveStats)
                {
                    calculation += offensiveStat.AttackPower;
                }

                return calculation;
            }
            set {}
        }
        public float DeBuffPower
        {
            get
            {
                float calculation = 0;
                foreach (IOffensiveStats offensiveStat in OffensiveStats)
                {
                    calculation += offensiveStat.DeBuffPower;
                }

                return calculation;
            }
            set { }
        }

        public float StaticDamagePower
        {
            get
            {
                float calculation = 0;
                foreach (IOffensiveStats offensiveStat in OffensiveStats)
                {
                    calculation += offensiveStat.StaticDamagePower;
                }

                return calculation;
            }
            set { }
        }

        public float HealPower
        {
            get
            {
                float calculation = 0;
                foreach (ISupportStats supportStats in SupportStats)
                {
                    calculation += supportStats.HealPower;
                }

                return calculation;
            }
            set { }
        }
        public float BuffPower
        {
            get
            {
                float calculation = 0;
                foreach (ISupportStats supportStats in SupportStats)
                {
                    calculation += supportStats.BuffPower;
                }

                return calculation;
            }
            set { }
        }

        public float BuffReceivePower
        {
            get
            {
                float calculation = 0;
                foreach (ISupportStats supportStats in SupportStats)
                {
                    calculation += supportStats.BuffReceivePower;
                }

                return calculation;
            }
            set { }
        }

        public float MaxHealth
        {
            get
            {
                float calculation = 0;
                foreach (IVitalityStats vitalityStat in VitalityStats)
                {
                    calculation += vitalityStat.MaxHealth;
                }

                return calculation;
            }
            set { }
        }
        public float MaxMortalityPoints
        {
            get
            {
                float calculation = 0;
                foreach (IVitalityStats vitalityStat in VitalityStats)
                {
                    calculation += vitalityStat.MaxMortalityPoints;
                }

                return calculation;
            }
            set { }
        }
        public float DamageReduction
        {
            get
            {
                float calculation = 0;
                foreach (IVitalityStats vitalityStat in VitalityStats)
                {
                    calculation += vitalityStat.DamageReduction;
                }

                return calculation;
            }
            set { }
        }
        public float DeBuffReduction
        {
            get
            {
                float calculation = 0;
                foreach (IVitalityStats vitalityStat in VitalityStats)
                {
                    calculation += vitalityStat.DeBuffReduction;
                }

                return calculation;
            }
            set { }
        }


        public float Enlightenment
        {
            get
            {
                float calculation = 0;
                foreach (ISpecialStats specialStats in SpecialStats)
                {
                    calculation += specialStats.Enlightenment;
                }

                return calculation;
            }
            set { }
        }
        public float CriticalChance
        {
            get
            {
                float calculation = 0;
                foreach (ISpecialStats specialStats in SpecialStats)
                {
                    calculation += specialStats.CriticalChance;
                }

                return calculation;
            }
            set { }
        }
        public float SpeedAmount
        {
            get
            {
                float calculation = 0;
                foreach (ISpecialStats specialStats in SpecialStats)
                {
                    calculation += specialStats.SpeedAmount;
                }

                return calculation;
            }
            set { }
        }
        public float InitiativePercentage
        {
            get
            {
                float calculation = 0;
                foreach (ICombatTemporalStatsBase specialStats in TemporalStats)
                {
                    calculation += specialStats.InitiativePercentage;
                }

                return calculation;
            }
            set { }
        }
        public int ActionsPerInitiative
        {
            get
            {
                int calculation = 0;
                foreach (ICombatTemporalStatsBase specialStats in TemporalStats)
                {
                    calculation += specialStats.ActionsPerInitiative;
                }

                return calculation;
            }
            set { }
        }
        public float HarmonyAmount
        {
            get
            {
                float calculation = 0;
                foreach (ICombatTemporalStatsBase specialStats in TemporalStats)
                {
                    calculation += specialStats.HarmonyAmount;
                }

                return calculation;
            }
            set { }
        }
    }
}
