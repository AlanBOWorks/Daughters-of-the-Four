using CombatEffects;
using CombatEntity;
using CombatSkills;
using UnityEngine;

namespace Stats
{
    public static class UtilsStatsEvents 
    {
        public static void CallOffensiveEvent(CombatingEntity user, CombatingEntity target, EffectResolution resolution)
        {
            var targetEvents = target.EventsHolder;
            var userEvents = user.EventsHolder;

            targetEvents.OnReceiveOffensiveAction(user,resolution);
            if(resolution.UsedEffect is SDamageEffect)
                targetEvents.OnDamageReceiveAction(user,resolution);

            userEvents.OnPerformOffensiveAction(target,resolution);
        }

        public static void CallProtectionEvent(CombatingEntity user, CombatingEntity target, EffectResolution resolution)
        {
        }
    }
}
