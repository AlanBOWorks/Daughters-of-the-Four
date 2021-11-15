using __ProjectExclusive.Player;
using CombatEntity;
using CombatSkills;
using CombatSystem.Events;
using Stats;
using UnityEngine;

namespace CombatEffects
{

    [CreateAssetMenu(fileName = "SuppOFFENSIVE - Follow Up [Effect]",
        menuName = "Combat/Effect/Follow Up")]
    public class SFollowUp : SEffect
    {

        protected override void DoEventCall(SystemEventsHolder systemEvents, ISkillParameters parameters,
            ref SkillComponentResolution resolution)
        {
            systemEvents.OnReceiveSupportEffect(parameters,ref resolution);
        }

        protected override SkillComponentResolution DoEffectOn(CombatingEntity user, CombatingEntity effectTarget, float effectValue,
            bool isCritical)
        {
            user.FollowUpHandler.SwitchFollowTarget(effectTarget);
            return new SkillComponentResolution(this,effectValue);
        }

        public override EnumSkills.SkillInteractionType GetComponentType() => EnumSkills.SkillInteractionType.FollowUp;
        public override Color GetDescriptiveColor()
        {
            return PlayerCombatSingleton.SkillInteractionColors.FollowUp;
        }

        public override string GetEffectValueText(float effectValue)
        {
            return effectValue.ToString("F1") + "%";
        }
    }
}
