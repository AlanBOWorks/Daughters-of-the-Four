using System.Collections;
using System.Collections.Generic;
using CombatEntity;
using CombatSkills;
using CombatSystem.Events;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    public abstract class SDeBuff : SBuff
    {
        protected override void DoEventCalls(SystemEventsHolder systemEvents, CombatEntityPairAction entities,
            ref SkillComponentResolution resolution)
        {
            systemEvents.OnReceiveOffensiveEffect(entities,ref resolution);
        }

        public override EnumSkills.SkillInteractionType GetComponentType() => EnumSkills.SkillInteractionType.DeBuff;
    }
}
