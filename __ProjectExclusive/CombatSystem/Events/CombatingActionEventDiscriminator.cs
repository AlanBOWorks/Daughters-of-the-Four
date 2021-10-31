using System.Collections.Generic;
using CombatEntity;
using CombatSkills;
using CombatSystem.CombatSkills;
using UnityEngine;

namespace CombatSystem.Events
{
    public sealed class CombatingActionEventDiscriminator : ISkillEventListener
    {

        public void OnSkillUse(SkillValuesHolders values)
        {
            var user = values.Performer;
            var target = values.Target;

            if(user == target) return;

            var entitiesPair = new CombatEntityPairAction(user,target);
            var skill = values.UsedSkill;
            
            if(skill.GetTargetType() == EnumSkills.TargetType.Offensive)
                CombatSystemSingleton.EventsHolder.OnReceiveOffensiveAction(entitiesPair,skill);
            else
                CombatSystemSingleton.EventsHolder.OnReceiveSupportAction(entitiesPair,skill);
        }

    }
}
