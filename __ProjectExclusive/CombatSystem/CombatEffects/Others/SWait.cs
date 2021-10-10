using CombatEntity;
using CombatSkills;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Wait [Effect]", menuName = "Combat/Effect/Wait")]
    public class SWait : SEffect
    {
        protected override void DoEffectOn(SkillValuesHolders values, CombatingEntity effectTarget, float effectValue, bool isCritical)
        {
            UtilsCombatStats.ResetActions(effectTarget.CombatStats);
        }
    }
}
