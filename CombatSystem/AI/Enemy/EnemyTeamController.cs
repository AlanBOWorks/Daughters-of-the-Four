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
    public class EnemyTeamControllerRandom : CombatTeamControllerBase, ITempoTeamStatesListener,
        ITempoEntityStatesExtraListener
    {
        private IReadOnlyList<CombatEntity> _activeEntities;
        public void OnTempoStartControl(in CombatTeamControllerBase controller, in CombatEntity firstEntity)
        {
            _activeEntities = controller.GetAllActiveMembers();
            HandleControl(in firstEntity);
        }

        public void OnControlFinishAllActors(in CombatEntity lastActor)
        {
            ForceFinish();
        }

        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
        }


        public void OnAfterEntityRequestSequence(in CombatEntity entity)
        {
        }
        public void OnAfterEntitySequenceFinish(in CombatEntity entity)
        {
            if (_activeEntities.Count <= 0) return;

            var nextEntity = _activeEntities[0];
            HandleControl(in nextEntity);
        }

        public void OnNoActionsForcedFinish(in CombatEntity entity)
        {
        }

        private CombatEntity _currentControl;
        private void HandleControl(in CombatEntity onEntity)
        {
            _currentControl = onEntity;
            var entitySkills = onEntity.GetCurrentSkills();
            HandleSkills(in entitySkills);
        }

        private void HandleSkills(in IReadOnlyList<CombatSkill> skills)
        {
            if(!UtilsCombatStats.CanActRequest(_currentControl)) return;

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
