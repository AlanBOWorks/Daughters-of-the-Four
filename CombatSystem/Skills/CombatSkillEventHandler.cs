using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Stats;

namespace CombatSystem.Skills
{
    public sealed class CombatSkillEventHandler : ISkillUsageListener, ITempoEntityStatesListener
    {
        public void OnSkillSubmit(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
            UtilsCombatStats.ReduceActions(performer.Stats, in usedSkill);
        }

        public void OnSkillPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
            usedSkill.DoSkill(in performer, in target);
        }

        public void OnEffectPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target, in IEffect effect)
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
            foreach (CombatSkill currentSkill in entity.AllSkills)
            {
                currentSkill.ResetCost();
            }
        }
    }
}
