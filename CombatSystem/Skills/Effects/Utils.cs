using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    public static class UtilsEffect
    {
        public static void DoDamageTo(in IHealthStats<float> target, in float damage)
        {
            DoDamageToShields(in target, in damage, out bool didDamageForBreakMethod);
            if(didDamageForBreakMethod) 
                return;


            DoDamageToHealth(in target,in damage, out didDamageForBreakMethod, out var healthLost);
            if(didDamageForBreakMethod)
            {
                //todo healthLost EventCall
                return;
            }


            DoDamageToMortality(in target, in damage, out var mortalityLost);
            //todo mortalityLost EventCall
        }

        public static void DoDamageToShields(in IHealthStats<float> target, in float damage, 
            out bool shieldBreak)
        {
            float targetShields = target.CurrentShields;
            shieldBreak = targetShields > 0 && damage > 0; //damage check is just a safeCheck

            if(!shieldBreak) return;

            targetShields -= 1; //By design shields are lost by units
            if (targetShields < 0) targetShields = 0;

            target.CurrentShields = targetShields;
        }

        public static void DoDamageToHealth(in IHealthStats<float> target, in float damage, 
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

        public static void DoDamageToMortality(in IHealthStats<float> target, in float damage,
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

