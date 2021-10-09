using CombatEntity;
using CombatSkills;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Wait [Effect]", menuName = "Combat/Effect/Wait")]
    public class SWait : SEffect
    {
        public override void DoEffect(SkillValuesHolders values, float effectModifier)
        {
            UtilsCombatStats.ResetActions(values.Target.CombatStats);
        }

    }
}
