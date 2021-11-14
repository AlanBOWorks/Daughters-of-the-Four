using __ProjectExclusive.Localizations;
using CombatEntity;
using CombatSkills;
using CombatSystem;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Damage [Effect]",
        menuName = "Combat/Effect/Damage")]
    public class SDamageEffect : SOffensiveEffect
    {
        public const float CriticalDamageModifier = 1.25f; 
        


        protected override SkillComponentResolution DoEffectOn(CombatingEntity user, CombatingEntity effectTarget, float effectValue, bool isCritical)
        {
            float userAttack = user.CombatStats.Attack;
            var targetStats = effectTarget.CombatStats;
            float targetResistance = targetStats.DamageResistance;

            // Modifiers
            if (isCritical)
                userAttack *= CriticalDamageModifier;
            userAttack *= effectValue;

            // Final
            float finalDamage = userAttack - targetResistance;

            UtilsCombatStats.DoDamageTo(effectTarget.CombatStats, finalDamage);
            return new SkillComponentResolution(this, finalDamage);
        }

        public override EnumSkills.SkillInteractionType GetComponentType() => EnumSkills.SkillInteractionType.Attack;
    }
}
