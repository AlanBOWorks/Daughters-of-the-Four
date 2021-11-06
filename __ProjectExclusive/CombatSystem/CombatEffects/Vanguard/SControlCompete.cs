using CombatEntity;
using CombatSkills;
using CombatSystem.Events;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "TEAM Control Compete [Effect]",
        menuName = "Combat/Effect/Control Compete", order = 40)]
    public class SControlCompete : SEffect
    {
        private const float ControlCriticalVariation = 1.25f;
        

        protected override void DoEventCall(SystemEventsHolder systemEvents, CombatEntityPairAction entities,
            ref SkillComponentResolution resolution)
        {
        }

        protected override SkillComponentResolution DoEffectOn(CombatingEntity user, CombatingEntity effectTarget, float controlVariation,
            bool isCritical)
        {
            if (isCritical)
                controlVariation *= ControlCriticalVariation;
            effectTarget.Team.CompeteControl(controlVariation);

            return new SkillComponentResolution(this,controlVariation);
        }

        public override EnumSkills.SkillInteractionType GetComponentType() => EnumSkills.SkillInteractionType.Control;
    }
}
