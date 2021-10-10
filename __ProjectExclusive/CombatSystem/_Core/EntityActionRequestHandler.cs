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
            _skillValues = new SkillValuesHolders();
        }

        private readonly ActionPerformer _actionPerformer;
        [ShowInInspector]
        private readonly SkillValuesHolders _skillValues;



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

        private IEnumerator<float> _LoopThroughActions(CombatingEntity currentActingEntity)
        {
            var eventsHolder = CombatSystemSingleton.EventsHolder;

            yield return Timing.WaitForOneFrame; //TODO request start actions animation;

            _skillValues.Inject(currentActingEntity);
            eventsHolder.OnInitiativeTrigger(currentActingEntity);
            var requestHandler = GetTeamTempoController(currentActingEntity);


            yield return Timing.WaitForOneFrame;

            while (currentActingEntity.CanAct())
            {
                do
                {
                    _skillValues.OnActionClear();
                    yield return Timing.WaitUntilDone(
                        requestHandler.OnRequestAction(_skillValues));
                } while (!_skillValues.IsValid());
                

                // TODO check if skillUsage is valid (maybe the target is dead, a problem happens or something)
                // if error is true; Clear the queue and Continue

                UtilsCombatStats.TickCurrentActions(currentActingEntity.CombatStats);

                // TODO Animation Execute and remove this wait
               
                yield return Timing.WaitUntilDone(_actionPerformer._PerformSkill(_skillValues));
                _skillValues.Clear();

                eventsHolder.OnDoMoreActions(currentActingEntity);
            }
            eventsHolder.OnFinishAllActions(currentActingEntity);
        }


    }

    public interface IEntitySkillRequestHandler
    {
        // These parameters are for giving the player the possibility of choosing various action in one go
        // even in between animations; For AI this is not necessary since the AI can just decide in the fly.
        // More advance AI could inject a sequence of action in the queue so it feels more intelligent
        IEnumerator<float> OnRequestAction(SkillValuesHolders skillValues);

    }
}
