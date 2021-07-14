using System;
using System.Collections.Generic;
using Characters;
using MEC;
using UnityEngine;

namespace _CombatSystem
{
    public class ActionsLooper
    {
        private readonly ITempoTriggerHandler _triggerHandler;
        public ActionsLooper(ITempoTriggerHandler triggerHandler)
        {
            _triggerHandler = triggerHandler;
        }

        private CombatingEntity _currentEntity;
        private CharacterCombatData _currentStats;

        public void StartUsingActions(CombatingEntity entity)
        {
            _currentEntity = entity;
            _currentStats = entity.CombatStats;
            _currentStats.RefillInitiativeActions();
            if(_currentStats.ActionsLefts > 0)
                _triggerHandler.OnInitiativeTrigger(entity);
            else
            {
                AllActionsFinish(); //If the entity doesn't have actions then just invoke OnFinish
            }
        }

        /// <summary>
        /// Use when the Action is completed
        /// </summary>
        public void ActionDone()
        {
            _currentStats.ActionsLefts--;
            if (_currentStats.ActionsLefts > 0)
            {
                _triggerHandler.OnDoMoreActions(_currentEntity);
            }
            else
            {
                _currentStats.ActionsLefts = 0;
                // It's not equals 0 because it could have a recovering skill that increments the 
                // initiative on Action
                
                AllActionsFinish();
            }
        }

        private void AllActionsFinish()
        {
            _triggerHandler.OnFinisAllActions(_currentEntity);
            CombatSystemSingleton.TempoHandler.ResumeFromTempoTrigger();
        }
    }
}
