using System;
using CombatEffects;
using CombatEntity;
using CombatSkills;
using CombatSystem.Events;
using CombatTeam;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace CombatSystem.Events
{
    // This is invoked by the CombatEvents (implicitly by being subscribed to it)
    public class CombatEntityFixedEvents : ICombatPreparationListener, IEventListenerHandler<SkillValuesHolders,CombatingEntity,SkillComponentResolution>
    {
#if UNITY_EDITOR
        [ShowInInspector,TextArea]
        private const string _behaviourExplanation = "A Sigleton event caller for fixed events that occurs always in the same " +
                                                     "pattern (It calls the entities's Events in the required order as a consequence)";
#endif
        private CombatingTeam _playerTeam;
        private CombatingTeam _enemyTeam;


        public void OnPreparationCombat(CombatingTeam playerTeam, CombatingTeam enemyTeam)
        {
            _playerTeam = playerTeam;
            _enemyTeam = enemyTeam;
        }

        public void OnAfterLoadScene()
        {
        }


        public void OnInitiativeTrigger(CombatingEntity element)
        {
            element.EventsHolder.OnInitiativeTrigger(element);
            element.SkillUsageTracker.ResetOnStartSequence();
        }

        public void OnDoMoreActions(CombatingEntity element)
        {
            var stats = element.CombatStats;
            UtilsCombatStats.DecreaseActions(stats);
            element.EventsHolder.OnDoMoreActions(element);

        }

        public void OnFinishAllActions(CombatingEntity element)
        {
            element.EventsHolder.OnFinishAllActions(element);
        }

        public void OnSkipActions(CombatingEntity element)
        {
            element.EventsHolder.OnSkipActions(element);
        }

        public void OnRoundFinish(CombatingEntity lastElement)
        {
            lastElement.EventsHolder.OnRoundFinish(lastElement);
            DoResetBurst(_enemyTeam);
            DoResetBurst(_playerTeam);

            // Privates 
            void DoResetBurst(CombatingTeam team)
            {
                foreach (CombatingEntity entity in team)
                {
                    UtilsCombatStats.ResetBurstStats(entity.CombatStats);
                }
            }
        }

       
        
        public void OnReceiveOffensiveAction(SkillValuesHolders element, SkillComponentResolution value)
        {
           
        }

        public void OnReceiveSupportAction(SkillValuesHolders element, SkillComponentResolution value)
        {
           
        }

        public void OnRecoveryReceiveAction(SkillValuesHolders element, SkillComponentResolution value)
        {
           
        }

        public void OnDamageReceiveAction(SkillValuesHolders element, SkillComponentResolution value)
        {
            
        }

        public void OnShieldLost(SkillValuesHolders element, SkillComponentResolution value)
        {
            
        }

        public void OnHealthLost(SkillValuesHolders element, SkillComponentResolution value)
        {
           
        }

        public void OnMortalityDeath(SkillValuesHolders element, SkillComponentResolution value)
        {
            var target = element.Target;
            target.Team.Events.OnMemberDeath(target);
        }
    }
}
