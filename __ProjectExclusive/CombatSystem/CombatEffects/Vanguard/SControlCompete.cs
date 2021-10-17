using CombatEntity;
using CombatSkills;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "TEAM Control Compete [Effect]",
        menuName = "Combat/Effect/Control Compete", order = 40)]
    public class SControlCompete : SEffect
    {
        private const float ControlCriticalVariation = 1.25f;
        protected override void DoEffectOn(SkillValuesHolders values, CombatingEntity effectTarget, float effectValue, bool isCritical)
        {
            if (isCritical)
                effectValue *= ControlCriticalVariation;
            effectTarget.Team.CompeteControl(effectValue);
        }
    }
}
