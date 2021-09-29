using CombatEntity;
using CombatSkills;
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
            ActDamageEffect(user,target,finalDamage);
        }

        public override void DoDirectEffect(CombatingEntity target, float damage)
        {
            ActDamageEffect(target,target,damage);
        }

        private void ActDamageEffect(CombatingEntity user, CombatingEntity target, float finalDamage)
        {
            // Act
            if (finalDamage <= 0)
            {
                // Todo invoke ZeroDamage Event
                return;
            }
            UtilsCombatStats.DoDamageTo(target.CombatStats, finalDamage);

            // Invoke
            EffectResolution effectResolution = new EffectResolution(this, finalDamage);
            UtilsStatsEvents.CallOffensiveEvent(user, target, effectResolution);
        }
    }
}
