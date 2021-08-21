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
        public enum Combat
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

        public static void OverrideStats(IOffensiveStats stats, float value = 0)
        {
            stats.AttackPower = value;
            stats.DeBuffPower = value;
            stats.StaticDamagePower = value;
        }

        public static void OverrideStats(ISupportStats stats, float value = 0)
        {
            stats.HealPower = value;
            stats.BuffPower = value;
            stats.BuffReceivePower = value;
        }

        public static void OverrideStats(IVitalityStats stats, float value = 0)
        {
            stats.MaxHealth = value;
            stats.MaxMortalityPoints = value;
            stats.DeBuffReduction = value;
            stats.DamageReduction = value;
        }

        public static void OverrideStats(ISpecialStats stats, float value = 0)
        {
            stats.Enlightenment = value;
            stats.CriticalChance = value;
            stats.SpeedAmount = value;
        }

        public static void OverrideStats(ICharacterBasicStats stats, float value = 0)
        {
            OverrideStats(stats as IOffensiveStats, value);
            OverrideStats(stats as ISupportStats, value);
            OverrideStats(stats as IVitalityStats, value);
            OverrideStats(stats as ISpecialStats, value);
        }

        public static void OverrideStats(ICombatTemporalStatsBase stats, float value = 0)
        {
            stats.InitiativePercentage = value;
            stats.HarmonyAmount = value;
            stats.ActionsPerInitiative = Mathf.RoundToInt(value);
        }

        public static void OverrideStats(ICharacterFullStats stats, float value)
        {
            OverrideStats(stats as ICharacterBasicStats,value);
            OverrideStats(stats as ICombatTemporalStatsBase,value);
        }

        public static void CopyStats(IOffensiveStats injection, IOffensiveStats copyFrom)
        {
            injection.AttackPower = copyFrom.AttackPower;
            injection.DeBuffPower = copyFrom.DeBuffPower;
            injection.StaticDamagePower = copyFrom.StaticDamagePower;
        }

        public static void CopyStats(ISupportStats injection, ISupportStats copyFrom)
        {
            injection.HealPower = copyFrom.HealPower;
            injection.BuffPower = copyFrom.BuffPower;
            injection.BuffReceivePower = copyFrom.BuffReceivePower;
        }

        public static void CopyStats(IVitalityStats injection, IVitalityStats copyFrom)
        {
            injection.MaxHealth = copyFrom.MaxHealth;
            injection.MaxMortalityPoints = copyFrom.MaxMortalityPoints;
            injection.DamageReduction = copyFrom.DamageReduction;
            injection.DeBuffReduction = copyFrom.DeBuffReduction;
        }

        public static void CopyStats(ISpecialStats injection, ISpecialStats copyFrom)
        {
            injection.Enlightenment = copyFrom.Enlightenment;
            injection.CriticalChance = copyFrom.CriticalChance;
            injection.SpeedAmount = copyFrom.SpeedAmount;
        }


        public static void CopyStats(ICharacterBasicStats injection, ICharacterBasicStats copyFrom)
        {
            CopyStats(injection as IOffensiveStats, copyFrom);
            CopyStats(injection as ISupportStats, copyFrom);
            CopyStats(injection as IVitalityStats, copyFrom);
            CopyStats(injection as ISpecialStats, copyFrom);
        }

        public static void CopyStats(ICombatTemporalStats injection, ICombatTemporalStats copyFrom)
        {
            injection.HealthPoints = copyFrom.HealthPoints;
            injection.ShieldAmount = copyFrom.ShieldAmount;
            injection.MortalityPoints = copyFrom.MortalityPoints;

            injection.HarmonyAmount = copyFrom.HarmonyAmount;
            injection.InitiativePercentage = copyFrom.InitiativePercentage;
            injection.ActionsPerInitiative = copyFrom.ActionsPerInitiative;
        }


        public static void CopyStats(ICharacterFullStats injection, ICharacterFullStats copyFrom)
        {
            CopyStats(injection as ICharacterBasicStats, copyFrom);
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
            return SRange.Percentage(stats.HealthPoints, 0, stats.MaxHealth);
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
            ICharacterFullStats stats = target.CombatStats;
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

            ICharacterFullStats stats = target.CombatStats;

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
            target.CombatStats.AccumulatedStaticDamage += damage;
        }



        public static void DoHealTo(CombatingEntity target, float heal)
        {
            if(!target.IsConscious() || heal < 0) return;
            
            var stats = target.CombatStats;

            float targetHealth = stats.HealthPoints + heal;
            if (targetHealth > stats.MaxHealth)
            {
                target.ReceivedStats.HealReceived = stats.MaxHealth - stats.HealthPoints;
                stats.HealthPoints = stats.MaxHealth;
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
                stats.AccumulatedStaticDamage = 0; //By design Heals counts 'StaticDamage'
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


        public static void HealToMax(CharacterCombatData stats)
        {
            if(!stats.IsAlive()) return;
            if(stats.HealthPoints < stats.MaxHealth)
                stats.HealthPoints = stats.MaxHealth;
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
            var stats = isBurstType
                ?  entity.CombatStats.BurstStats
                : (ICharacterBasicStats) entity.CombatStats.BaseStats;
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
            SetInitiative(stats, stats.InitiativePercentage + addition);


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
            float userEnlightenment = entity.CombatStats.Enlightenment; //Generally value = 1;
            addition *= userEnlightenment;

            float targetHarmony = stats.HarmonyAmount + addition;
            stats.HarmonyAmount = Mathf.Clamp(
                targetHarmony, 
                StatsCap.MinHarmony, StatsCap.MaxHarmony);

            UtilsStats.EnqueueTemporalStatsEvent(entity);
        }
        public static void AddHarmony(CombatingEntity entity, float addition)
            => AddHarmony(entity, entity.CombatStats, addition);
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

        public static bool IsCriticalPerformance(ICharacterBasicStats stats, CombatSkill skill, float criticalCheck)
        {
            if (!skill.CanCrit) return false;
            return criticalCheck < stats.CriticalChance + skill.CriticalAddition;
        }

        public static bool IsCriticalPerformance(ICharacterBasicStats stats, SSkillPreset preset, float criticalCheck)
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


        public static float CalculateShieldsPower(ICharacterFullStats user)
        {
            float shields = user.HealPower + user.BuffPower;
            shields *= .5f; //The average of Heal && buffPower is the Shields power

            return shields;
        }

        public static void VariateBuffUser(ICharacterBasicStats user, ref float buffValue)
        {
            var userBuffPower = user.BuffPower; //Generally value == 1;
            buffValue *= userBuffPower;
        }
        public static void VariateBuffTarget(ICharacterBasicStats target, ref float buffValue)
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