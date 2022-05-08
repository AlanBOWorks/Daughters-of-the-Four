using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Skills
{
    public sealed class CombatSkillEventHandler : ISkillUsageListener, ITempoEntityStatesListener
    {
        public void OnSkillSubmit(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
            UtilsCombatStats.TickActions(performer.Stats, in usedSkill);
        }

        public void OnSkillPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
            usedSkill.DoSkill(in performer, in target);
            usedSkill.IncreaseCost();
        }

        public void OnEffectPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target, in IEffect effect)
        {

        }

        public void OnSkillFinish()
        {
            
        }

        public void OnEntityRequestSequence(CombatEntity entity, bool canControl)
        {
            entity.OnSequenceStart();
        }

        public void OnEntityRequestAction(CombatEntity entity)
        {
            entity.OnActionStart();
        }


        public void OnEntityFinishAction(CombatEntity entity)
        {
            entity.OnActionFinish();
        }

        public void OnEntityFinishSequence(CombatEntity entity,in bool isForcedByController)
        {
            foreach (CombatSkill skill in entity.AllSkills)
            {
                skill.ResetCost();
            }

            entity.OnSequenceFinish();
        }
    }
}
