using __ProjectExclusive.Player;
using CombatEntity;
using CombatSkills;
using CombatSystem.Events;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "TEAM Control Burst [Effect]",
        menuName = "Combat/Effect/Control Burst",order = 40)]
    public class SControlBurst : SEffect
    {
        private const float ControlCriticalModifier = 1.25f;
        protected override void DoEventCall(SystemEventsHolder systemEvents, ISkillParameters parameters,
            ref SkillComponentResolution resolution)
        {
        }

        protected override SkillComponentResolution DoEffectOn(CombatingEntity user, CombatingEntity effectTarget, float controlAddition,
            bool isCritical)
        {
            if (isCritical)
                controlAddition *= ControlCriticalModifier;
            effectTarget.Team.BurstControl(controlAddition);
            return new SkillComponentResolution(this, controlAddition);
        }

        public override EnumSkills.SkillInteractionType GetComponentType() => EnumSkills.SkillInteractionType.Buff;
        public override Color GetDescriptiveColor()
        {
            return PlayerCombatSingleton.SkillInteractionColors.Control;
        }

        public override string GetEffectValueText(float effectValue)
        {
            return effectValue.ToString("####");
        }
    }
}
