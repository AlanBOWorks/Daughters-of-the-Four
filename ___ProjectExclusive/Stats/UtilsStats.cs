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
            DeDuffReduction = DeBuffReductionIndex
        }

        public const int EnlightenmentIndex = 1000;
        public const int CriticalIndex = EnlightenmentIndex + 1;
        public const int SpeedIndex = CriticalIndex + 1;
        public enum Special
        {
            Enlightenment = EnlightenmentIndex,
            Critical = CriticalIndex,
            Speed = SpeedIndex
        }

        public const int HealthIndex = 10000;
        public const int ShieldIndex = HealthIndex + 1;
        public const int AccumulatedStaticIndex = ShieldIndex + 1; 
        public const int MortalityIndex = AccumulatedStaticIndex + 1;
        public const int HarmonyIndex = MortalityIndex + 1;
        public const int InitiativeIndex = HarmonyIndex + 1;
        public const int ActionsIndex = InitiativeIndex + 1;
        public enum TemporalCombatStats
        {
            Health = HealthIndex,
            Shield = ShieldIndex,
            AccumulatedStatic = AccumulatedStaticIndex,
            Mortality = MortalityIndex,
            Harmony = HarmonyIndex,
            Initiative = InitiativeIndex,
            Actions = ActionsIndex
        }
    }

    public static class UtilsEnumStats
    {
        public static float GetStat(IOffensiveStatsData stats, EnumStats.Offensive type)
        {
            return type switch
            {
                EnumStats.Offensive.Attack => stats.GetAttackPower(),
                EnumStats.Offensive.DeBuff => stats.GetDeBuffPower(),
                EnumStats.Offensive.StaticPower => stats.GetStaticDamagePower(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public static float GetStat(ISupportStatsData stats, EnumStats.Support type)
        {
            return type switch
            {
                EnumStats.Support.Heal => stats.GetHealPower(),
                EnumStats.Support.Buff => stats.GetBuffPower(),
                EnumStats.Support.ReceiveBuffIndex => stats.GetBuffReceivePower(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public static float GetStat(IVitalityStatsData stats, EnumStats.Vitality type)
        {
            return type switch
            {
                EnumStats.Vitality.MaxHealth => stats.GetMaxHealth(),
                EnumStats.Vitality.MaxMortality => stats.GetMaxMortalityPoints(),
                EnumStats.Vitality.DamageReduction => stats.GetDamageReduction(),
                EnumStats.Vitality.DeDuffReduction => stats.GetDeBuffReduction(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public static float GetStat(ISpecialStatsData stats, EnumStats.Special type)
        {
            return type switch
            {
                EnumStats.Special.Enlightenment => stats.GetEnlightenment(),
                EnumStats.Special.Critical => stats.GetCriticalChance(),
                EnumStats.Special.Speed => stats.GetSpeedAmount(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public static float GetStat(ICombatTemporalStats stats, EnumStats.TemporalCombatStats type)
        {
            switch (type)
            {
                case EnumStats.TemporalCombatStats.Health:
                    return stats.HealthPoints;
                case EnumStats.TemporalCombatStats.Shield:
                    return stats.ShieldAmount;
                case EnumStats.TemporalCombatStats.AccumulatedStatic:
                    return stats.AccumulatedStatic;
                case EnumStats.TemporalCombatStats.Mortality:
                    return stats.MortalityPoints;
                case EnumStats.TemporalCombatStats.Harmony:
                    return stats.GetHarmonyAmount();
                case EnumStats.TemporalCombatStats.Initiative:
                    return stats.GetInitiativePercentage();
                case EnumStats.TemporalCombatStats.Actions:
                    return stats.GetActionsPerInitiative();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }

    public static class UtilsStats
    {
        public static CharacterCombatStatsBasic ZeroValuesBasic = new CharacterCombatStatsBasic(0);
        public static CharacterCombatStatsFull ZeroValuesFull = new CharacterCombatStatsFull(0);

        public static float StatsFormula(float baseStat, float buffStat, float burstStat)
        {
            return baseStat * (1 + buffStat) * (1 + burstStat); //Exponential grow for buff * Burst
        }

        public static float GrowFormula(float baseStat, float growStat, float upgradeAmount)
        {
            return baseStat + growStat * upgradeAmount;
        }

        public static void Add(ICharacterBasicStats injection, IOffensiveStatsData stats)
        {
            injection.AttackPower = injection.GetAttackPower() + stats.GetAttackPower();
            injection.DeBuffPower = injection.GetDeBuffPower() + stats.GetDeBuffPower();
            injection.StaticDamagePower = injection.GetStaticDamagePower() + stats.GetStaticDamagePower();
        }
        public static void Add(ICharacterBasicStats injection, ISupportStatsData stats)
        {
            injection.HealPower = injection.GetHealPower() + stats.GetHealPower();
            injection.BuffPower = injection.GetBuffPower() + stats.GetBuffPower();
            injection.BuffReceivePower = injection.GetBuffReceivePower() + stats.GetBuffReceivePower();
        }
        public static void Add(ICharacterBasicStats injection, IVitalityStatsData stats)
        {
            injection.MaxHealth = injection.GetMaxHealth() + stats.GetMaxHealth();
            injection.MaxMortalityPoints = injection.GetMaxMortalityPoints() + stats.GetMaxMortalityPoints();
            injection.DeBuffReduction = injection.GetDeBuffReduction() + stats.GetDeBuffReduction();
            injection.DamageReduction = injection.GetDamageReduction() + stats.GetDamageReduction();
        }

        public static void Add(ICharacterBasicStats injection, ISpecialStatsData stats)
        {
            injection.SpeedAmount = injection.GetSpeedAmount() + stats.GetSpeedAmount();
            injection.CriticalChance = injection.GetCriticalChance() + stats.GetCriticalChance();
            injection.Enlightenment = injection.GetEnlightenment() + stats.GetEnlightenment();
        }

        public static void Add(ICharacterBasicStats injection, ICombatTempoStatsData stats)
        {
            injection.InitiativePercentage = injection.GetInitiativePercentage() + stats.GetInitiativePercentage();
            injection.ActionsPerInitiative = injection.GetActionsPerInitiative() + stats.GetActionsPerInitiative();
        }

        public static void Add(ICharacterBasicStats injection, ICharacterBasicStatsData stats)
        {
            Add(injection, stats as IOffensiveStatsData);
            Add(injection, stats as ISupportStatsData);
            Add(injection, stats as IVitalityStatsData);
            Add(injection, stats as ISpecialStatsData);
            Add(injection, stats as ICombatTempoStatsData);
        }

        public static void OverrideStats(IOffensiveStatsInjection stats, float value = 0)
        {
            stats.AttackPower = value;
            stats.DeBuffPower = value;
            stats.StaticDamagePower = value;
        }

        public static void OverrideStats(ISupportStatsInjection stats, float value = 0)
        {
            stats.HealPower = value;
            stats.BuffPower = value;
            stats.BuffReceivePower = value;
        }

        public static void OverrideStats(IVitalityStatsInjection stats, float value = 0)
        {
            stats.MaxHealth = value;
            stats.MaxMortalityPoints = value;
            stats.DeBuffReduction = value;
            stats.DamageReduction = value;
        }

        public static void OverrideStats(ISpecialStatsInjection stats, float value = 0)
        {
            stats.Enlightenment = value;
            stats.CriticalChance = value;
            stats.SpeedAmount = value;
        }

        public static void OverrideStats(ICharacterBasicStats stats, float value = 0)
        {
            OverrideStats(stats as IOffensiveStatsInjection, value);
            OverrideStats(stats as ISupportStatsInjection, value);
            OverrideStats(stats as IVitalityStatsInjection, value);
            OverrideStats(stats as ISpecialStatsInjection, value);
        }

        public static void OverrideStats(ICombatTemporalStatsBaseInjection stats, float value = 0)
        {
            stats.InitiativePercentage = value;
            stats.HarmonyAmount = value;
            stats.ActionsPerInitiative = Mathf.RoundToInt(value);
        }

        public static void OverrideStats(ICharacterFullStats stats, float value)
        {
            OverrideStats(stats as ICharacterBasicStats, value);
            OverrideStats(stats as ICombatTemporalStatsBaseInjection, value);
        }

        public static void CopyStats(IOffensiveStatsInjection injection, IOffensiveStatsData copyFrom)
        {
            injection.AttackPower = copyFrom.GetAttackPower();
            injection.DeBuffPower = copyFrom.GetDeBuffPower();
            injection.StaticDamagePower = copyFrom.GetStaticDamagePower();
        }

        public static void CopyStats(ISupportStatsInjection injection, ISupportStatsData copyFrom)
        {
            injection.HealPower = copyFrom.GetHealPower();
            injection.BuffPower = copyFrom.GetBuffPower();
            injection.BuffReceivePower = copyFrom.GetBuffReceivePower();
        }

        public static void CopyStats(IVitalityStatsInjection injection, IVitalityStatsData copyFrom)
        {
            injection.MaxHealth = copyFrom.GetMaxHealth();
            injection.MaxMortalityPoints = copyFrom.GetMaxMortalityPoints();
            injection.DamageReduction = copyFrom.GetDamageReduction();
            injection.DeBuffReduction = copyFrom.GetDeBuffReduction();
        }

        public static void CopyStats(ISpecialStatsInjection injection, ISpecialStatsData copyFrom)
        {
            injection.Enlightenment = copyFrom.GetEnlightenment();
            injection.CriticalChance = copyFrom.GetCriticalChance();
            injection.SpeedAmount = copyFrom.GetSpeedAmount();
        }


        public static void CopyStats(ICharacterBasicStatsInjection injection, ICharacterBasicStatsData copyFrom)
        {
            CopyStats(injection as IOffensiveStatsInjection, copyFrom);
            CopyStats(injection as ISupportStatsInjection, copyFrom);
            CopyStats(injection as IVitalityStatsInjection, copyFrom);
            CopyStats(injection as ISpecialStatsInjection, copyFrom);
            CopyStats(injection as ICombatTemporalStatsBaseInjection, copyFrom);
        }

        public static void CopyStats(ICombatTemporalStatsBaseInjection injection, ICombatTemporalStatsBaseData copyFrom)
        {
            injection.HarmonyAmount = copyFrom.GetHarmonyAmount();
            injection.InitiativePercentage = copyFrom.GetInitiativePercentage();
            injection.ActionsPerInitiative = copyFrom.GetActionsPerInitiative();
        }

        public static void CopyStats(ICombatTemporalStats injection, ICombatTemporalStats copyFrom)
        {
            injection.HealthPoints = copyFrom.HealthPoints;
            injection.ShieldAmount = copyFrom.ShieldAmount;
            injection.MortalityPoints = copyFrom.MortalityPoints;

        }


        public static void CopyStats(ICharacterFullStats injection, ICharacterFullStats copyFrom)
        {
            CopyStats(injection as ICharacterBasicStatsInjection, copyFrom);
            CopyStats(injection as ICombatTemporalStats, copyFrom);
            
        }

        public static void CopyStats(IStatsUpgradable injection, IStatsUpgradable copyFrom)
        {
            injection.VitalityAmount = copyFrom.VitalityAmount;
            injection.Enlightenment = copyFrom.Enlightenment;
            injection.OffensivePower = copyFrom.OffensivePower;
            injection.SupportPower = copyFrom.SupportPower;
        }

        
        public static CharacterCombatData GenerateCombatData(IPlayerCharacterStats playerStats)
        {
            var copyStats = new PlayerCharacterCombatStats(playerStats);
            return new CharacterCombatData(copyStats);
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
            return SRange.Percentage(stats.HealthPoints, 0, stats.GetMaxHealth());
        }

        public static float MortalityPercent(CombatingEntity entity)
        {
            var stats = entity.CombatStats;
            return SRange.Percentage(stats.MortalityPoints, 0, stats.GetMaxMortalityPoints());
        }


        public static float CalculateFinalDamage(
            CharacterCombatData attacker,
            CharacterCombatData receiver, float damageModifier)
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
            CharacterCombatData attacker,
            CharacterCombatData receiver, float damageModifier)
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
            ICharacterFullStatsData stats = target.CombatStats;
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

            ICombatTemporalStats stats = target.CombatStats;

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
            float maxHealth = stats.GetMaxHealth();
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
            float maxHealth = stats.GetMaxHealth();
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


        public static void HealToMax(CharacterCombatData stats)
        {
            if(!stats.IsAlive()) return;
            float maxHealth = stats.GetMaxHealth();
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


        public static void SetInitiative(ICharacterBasicStats stats, float targetValue)
        {
            const float lowerCap = 0;
            const float maxCap = GlobalCombatParams.InitiativeCheck;

            targetValue = Mathf.Clamp(targetValue, lowerCap, maxCap);
            stats.InitiativePercentage = targetValue;
        }
        public static void SetInitiative(CombatingEntity entity, float targetValue, bool isBurstType)
        {
            ICharacterBasicStats stats = isBurstType
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
                : (ICharacterBasicStats) entity.CombatStats.BaseStats;
            SetInitiative(stats, stats.GetInitiativePercentage() + addition);


            entity.Events.InvokeTemporalStatChange();
            var tempoHandler = CombatSystemSingleton.TempoHandler;

            tempoHandler.CallUpdateOnInitiativeBar(entity);
            tempoHandler.CheckAndInjectEntityInitiative(entity);
        }

        public static void SetActionAmount(CharacterCombatData stats, int targetValue = 0)
        {
            const int lowerCap = GlobalCombatParams.ActionsLowerCap;
            const int maxCap = GlobalCombatParams.ActionsPerInitiativeCap;

            targetValue = Mathf.Clamp(targetValue, lowerCap, maxCap);
            stats.ActionsLefts = targetValue;
        }

        public static void AddActionAmount(CharacterCombatData stats, int addition = 1)
        {
            SetActionAmount(stats, stats.ActionsLefts + addition);
        }


        public static void AddHarmony(CombatingEntity entity, ICombatTemporalStatsBase stats, float addition)
        {
            float userEnlightenment = entity.CombatStats.GetEnlightenment(); //Generally value = 1;
            addition *= userEnlightenment;

            float targetHarmony = stats.GetHarmonyAmount() + addition;
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
            float userEnlightenment = userStats.GetEnlightenment(); //Generally value = 1;
            float targetEnlightenment = targetStats.GetEnlightenment();
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

        public static bool IsCriticalPerformance(ICharacterBasicStatsData stats, CombatSkill skill, float criticalCheck)
        {
            if (!skill.CanCrit) return false;
            return criticalCheck < stats.GetCriticalChance() + skill.CriticalAddition;
        }

        public static bool IsCriticalPerformance(ICharacterBasicStats stats, SSkillPreset preset, float criticalCheck)
        {
            if (!preset.canCrit) return false;
            return criticalCheck < stats.GetCriticalChance() + preset.criticalAddition;
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


        public static float CalculateShieldsPower(ICharacterFullStatsData user)
        {
            float shields = user.GetHealPower() + user.GetBuffPower();
            shields *= .5f; //The average of Heal && buffPower is the Shields power

            return shields;
        }

        public static void VariateBuffUser(ICharacterBasicStatsData user, ref float buffValue)
        {
            var userBuffPower = user.GetBuffPower(); //Generally value == 1;
            buffValue *= userBuffPower;
        }
        public static void VariateBuffTarget(ICharacterBasicStatsData target, ref float buffValue)
        {
            var targetReceiveBuffPower = target.GetBuffReceivePower(); //Generally value == 0;
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