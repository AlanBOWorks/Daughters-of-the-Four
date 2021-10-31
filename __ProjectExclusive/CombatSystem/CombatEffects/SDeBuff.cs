using System.Collections;
using System.Collections.Generic;
using CombatEntity;
using CombatSystem.Events;
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
    }
}
