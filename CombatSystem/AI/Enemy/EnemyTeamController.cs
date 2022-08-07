using System;
using System.Collections.Generic;
using System.Linq;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Stats;
using CombatSystem.Team;
using MEC;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CombatSystem.AI
{
    public class EnemyTeamController : CombatTeamControllerBase, ISkillUsageListener,
        ITempoControlStatesExtraListener
       
    {
        private static readonly IControllerHandler OnNullController = new RandomController();

        internal IControllerHandler DedicatedTeamController { set; private get; }
        private IControllerHandler GetEnemyController() => DedicatedTeamController ?? OnNullController;

       



        public override void InvokeStartControl()
        {
            DoNextControl();
        }

        private void DoNextControl()
        {
            var controller = GetEnemyController();
            controller.DoControl(this, out var values);
            var selectedSkill = values.UsedSkill;
            if (selectedSkill == null)
            {
                throw new ArgumentNullException(nameof(selectedSkill),"[Enemy Controller] -> Selected Skill is Null");
            }

            var onTarget = values.Target;
            if (onTarget == null)
            {
                throw new ArgumentNullException(nameof(onTarget), "[Enemy Controller] -> Selected target is Null");
            }

            var selectedActor = values.Performer;
            if (selectedActor == null)
            {
                throw new ArgumentNullException(nameof(selectedActor), "Enemy Controller] -> Performer is Null");
            }

            var eventHolder = CombatSystemSingleton.EventsHolder;
            eventHolder.OnCombatSkillSubmit(in values);
        }

        public void OnCombatSkillSubmit(in SkillUsageValues values)
        {

        }

        public void OnCombatSkillPerform(in SkillUsageValues values)
        {
            
        }

        public void OnCombatSkillFinish(CombatEntity performer)
        {
            Timing.RunCoroutine(_WaitForNextAction());
        }

        public void OnTempoPreStartControl(CombatTeamControllerBase controller, CombatEntity firstEntity)
        {
        }

        public void LateOnAllActorsNoActions(CombatEntity lastActor)
        {
            Timing.RunCoroutine(_WaitForFinishAction());
        }

        public void OnTempoFinishLastCall(CombatTeamControllerBase controller)
        {
        }

        // Problem: there's invoke racing with the AI submitting speed
        // Solution: wait until is ready
        private IEnumerator<float> _WaitForNextAction()
        {
            yield return Timing.WaitForOneFrame;
            if (ControllingTeam.CanControl())
                DoNextControl();
        }
        private IEnumerator<float> _WaitForFinishAction()
        {
            do
            {
                yield return Timing.WaitForOneFrame;

            } while (CombatSystemSingleton.SkillQueuePerformer.IsActing());
            
            InvokeFinishControl();
        }




        private sealed class RandomController : IControllerHandler
        {
            public void DoControl(CombatTeamControllerBase controller,
                out SkillUsageValues controlValues)
            {
                var selectedActor = SelectPerformer(controller);
                var skills = selectedActor.GetCurrentSkills();
                var selectedSkill = SelectSkill(skills);
                var target = SelectTarget(selectedActor, selectedSkill);

                controlValues = new SkillUsageValues(selectedActor,target, selectedSkill);
            }

            private static CombatEntity SelectPerformer(CombatTeamControllerBase controller)
            {
                var entities = controller.GetAllControllingMembers();
                if (entities.Count <= 0) return null;
                int randomPick = Random.Range(0, entities.Count);

                return entities[randomPick];
            }
            private static CombatSkill SelectSkill(IReadOnlyList<CombatSkill> skills)
            {
                int randomPick = Random.Range(0, skills.Count);
                return skills[randomPick];
            }
            private static CombatEntity SelectTarget(CombatEntity performer, ISkill skill)
            {
                var possibleTargets = UtilsTarget.GetPossibleTargets(skill, performer);
                var randomPick = Random.Range(0, possibleTargets.Count());

                return possibleTargets[randomPick];
            }
        }
    }


    internal interface IControllerHandler
    {
        void DoControl(CombatTeamControllerBase controller,
            out SkillUsageValues controlValues);
    }
}
