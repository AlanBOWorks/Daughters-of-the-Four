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
        private const float MaxWaitBetweenAnimations = 1f; 
        public IEnumerator<float> _PerformSkill(SkillValuesHolders values)
        {
            values.Target.GuardHandler.VariateTarget(values);
            values.RollForCritical();

            yield return Timing.WaitForSeconds(SpaceBetweenAnimations);

            var performer = values.Performer;
            var entityHolder = performer.InstantiatedHolder;
            var animationHandler = entityHolder.AnimationHandler;

            if (entityHolder != null && animationHandler != null)
            {
                // Todo check for special animation and do wait below
                // yield return Timing.WaitUntilDone(animationHandler._DoPerformSkillAnimation(values));
                animationHandler.DoPerformSkillAnimation(values);
                yield return Timing.WaitForSeconds(MaxWaitBetweenAnimations);
            }

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
                mainEffect.DoActionEffect(values);

            foreach (EffectParameter effectParameter in effects)
            {
                effectParameter.DoActionEffect(values);
            }
            if (skill.IsMainEffectAfterListEffects)
                mainEffect.DoActionEffect(values);



            skill.OnUseIncreaseCost();
        }

    }
}
