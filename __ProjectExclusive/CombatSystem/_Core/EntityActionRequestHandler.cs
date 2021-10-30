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
            eventsHolder.OnFirstAction(currentActingEntity);
            var requestHandler = GetTeamTempoController(currentActingEntity);


            yield return Timing.WaitForOneFrame;

            while (currentActingEntity.CanAct())
            {
                do
                {
                    _skillValues.OnActionClear();
                    // using waitUntilDone except waitUntilTrue(_skillValues.IsValid) is because the player
                    // might use these values correctly but changes the opinion and switch same values.
                    yield return Timing.WaitUntilDone(
                        requestHandler.HandleRequestAction(_skillValues));
                } while (!_skillValues.IsValid());
                
                yield return Timing.WaitUntilDone(_actionPerformer._PerformSkill(_skillValues));
                eventsHolder.OnFinishAction(currentActingEntity);
                // Death events are meant to be the last events to be send (since some previous events could prevent death
                // conditions and this could create false positives)
                CombatSystemSingleton.EntityDeathHandler.HandleDeaths(); 
            }
            eventsHolder.OnFinishAllActions(currentActingEntity);
            _skillValues.Clear();
        }


    }

    public interface IEntitySkillRequestHandler
    {
        // These parameters are for giving the player the possibility of choosing various action in one go
        // even in between animations; For AI this is not necessary since the AI can just decide in the fly.
        // More advance AI could inject a sequence of action in the queue so it feels more intelligent
        IEnumerator<float> HandleRequestAction(SkillValuesHolders skillValues);

    }
}
