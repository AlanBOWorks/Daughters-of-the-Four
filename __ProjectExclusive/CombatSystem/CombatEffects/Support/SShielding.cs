using __ProjectExclusive.Player;
using CombatEntity;
using CombatSkills;
using CombatSystem.Events;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Shielding [Effect]", 
        menuName = "Combat/Effect/Shielding")]
    public class SShielding : SEffect
    {
        private const float AdditionShieldingAddition = .5f;

        protected override void DoEventCall(SystemEventsHolder systemEvents, CombatingEntity receiver,
            ref SkillComponentResolution resolution)
        {
            systemEvents.OnReceiveSupportEffect(receiver,ref resolution);
        }

        protected override SkillComponentResolution DoEffectOn(CombatingEntity user, CombatingEntity effectTarget, float shieldAddition,
            bool isCritical)
        {
            if (isCritical)
                shieldAddition += AdditionShieldingAddition;

            UtilsCombatStats.DoShielding(user.CombatStats, effectTarget.CombatStats, shieldAddition);
            return new SkillComponentResolution(this,shieldAddition);
        }

        public override EnumSkills.SkillInteractionType GetComponentType() => EnumSkills.SkillInteractionType.Shielding;
        public override Color GetDescriptiveColor()
        {
            return PlayerCombatSingleton.SkillInteractionColors.Shielding;
        }

        public override string GetEffectValueText(float effectValue)
        {
            return effectValue.ToString("F1") + "u";
        }
    }
}
