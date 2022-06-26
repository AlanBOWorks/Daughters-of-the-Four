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
    public class EnemyTeamControllerRandom : CombatTeamControllerBase, ITempoTeamStatesListener, ITempoTeamStatesExtraListener,
        ISkillUsageListener
    {
        public override void InvokeStartControl()
        {
            StartControl();
        }

        public void OnTempoPreStartControl(CombatTeamControllerBase controller)
        {
        }

        public void OnTempoStartControl(CombatTeamControllerBase controller)
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

        public void OnCombatSkillSubmit(in SkillUsageValues values)
        {
            Step();
        }

        public void OnCombatSkillPerform(in SkillUsageValues values)
        {
        }
        public void OnCombatSkillFinish(CombatEntity performer)
        {
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
            if (!UtilsCombatStats.CanControlAct(_currentControl)) return;

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
            SkillUsageValues values = new SkillUsageValues(_currentControl,target,skill);
            CombatSystemSingleton.EventsHolder.OnCombatSkillSubmit(in values);
        }

        private CombatEntity SelectTarget(in CombatSkill skill)
        {
            var possibleTargets = UtilsTarget.GetPossibleTargets(skill, _currentControl);
            var randomPick = Random.Range(0, possibleTargets.Count());

            return possibleTargets[randomPick];
        }

    }
}
