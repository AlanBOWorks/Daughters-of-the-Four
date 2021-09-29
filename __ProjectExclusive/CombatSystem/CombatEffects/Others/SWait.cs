using CombatEntity;
using CombatSkills;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    public class SWait : SEffect
    {
        public override void DoEffect(SkillValuesHolders values, float effectModifier)
        {
            throw new System.NotImplementedException();
        }

        public override void DoDirectEffect(CombatingEntity target, float effectValue)
        {
            var stats = target.CombatStats;
            int currentActions = stats.CurrentActions;
            UtilsCombatStats.OverrideActionsAmount(stats,0);

        }
    }
}
