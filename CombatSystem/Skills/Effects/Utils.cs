using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.UI;
using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    public static class UtilsCombatSkill
    {
        public static void DoSkillOnTarget(ISkill skill, CombatEntity performer, CombatEntity onTarget)
        {
            CombatEntity exclusion = skill.IgnoreSelf() ? performer : null;
            var effects = skill.GetEffects();
            DoSkillOnTarget();

            void DoSkillOnTarget()
            {
                DoEffectsOnTarget(performer, exclusion, onTarget, effects);

            }
        }

        public static void DoVanguardSkillOnPerformer(in VanguardSkillUsageValues values, IReadOnlyDictionary<CombatEntity, int> offensiveRecords)
        {
            var skill = values.UsedSkill;
            var performer = values.EffectsHolder.GetMainEntity();
            var iterations = values.Accumulation;

            float totalOffensiveIterations = 0;
            foreach (var offensiveRecord in offensiveRecords)
            {
                totalOffensiveIterations += offensiveRecord.Value;
            }

            foreach (var effect in skill.GetPerformVanguardEffects())
            {
                var effectType = effect.TargetType;
                switch (effectType)
                {
                    case EnumsEffect.TargetType.Target:
                    case EnumsEffect.TargetType.TargetTeam:
                    case EnumsEffect.TargetType.TargetLine:
                        DoEffectOnTargets();
                        break;
                    case EnumsEffect.TargetType.All:
                        throw new ArgumentOutOfRangeException();
                    default:
                        DoEffectOnPerformer();
                        break;
                }


                void DoEffectOnTargets()
                {
                    foreach ((CombatEntity combatEntity, var i) in offensiveRecords)
                    {
                        DoEffectOnTarget(combatEntity, i);
                    }
                }

                void DoEffectOnTarget(CombatEntity onTarget, int targetIterations)
                {
                    PerformEffectValues performValue = new PerformEffectValues(
                        effect.Effect,
                        effect.EffectValue * iterations * targetIterations,
                        effect.TargetType);
                    DoEffect(performer, null, onTarget, performValue);
                }

                void DoEffectOnPerformer()
                {
                    PerformEffectValues performValue = new PerformEffectValues(
                        effect.Effect,
                        totalOffensiveIterations * effect.EffectValue * iterations,
                        effectType);
                    DoEffect(performer, null, performer, performValue);
                }
            }

           
        }


        private static void DoEffectsOnTarget(
            CombatEntity performer, CombatEntity exclusion,
            CombatEntity target,
            IEnumerable<PerformEffectValues> effects)
        {
            foreach (var effect in effects)
            {
                DoEffect(performer, exclusion, target, effect);
            }
        }

        private static void DoEffect(
            CombatEntity performer, 
            CombatEntity exclusion, 
            CombatEntity target,
            PerformEffectValues values)
        {
            var targetType = values.TargetType;
            var targets = UtilsTarget.GetEffectTargets(targetType, performer, target);
            var preset = values.Effect;
            var effectValue = values.EffectValue;

            DoEffectsTarget();

            void DoEffectsTarget()
            {
                var eventsHolder = CombatSystemSingleton.EventsHolder;


                bool isFirstEffect = true;
                foreach (var effectTarget in targets)
                {
                    bool isPerformerExclusive = (targetType == EnumsEffect.TargetType.Performer && performer == exclusion);
                    if (!isPerformerExclusive)
                        if (effectTarget == exclusion)
                            continue;

                    DoEffectOnTarget();
                    void DoEffectOnTarget()
                    {
                        var entities = new EntityPairInteraction(performer,target);
                        preset.DoEffect(entities, effectValue);

                        if (isFirstEffect)
                        {
                            eventsHolder.OnCombatPrimaryEffectPerform(entities, in values);
                            isFirstEffect = false;
                        }
                        else
                            eventsHolder.OnCombatSecondaryEffectPerform(entities, in values);

                    }
                }
            }
        }
    }

    public static class UtilsEffect
    {
        
    }

    public static class UtilsCombatEffect
    {
        public static void DoDamageTo(CombatEntity target, CombatEntity performer, in float damage,
            bool eventCallback = true)
        {
            if( damage <= 0) return;

            PerformDamage(target, performer, damage, eventCallback);
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            eventsHolder.OnDamageReceive(performer, target);
        }

        private static void PerformDamage(CombatEntity target, CombatEntity performer, float damage, bool eventCallback)
        {
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            if (eventCallback)
                eventsHolder.OnDamageBeforeDone(performer, target, damage);


            IDamageableStats<float> healthStats = target.Stats;
            DoDamageToShields(healthStats, damage, out bool didDamageForBreakMethod);


            if (didDamageForBreakMethod)
            {
                float shieldBreaks = 1;
                target.DamageReceiveTracker.DoShields(performer, shieldBreaks); //by design shields are lost in ones
                performer.DamageDoneTracker.DoShields(target, shieldBreaks);

                if (eventCallback)
                    eventsHolder.OnShieldLost(performer, target, shieldBreaks);

                return;
            }


            DoDamageToHealth(healthStats, damage, out didDamageForBreakMethod, out var healthLost);
            if (didDamageForBreakMethod)
            {
                target.DamageReceiveTracker.DoHealth(performer, damage);
                performer.DamageDoneTracker.DoHealth(target, damage);

                if (eventCallback)
                    eventsHolder.OnHealthLost(performer, target, damage);

                return;
            }


            DoDamageToMortality(healthStats, damage, out var mortalityLost);
            target.DamageReceiveTracker.DoMortality(in performer, in damage);
            performer.DamageDoneTracker.DoMortality(in target, in damage);

            if (eventCallback)
                eventsHolder.OnMortalityLost(performer, target, damage);
        }

        public static void DoDamageToShields(IDamageableStats<float> target, float damage,
            out bool shieldBreak)
        {
            float targetShields = target.CurrentShields;
            shieldBreak = targetShields > 0 && damage > 0; //damage check is just a safeCheck

            if(!shieldBreak) return;

            targetShields -= 1; //By design shields are lost by units
            if (targetShields < 0) targetShields = 0;

            target.CurrentShields = targetShields;
        }

        public static void DoDamageToHealth(IDamageableStats<float> target, float damage,
            out bool healthDamage,
            out bool healthLost)
        {
            float targetHealth = target.CurrentHealth;
            healthDamage = targetHealth > 0 && damage > 0; //damage check is just a safeCheck

            if (!healthDamage)
            {
                healthLost = false;
                return;
            }

            targetHealth -= damage;

            if (targetHealth <= 0)
            {
                targetHealth = 0;
                healthLost = true;
            }
            else
            {
                healthLost = false;
            }

            target.CurrentHealth = targetHealth;
        }

        public static void DoDamageToMortality(IDamageableStats<float> target, float damage,
            out bool mortalityLost)
        {
            if (damage <= 0) //damage check is just a safeCheck
            {
                mortalityLost = false;
                return;
            }

            target.CurrentMortality -= damage;
            if (target.CurrentMortality > 0)
            {
                mortalityLost = false;
                return;
            }

            target.CurrentMortality = 0;
            mortalityLost = true;
        }

        public static void DoHealTo(CombatStats target, float healAmount)
        {
            float targetHealth = target.CurrentHealth + healAmount;
            DoOverrideHealth(target, ref targetHealth);
        }

        public static void DoHealToPercent(in CombatStats target, in float healPercent)
        {
            if(healPercent < 0) return;
            float targetHealth = UtilsStatsFormula.CalculateMaxHealth(target) * (1 + healPercent);

            DoOverrideHealth(target, ref targetHealth);
        }

        private static void DoOverrideHealth(CombatStats target, ref float targetHealth, float maxHealth)
        {
            if (targetHealth >= maxHealth)
            {
                targetHealth = maxHealth;

                //todo call MaxHealth event
            }

            target.CurrentHealth = targetHealth;
        }
        private static void DoOverrideHealth(CombatStats target, ref float targetHealth)
        {
            float maxHealth = UtilsStatsFormula.CalculateMaxHealth(target);
            DoOverrideHealth(target, ref targetHealth, maxHealth);
        }
    }

    public static class UtilsStructureEffect
    {
        public static T GetElement<T>(EnumsEffect.ConcreteType type, IFullEffectStructureRead<T> theme)
        {
            return type switch
            {
                EnumsEffect.ConcreteType.DefaultOffensive => theme.OffensiveEffectType,
                EnumsEffect.ConcreteType.DefaultSupport => theme.SupportEffectType,
                EnumsEffect.ConcreteType.DefaultTeam => theme.TeamEffectType,

                EnumsEffect.ConcreteType.DamageType => (theme.DamageType ?? theme.OffensiveEffectType),
                EnumsEffect.ConcreteType.DoT => (theme.DamageOverTimeType ?? theme.OffensiveEffectType),
                EnumsEffect.ConcreteType.DeBuff => (theme.DeBuffEffectType ?? theme.OffensiveEffectType),
                EnumsEffect.ConcreteType.DeBurst => (theme.DeBurstEffectType ?? theme.OffensiveEffectType),
                EnumsEffect.ConcreteType.Heal => (theme.HealType ?? theme.SupportEffectType),
                EnumsEffect.ConcreteType.Shielding => (theme.ShieldingType ?? theme.SupportEffectType),
                EnumsEffect.ConcreteType.Buff => (theme.BuffEffectType ?? theme.SupportEffectType),
                EnumsEffect.ConcreteType.Burst => (theme.BurstEffectType ?? theme.SupportEffectType),
                EnumsEffect.ConcreteType.Guarding => (theme.GuardingType ?? theme.TeamEffectType),
                EnumsEffect.ConcreteType.ControlGain => (theme.ControlType ?? theme.TeamEffectType),
                EnumsEffect.ConcreteType.Stance => (theme.StanceType ?? theme.TeamEffectType),
                EnumsEffect.ConcreteType.ControlBurst => (theme.ControlBurstType ?? theme.TeamEffectType),

                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }


        public static T GetUnityElement<T>(EnumsEffect.ConcreteType type, IFullEffectStructureRead<T> theme) where T: UnityEngine.Object
        {
            return type switch
            {
                EnumsEffect.ConcreteType.DefaultOffensive => theme.OffensiveEffectType,
                EnumsEffect.ConcreteType.DefaultSupport => theme.SupportEffectType,
                EnumsEffect.ConcreteType.DefaultTeam => theme.TeamEffectType,

                EnumsEffect.ConcreteType.DamageType => GetOffensiveIfNull(theme.DamageType),
                EnumsEffect.ConcreteType.DoT => GetOffensiveIfNull(theme.DamageOverTimeType),
                EnumsEffect.ConcreteType.DeBuff => GetOffensiveIfNull(theme.DeBuffEffectType),
                EnumsEffect.ConcreteType.DeBurst => GetOffensiveIfNull(theme.DeBurstEffectType),
                EnumsEffect.ConcreteType.Heal => GetSupportIfNull(theme.HealType),
                EnumsEffect.ConcreteType.Shielding => GetSupportIfNull(theme.ShieldingType),
                EnumsEffect.ConcreteType.Buff => GetSupportIfNull(theme.BuffEffectType),
                EnumsEffect.ConcreteType.Burst => GetSupportIfNull(theme.BurstEffectType),
                EnumsEffect.ConcreteType.Guarding => GetTeamIfNull(theme.GuardingType),
                EnumsEffect.ConcreteType.ControlGain => GetTeamIfNull(theme.ControlType),
                EnumsEffect.ConcreteType.Stance => GetTeamIfNull(theme.StanceType),
                EnumsEffect.ConcreteType.ControlBurst => GetTeamIfNull(theme.ControlBurstType),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            T GetOffensiveIfNull(T element)
            {
                if (element) return element;
                return theme.OffensiveEffectType;
            }

            T GetSupportIfNull(T element)
            {
                if (element) return element;
                return theme.SupportEffectType;
            }

            T GetTeamIfNull(T element)
            {
                if (element) return element;
                return theme.TeamEffectType;
            }
        }
    }
}

