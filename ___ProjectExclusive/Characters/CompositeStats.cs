using System.Collections.Generic;
using Stats;
using UnityEngine;

namespace Characters
{
    public class CompositeStats : ICharacterBasicStatsData
    {
        public readonly List<IOffensiveStatsData> OffensiveStats;
        public readonly List<ISupportStatsData> SupportStats;
        public readonly List<IVitalityStatsData> VitalityStats;
        public readonly List<ISpecialStatsData> SpecialStats;
        public readonly List<ICombatTemporalStatsBaseData> TemporalStats;

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
            OffensiveStats = new List<IOffensiveStatsData>();
            SupportStats = new List<ISupportStatsData>();
            VitalityStats = new List<IVitalityStatsData>();
            SpecialStats = new List<ISpecialStatsData>();
            TemporalStats = new List<ICombatTemporalStatsBaseData>();
        }

        public CompositeStats(ICharacterBasicStats stats)
        {
            OffensiveStats = new List<IOffensiveStatsData>
            {
                stats
            };
            SupportStats = new List<ISupportStatsData>
            {
                stats
            };
            VitalityStats = new List<IVitalityStatsData>
            {
                stats
            };
            SpecialStats = new List<ISpecialStatsData>
            {
                stats
            };
            TemporalStats = new List<ICombatTemporalStatsBaseData>
            {
                stats
            };
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
                foreach (IOffensiveStatsData offensiveStat in OffensiveStats)
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
                foreach (IOffensiveStatsData offensiveStat in OffensiveStats)
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
                foreach (IOffensiveStatsData offensiveStat in OffensiveStats)
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
                foreach (ISupportStatsData supportStats in SupportStats)
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
                foreach (ISupportStatsData supportStats in SupportStats)
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
                foreach (ISupportStatsData supportStats in SupportStats)
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
                foreach (IVitalityStatsData vitalityStat in VitalityStats)
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
                foreach (IVitalityStatsData vitalityStat in VitalityStats)
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
                foreach (IVitalityStatsData vitalityStat in VitalityStats)
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
                foreach (IVitalityStatsData vitalityStat in VitalityStats)
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
                foreach (ISpecialStatsData specialStats in SpecialStats)
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
                foreach (ISpecialStatsData specialStats in SpecialStats)
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
                foreach (ISpecialStatsData specialStats in SpecialStats)
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
                foreach (ICombatTemporalStatsBaseData specialStats in TemporalStats)
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
                foreach (ICombatTemporalStatsBaseData specialStats in TemporalStats)
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
                foreach (ICombatTemporalStatsBaseData specialStats in TemporalStats)
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
