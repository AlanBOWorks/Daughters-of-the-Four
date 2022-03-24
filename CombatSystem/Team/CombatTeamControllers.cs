using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Stats;
using MEC;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

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

        private bool _hasFinishCurrentEntity;
        public bool CurrentControllerHasFinish() => _hasFinishCurrentEntity;


        public void OnEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            if(!canAct) return;

            var controller = UtilsTeam.GetElement(entity, this);
            CurrentController = controller;
            CurrentController.InjectionOnRequestSequence(entity);

            DoRequest(entity);
        }

        public void OnEntityRequestAction(CombatEntity entity)
        {
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
        }

        public void OnEntityFinishSequence(CombatEntity entity)
        {
        }

        private CoroutineHandle _controlHandle;
        private void DoRequest(CombatEntity actingEntity)
        {
            if (_controlHandle.IsRunning)
                throw new AccessViolationException("Requesting control while there's an active control already");


            var controller = CurrentController;

            _controlHandle = Timing.RunCoroutine(_DoControl());
            CombatSystemSingleton.LinkCoroutineToMaster(in _controlHandle);

            IEnumerator<float> _DoControl()
            {
                UpdateHasFinishCurrentEntity();
                var eventsHolder = CombatSystemSingleton.EventsHolder;

                yield return Timing.WaitForOneFrame; //safe wait
                while (!_hasFinishCurrentEntity)
                {
                    eventsHolder.OnEntityRequestAction(actingEntity);
                    yield return Timing.WaitUntilDone(
                        controller._ReadyToRequest(actingEntity));

                    controller.PerformRequestAction(actingEntity, out var usedSkill, out var onTarget);
                    yield return Timing.WaitForOneFrame;

                    eventsHolder.OnSkillSubmit(in actingEntity, in usedSkill,in onTarget);
                    UpdateHasFinishCurrentEntity();
                }

                //_hasFinishCurrentEntity = true; this always happens because the while(!(_hastFinishCurrentEntity == true)) happens
            }

            void UpdateHasFinishCurrentEntity()
            {
                _hasFinishCurrentEntity = 
                    !UtilsCombatStats.CanActRequest(actingEntity) 
                    || controller.HasForcedFinishControlling();
            }
        }

    }

    public sealed class CombatTeamControllerRandom : ITeamController
    {
        private CombatStats _controlling;

        public void InjectionOnRequestSequence(CombatEntity entity)
        {
            _controlling = entity.Stats;
        }

        public IEnumerator<float> _ReadyToRequest(CombatEntity performer)
        {
            //yield return Timing.WaitForOneFrame;
            yield return Timing.WaitForSeconds(10);
        }

        public void PerformRequestAction(CombatEntity performer, out CombatSkill usedSkill, out CombatEntity target)
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

        public bool HasForcedFinishControlling()
        {
            return !UtilsCombatStats.CanActRequest(in _controlling);
        }
    }
}
