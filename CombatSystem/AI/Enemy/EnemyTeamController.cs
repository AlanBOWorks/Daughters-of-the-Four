using System.Collections.Generic;
using System.Linq;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Stats;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.AI
{
    public class EnemyTeamController : CombatTeamControllerBase, 
        ITempoControlStatesListener,
        ITempoControlStatesExtraListener,
        ITempoEntityActionStatesListener
    {
        public override void InvokeStartControl()
        {
            StartControl();
        }


        public void OnEntityRequestAction(CombatEntity entity)
        {
        }

        public void OnEntityBeforeSkill(CombatEntity entity)
        {
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
            DoNextControl();
        }

        public void OnEntityEmptyActions(CombatEntity entity)
        {
            DoNextControl();
        }

        public void OnTempoPreStartControl(CombatTeamControllerBase controller, CombatEntity firstEntity)
        {
        }

        public void OnTempoStartControl(CombatTeamControllerBase controller, CombatEntity firstControl)
        {
            
        }

        public void OnAllActorsNoActions(CombatEntity lastActor)
        {
            ForceFinish();
        }

        public void OnTempoFinishControl(CombatTeamControllerBase controller)
        {
        }

        public void OnTempoFinishLastCall(CombatTeamControllerBase controller)
        {
        }

        private void StartControl()
        {
            DoNextControl();
        }


        private void DoNextControl()
        {
            var entities = GetAllControllingMembers();
            if(entities.Count <= 0) return;

            var pick = PickEntity(entities);
            HandleEntity(pick);

        }


        private static CombatEntity PickEntity(IReadOnlyList<CombatEntity> members)
        {
            int randomPick = Random.Range(0, members.Count);
            return members[randomPick];
        }


        private CombatEntity _currentControl;
        private void HandleEntity(CombatEntity onEntity)
        {
            EnemyCombatSingleton.EnemyEventsHolder.OnControlEntitySelect(onEntity);
            _currentControl = onEntity;
            var entitySkills = onEntity.GetCurrentSkills();


            HandleSkills(entitySkills);
        }

        private void HandleSkills(IReadOnlyList<CombatSkill> skills)
        {
            var selectedSkill = SelectSkill(skills);
            HandleSkill(in selectedSkill);
        }

        private static CombatSkill SelectSkill(IReadOnlyList<CombatSkill> skills)
        {
            int randomPick = Random.Range(0, skills.Count - 1);
            return skills[randomPick];
        }


        private void HandleSkill(in CombatSkill skill)
        {
            var eventsHolder = EnemyCombatSingleton.EnemyEventsHolder;
            eventsHolder.OnControlSkillSelect(in skill);

            var target = SelectTarget(in skill);

            eventsHolder.OnTargetSelect(in target);
            SkillUsageValues values = new SkillUsageValues(_currentControl,target,skill);
            CombatSystemSingleton.EventsHolder.OnCombatSkillSubmit(in values);
        }

        private CombatEntity SelectTarget(in CombatSkill skill)
        {
            var possibleTargets = UtilsTarget.GetPossibleTargets(skill, _currentControl);
            var randomPick = Random.Range(0, possibleTargets.Count());

            return possibleTargets[randomPick];
        }


        private interface IControllerHandler
        {
            
        }

    }
}
