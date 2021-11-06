using CombatEntity;
using CombatSkills;
using CombatSystem.Events;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Wait [Effect]", menuName = "Combat/Effect/Wait")]
    public class SWait : SEffect
    {
        protected override void DoEventCall(SystemEventsHolder systemEvents, CombatEntityPairAction entities,
            ref SkillComponentResolution resolution)
        {
            // todo make WaitEventCall
        }

        protected override SkillComponentResolution DoEffectOn(CombatingEntity user, CombatingEntity effectTarget, 
            float afterWaitInitiativeAddition,
            bool isCritical)
        {
            UtilsCombatStats.ResetActions(effectTarget.CombatStats);
            return new SkillComponentResolution(this, afterWaitInitiativeAddition);
        }

        public override EnumSkills.SkillInteractionType GetComponentType() => EnumSkills.SkillInteractionType.Wait;
    }
}
