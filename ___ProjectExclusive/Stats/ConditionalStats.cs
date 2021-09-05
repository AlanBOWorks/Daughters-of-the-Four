using Characters;

namespace Stats
{
    public class ConditionalStats<T> : DictionaryBasicStats<T,IConditionalStat>
    {
        public ConditionalStats(CombatingEntity user) : base()
        {
            User = user;
        }
        protected readonly CombatingEntity User;
    }

    public class ConditionalStats : ConditionalStats<float>, IBasicStatsData<float>
    {
        public ConditionalStats(CombatingEntity user) : base(user)
        { }

        public float AttackPower
        {
            get
            {
                float value = 0;
                foreach (var pair in OffensiveStats)
                {
                    if (pair.Value.CanBeUsed(User))
                        value += pair.Key.AttackPower;
                }
                return value;
            }
        }
        public float DeBuffPower
        {
            get
            {
                float value = 0;
                foreach (var pair in OffensiveStats)
                {
                    if (pair.Value.CanBeUsed(User))
                        value += pair.Key.DeBuffPower;
                }
                return value;
            }
        }
        public float StaticDamagePower
        {
            get
            {
                float value = 0;
                foreach (var pair in OffensiveStats)
                {
                    if (pair.Value.CanBeUsed(User))
                        value += pair.Key.StaticDamagePower;
                }
                return value;
            }
        }

        public float HealPower
        {
            get
            {
                float value = 0;
                foreach (var pair in SupportStats)
                {
                    if (pair.Value.CanBeUsed(User))
                        value += pair.Key.HealPower;
                }
                return value;
            }
        }
        public float BuffPower
        {
            get
            {
                float value = 0;
                foreach (var pair in SupportStats)
                {
                    if (pair.Value.CanBeUsed(User))
                        value += pair.Key.BuffPower;
                }
                return value;
            }
        }
        public float BuffReceivePower
        {
            get
            {
                float value = 0;
                foreach (var pair in SupportStats)
                {
                    if (pair.Value.CanBeUsed(User))
                        value += pair.Key.BuffReceivePower;
                }
                return value;
            }
        }
        public float MaxHealth
        {
            get
            {
                float value = 0;
                foreach (var pair in VitalityStats)
                {
                    if (pair.Value.CanBeUsed(User))
                        value += pair.Key.MaxHealth;
                }
                return value;
            }
        }
        public float MaxMortalityPoints
        {
            get
            {
                float value = 0;
                foreach (var pair in VitalityStats)
                {
                    if (pair.Value.CanBeUsed(User))
                        value += pair.Key.MaxMortalityPoints;
                }
                return value;
            }
        }
        public float DamageReduction
        {
            get
            {
                float value = 0;
                foreach (var pair in VitalityStats)
                {
                    if (pair.Value.CanBeUsed(User))
                        value += pair.Key.DamageReduction;
                }
                return value;
            }
        }
        public float DeBuffReduction
        {
            get
            {
                float value = 0;
                foreach (var pair in VitalityStats)
                {
                    if (pair.Value.CanBeUsed(User))
                        value += pair.Key.DeBuffReduction;
                }
                return value;
            }
        }
        public float DisruptionResistance
        {
            get
            {
                float value = 0;
                foreach (var pair in ConcentrationStats)
                {
                    if (pair.Value.CanBeUsed(User))
                        value += pair.Key.DisruptionResistance;
                }
                return value;
            }
        }
        public float CriticalChance
        {
            get
            {
                float value = 0;
                foreach (var pair in ConcentrationStats)
                {
                    if (pair.Value.CanBeUsed(User))
                        value += pair.Key.CriticalChance;
                }
                return value;
            }
        }
        public float SpeedAmount
        {
            get
            {
                float value = 0;
                foreach (var pair in ConcentrationStats)
                {
                    if (pair.Value.CanBeUsed(User))
                        value += pair.Key.SpeedAmount;
                }
                return value;
            }
        }
        public float InitiativePercentage
        {
            get
            {
                float value = 0;
                foreach (var pair in TemporalStats)
                {
                    if (pair.Value.CanBeUsed(User))
                        value += pair.Key.InitiativePercentage;
                }
                return value;
            }
        }
        public float ActionsPerInitiative
        {
            get
            {
                float value = 0;
                foreach (var pair in TemporalStats)
                {
                    if (pair.Value.CanBeUsed(User))
                        value += pair.Key.ActionsPerInitiative;
                }
                return value;
            }
        }
        public float HarmonyAmount
        {
            get
            {
                float value = 0;
                foreach (var pair in TemporalStats)
                {
                    if (pair.Value.CanBeUsed(User))
                        value += pair.Key.HarmonyAmount;
                }
                return value;
            }
        }
    }
}
