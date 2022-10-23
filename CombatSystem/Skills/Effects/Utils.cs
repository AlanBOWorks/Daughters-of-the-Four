using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.UI;
using CombatSystem.Stats;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Skills
{
    public static class UtilsCombatSkill
    {
        public static void DoSkillOnTarget(IFullSkill skill, CombatEntity performer, CombatEntity onTarget)
        {
            CombatEntity exclusion = skill.IgnoreSelf ? performer : null;
            var effects = skill.GetEffects();
            DoSkillOnTarget();



            if (skill is ICombatSkill combatSkill && combatSkill.Preset is IVanguardSkill vanguardSkill)
            {
                bool callEvents = false;
                if(vanguardSkill.HasPunishEffects())
                {
                    performer.Team.PunishEffectsHolder.AddSkillEffects(vanguardSkill);
                    callEvents = true;
                }
                if(vanguardSkill.HasCounterEffects())
                {
                    performer.CounterEffects.AddSkillEffects(vanguardSkill);
                    callEvents = true;
                }

                if(callEvents)
                    CombatSystemSingleton.EventsHolder.OnVanguardSkillSubscribe(vanguardSkill, performer);
            }

            void DoSkillOnTarget()
            {
                var actors = new EffectActors(performer, exclusion, onTarget);
                DoEffectsOnTarget(actors, effects, skill.LuckModifier);
            }
        }

        private static void DoEffectsOnTarget(
           EffectActors actors,
            IEnumerable<PerformEffectValues> effects,
           float critModifier)
        {
            foreach (var effect in effects)
            {
                DoEffect(actors, effect, critModifier);
            }
        }

        private static void DoEffect(
            EffectActors actors,
            PerformEffectValues values,
            float skillLuckModifier)
        {
            actors.Extract(out var performer, out var exclusion, out var target);

            var targetType = values.TargetType;
            var targets = UtilsTarget.GetEffectTargets(targetType, performer, target);
            var effect = values.Effect;
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
                        var entities = new EntityPairInteraction(performer, effectTarget);
                        float targetEffectValue = effectValue;
                        float luckValue = CalculateFinalLuck();

                        // HERE do effect is done
                        effect.DoEffect(entities,ref targetEffectValue, ref luckValue);
                        var submitEffect = new SubmitEffectValues(effect ,targetEffectValue);

                        if (isFirstEffect)
                        {
                            eventsHolder.OnCombatPrimaryEffectPerform(entities, in submitEffect);
                            isFirstEffect = false;
                        }
                        else
                            eventsHolder.OnCombatSecondaryEffectPerform(entities, in submitEffect);
                    }

                    float CalculateFinalLuck()
                    {
                        if (skillLuckModifier <= 0) return 1;

                        float entitiesLuck = CalculateEntitiesLuck();
                        return 1 + skillLuckModifier * entitiesLuck;
                    }

                    float CalculateEntitiesLuck()
                    {
                        float performerLuck = performer.DiceValuesHolder.LuckFinalRoll;
                        float onTargetLuck = target.DiceValuesHolder.LuckFinalRoll;

                        bool areSameTeam = performer.Team.Contains(target);
                        if (areSameTeam)
                            return (performerLuck + onTargetLuck) * .5f;

                        if (onTargetLuck < 0) onTargetLuck = 0;
                        return performerLuck - onTargetLuck;

                    }
                }
            }
        }


        public static void DoVanguardEffects(EnumsEffect.TargetType type, in VanguardEffectUsageValues values)
        {
            var effectPerformer = values.EffectPerformer;
            var attacker = values.Attacker;
            var effect = values.Effect;
            var effectAccumulation = values.Accumulation;

            var effectTargets = UtilsTarget.GetEffectTargets(type, effectPerformer, attacker);
            float luckModifier = 1;

            var eventHolder = CombatSystemSingleton.EventsHolder;
            foreach (var target in effectTargets)
            {
                var entitiesPair = new EntityPairInteraction(effectPerformer, target);
                effect.DoEffect(entitiesPair, ref effectAccumulation, ref luckModifier);

                eventHolder.OnVanguardEffectPerform(type, values);
                eventHolder.OnCombatVanguardEffectPerform(entitiesPair,
                    new SubmitEffectValues(effect,effectAccumulation));
            }
        }

        private readonly struct EffectActors
        {
            public readonly CombatEntity Performer;
            public readonly CombatEntity Exclusion;
            public readonly CombatEntity Target;

            public EffectActors(CombatEntity performer, CombatEntity exclusion, CombatEntity target)
            {
                Performer = performer;
                Exclusion = exclusion;
                Target = target;
            }

            public EffectActors(CombatEntity performer, CombatEntity target) : this(performer,null, target)
            { }

            public void Extract(out CombatEntity performer, out CombatEntity exclusion, out CombatEntity target)
            {
                performer = Performer;
                exclusion = Exclusion;
                target = Target;
            }
        }
    }

    public static class UtilsEffect
    {
        /// <summary>
        /// Rounds the effect value to snap values between [.25f] values as a Percent
        /// </summary>
        public static float RoundEffectValueWithHalf_Percent(float currentEffectValue, bool isPercentType)
        {
            // Note: by design, values snaps in 25% so it's easier for the player tracking the percentages
            if(!isPercentType)
                return Mathf.Round(currentEffectValue * 4) * .25f;

            float amount = Mathf.Round(currentEffectValue * 4 * 100) * .25f;
            return amount * .01f;
        }

        public static IEnumerable<T> GetEnumerable<T>(ISkillTargetingStructureRead<T> structure)
        {
            yield return structure.TargetSingleType;
            yield return structure.TargetLineType;
            yield return structure.TargetTeamType;
            yield return structure.PerformerSingleType;
            yield return structure.PerformerLineType;
            yield return structure.PerformerTeamType;
        }

        public static T GetElement<T>(EnumsEffect.TargetType type, ISkillTargetingStructureRead<T> structure)
        {
            return type switch
            {
                EnumsEffect.TargetType.Target => structure.TargetSingleType,
                EnumsEffect.TargetType.TargetLine => structure.TargetLineType,
                EnumsEffect.TargetType.TargetTeam => structure.TargetTeamType,
                EnumsEffect.TargetType.Performer => structure.PerformerSingleType,
                EnumsEffect.TargetType.PerformerLine => structure.PerformerLineType,
                EnumsEffect.TargetType.PerformerTeam => structure.PerformerTeamType,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }

    public static class UtilsCombatEffect
    {
        public static void DoDamageTo(CombatEntity target, CombatEntity performer, float damage,
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



        public static void DoHealPercent(CombatStats target, float healPercent, out float healedAmount)
        {
            if (healPercent < 0)
            {
                healedAmount = 0;
                return;
            }

            float maxHeal = UtilsStatsFormula.CalculateMaxHealth(target);
            healedAmount = maxHeal * healPercent;

            float targetHealth = target.CurrentHealth + healedAmount;
            DoOverrideHealth(target, ref targetHealth);
        }
        public static void DoHealPercent(in CombatStats target, in float healPercent)
        {
            DoHealPercent(target,healPercent, out _);
        }
        private static void DoOverrideHealth(CombatStats target, ref float targetHealth)
        {
            float maxHealth = UtilsStatsFormula.CalculateMaxHealth(target);
            DoOverrideHealth(target, ref targetHealth, maxHealth);
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
    }

    public static class UtilsStructureEffect
    {
        public static T GetElement<T>(EnumsEffect.ConcreteType type, IFullEffectStructureRead<T> theme)
        {
            return type switch
            {
                EnumsEffect.ConcreteType.DefaultOffensive => theme.OffensiveEffectType,
                EnumsEffect.ConcreteType.DefaultSupport => theme.SupportEffectType,
                EnumsEffect.ConcreteType.DefaultTeam => theme.VanguardEffectType,

                EnumsEffect.ConcreteType.DamageType => (theme.DamageType ?? theme.OffensiveEffectType),
                EnumsEffect.ConcreteType.DoT => (theme.DamageOverTimeType ?? theme.OffensiveEffectType),
                EnumsEffect.ConcreteType.DeBuff => (theme.DeBuffEffectType ?? theme.OffensiveEffectType),
                EnumsEffect.ConcreteType.DeBurst => (theme.DeBurstEffectType ?? theme.OffensiveEffectType),

                EnumsEffect.ConcreteType.Heal => (theme.HealType ?? theme.SupportEffectType),
                EnumsEffect.ConcreteType.Shielding => (theme.ShieldingType ?? theme.SupportEffectType),
                EnumsEffect.ConcreteType.Buff => (theme.BuffEffectType ?? theme.SupportEffectType),
                EnumsEffect.ConcreteType.Burst => (theme.BurstEffectType ?? theme.SupportEffectType),

                EnumsEffect.ConcreteType.Guarding => (theme.GuardingType ?? theme.VanguardEffectType),
                EnumsEffect.ConcreteType.Counter => (theme.CounterType ?? theme.VanguardEffectType),
                EnumsEffect.ConcreteType.Revenge => (theme.RevengeType ?? theme.VanguardEffectType),

                EnumsEffect.ConcreteType.ControlGain => (theme.ControlType ?? theme.FlexibleEffectType),
                EnumsEffect.ConcreteType.Stance => (theme.StanceType ?? theme.FlexibleEffectType),
                EnumsEffect.ConcreteType.Initiative => (theme.InitiativeType ?? theme.FlexibleEffectType),

                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }


    }
}

