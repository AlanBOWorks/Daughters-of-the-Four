using CombatEntity;
using CombatSkills;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Heal [Effect]",
        menuName = "Combat/Effect/Heal")]
    public class SHealEffect : SEffect
    {
        public override void DoEffect(SkillValuesHolders values, float healModifier)
        {
            var user = values.User;
            var target = values.Target;

            float userHealModifier = user.CombatStats.Heal; //It's in percent

            float targetHealPercent = userHealModifier * healModifier;
            UtilsCombatStats.DoHealTo(target.CombatStats, targetHealPercent,values.IsCritical);
        }

        public override void DoDirectEffect(CombatingEntity target, float effectValue)
        {
            UtilsCombatStats.DoHealTo(target.CombatStats,effectValue);
        }

    }
}
