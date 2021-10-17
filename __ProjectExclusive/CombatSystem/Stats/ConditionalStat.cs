using System.Collections.Generic;
using CombatEntity;
using UnityEngine;

namespace Stats
{
    public class ConditionalStat : IBaseStatsRead<float>
    {
        public ConditionalStat()
        {
            _offensiveStats = new Dictionary<IOffensiveStatsRead<float>, IConditionalStatCheck>();
            _supportStats = new Dictionary<ISupportStatsRead<float>, IConditionalStatCheck>();
            _vitalityStats = new Dictionary<IVitalityStatsRead<float>, IConditionalStatCheck>();
            _concentrationStats = new Dictionary<IConcentrationStatsRead<float>, IConditionalStatCheck>();
        }

        private CombatingEntity _user;
        private readonly Dictionary<IOffensiveStatsRead<float>, IConditionalStatCheck> _offensiveStats;
        private readonly Dictionary<ISupportStatsRead<float>, IConditionalStatCheck> _supportStats;
        private readonly Dictionary<IVitalityStatsRead<float>, IConditionalStatCheck> _vitalityStats;
        private readonly Dictionary<IConcentrationStatsRead<float>, IConditionalStatCheck> _concentrationStats;

        public void Injection(CombatingEntity user) => _user = user;

        public void Subscribe(IConditionalStatCheck conditionalStat)
        {
            if(conditionalStat is IOffensiveStatsRead<float> offensiveStats)
                _offensiveStats.Add(offensiveStats,conditionalStat);
            if(conditionalStat is ISupportStatsRead<float> supportStats)
                _supportStats.Add(supportStats,conditionalStat);
            if(conditionalStat is IVitalityStatsRead<float> vitalityStats)
                _vitalityStats.Add(vitalityStats,conditionalStat);
            if(conditionalStat is IConcentrationStatsRead<float> concentrationStats)
                _concentrationStats.Add(concentrationStats,conditionalStat);
        }

        public float Attack
        {
            get
            {
                float value = 0;
                foreach (var pair in _offensiveStats)
                {
                    if (pair.Value.CanBeApplied(_user))
                        value += pair.Key.Attack;
                }

                return value;
            }
        }
        public float Persistent
        {
            get
            {
                float value = 0;
                foreach (var pair in _offensiveStats)
                {
                    if (pair.Value.CanBeApplied(_user))
                        value += pair.Key.Persistent;
                }

                return value;
            }
        }
        public float Debuff
        {
            get
            {
                float value = 0;
                foreach (var pair in _offensiveStats)
                {
                    if (pair.Value.CanBeApplied(_user))
                        value += pair.Key.Debuff;
                }

                return value;
            }
        }
        public float FollowUp
        {
            get
            {
                float value = 0;
                foreach (var pair in _offensiveStats)
                {
                    if (pair.Value.CanBeApplied(_user))
                        value += pair.Key.FollowUp;
                }

                return value;
            }
        }

        public float Heal
        {
            get
            {
                float value = 0;
                foreach (var pair in _supportStats)
                {
                    if (pair.Value.CanBeApplied(_user))
                        value += pair.Key.Heal;
                }

                return value;
            }
        }
        public float Buff
        {
            get
            {
                float value = 0;
                foreach (var pair in _supportStats)
                {
                    if (pair.Value.CanBeApplied(_user))
                        value += pair.Key.Buff;
                }

                return value;
            }
        }
        public float ReceiveBuff
        {
            get
            {
                float value = 0;
                foreach (var pair in _supportStats)
                {
                    if (pair.Value.CanBeApplied(_user))
                        value += pair.Key.ReceiveBuff;
                }

                return value;
            }
        }
        public float Shielding
        {
            get
            {
                float value = 0;
                foreach (var pair in _supportStats)
                {
                    if (pair.Value.CanBeApplied(_user))
                        value += pair.Key.Shielding;
                }

                return value;
            }
        }

        public float MaxHealth
        {
            get
            {
                float value = 0;
                foreach (var pair in _vitalityStats)
                {
                    if (pair.Value.CanBeApplied(_user))
                        value += pair.Key.MaxHealth;
                }

                return value;
            }
        }
        public float MaxMortality
        {
            get
            {
                float value = 0;
                foreach (var pair in _vitalityStats)
                {
                    if (pair.Value.CanBeApplied(_user))
                        value += pair.Key.MaxMortality;
                }

                return value;
            }
        }
        public float DebuffResistance
        {
            get
            {
                float value = 0;
                foreach (var pair in _vitalityStats)
                {
                    if (pair.Value.CanBeApplied(_user))
                        value += pair.Key.DebuffResistance;
                }

                return value;
            }
        }
        public float DamageResistance
        {
            get
            {
                float value = 0;
                foreach (var pair in _vitalityStats)
                {
                    if (pair.Value.CanBeApplied(_user))
                        value += pair.Key.DamageResistance;
                }

                return value;
            }
        }

        public float InitiativeSpeed
        {
            get
            {
                float value = 0;
                foreach (var pair in _concentrationStats)
                {
                    if (pair.Value.CanBeApplied(_user))
                        value += pair.Key.InitiativeSpeed;
                }

                return value;
            }
        }
        public float InitialInitiative
        {
            get
            {
                float value = 0;
                foreach (var pair in _concentrationStats)
                {
                    if (pair.Value.CanBeApplied(_user))
                        value += pair.Key.InitialInitiative;
                }

                return value;
            }
        }
        public float ActionsPerSequence
        {
            get
            {
                float value = 0;
                foreach (var pair in _concentrationStats)
                {
                    if (pair.Value.CanBeApplied(_user))
                        value += pair.Key.ActionsPerSequence;
                }

                return value;
            }
        }
        public float Critical
        {
            get
            {
                float value = 0;
                foreach (var pair in _concentrationStats)
                {
                    if (pair.Value.CanBeApplied(_user))
                        value += pair.Key.Critical;
                }

                return value;
            }
        }
    }

    public interface IConditionalStatCheck
    {
        bool CanBeApplied(CombatingEntity user);
    }
}
