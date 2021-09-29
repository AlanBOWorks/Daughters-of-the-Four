using CombatEntity;
using CombatSkills;
using CombatSystem;
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
            CombatSystemSingleton.EventsHolder.OnPerformSupportAction(values,new EffectResolution(this,targetHealPercent));
        }


    }
}
