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
        public void DoEffect(ISkillParameters parameters, float effectValue, bool isCritical)
        {
            var user = parameters.Performer;
            var effectTargets = parameters.EffectTargets;

            var eventsHolder = CombatSystemSingleton.EventsHolder;
            foreach (var effectTarget in effectTargets)
            {
                var effectResolution = DoEffectOn(user, effectTarget, effectValue, isCritical);
                DoEventCall(eventsHolder,parameters,ref effectResolution);
            }
        }

        protected abstract void DoEventCall(SystemEventsHolder systemEvents, ISkillParameters parameters,
            ref SkillComponentResolution resolution);
        protected abstract SkillComponentResolution DoEffectOn(
            CombatingEntity user, CombatingEntity effectTarget, float effectValue, bool isCritical);
    }


    public abstract class SOffensiveEffect : SEffect
    {
        protected override void DoEventCall(SystemEventsHolder systemEvents, ISkillParameters parameters,
            ref SkillComponentResolution resolution)
        {
            systemEvents.OnReceiveOffensiveEffect(parameters,ref resolution);
        }
    }

    public abstract class SSupportEffect : SEffect
    {
        protected override void DoEventCall(SystemEventsHolder systemEvents, ISkillParameters parameters,
            ref SkillComponentResolution resolution)
        {
            systemEvents.OnReceiveSupportEffect(parameters, ref resolution);
        }
    }
}
