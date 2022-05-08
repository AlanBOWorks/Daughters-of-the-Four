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
    public class EnemyTeamControllerRandom : CombatTeamControllerBase, ITempoTeamStatesListener, ISkillUsageListener
    {
        public override void InvokeStartControl()
        {
            StartControl();
        }

        public void OnTempoPreStartControl(in CombatTeamControllerBase controller)
        {
        }

        public void OnControlFinishAllActors(in CombatEntity lastActor)
        {
            ForceFinish();
        }

        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
        }

        public void OnTempoFinishLastCall(in CombatTeamControllerBase controller)
        {
        }

        public void OnSkillSubmit(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
        }

        public void OnSkillPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
        }

        public void OnEffectPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target, in IEffect effect)
        {
        }

        public void OnSkillFinish()
        {
            Step();
        }


        private void StartControl()
        {
            DoControl();
        }

        private void Step()
        {
            DoControl();
        }

        private void DoControl()
        {
            var entities = GetAllControllingMembers();
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
            if (!UtilsCombatStats.CanControlRequest(_currentControl)) return;

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

            eventsHolder.OnTargetSelect(in target);
            CombatSystemSingleton.EventsHolder.OnSkillSubmit(in _currentControl, in skill, in target);
        }

        private CombatEntity SelectTarget(in CombatSkill skill)
        {
            var possibleTargets = UtilsTarget.GetPossibleTargets(skill, _currentControl);
            var randomPick = Random.Range(0, possibleTargets.Count());

            return possibleTargets[randomPick];
        }

    }
}
