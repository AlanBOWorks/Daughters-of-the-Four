using System.Collections.Generic;
using CombatSkills;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Events
{
    


#if UNITY_EDITOR
    internal class DebugSkillEvents : ISkillEventListener
    {
        public void OnSkillUse(SkillValuesHolders values)
        {
            Debug.Log($"Used Skill: {values.UsedSkill.GetSkillName()}____ \n" +
                      $"- Performer >>> {values.Performer.GetEntityName()} \n" +
                      $"- Target >>> {values.Target.GetEntityName()}");
        }

        public void OnSkillCostIncreases(SkillValuesHolders values)
        {
        }
    }


#endif
}
