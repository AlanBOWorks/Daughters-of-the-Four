using CombatEntity;
using CombatSkills;
using CombatSystem;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Damage [Effect]",
        menuName = "Combat/Effect/Damage")]
    public class SDamageEffect : SEffect
    {
        public const float CriticalDamageModifier = 1.25f; 
        public override void DoEffect(SkillValuesHolders values, float effectModifier)
        {
            var user = values.User;
            var target = values.Target;

            float userAttack = user.CombatStats.Attack;
            float targetResistance = target.CombatStats.DamageResistance;

            // Modifiers
            if (values.IsCritical)
                userAttack *= CriticalDamageModifier;
            userAttack *= effectModifier;

            // Final
            float finalDamage = userAttack - targetResistance;
            if(finalDamage <= 0) return;


            UtilsCombatStats.DoDamageTo(target.CombatStats, finalDamage);

            EffectResolution effectResolution = new EffectResolution(this, finalDamage);
            CombatSystemSingleton.EventsHolder.OnDamageReceiveAction(values,effectResolution);
        }
    }
}
