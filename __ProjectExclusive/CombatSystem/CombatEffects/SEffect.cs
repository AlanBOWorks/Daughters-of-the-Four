using CombatEntity;
using CombatSkills;
using CombatSystem;
using CombatSystem.CombatSkills;
using CombatSystem.Events;
using UnityEngine;

namespace CombatEffects
{
    public abstract class SEffect : SSkillComponentEffect, IEffect
    {
        public void DoEffect(CombatEntityPairAction entities, EnumEffects.TargetType effectTargetType, float effectValue, bool isCritical)
        {
            var user = entities.User;
            var skillTarget = entities.Target;
            var effectTargets = UtilsTarget.GetPossibleTargets(user, skillTarget, effectTargetType);

            var eventsHolder = CombatSystemSingleton.EventsHolder;
            foreach (var effectTarget in effectTargets)
            {
                var effectResolution = DoEffectOn(user, effectTarget, effectValue, isCritical);
                DoEventCall(eventsHolder,entities,ref effectResolution);
            }
        }

        protected abstract void DoEventCall(SystemEventsHolder systemEvents,CombatEntityPairAction entities,ref SkillComponentResolution resolution);
        protected abstract SkillComponentResolution DoEffectOn(
            CombatingEntity user, CombatingEntity effectTarget, float effectValue, bool isCritical);
    }


    public abstract class SOffensiveEffect : SEffect
    {
        protected override void DoEventCall(SystemEventsHolder systemEvents, CombatEntityPairAction entities,
            ref SkillComponentResolution resolution)
        {
            systemEvents.OnReceiveOffensiveAction(entities,ref resolution);
        }
    }

    public abstract class SSupportEffect : SEffect
    {
        protected override void DoEventCall(SystemEventsHolder systemEvents, CombatEntityPairAction entities,
            ref SkillComponentResolution resolution)
        {
            systemEvents.OnReceiveSupportAction(entities, ref resolution);
        }
    }
}
