using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills.Effects;
using CombatSystem.Stats;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Skills
{
    public sealed class CombatEntityEventHandler 
    {
        public void OnCombatSkillPreSubmit(ICombatSkill usedSkill, CombatEntity performer)
        {
            var performerStats = performer.Stats;
            UtilsCombatStats.TickActions(performerStats, usedSkill);
            usedSkill.IncreaseCost();
        }

        public void OnCombatSkillSubmit(ICombatSkill usedSkill, CombatEntity performer)
        {
            var eventHolder = CombatSystemSingleton.EventsHolder;
            eventHolder.OnEntityBeforeSkill(performer);

            bool canKeepActing = UtilsCombatStats.CanControlAct(performer);
            if (canKeepActing) return;

            var performerTeam = performer.Team;
            performerTeam.RemoveFromControllingEntities(performer, false);

            eventHolder.OnEntityEmptyActions(performer);

            if(performerTeam.CanControl()) return;
            eventHolder.OnAllActorsNoActions(performer);
        }

        public void OnCombatSkillPerform(ICombatSkill usedSkill, CombatEntity performer, CombatEntity target)
        {
            UtilsCombatSkill.DoSkillOnTarget(usedSkill,performer, target);
        }

        public void OnEntityRequestSequence(CombatEntity entity, bool canControl)
        {
            entity.OnSequenceStart();
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            if (!canControl)
            {
                eventsHolder.OnNoActionsForcedFinish(entity);
                return;
            }
            HandleEntityTeamOnRequestSequence(entity);
            eventsHolder.OnAfterEntityRequestSequence(entity);
        }
        private static void HandleEntityTeamOnRequestSequence(CombatEntity entity)
        {
            var entityTeam = entity.Team;
            entityTeam.OnEntityRequestSequence(entity);
        }


        public void OnEntityRequestAction(CombatEntity entity)
        {
            entity.OnActionStart();
        }


        public void OnEntityFinishAction(CombatEntity entity)
        {
            entity.OnActionFinish();
           
        }

        public void OnEntityAfterFinishAction(in CombatEntity entity)
        {
            bool hasActionsLeft = UtilsCombatStats.HasActionsLeft(entity.Stats);
            var eventHolder = CombatSystemSingleton.EventsHolder;
            if (hasActionsLeft)
                eventHolder.OnEntityRequestAction(entity);
            else
                eventHolder.OnEntityFinishSequence(entity, false);
        }

        public void OnEntityFinishSequence(CombatEntity entity,in bool isForcedByController)
        {
            foreach (CombatSkill skill in entity.AllSkills)
            {
                skill.ResetCost();
            }

            entity.OnSequenceFinish();

            var eventsHolder = CombatSystemSingleton.EventsHolder;
            var isTrinityRole = UtilsTeam.IsTrinityRole(in entity);
            if (isTrinityRole)
                eventsHolder.OnTrinityEntityFinishSequence(entity);
            else
                eventsHolder.OnOffEntityFinishSequence(entity);

            eventsHolder.OnAfterEntitySequenceFinish(entity);
        }


        public void RequestEntityAction(in CombatEntity entity)
        {
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            eventsHolder.OnEntityRequestAction(entity);
        }

    }
}
