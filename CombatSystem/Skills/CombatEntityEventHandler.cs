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
        public void OnCombatSkillPreSubmit(in CombatEntity performer,in CombatSkill usedSkill)
        {
            var performerStats = performer.Stats;
            UtilsCombatStats.TickActions(performerStats, usedSkill);
            usedSkill.IncreaseCost();
        }

        public void OnCombatSkillSubmit(in CombatEntity performer, in CombatSkill usedSkill)
        {
           

            var eventHolder = CombatSystemSingleton.EventsHolder;
            eventHolder.OnEntityBeforeSkill(performer);


            if (UtilsCombatStats.CanControlRequest(performer)) return;
            var performerTeam = performer.Team;
            performerTeam.RemoveFromControllingEntities(performer, false);

            eventHolder.OnEntityEmptyActions(performer);

            if(performerTeam.CanControl()) return;
            eventHolder.OnAllActorsNoActions(performer);
        }

        public void OnCombatSkillPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
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
            eventsHolder.OnAfterEntityRequestSequence(entity);
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
            bool canAct = UtilsCombatStats.CanControlRequest(entity);
            var eventHolder = CombatSystemSingleton.EventsHolder;
            if (!canAct)
                eventHolder.OnEntityFinishSequence(entity, false);
            else
                eventHolder.OnEntityRequestAction(entity);
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

        public void TryCallOnFinishAllActors(in CombatEntity entity)
        {
            var entityTeam = entity.Team;
            bool hasMoreEntitiesToControl = entityTeam.CanControl();
            if (hasMoreEntitiesToControl) return;


            var controllingMembers = entityTeam.GetControllingMembers();
            if (controllingMembers.Count > 0) return;

            var eventsHolder = CombatSystemSingleton.EventsHolder;
            eventsHolder.OnControlFinishAllActors(entity);
        }
    }
}
