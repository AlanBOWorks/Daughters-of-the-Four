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
    public class EnemyTeamControllerRandom : CombatTeamControllerBase, ITempoTeamStatesListener, ITempoEntityStatesListener
    {
        public void OnTempoStartControl(in CombatTeamControllerBase controller, in CombatEntity firstEntity)
        {
            DoControl();  
            ForceFinish();
        }

        public void OnControlFinishAllActors(in CombatEntity lastActor)
        {
        }

        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
        }

        public void OnTempoFinishLastCall(in CombatTeamControllerBase controller)
        {
            Reset();
        }

        public void OnEntityRequestSequence(CombatEntity entity, bool canAct)
        {

        }

        public void OnEntityRequestAction(CombatEntity entity)
        {
            
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
        }

        public void OnEntityFinishSequence(CombatEntity entity, in bool isForcedByController)
        {
        }


        private void DoControl()
        {
            var entities = GetAllActiveMembers();
            var pick = PickEntity(in entities);
            HandleEntity(in pick);
        }


        private static CombatEntity PickEntity(in IReadOnlyList<CombatEntity> members)
        {
            int randomPick = Random.Range(0, members.Count-1);
            return members[randomPick];
        }


        private CombatEntity _currentControl;
        private void HandleEntity(in CombatEntity onEntity)
        {
            EnemyCombatSingleton.EnemyEventsHolder.OnControlEntitySelect(in onEntity);
            _currentControl = onEntity;
            var entitySkills = onEntity.GetCurrentSkills();


            HandleSkills(in entitySkills);
        }

        private void HandleSkills(in IReadOnlyList<CombatSkill> skills)
        {
            if (!UtilsCombatStats.CanActRequest(_currentControl)) return;

            var selectedSkill = SelectSkill(in skills);
            HandleSkill(in selectedSkill);
        }

        private static CombatSkill SelectSkill(in IReadOnlyList<CombatSkill> skills)
        {
            int randomPick = Random.Range(0, skills.Count - 1);
            return skills[randomPick];
        }


        private void HandleSkill(in CombatSkill skill)
        {
            var eventsHolder = EnemyCombatSingleton.EnemyEventsHolder;
            eventsHolder.OnControlSkillSelect(in skill);

            var target = SelectTarget(in skill);
            CombatSystemSingleton.EventsHolder.OnSkillSubmit(in _currentControl,in skill,in target);

            eventsHolder.OnTargetSelect(in target);
        }

        private CombatEntity SelectTarget(in CombatSkill skill)
        {
            var possibleTargets = UtilsTarget.GetPossibleTargets(skill, _currentControl);
            var randomPick = Random.Range(0, possibleTargets.Count());

            return possibleTargets[randomPick];
        }

        private void Reset()
        {
            _currentControl = null;
        }
    }
}
