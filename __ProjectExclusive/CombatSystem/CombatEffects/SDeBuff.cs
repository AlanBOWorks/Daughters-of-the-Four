using System.Collections;
using System.Collections.Generic;
using __ProjectExclusive.Player;
using CombatEntity;
using CombatSkills;
using CombatSystem.Events;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    public abstract class SDeBuff : SBuff
    {
        protected override void DoEventCalls(SystemEventsHolder systemEvents, CombatingEntity receiver,
            ref SkillComponentResolution resolution)
        {
            systemEvents.OnReceiveOffensiveEffect(receiver,ref resolution);
        }

        public override EnumSkills.SkillInteractionType GetComponentType() => EnumSkills.SkillInteractionType.DeBuff;
        public override Color GetDescriptiveColor()
        {
            return PlayerCombatSingleton.SkillInteractionColors.Debuff;
        }

    }
}
