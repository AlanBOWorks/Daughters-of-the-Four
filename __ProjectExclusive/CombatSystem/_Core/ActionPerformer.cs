using System.Collections.Generic;
using CombatEffects;
using CombatSkills;
using MEC;
using Stats;
using UnityEngine;

namespace CombatSystem
{
    public class ActionPerformer
    {
        private const float SpaceBetweenAnimations = .12f;
        public IEnumerator<float> _PerformSkill(SkillValuesHolders values)
        {
            values.Target.GuardHandler.VariateTarget(values);
            values.RollForCritical();

            yield return Timing.WaitForSeconds(SpaceBetweenAnimations);

            var entityHolder = values.Performer.InstantiatedHolder;

            if(entityHolder != null && entityHolder.AnimationHandler != null)
                yield return Timing.WaitUntilDone(entityHolder.AnimationHandler._DoPerformSkillAnimation(values));

            yield return Timing.WaitForOneFrame;
            DoEffects(values);

            yield return Timing.WaitForSeconds(SpaceBetweenAnimations);
        }

        private static void DoEffects(SkillValuesHolders values)
        {
            var skill = values.UsedSkill;
            var effects = skill.GetEffects();

            foreach (EffectParameter effectParameter in effects)
            {
                effectParameter.DoEffect(values);
            }
            skill.PutInCooldown();
            CombatSystemSingleton.EventsHolder.OnSkillUse(values);
        }
    }
}
