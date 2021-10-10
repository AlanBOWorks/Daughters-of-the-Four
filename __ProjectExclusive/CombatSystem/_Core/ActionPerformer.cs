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
            values.RollForCritical();

            yield return Timing.WaitForSeconds(SpaceBetweenAnimations);

            var entityHolder = values.User.InstantiatedHolder;
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
                DoEffect(effectParameter);
            }
            CombatSystemSingleton.EventsHolder.OnSkillUse(values);


            void DoEffect(EffectParameter effect)
            {
                effect.DoEffect(values);
            }
        }
    }
}
