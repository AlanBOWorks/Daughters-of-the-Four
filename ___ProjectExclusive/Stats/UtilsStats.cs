using System;
using _CombatSystem;
using _Team;
using Characters;
using Skills;
using SMaths;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Stats 
{
    public static class StatsCap
    {
        public const float MinHarmony = -1;
        public const float MaxHarmony = 1;
    }

    public static class EnumStats
    {
        /// <summary>
        /// Wrappers of the subStats: <br></br>
        /// <see cref="Offensive"/>,<see cref="Support"/>,<see cref="Vitality"/>, <see cref="Concentration"/>,
        /// <see cref="HealthCombatStats"/>)
        /// </summary>
        public enum Primordial
        {
            Offensive,
            Support,
            Vitality,
            Concentration,
        }

        public const int AttackIndex = 0;
        public const int DeBuffIndex = AttackIndex + 1;
        public const int StaticDamageIndex = DeBuffIndex + 1;
        public enum Offensive
        {
            Attack = AttackIndex,
            DeBuff = DeBuffIndex,
            StaticPower = StaticDamageIndex
        }

        public const int HealIndex = 10;
        public const int BuffIndex = HealIndex + 1;
        public const int ReceiveBuffIndex = BuffIndex+1;
        public enum Support
        {
            Heal = HealIndex,
            Buff = BuffIndex,
            ReceiveBuffIndex = EnumStats.ReceiveBuffIndex
        }

        public const int MaxHealthIndex = 100;
        public const int MaxMortalityIndex = MaxHealthIndex + 1;
        public const int DamageReductionIndex = MaxMortalityIndex + 1;
        public const int DeBuffReductionIndex = DamageReductionIndex + 1;
        public enum Vitality
        {
            MaxHealth = MaxHealthIndex,
            MaxMortality = MaxMortalityIndex,
            DamageReduction = DamageReductionIndex,
            DeBuffReduction = DeBuffReductionIndex
        }

        public const int EnlightenmentIndex = 1000;
        public const int CriticalIndex = EnlightenmentIndex + 1;
        public const int SpeedIndex = CriticalIndex + 1;
        public enum Concentration
        {
            Enlightenment = EnlightenmentIndex,
            Critical = CriticalIndex,
            Speed = SpeedIndex
        }

        public const int HealthIndex = 10000;
        public const int ShieldIndex = HealthIndex + 1;
        public const int AccumulatedStaticIndex = ShieldIndex + 1; 
        public const int MortalityIndex = AccumulatedStaticIndex + 1;

        public const int HarmonyIndex = 20000 + 1;
        public const int InitiativeIndex = HarmonyIndex + 1;
        public const int ActionsIndex = InitiativeIndex + 1;
        public enum HealthCombatStats
        {
            Health = HealthIndex,
            Shield = ShieldIndex,
            AccumulatedStatic = AccumulatedStaticIndex,
            Mortality = MortalityIndex,
            
        }
        public enum TemporalStats
        {
            Harmony = HarmonyIndex,
            Initiative = InitiativeIndex,
            Actions = ActionsIndex
        }


        public enum StatsType
        {
            Base,
            Buff,
            Burst
        }
    }

    public static class UtilsEnumStats
    {

        public static IBasicStats<float> GetStats(CombatingEntity entity, EnumStats.StatsType statsType)
        {
            var combatStats = entity.CombatStats;
            return GetStats(combatStats, statsType);
        }

        public static IBasicStats<float> GetStats(CombatStatsHolder combatStats, EnumStats.StatsType statsType)
        {
            switch (statsType)
            {
                case EnumStats.StatsType.Base:
                    return combatStats.BaseStats;
                case EnumStats.StatsType.Buff:
                    return combatStats.BuffStats;
                case EnumStats.StatsType.Burst:
                    return combatStats.BurstStats;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statsType), statsType, null);
            }
        }

        /// <summary>
        /// Gets only the [<see cref="EnumStats.StatsType.Buff"/>] || [<see cref="EnumStats.StatsType.Burst"/>]
        /// </summary>
        public static IBasicStats<float> GetBuffOrBurstStats(CombatStatsHolder combatStats, EnumStats.StatsType statsType)
        {
            return statsType == EnumStats.StatsType.Burst 
                ? combatStats.BurstStats 
                : combatStats.BuffStats;
        }


        public static T GetStat<T>(IMasterStats<T> masterStats, EnumStats.Primordial type)
        {
            return type switch
            {
                EnumStats.Primordial.Offensive => masterStats.OffensivePower,
                EnumStats.Primordial.Support => masterStats.SupportPower,
                EnumStats.Primordial.Vitality => masterStats.VitalityAmount,
                EnumStats.Primordial.Concentration => masterStats.ConcentrationAmount,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public static T GetStat<T>(IOffensiveStatsData<T> stats, EnumStats.Offensive type)
        {
            return type switch
            {
                EnumStats.Offensive.Attack => stats.AttackPower,
                EnumStats.Offensive.DeBuff => stats.DeBuffPower,
                EnumStats.Offensive.StaticPower => stats.StaticDamagePower,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public static T GetStat<T>(ISupportStatsData<T> stats, EnumStats.Support type)
        {
            return type switch
            {
                EnumStats.Support.Heal => stats.HealPower,
                EnumStats.Support.Buff => stats.BuffPower,
                EnumStats.Support.ReceiveBuffIndex => stats.BuffReceivePower,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public static T GetStat<T>(IVitalityStatsData<T> stats, EnumStats.Vitality type)
        {
            return type switch
            {
                EnumStats.Vitality.MaxHealth => stats.MaxHealth,
                EnumStats.Vitality.MaxMortality => stats.MaxMortalityPoints,
                EnumStats.Vitality.DamageReduction => stats.DamageReduction,
                EnumStats.Vitality.DeBuffReduction => stats.DeBuffReduction,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public static T GetStat<T>(IConcentrationStatsData<T> stats, EnumStats.Concentration type)
        {
            return type switch
            {
                EnumStats.Concentration.Enlightenment => stats.Enlightenment,
                EnumStats.Concentration.Critical => stats.CriticalChance,
                EnumStats.Concentration.Speed => stats.SpeedAmount,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public static T GetStat<T>(ICombatHealthStatsData<T> stats, EnumStats.HealthCombatStats type)
        {
            switch (type)
            {
                case EnumStats.HealthCombatStats.Health:
                    return stats.HealthPoints;
                case EnumStats.HealthCombatStats.Shield:
                    return stats.ShieldAmount;
                case EnumStats.HealthCombatStats.AccumulatedStatic:
                    return stats.AccumulatedStatic;
                case EnumStats.HealthCombatStats.Mortality:
                    return stats.MortalityPoints;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        public static T GetStat<T>(ITemporalStatsData<T> stats, EnumStats.TemporalStats type)
        {
            switch (type)
            {
                case EnumStats.TemporalStats.Harmony:
                    return stats.HarmonyAmount;
                case EnumStats.TemporalStats.Initiative:
                    return stats.InitiativePercentage;
                case EnumStats.TemporalStats.Actions:
                    return stats.ActionsPerInitiative;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }



    public static class UtilsStats
    {
        public static CombatStatsBasic ZeroValuesBasic = new CombatStatsBasic(0);
        public static CombatStatsFull ZeroValuesFull = new CombatStatsFull(0);

        public static float StatsFormula(float baseStat, float buffStat, float burstStat)
        {
            return baseStat * (1 + buffStat) * (1 + burstStat); //Exponential grow for buff * Burst
        }

        public static float GrowFormula(float baseStat, float growStat, float upgradeAmount)
        {
            return baseStat + growStat * upgradeAmount;
        }

        public static void AddByCheck(IBasicStats<float> injection, IStatsData stats)
        {
            if (stats is IOffensiveStatsData<float> offensiveStats)
                Add(injection, offensiveStats);
            if (stats is ISupportStatsData<float> supportStats)
                Add(injection, supportStats);
            if (stats is IVitalityStatsData<float> vitalityStats)
                Add(injection, vitalityStats);
            if (stats is IConcentrationStatsData<float> concentrationStats)
                Add(injection, concentrationStats);
            if (stats is ITemporalStatsData<float> tempoStats)
                Add(injection, tempoStats);
        }
        public static void Add(IBasicStats<float> injection, IOffensiveStatsData<float> stats)
        {
            injection.AttackPower += stats.AttackPower;
            injection.DeBuffPower += stats.DeBuffPower;
            injection.StaticDamagePower += stats.StaticDamagePower;
        }
        public static void Add(IBasicStats<float> injection, ISupportStatsData<float> stats)
        {
            injection.HealPower += stats.HealPower;
            injection.BuffPower += stats.BuffPower;
            injection.BuffReceivePower += stats.BuffReceivePower;
        }
        public static void Add(IBasicStats<float> injection, IVitalityStatsData<float> stats)
        {
            injection.MaxHealth += stats.MaxHealth;
            injection.MaxMortalityPoints += stats.MaxMortalityPoints;
            injection.DeBuffReduction += stats.DeBuffReduction;
            injection.DamageReduction += stats.DamageReduction;
        }

        public static void Add(IBasicStats<float> injection, IConcentrationStatsData<float> stats)
        {
            injection.SpeedAmount += stats.SpeedAmount;
            injection.CriticalChance += stats.CriticalChance;
            injection.Enlightenment += stats.Enlightenment;
        }

        public static void Add(IBasicStats<float> injection, ITemporalStatsData<float> stats)
        {
            injection.InitiativePercentage += stats.InitiativePercentage;
            injection.ActionsPerInitiative += stats.ActionsPerInitiative;
            injection.HarmonyAmount += stats.HarmonyAmount;
        }

        public static void Add(IBasicStats<float> injection, IBasicStatsData<float> stats)
        {
            Add(injection, stats as IOffensiveStatsData<float>);
            Add(injection, stats as ISupportStatsData<float>);
            Add(injection, stats as IVitalityStatsData<float>);
            Add(injection, stats as IConcentrationStatsData<float>);
            Add(injection, stats as ITemporalStatsData<float>);
        }

        public static void OverrideStats(IOffensiveStatsInjection<float> stats, float value = 0)
        {
            stats.AttackPower = value;
            stats.DeBuffPower = value;
            stats.StaticDamagePower = value;
        }

        public static void OverrideStats(ISupportStatsInjection<float> stats, float value = 0)
        {
            stats.HealPower = value;
            stats.BuffPower = value;
            stats.BuffReceivePower = value;
        }

        public static void OverrideStats(IVitalityStatsInjection<float> stats, float value = 0)
        {
            stats.MaxHealth = value;
            stats.MaxMortalityPoints = value;
            stats.DeBuffReduction = value;
            stats.DamageReduction = value;
        }

        public static void OverrideStats(IConcentrationStatsInjection<float> stats, float value = 0)
        {
            stats.Enlightenment = value;
            stats.CriticalChance = value;
            stats.SpeedAmount = value;
        }

        public static void OverrideStats(IBasicStatsInjection<float> stats, float value = 0)
        {
            OverrideStats(stats as IOffensiveStatsInjection<float>, value);
            OverrideStats(stats as ISupportStatsInjection<float>, value);
            OverrideStats(stats as IVitalityStatsInjection<float>, value);
            OverrideStats(stats as IConcentrationStatsInjection<float>, value);
        }

        public static void OverrideStats(ITemporalStatsInjection<float> stats, float value = 0)
        {
            stats.InitiativePercentage = value;
            stats.HarmonyAmount = value;
            stats.ActionsPerInitiative = Mathf.RoundToInt(value);
        }

        public static void OverrideStats(IFullStatsInjection<float> stats, float value)
        {
            OverrideStats(stats as IBasicStats<float>, value);
            OverrideStats(stats as ITemporalStatsInjection<float>, value);
        }

        public static void CopyStats(IOffensiveStatsInjection<float> injection, IOffensiveStatsData<float> copyFrom)
        {
            injection.AttackPower = copyFrom.AttackPower;
            injection.DeBuffPower = copyFrom.DeBuffPower;
            injection.StaticDamagePower = copyFrom.StaticDamagePower;
        }

        public static void CopyStats(ISupportStatsInjection<float> injection, ISupportStatsData<float> copyFrom)
        {
            injection.HealPower = copyFrom.HealPower;
            injection.BuffPower = copyFrom.BuffPower;
            injection.BuffReceivePower = copyFrom.BuffReceivePower;
        }

        public static void CopyStats(IVitalityStatsInjection<float> injection, IVitalityStatsData<float> copyFrom)
        {
            injection.MaxHealth = copyFrom.MaxHealth;
            injection.MaxMortalityPoints = copyFrom.MaxMortalityPoints;
            injection.DamageReduction = copyFrom.DamageReduction;
            injection.DeBuffReduction = copyFrom.DeBuffReduction;
        }

        public static void CopyStats(IConcentrationStatsInjection<float> injection, IConcentrationStatsData<float> copyFrom)
        {
            injection.Enlightenment = copyFrom.Enlightenment;
            injection.CriticalChance = copyFrom.CriticalChance;
            injection.SpeedAmount = copyFrom.SpeedAmount;
        }


        public static void CopyStats(IBasicStatsInjection<float> injection, IBasicStatsData<float> copyFrom)
        {
            CopyStats(injection as IOffensiveStatsInjection<float>, copyFrom);
            CopyStats(injection as ISupportStatsInjection<float>, copyFrom);
            CopyStats(injection as IVitalityStatsInjection<float>, copyFrom);
            CopyStats(injection as IConcentrationStatsInjection<float>, copyFrom);
            CopyStats(injection as ITemporalStatsInjection<float>, copyFrom);
        }

        public static void CopyStats(ITemporalStatsInjection<float> injection, ITemporalStatsData<float> copyFrom)
        {
            injection.HarmonyAmount = copyFrom.HarmonyAmount;
            injection.InitiativePercentage = copyFrom.InitiativePercentage;
            injection.ActionsPerInitiative = copyFrom.ActionsPerInitiative;
        }

        public static void CopyStats(ICombatHealthStats<float> injection, ICombatHealthStatsData<float> copyFrom)
        {
            injection.HealthPoints = copyFrom.HealthPoints;
            injection.ShieldAmount = copyFrom.ShieldAmount;
            injection.MortalityPoints = copyFrom.MortalityPoints;

        }


        public static void CopyStats(IFullStatsInjection<float> injection, IFullStatsData<float> copyFrom)
        {
            CopyStats(injection as IBasicStatsInjection<float>, copyFrom);
            CopyStats(injection as ICombatHealthStats<float>, copyFrom);
        }

        public static void CopyStats(IStatsUpgradable injection, IStatsUpgradable copyFrom)
        {
            injection.VitalityAmount = copyFrom.VitalityAmount;
            injection.ConcentrationAmount = copyFrom.ConcentrationAmount;
            injection.OffensivePower = copyFrom.OffensivePower;
            injection.SupportPower = copyFrom.SupportPower;
        }

        
        public static CombatStatsHolder GenerateCombatData(IPlayerCharacterStats playerStats)
        {
            var copyStats = new PlayerCombatStats(playerStats);
            return new CombatStatsHolder(copyStats);
        }

        public static void EnqueueStatsBuffEvent(CombatingEntity target)
        {
            //TODO
        }

        private static CharacterEventsTracker GetStatsEvents() => CombatSystemSingleton.CharacterEventsTracker;
        public static void EnqueueTemporalStatsEvent(CombatingEntity target)
            => GetStatsEvents().EnqueueTemporalChangeListener(target);
        public static void EnqueueDamageEvent(CombatingEntity target, float damage)
            => GetStatsEvents().EnqueueOnDamageListener(target, damage);
        public static void EnqueueHealthZeroStatsEvent(CombatingEntity target)
            => GetStatsEvents().EnqueueZeroHealthListener(target);
        public static void EnqueueMortalityZeroEvent(CombatingEntity target)
            => GetStatsEvents().EnqueueZeroMortalityListener(target);
    }

    public static class UtilsCombatStats
    {
        public const int PredictedAmountOfSkillsPerState = 4 + 2 + 1; // 4 Unique + 2 common + 1 Ultimate
        public const int PredictedTotalOfSkills = PredictedAmountOfSkillsPerState * 3; // *3 types of states

        public static float HealthPercentage(CombatingEntity entity)
        {
            var stats = entity.CombatStats;
            return SRange.Percentage(stats.HealthPoints, 0, stats.MaxHealth);
        }

        public static float MortalityPercent(CombatingEntity entity)
        {
            var stats = entity.CombatStats;
            return SRange.Percentage(stats.MortalityPoints, 0, stats.MaxMortalityPoints);
        }


        public static float CalculateFinalDamage(
            CombatStatsHolder attacker,
            CombatStatsHolder receiver, float damageModifier)
        {
            float baseDamage = attacker.CalculateBaseAttackPower() * damageModifier
                               - receiver.CalculateDamageReduction();

            float buffVariation = attacker.BuffStats.AttackPower 
                                  - receiver.BuffStats.DamageReduction;
            float burstVariation = attacker.BurstStats.AttackPower
                                   - receiver.BurstStats.DamageReduction;

            float total = UtilsStats.StatsFormula(baseDamage, buffVariation, burstVariation);
            if (total < 0) total = 0;

            return total;
        }

        public static float CalculateFinalStaticDamage(
            CombatStatsHolder attacker,
            CombatStatsHolder receiver, float damageModifier)
        {
            float baseDamage = attacker.CalculateBaseStaticDamagePower() * damageModifier
                               - receiver.CalculateDamageReduction();

            float buffVariation = attacker.BuffStats.StaticDamagePower
                                  - receiver.BuffStats.DamageReduction;
            float burstVariation = attacker.BurstStats.StaticDamagePower
                                   - receiver.BurstStats.DamageReduction;

            float total = UtilsStats.StatsFormula(baseDamage, buffVariation, burstVariation);
            if (total < 0) total = 0;

            return total;
        }

        public static void DoDamageToShield(CombatingEntity target, float damage)
        {
            IFullStats<float> stats = target.CombatStats.BaseStats;
            float shields = stats.ShieldAmount;

            if (shields <= 0 || damage <= 0) return;

            UtilsStats.EnqueueTemporalStatsEvent(target);
            UtilsStats.EnqueueDamageEvent(target, damage);

            shields -= damage;
            if (shields < 0) shields = 0;
            stats.ShieldAmount = shields;
        }

        /// <returns>The damage left</returns>
        public static void DoDamageTo(CombatingEntity target, float damage)
        {
            if(damage <= 0) return;

            ICombatHealthStats<float> stats = target.CombatStats.BaseStats;

            UtilsStats.EnqueueTemporalStatsEvent(target);
            UtilsStats.EnqueueDamageEvent(target,damage);
            SubmitDamageToEntity();

            if (stats.ShieldAmount > 0)
            {
                stats.ShieldAmount = CalculateDamageOnVitality(stats.ShieldAmount);

                return; // Shield are the counter of 'Raw' damage > absorbs the rest of the damage
            }
            stats.HealthPoints = CalculateDamageOnVitality(stats.HealthPoints);
            if(stats.HealthPoints > 0) return;



            bool teamIsInDanger = target.CharacterGroup.Team.IsInDangerState();
            if (teamIsInDanger)
            {
                // When in danger, characters receive permanent damage (a.k.a. Mortality damage)
                stats.MortalityPoints = CalculateDamageOnVitality(stats.MortalityPoints);
                UtilsStats.EnqueueMortalityZeroEvent(target);
            }
            else
            {
                // If not, just make them KO
                target.CombatStats.TeamData.knockOutHandler.Add(target);
                UtilsStats.EnqueueHealthZeroStatsEvent(target);
            }


            float CalculateDamageOnVitality(float vitalityStat)
            {
                vitalityStat -= damage;
                if (vitalityStat >= 0) damage = 0;
                else
                {
                    damage = -vitalityStat;
                    vitalityStat = 0;
                }

                return vitalityStat;
            }
            void SubmitDamageToEntity()
            {
                target.ReceivedStats.DamageReceived = damage;
            }
        }

        public static void DoStaticDamage(CombatingEntity target, float damage)
        {
            DoDamageToShield(target,damage); //By design StaticDamage counters 'Shields' by dealing damage additionally
            target.CombatStats.AccumulatedStatic += damage;
        }



        public static void DoHealTo(CombatingEntity target, float heal)
        {
            if(!target.IsConscious() || heal < 0) return;
            
            var stats = target.CombatStats;

            float targetHealth = stats.HealthPoints + heal;
            float maxHealth = stats.MaxHealth;
            if (targetHealth > maxHealth)
            {
                target.ReceivedStats.HealReceived = maxHealth - stats.HealthPoints;
                stats.HealthPoints = maxHealth;
            }
            else
            {
                stats.HealthPoints = targetHealth;
                target.ReceivedStats.HealReceived += heal;
            }
            ResetAccumulatedStaticDamage();

            UtilsStats.EnqueueTemporalStatsEvent(target);

            void ResetAccumulatedStaticDamage()
            {
                stats.AccumulatedStatic = 0; //By design Heals counts 'StaticDamage'
            }
        }

        public static void DoOverHealTo(CombatingEntity target,float overHeal)
        {
            var stats = target.CombatStats;
            float currentHealth = stats.HealthPoints;
            float maxHealth = stats.MaxHealth;
            float currentOverHeal = currentHealth - maxHealth;

            // Is inValid?
            if (!target.IsConscious() || overHeal < 0) return;

            // Is not at max HP nor overHealed?
            if(currentOverHeal < 0) return;

            // Is new overHeal lower?
            if(overHeal < currentOverHeal) return;

            stats.HealthPoints = maxHealth + overHeal;

            UtilsStats.EnqueueTemporalStatsEvent(target);
        }


        public static void HealToMax(CombatStatsHolder stats)
        {
            if(!stats.IsAlive()) return;
            float maxHealth = stats.MaxHealth;
            if(stats.HealthPoints < maxHealth) // This is because the target could have overHeal state
                stats.HealthPoints = maxHealth;
        }

        public static void DoGiveShieldsTo(CombatingEntity target, float shieldsAmount)
        {
            if(!target.IsConscious() || shieldsAmount < 0) return;

            var stats = target.CombatStats;
            stats.ShieldAmount += shieldsAmount;

            UtilsStats.EnqueueTemporalStatsEvent(target);
        }


        public static void SetInitiative(IBasicStats<float> stats, float targetValue)
        {
            const float lowerCap = 0;
            const float maxCap = GlobalCombatParams.InitiativeCheck;

            targetValue = Mathf.Clamp(targetValue, lowerCap, maxCap);
            stats.InitiativePercentage = targetValue;
        }
        public static void SetInitiative(CombatingEntity entity, float targetValue, bool isBurstType)
        {
            IBasicStats<float> stats = isBurstType
                ? entity.CombatStats.BurstStats
                : entity.CombatStats.BaseStats;
            SetInitiative(stats, targetValue);

            entity.Events.InvokeTemporalStatChange();
            var tempoHandler = CombatSystemSingleton.TempoHandler;

            tempoHandler.CallUpdateOnInitiativeBar(entity);
            tempoHandler.CheckAndInjectEntityInitiative(entity);
        }
        public static void AddInitiative(CombatingEntity entity, float addition, bool isBurstType)
        {
            var stats = isBurstType 
                ? entity.CombatStats.BurstStats 
                : (IBasicStats<float>) entity.CombatStats.BaseStats;
            SetInitiative(stats, stats.InitiativePercentage + addition);


            entity.Events.InvokeTemporalStatChange();
            var tempoHandler = CombatSystemSingleton.TempoHandler;

            tempoHandler.CallUpdateOnInitiativeBar(entity);
            tempoHandler.CheckAndInjectEntityInitiative(entity);
        }

        public static void SetActionAmount(CombatStatsHolder stats, float targetValue = 0)
        {
            const int lowerCap = GlobalCombatParams.ActionsLowerCap;
            const int maxCap = GlobalCombatParams.ActionsPerInitiativeCap;

            targetValue = Mathf.Clamp(targetValue, lowerCap, maxCap);
            stats.ActionsLefts = Mathf.RoundToInt(targetValue);
        }

        public static void AddActionAmount(CombatStatsHolder stats,float addition = 1)
        {
            SetActionAmount(stats, stats.ActionsLefts + addition);
        }


        public static void AddHarmony(CombatingEntity entity, ITemporalStats<float> stats, float addition)
        {
            float userEnlightenment = entity.CombatStats.Enlightenment; //Generally value = 1;
            addition *= userEnlightenment;

            float targetHarmony = stats.HarmonyAmount + addition;
            targetHarmony = Mathf.Clamp(
                targetHarmony, 
                StatsCap.MinHarmony, StatsCap.MaxHarmony);
            stats.HarmonyAmount = targetHarmony;

            UtilsStats.EnqueueTemporalStatsEvent(entity);
        }
        public static void AddHarmony(CombatingEntity entity, float addition)
            => AddHarmony(entity, entity.CombatStats.BaseStats, addition);
        public static void RemoveHarmony(CombatingEntity user, CombatingEntity target, float reduction)
        {
            var userStats = user.CombatStats;
            var targetStats = target.CombatStats;
            float userEnlightenment = userStats.Enlightenment; //Generally value = 1;
            float targetEnlightenment = targetStats.Enlightenment;
            float harmonyVariation = userEnlightenment - targetEnlightenment;
            reduction += reduction * harmonyVariation;

            float targetHarmony = targetStats.HarmonyAmount;
            targetHarmony -= reduction;
            if (targetHarmony < 0) targetHarmony = 0;
            targetStats.HarmonyAmount = targetHarmony;
        }

        public static void AddTeamControl(CombatingTeam team, float addition)
        {
            CombatSystemSingleton.CombatTeamControlHandler.DoVariation(team,addition);
        }

        public static void DoBurstControl(CombatingTeam team, float burstControl)
        {
            CombatSystemSingleton.CombatTeamControlHandler.DoBurstControl(team,burstControl);
        }

        public static void DoCounterBurstControl(CombatingTeam team, float counterBurst)
        {
            CombatSystemSingleton.CombatTeamControlHandler.DoCounterBurstControl(team,counterBurst);
        }

        public static bool IsCriticalPerformance(IBasicStatsData<float> stats, CombatSkill skill, float criticalCheck)
        {
            if (!skill.CanCrit) return false;
            return criticalCheck < stats.CriticalChance + skill.CriticalAddition;
        }

        public static bool IsCriticalPerformance(IBasicStats<float> stats, SSkillPreset preset, float criticalCheck)
        {
            if (!preset.canCrit) return false;
            return criticalCheck < stats.CriticalChance + preset.criticalAddition;
        }
        public static bool IsCriticalPerformance(SSkillPreset preset, float criticalCheck)
        {
            if (!preset.canCrit) return false;
            return criticalCheck < preset.criticalAddition;
        }

        public const float RandomLow = .8f;
        public const float RandomHigh = 1.2f;

        public static float CalculateRandomModifier(float randomCheck)
        {
            return Mathf.Lerp(RandomLow, RandomHigh, randomCheck);
        }
        public static float UpdateRandomness( ref float randomValue, bool isCritical)
        {
            if (isCritical)
            {
                return RandomHigh;
            }

            randomValue = Random.value;
            return CalculateRandomModifier(randomValue);
        }


        public static float CalculateShieldsPower(IFullStatsData<float> user)
        {
            float shields = user.HealPower + user.BuffPower;
            shields *= .5f; //The average of Heal && buffPower is the Shields power

            return shields;
        }

        public static void VariateBuffUser(IBasicStatsData<float> user, ref float buffValue)
        {
            var userBuffPower = user.BuffPower; //Generally value == 1;
            buffValue *= userBuffPower;
        }
        public static void VariateBuffTarget(IBasicStatsData<float> target, ref float buffValue)
        {
            var targetReceiveBuffPower = target.BuffReceivePower; //Generally value == 0;
            buffValue += buffValue * targetReceiveBuffPower;
        }


    }

    public static class UtilityBuffStats
    {
        public const string BuffTooltip = "Buff";
        public const string BurstTooltip = "Burst";
        public const string DeBuffTooltip = "DeBuff";
        public const string DeBurstToolTip = "DeBurst";

        public static string GetBuffTooltip(bool isBuff)
        {
            return (isBuff) ? BurstTooltip : BuffTooltip;
        }

        public static string GetDeBuffTooltip(bool isBuff)
        {
            return (isBuff) ? DeBuffTooltip : DeBurstToolTip;
        }

        public static string GetTooltip(bool isBuff, bool isPositive)
        {
            return (isPositive) ? GetBuffTooltip(isBuff) : GetDeBuffTooltip(isBuff);
        }
    }
}