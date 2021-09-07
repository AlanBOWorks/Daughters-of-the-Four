using Characters;
using Skills;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Wait Effect [Preset]",
        menuName = "Combat/Effects/Waits")]
    public class SEffectWait : SEffectBase
    {
        public override void DoEffect(SkillArguments arguments, CombatingEntity target, float effectModifier = 1)
        {
            DoEffect(target,effectModifier);
        }


        public override void DoEffect(CombatingEntity target, float effectModifier)
        {
            var combatStatsHolder = target.CombatStats;

            UtilsCombatStats.SetInitiative(target, combatStatsHolder.BaseStats, effectModifier);
            combatStatsHolder.ResetActionsAmount();
        }
    }
}
