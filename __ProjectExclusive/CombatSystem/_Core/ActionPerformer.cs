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
        public ActionPerformer()
        {
            _values = new SkillValuesHolders();
        }

        private readonly SkillValuesHolders _values;

        private const float SpaceBetweenAnimations = .12f;
        public IEnumerator<float> _PerformSkill(SkillParameters skillParameters)
        {
            _values.Inject(skillParameters);
            _values.RollForCritical();

            yield return Timing.WaitForSeconds(SpaceBetweenAnimations);

            //Todo for animation; wait until animation finish or invoked next step (by animation events)
            yield return Timing.WaitForOneFrame;
            DoEffects(_values);

            yield return Timing.WaitForSeconds(SpaceBetweenAnimations);
        }

        private static void DoEffects(SkillValuesHolders values)
        {
            var user = values.User;
            var skill = values.UsedSkill;
            var effects = skill.GetEffects();

            foreach (EffectParameter effectParameter in effects)
            {
                DoEffect(effectParameter);
            }
            CombatSystemSingleton.EventsHolder.OnSkillUse(values);
            values.Clear();


            void DoEffect(EffectParameter effect)
            {
                effect.DoEffect(values);
            }
        }
    }
}
