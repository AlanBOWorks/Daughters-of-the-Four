using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    public static class UtilsSkillEffect
    {
        public static void DoEffectsOnTarget(
            IEnumerable<IEffect> effects,
            CombatEntity performer, CombatEntity exclusion, 
            CombatSkill skillReference)
        {
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            // Problem: effects might be in a Scriptable and exceptions in execution during develop
            // could make the IEnumerator<IEffect>'s index being wrong, so the iteration will be wrong
            // Solution: reset before and after the iterator
            foreach (var effect in effects)
            {
                var targetType = effect.TargetType;
                var targets = UtilsTarget.GetEffectTargets(targetType);
                DoEffectOnTargets(in effect, in targets);
            }


            void DoEffectOnTargets(in IEffect effect, in IEnumerable<CombatEntity> targets)
            {
                int i = 0;
                foreach (var effectTarget in targets)
                {
                    if(effectTarget == exclusion) continue;

                    effect.DoEffect(in performer);
                    eventsHolder.OnEffectPerform(in performer, in skillReference, in effectTarget, in effect);
                    i++;
                }
                Debug.Log($"Iteration: {i}");
            }
        }
    }

    public static class UtilsEffect
    {
        public static void DoDamageTo(in CombatEntity target, in CombatEntity performer, in float damage, bool eventCallback = true)
        {
            if( damage <= 0) return;

            var eventsHolder = CombatSystemSingleton.EventsHolder;
            if(eventCallback)
                eventsHolder.OnDamageDone(in target, in performer, in damage);


            IDamageableStats<float> healthStats = target.Stats;
            DoDamageToShields(in healthStats, in damage, out bool didDamageForBreakMethod);


            if (didDamageForBreakMethod)
            {
                float shieldBreaks = 1;
                target.DamageReceiveTracker.DoShields(in performer, in shieldBreaks); //by design shields are lost in ones
                performer.DamageDoneTracker.DoShields(in target, in shieldBreaks);

                if(eventCallback)
                    eventsHolder.OnShieldLost(in target, in performer,in shieldBreaks);

                return;
            }


            DoDamageToHealth(in healthStats, in damage, out didDamageForBreakMethod, out var healthLost);
            if (didDamageForBreakMethod)
            {
                target.DamageReceiveTracker.DoHealth(in performer, in damage);
                performer.DamageDoneTracker.DoHealth(in target, in damage);

                if(eventCallback)
                    eventsHolder.OnHealthLost(in target, in performer, in damage);

                return;
            }


            DoDamageToMortality(in healthStats, in damage, out var mortalityLost);
            target.DamageReceiveTracker.DoMortality(in performer, in damage);
            performer.DamageDoneTracker.DoMortality(in target, in damage);

            if(eventCallback)
                eventsHolder.OnMortalityLost(in target, in performer, in damage);
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
    }
}

