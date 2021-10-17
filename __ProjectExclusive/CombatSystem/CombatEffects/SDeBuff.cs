using System.Collections;
using System.Collections.Generic;
using CombatEntity;
using UnityEngine;

namespace CombatEffects
{
    public abstract class SDeBuff : SBuff
    {
        protected override void DoEventCalls(CombatingEntity user, CombatingEntity effectTarget, SkillComponentResolution resolution)
        {
            var userEventsHolder = user.EventsHolder;
            var targetEventsHolder = effectTarget.EventsHolder;

            userEventsHolder.OnPerformOffensiveAction(effectTarget,ref resolution);

            if (userEventsHolder == targetEventsHolder) return;
            targetEventsHolder.OnReceiveOffensiveAction(user,ref resolution);
        }
    }
}
