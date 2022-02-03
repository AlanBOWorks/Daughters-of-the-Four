using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Stats;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    public sealed class CombatTeamControllersHandler : IOppositionTeamStructureRead<ITeamController>,
        ITempoEntityStatesListener
    {

        private static readonly CombatTeamControllerRandom NullFallback;

        static CombatTeamControllersHandler()
        {
            NullFallback = new CombatTeamControllerRandom();
        }

        public CombatTeamControllersHandler() : 
            this(NullFallback, NullFallback)
        { }

        public CombatTeamControllersHandler(ITeamController playerController) 
        : this(playerController, NullFallback)
        { }

        public CombatTeamControllersHandler(ITeamController playerController, ITeamController enemyController)
        {
            PlayerTeamType = playerController;
            EnemyTeamType = enemyController;
        }
        public ITeamController CurrentController { get; private set; }

        [ShowInInspector,HorizontalGroup()]
        public ITeamController PlayerTeamType { get; set; }
        [ShowInInspector,HorizontalGroup()]
        public ITeamController EnemyTeamType { get; set; }

        public bool CurrentControllerHasFinish() => CurrentController.HasFinish();


        public void OnEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            if(!canAct) return;

            var controller = UtilsTeam.GetElement(entity, this);
            CurrentController = controller;
            CurrentController.Injection(entity);
        }

        public void OnEntityRequestControl(CombatEntity entity)
        {
            DoRequest(entity);
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
            DoRequest(entity);
        }

        public void OnEntityFinishSequence(CombatEntity entity)
        {
        }

        private void DoRequest(CombatEntity actingEntity)
        {
            var controller = CurrentController;

            var controlCoroutine = Timing.RunCoroutine(_DoControl());
            CombatSystemSingleton.LinkCoroutineToMaster(in controlCoroutine);

            IEnumerator<float> _DoControl()
            {
                while (!controller.HasFinish())
                {
                    yield return Timing.WaitUntilDone(controller._ReadyToRequest(actingEntity));
                    controller.RequestAction(actingEntity, out var usedSkill, out var onTarget);
                    yield return Timing.WaitForOneFrame;
                    var eventsHolder = CombatSystemSingleton.EventsHolder;
                    eventsHolder.OnSkillSubmit(in actingEntity, in usedSkill, in onTarget);
                    yield return Timing.WaitForOneFrame;
                    eventsHolder.OnSkillPerform(in actingEntity, in usedSkill, in onTarget);
                }
            }
        }

    }

    public sealed class CombatTeamControllerRandom : ITeamController
    {
        private CombatStats _controlling;

        public void Injection(CombatEntity entity)
        {
            _controlling = entity.Stats;
        }

        public IEnumerator<float> _ReadyToRequest(CombatEntity performer)
        {
            //yield return Timing.WaitForOneFrame;
            yield return Timing.WaitForSeconds(10);
        }

        public void RequestAction(CombatEntity performer, out CombatSkill usedSkill, out CombatEntity target)
        {
            var currentSkills = performer.GetCurrentSkills();
            int skillsAmount;
            if (currentSkills == null || (skillsAmount = currentSkills.Count) <= 0)
            {
                usedSkill = null;
                target = null;
                return;
            }

            var randomPick = Random.Range(0, skillsAmount -1);

            usedSkill = currentSkills[randomPick];

            var possibleTargets = UtilsTarget.GetPossibleTargets(usedSkill, performer);

            randomPick = Random.Range(0, possibleTargets.Count - 1);
            target = possibleTargets[randomPick];
        }

        public bool HasFinish()
        {
            return !UtilsCombatStats.CanActRequest(in _controlling);
        }
    }
}
