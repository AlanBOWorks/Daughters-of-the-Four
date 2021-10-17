using CombatEntity;
using CombatSkills;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "TEAM Control Burst [Effect]",
        menuName = "Combat/Effect/Control Burst",order = 40)]
    public class SControlBurst : SEffect
    {
        private const float ControlCriticalModifier = 1.25f;
        protected override void DoEffectOn(SkillValuesHolders values, CombatingEntity effectTarget, float effectValue, bool isCritical)
        {
            if (isCritical) 
                effectValue *= ControlCriticalModifier;
            effectTarget.Team.BurstControl(effectValue);
        }
    }
}
