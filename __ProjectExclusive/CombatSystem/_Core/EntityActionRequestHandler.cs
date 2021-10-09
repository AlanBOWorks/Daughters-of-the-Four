using System;
using System.Collections.Generic;
using __ProjectExclusive.Player;
using CombatEntity;
using CombatSkills;
using CombatSystem.Enemy;
using CombatSystem.Events;
using MEC;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace CombatSystem
{
    internal class EntityActionRequestHandler : IEntityTempoHandler
    {
        public EntityActionRequestHandler()
        {
            _actionPerformer = new ActionPerformer();
            _requestedActions = new Queue<SkillUsageValues>();
        }

        private readonly ActionPerformer _actionPerformer;


        [ShowInInspector]
        private readonly Queue<SkillUsageValues> _requestedActions;


        public IEnumerator<float> _RequestFinishActions(CombatingEntity entity)
        {
            return _LoopThroughActions(entity);
        }

        private static IEntitySkillRequestHandler GetTeamTempoController(CombatingEntity entity)
        {
            return CombatSystemSingleton.VolatilePlayerTeam.Contains(entity) //Is Players?

                ? PlayerCombatSingleton.EntitySkillRequestHandler
                : EnemyCombatSingleton.EntitySkillRequestHandler;
        }

        private IEnumerator<float> _LoopThroughActions(CombatingEntity actionPerformer)
        {
            var eventsHolder = CombatSystemSingleton.EventsHolder;

            yield return Timing.WaitForOneFrame; //TODO request start actions animation;

            eventsHolder.OnInitiativeTrigger(actionPerformer);
            var requestHandler = GetTeamTempoController(actionPerformer);

            requestHandler.OnRequestAction(actionPerformer,_requestedActions);
            yield return Timing.WaitForOneFrame;

            while (actionPerformer.CanAct())
            {
                while (_requestedActions.Count <= 0) yield return Timing.WaitForOneFrame;

                var request = _requestedActions.Dequeue();
                // TODO check if skillUsage is valid (maybe the target is dead, a problem happens or something)
                // if error is true; Clear the queue and Continue

                UtilsCombatStats.TickCurrentActions(actionPerformer.CombatStats);

                // TODO Animation Execute and remove this wait
                SkillParameters actionParameters =
                    new SkillParameters(request.UsedSkill,actionPerformer,request.Target);
                yield return Timing.WaitUntilDone(_actionPerformer._PerformSkill(actionParameters));


                eventsHolder.OnDoMoreActions(actionPerformer);
                requestHandler.OnDoMoreActions();
            }
            eventsHolder.OnFinishAllActions(actionPerformer);
        }


    }

    public interface IEntitySkillRequestHandler
    {
        // These parameters are for giving the player the possibility of choosing various action in one go
        // even in between animations; For AI this is not necessary since the AI can just decide in the fly.
        // More advance AI could inject a sequence of action in the queue so it feels more intelligent
        void OnRequestAction(CombatingEntity currentEntity, Queue<SkillUsageValues> injectSkillInQueue);
        void OnDoMoreActions();
        void OnFailRequest(CombatingEntity currentEntity, Queue<SkillUsageValues> injectSkillInQueue);
    }
}
