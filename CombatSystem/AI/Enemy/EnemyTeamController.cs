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
    public class EnemyTeamControllerRandom : CombatTeamControllerBase, ITempoTeamStatesListener
    {
        public void OnTempoStartControl(in CombatTeamControllerBase controller, in CombatEntity firstEntity)
        {
            var activeEntities = GetAllActiveMembers();
            HandleEntities(in activeEntities);
        }

        public void OnControlFinishAllActors(in CombatEntity lastActor)
        {
        }

        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
        }

        public void OnTempoFinishLastCall(in CombatTeamControllerBase controller)
        {
        }

        private void HandleEntities(in IReadOnlyList<CombatEntity> members)
        {
            while (members.Count > 0)
            {
                var actor = members[0];
                HandleEntity(in actor);
            }
            ForceFinish();
        }


        private CombatEntity _currentControl;
        private void HandleEntity(in CombatEntity onEntity)
        {
            _currentControl = onEntity;
            var entitySkills = onEntity.GetCurrentSkills();
            HandleSkills(in entitySkills);
        }

        private void HandleSkills(in IReadOnlyList<CombatSkill> skills)
        {
            while (UtilsCombatStats.CanActRequest(_currentControl))
            {
                var selectedSkill = SelectSkill(in skills);
                HandleSkill(in selectedSkill);
            }
        }

        private static CombatSkill SelectSkill(in IReadOnlyList<CombatSkill> skills)
        {
            int randomPick = Random.Range(0, skills.Count - 1);
            return skills[randomPick];
        }


        private void HandleSkill(in CombatSkill skill)
        {
            var target = SelectTarget(in skill);
            CombatSystemSingleton.EventsHolder.OnSkillSubmit(in _currentControl,in skill,in target);
        }

        private CombatEntity SelectTarget(in CombatSkill skill)
        {
            var possibleTargets = UtilsTarget.GetPossibleTargets(skill, _currentControl);
            var randomPick = Random.Range(0, possibleTargets.Count());

            return possibleTargets[randomPick];
        }
    }
}
