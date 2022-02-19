using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Stats;

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

        public void OnEntityRequestSequence(CombatEntity entity, bool canAct)
        {
        }

        public void OnEntityRequestControl(CombatEntity entity)
        {
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {

        }

        public void OnEntityFinishSequence(CombatEntity entity)
        {
            foreach (CombatSkill skill in entity.AllSkills)
            {
                skill.ResetCost();
            }
        }
    }
}
