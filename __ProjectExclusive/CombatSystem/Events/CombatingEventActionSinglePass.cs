using System.Collections.Generic;
using CombatEntity;
using CombatSkills;
using CombatSystem.CombatSkills;
using UnityEngine;

namespace CombatSystem.Events
{
    /// <summary>
    /// Meant to single call [<see cref="ISkillEventListener"/>]'s events instead of invoking in each
    /// effect (since skill have multiples effects). This is done to prevent multiple undesired invokes
    /// for passive, reactions and counter mechanics (where should be invoked once)
    /// </summary>
    public sealed class CombatingEventActionSinglePass : ISkillEventListener
    {

        public void OnSkillUse(SkillValuesHolders values)
        {
            var user = values.Performer;
            var targets = values.EffectTargets;


            var skill = values.UsedSkill;
            
            if(skill.GetTargetType() == EnumSkills.TargetType.Offensive)
                HandleOffensives();
            else
                HandleSupport();
            


            //-----
            void HandleOffensives()
            {
                foreach (var target in targets)
                {
                    CombatSystemSingleton.EventsHolder.OnReceiveOffensiveAction(values,target);
                }
            }
            void HandleSupport()
            {
                foreach (var target in targets)
                {
                    CombatSystemSingleton.EventsHolder.OnReceiveSupportAction(values,target);
                }
            }
        }


        public void OnSkillCostIncreases(SkillValuesHolders values)
        {
        }
    }
}
