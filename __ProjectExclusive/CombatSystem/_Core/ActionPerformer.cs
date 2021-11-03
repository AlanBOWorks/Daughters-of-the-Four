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
            PerformSkill(values);

            yield return Timing.WaitForSeconds(SpaceBetweenAnimations);
        }

        private static void PerformSkill(SkillValuesHolders values)
        {
            var skill = values.UsedSkill;
            var mainEffect = skill.GetMainEffect();
            var effects = skill.GetEffects();


            // Before effects because OnSkillUse could have buff/reaction effects that mitigates/amplified effects
            CombatSystemSingleton.EventsHolder.OnSkillUse(values);

            if(!skill.IsMainEffectAfterListEffects)
                mainEffect.DoEffect(values);

            foreach (EffectParameter effectParameter in effects)
            {
                effectParameter.DoEffect(values);
            }
            if (skill.IsMainEffectAfterListEffects)
                mainEffect.DoEffect(values);



            skill.PutInCooldown();
        }

    }
}
