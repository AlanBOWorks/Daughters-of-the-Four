using System;
using System.Collections.Generic;
using _CombatSystem;
using Characters;
using MEC;
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

        private readonly ReflectionStats _stats;
        private readonly SkillArguments _skillArguments;

        public ICharacterFullStats CurrentStats => _stats;

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

        private class ReflectionStats : ICharacterFullStats
        {
            private ICharacterFullStats _statsReference;
            private readonly CharacterCombatStatsBasic _calculatedStats;

            public ReflectionStats()
            {
                _calculatedStats = new CharacterCombatStatsBasic();
            }

            public void Injection(ICharacterFullStats statsReference)
            {
                _statsReference = statsReference;
                UtilsStats.CopyStats(_calculatedStats,statsReference);
            }

            public float AttackPower
            {
                get => _calculatedStats.AttackPower;
                set => _statsReference.AttackPower = value;
            }
            public float DeBuffPower
            {
                get => _calculatedStats.DeBuffPower;
                set => _statsReference.DeBuffPower = value;
            }
            public float StaticDamagePower
            {
                get => _calculatedStats.StaticDamagePower;
                set => _statsReference.StaticDamagePower = value;
            }
            public float HealPower
            {
                get => _calculatedStats.HealPower;
                set => _statsReference.HealPower = value;
            }
            public float BuffPower
            {
                get => _calculatedStats.BuffPower;
                set => _statsReference.BuffPower = value;
            }
            public float BuffReceivePower
            {
                get => _calculatedStats.BuffReceivePower;
                set => _statsReference.BuffReceivePower = value;
            }
            public float MaxHealth
            {
                get => _calculatedStats.MaxHealth;
                set => _statsReference.MaxHealth = value;
            }
            public float MaxMortalityPoints
            {
                get => _calculatedStats.MaxMortalityPoints;
                set => _statsReference.MaxMortalityPoints = value;
            }
            public float DamageReduction
            {
                get => _calculatedStats.DamageReduction;
                set => _statsReference.DamageReduction = value;
            }
            public float DeBuffReduction
            {
                get => _calculatedStats.DeBuffReduction;
                set => _statsReference.DeBuffReduction = value;
            }
            public float Enlightenment
            {
                get => _calculatedStats.Enlightenment;
                set => _statsReference.Enlightenment = value;
            }
            public float CriticalChance
            {
                get => _calculatedStats.CriticalChance;
                set => _statsReference.CriticalChance = value;
            }
            public float SpeedAmount
            {
                get => _calculatedStats.SpeedAmount;
                set => _statsReference.SpeedAmount = value;
            }
            public float InitiativePercentage
            {
                get => _calculatedStats.InitiativePercentage;
                set => _statsReference.InitiativePercentage = value;
            }
            public int ActionsPerInitiative { get; set; }
            public float HarmonyAmount
            {
                get => _calculatedStats.HarmonyAmount;
                set => _statsReference.HarmonyAmount = value;
            }
            public float HealthPoints
            {
                get => _statsReference.HealthPoints;
                set => _statsReference.HealthPoints = value;
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
        }
    }
}
