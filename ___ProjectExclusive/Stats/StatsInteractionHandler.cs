using System;
using System.Collections.Generic;
using _CombatSystem;
using Characters;
using MEC;
using Sirenix.OdinInspector;
using Skills;
using Random = UnityEngine.Random;

namespace Stats
{
    /// <summary>
    /// Its job is to keep track the <see cref="CharacterCombatStatsFull"/> of all participants in one calculation
    /// and then execute the requested Skills/actions in a one go.<br></br>
    /// <br></br>
    /// This exits because some effects could have conditions and it could trigger false positives 
    /// as a consequence of having the stats doing their job step by step instead before the SKill
    /// <example>(eg: a skill could increase the damage
    /// done if the enemy has 50% or less HP, but in the start of the operation the enemy didn't have less than
    /// 50%, yet because a previous effect of dealing damage it triggers a false positive)</example>
    /// </summary>
    public class StatsInteractionHandler
    {
        public StatsInteractionHandler()
        {
            _stats = new ReflectionStats();
            _skillArguments = new SkillArguments();
        }

        [ShowInInspector]
        private readonly ReflectionStats _stats;
        [ShowInInspector]
        private readonly SkillArguments _skillArguments;

        public ICharacterFullStatsData CurrentStats => _stats;

        private void Injection(CombatingEntity user, CombatingEntity target)
        {
            _stats.Injection(user.CombatStats);

            _skillArguments.User = user;
            _skillArguments.UserStats = _stats;

            Injection(target);
        }

        private void Injection(CombatingEntity target)
        {
            _skillArguments.InitialTarget = target;
        }


        public void DoSkill(CombatSkill skill, CombatingEntity user, CombatingEntity target)
        {
            Injection(user,target);
            var skillPreset = skill.Preset;
            if (skill is null)
            {
                throw new NullReferenceException("DoSkills() was invoked before preparation");
            }

            bool isOffensiveSkill = skillPreset.GetSkillType() == EnumSkills.TargetingType.Offensive;
            var targetGuarding = target.Guarding;
            if (isOffensiveSkill && targetGuarding.HasProtector())
            {
                targetGuarding.VariateTarget(ref target);
            }

            //>>>>>>>>>>>>>>>>>>> DO Randomness
            float randomValue = Random.value;
            bool isCritical;
            var combatStats = user.CombatStats;
            if (UtilsCombatStats.IsCriticalPerformance(combatStats, skill, randomValue))
            {
                isCritical = true;
                float defaultHarmonyAddition
                    = CombatSystemSingleton.ParamsVariable.criticalHarmonyAddition;
                UtilsCombatStats.AddHarmony(target, defaultHarmonyAddition);

                var criticalBuff = user.CharacterCriticalBuff;
                criticalBuff?.OnCriticalAction();
            }
            else
            {
                isCritical = false;
            }

            _skillArguments.IsCritical = isCritical;


            skillPreset.DoMainEffect(_skillArguments);
            skillPreset.DoSecondaryEffects(_skillArguments);
        }

        private class ReflectionStats : ICharacterFullStatsData
        {
            private ICharacterFullStatsData _statsReference;
            private readonly CharacterCombatStatsFull _calculatedStats;

            public ReflectionStats()
            {
                _calculatedStats = new CharacterCombatStatsFull();
            }

            public void Injection(ICharacterFullStatsData statsReference)
            {
                _statsReference = statsReference;
                UtilsStats.CopyStats(_calculatedStats as ICharacterBasicStatsInjection, statsReference);
            }

            public float AttackPower => _calculatedStats.AttackPower;

            public float DeBuffPower => _calculatedStats.DeBuffPower;

            public float StaticDamagePower => _calculatedStats.StaticDamagePower;

            public float HealPower => _calculatedStats.HealPower;

            public float BuffPower => _calculatedStats.BuffPower;

            public float BuffReceivePower => _calculatedStats.BuffReceivePower;

            public float MaxHealth => _calculatedStats.MaxHealth;

            public float MaxMortalityPoints => _calculatedStats.MaxMortalityPoints;

            public float DamageReduction => _calculatedStats.DamageReduction;

            public float DeBuffReduction => _calculatedStats.DeBuffReduction;

            public float Enlightenment => _calculatedStats.Enlightenment;

            public float CriticalChance => _calculatedStats.CriticalChance;

            public float SpeedAmount => _calculatedStats.SpeedAmount;

            public float InitiativePercentage
            {
                get => _calculatedStats.InitiativePercentage;
                set => _calculatedStats.InitiativePercentage = value;
            }

            public int ActionsPerInitiative
            {
                get => _calculatedStats.ActionsPerInitiative;
                set => _calculatedStats.ActionsPerInitiative = value;
            }

            public float HarmonyAmount
            {
                get => _calculatedStats.HarmonyAmount;
                set => _calculatedStats.HarmonyAmount = value;
            }

            public float HealthPoints
            {
                get => _statsReference.HealthPoints;
                set => _calculatedStats.HealthPoints = value;
            }
            public float ShieldAmount
            {
                get => _statsReference.ShieldAmount;
                set => _statsReference.ShieldAmount = value;
            }
            public float MortalityPoints
            {
                get => _statsReference.MortalityPoints;
                set => _statsReference.MortalityPoints = value;
            }

            public void SetInitiativePercentage(float value)
            {
                _calculatedStats.InitiativePercentage = value;
            }

            public void SetActionsPerInitiative(int value)
            {
                _calculatedStats.ActionsPerInitiative = value;
            }

            public void SetHarmonyAmount(float value)
            {
                _calculatedStats.HarmonyAmount = value;
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
}
