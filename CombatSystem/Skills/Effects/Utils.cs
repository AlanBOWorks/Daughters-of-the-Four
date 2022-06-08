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
        public static void DoSkillOnTarget(in CombatSkill skill, in CombatEntity performer, in CombatEntity onTarget)
        {
            CombatSystemSingleton.SkillTargetingHandler.HandleSkill(in performer, in skill, in onTarget);



            var preset = skill.Preset;
            CombatEntity exclusion = skill.IgnoreSelf() ? performer : null;

            if(preset is SSkillPreset assetSkill)
                DoSkillOnTarget(in assetSkill, in performer, in exclusion);
        }

        private static void DoSkillOnTarget(in SSkillPreset preset, in CombatEntity performer, in CombatEntity exclusion)
        {
            var effects = preset.GetEffectValues();
            foreach (var effect in effects)
            {
                PerformEffectValues values = effect.GenerateValues();
                DoEffect(in performer, in exclusion, in values);
            }
        }

        private static void DoEffect(in CombatEntity performer, in CombatEntity exclusion, in PerformEffectValues values)
        {
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            var targets = UtilsTarget.GetEffectTargets(values.TargetType);
            var preset = values.Effect;
            var effectValue = values.EffectValue;

            int i = 0;
            foreach (var effectTarget in targets)
            {
                if (effectTarget == exclusion) continue;
                preset.DoEffect(in performer,in effectTarget,in effectValue);

                if(i == 0)
                    eventsHolder.OnCombatPrimaryEffectPerform(performer, effectTarget, in values);
                else
                    eventsHolder.OnCombatSecondaryEffectPerform(performer, effectTarget, in values);

                i++;
            }
        }
    }

    public static class UtilsEffect
    {
        public static IEnumerable<IEffectPreset> ExtractEffects(in CombatSkill skill)
        {
            var skillPreset = skill.Preset;
            if (skillPreset is SSkillPreset assetSkill)
            {
                return ExtractEffects(assetSkill);
            }

            return null;
        }

        public static IEnumerable<IEffectPreset> ExtractEffects(SSkillPreset preset)
        {
            foreach (var value in preset.GetEffectValues())
            {
                yield return value;
            }
        }


        public static void DoDamageTo(in CombatEntity target, in CombatEntity performer, in float damage, bool eventCallback = true)
        {
            if( damage <= 0) return;

            PerformDamage(target, performer, damage, eventCallback);
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            eventsHolder.OnDamageReceive(in performer, in target);
        }

        private static void PerformDamage(CombatEntity target, CombatEntity performer, float damage, bool eventCallback)
        {
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            if (eventCallback)
                eventsHolder.OnDamageBeforeDone(in performer, in target, in damage);


            IDamageableStats<float> healthStats = target.Stats;
            DoDamageToShields(in healthStats, in damage, out bool didDamageForBreakMethod);


            if (didDamageForBreakMethod)
            {
                float shieldBreaks = 1;
                target.DamageReceiveTracker.DoShields(in performer, in shieldBreaks); //by design shields are lost in ones
                performer.DamageDoneTracker.DoShields(in target, in shieldBreaks);

                if (eventCallback)
                    eventsHolder.OnShieldLost(in performer, in target, in shieldBreaks);

                return;
            }


            DoDamageToHealth(in healthStats, in damage, out didDamageForBreakMethod, out var healthLost);
            if (didDamageForBreakMethod)
            {
                target.DamageReceiveTracker.DoHealth(in performer, in damage);
                performer.DamageDoneTracker.DoHealth(in target, in damage);

                if (eventCallback)
                    eventsHolder.OnHealthLost(in performer, in target, in damage);

                return;
            }


            DoDamageToMortality(in healthStats, in damage, out var mortalityLost);
            target.DamageReceiveTracker.DoMortality(in performer, in damage);
            performer.DamageDoneTracker.DoMortality(in target, in damage);

            if (eventCallback)
                eventsHolder.OnMortalityLost(in performer, in target, in damage);
        }

        public static void DoDamageToShields(in IDamageableStats<float> target, in float damage, 
            out bool shieldBreak)
        {
            float targetShields = target.CurrentShields;
            shieldBreak = targetShields > 0 && damage > 0; //damage check is just a safeCheck

            if(!shieldBreak) return;

            targetShields -= 1; //By design shields are lost by units
            if (targetShields < 0) targetShields = 0;

            target.CurrentShields = targetShields;
        }

        public static void DoDamageToHealth(in IDamageableStats<float> target, in float damage, 
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

        public static void DoDamageToMortality(in IDamageableStats<float> target, in float damage,
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

        public static void DoHealTo(in CombatStats target,in float healAmount)
        {
            float targetHealth = target.CurrentHealth + healAmount;
            DoOverrideHealth(in target, ref targetHealth);
        }

        public static void DoHealToPercent(in CombatStats target, in float healPercent)
        {
            if(healPercent < 0) return;
            float targetHealth = UtilsStatsFormula.CalculateMaxHealth(target) * (1 + healPercent);

            DoOverrideHealth(in target, ref targetHealth);
        }

        private static void DoOverrideHealth(in CombatStats target, ref float targetHealth, in float maxHealth)
        {
            if (targetHealth >= maxHealth)
            {
                targetHealth = maxHealth;

                //todo call MaxHealth event
            }

            target.CurrentHealth = targetHealth;
        }
        private static void DoOverrideHealth(in CombatStats target, ref float targetHealth)
        {
            float maxHealth = UtilsStatsFormula.CalculateMaxHealth(target);
            DoOverrideHealth(in target, ref targetHealth, in maxHealth);
        }
    }

    public static class UtilsStructureEffect
    {
        public static T GetElement<T>(EnumsEffect.ConcreteType type, IEffectStructureRead<T> theme)
        {
            return type switch
            {
                EnumsEffect.ConcreteType.DamageType => theme.DamageType,
                EnumsEffect.ConcreteType.DoT => theme.DamageOverTimeType,
                EnumsEffect.ConcreteType.DeBuff => theme.DeBuffEffectType,
                EnumsEffect.ConcreteType.DeBurst => theme.DeBurstEffectType,
                EnumsEffect.ConcreteType.Heal => theme.HealType,
                EnumsEffect.ConcreteType.Shielding => theme.ShieldingType,
                EnumsEffect.ConcreteType.Buff => theme.BuffEffectType,
                EnumsEffect.ConcreteType.Burst => theme.BurstEffectType,
                EnumsEffect.ConcreteType.Guarding => theme.GuardingType,
                EnumsEffect.ConcreteType.ControlGain => theme.ControlType,
                EnumsEffect.ConcreteType.Stance => theme.StanceType,
                EnumsEffect.ConcreteType.ControlBurst => theme.ControlBurstType,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        public static T GetElement<T>(EnumsEffect.ConcreteType type, IFullEffectStructureRead<T> theme)
        {
            return type switch
            {
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

