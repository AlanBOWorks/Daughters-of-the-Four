using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;

namespace CombatSystem.Skills
{
    public sealed class CombatSkillEventHandler : ISkillUsageListener, ITempoEntityStatesListener, ITempoTeamStatesListener
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

        public void OnMainEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            entity.OnSequenceStart();
        }

        public void OnOffEntityRequestSequence(CombatEntity entity, bool canAct)
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

        public void OnEntityFinishSequence(CombatEntity entity)
        {
            foreach (CombatSkill skill in entity.AllSkills)
            {
                skill.ResetCost();
            }

            entity.OnSequenceFinish();
        }

        public void OnTempoFinishControl(CombatEntity mainEntity)
        {
            OnEntityFinishSequence(mainEntity);
        }

        public void OnTempoStartControl(in CombatTeamControllerBase controller)
        {
            
        }

        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
            var currentEntities = controller.Dictionary;
            foreach (var pair in currentEntities)
            {
                OnEntityFinishSequence(pair.Key);
            }
        }
    }
}
