using CombatEntity;
using CombatSkills;
using CombatSystem;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Heal [Effect]",
        menuName = "Combat/Effect/Heal")]
    public class SHealEffect : SSupportEffect
    {
        private const float CritHealModifier = 1.5f;
        protected override SkillComponentResolution DoEffectOn(CombatingEntity user, CombatingEntity effectTarget, float effectValue, bool isCritical)
        {
            float userHealModifier = user.CombatStats.Heal; //It's in percent

            float targetHealPercent = userHealModifier * effectValue;
            if (isCritical) targetHealPercent *= CritHealModifier;

            UtilsCombatStats.DoHealTo(effectTarget.CombatStats, targetHealPercent);
            return new SkillComponentResolution(this, targetHealPercent);
        }

        public override EnumSkills.SkillInteractionType GetComponentType() => EnumSkills.SkillInteractionType.Heal;
    }
}
