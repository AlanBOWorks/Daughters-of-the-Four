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
    public class CombatEntityFixedEvents :IEventListenerHandler<SkillValuesHolders,CombatingEntity,SkillComponentResolution>
    {
#if UNITY_EDITOR
        [ShowInInspector,TextArea]
        private const string _behaviourExplanation = "A Sigleton event caller for fixed events that occurs always in the same " +
                                                     "pattern (It calls the entities's Events in the required order as a consequence)";
#endif
       

        public void OnInitiativeTrigger(CombatingEntity element)
        {
            element.CombatStats.GetBurstStat().SelfBurst.ResetAsBurst();
            element.SkillUsageTracker.ResetOnStartSequence();
            element.SkillsHolder.TickCoolDowns();
            element.EventsHolder.OnInitiativeTrigger(element);
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
            element.CombatStats.GetBurstStat().ReceivedBurst.ResetAsBurst();
            element.GuardHandler.RemoveGuarding();
        }

        public void OnSkipActions(CombatingEntity element)
        {
            element.EventsHolder.OnSkipActions(element);
        }

        public void OnRoundFinish(CombatingEntity lastElement)
        {
            lastElement.EventsHolder.OnRoundFinish(lastElement);
            ResetTeamBurst();

            void ResetTeamBurst()
            {
                CombatSystemSingleton.VolatilePlayerTeam.DoResetBurst();
                CombatSystemSingleton.VolatileEnemyTeam.DoResetBurst();
            }
        }

       
        
        public void OnReceiveOffensiveAction(SkillValuesHolders element,ref SkillComponentResolution value)
        {
           
        }

        public void OnReceiveSupportAction(SkillValuesHolders element,ref SkillComponentResolution value)
        {
           
        }

        public void OnRecoveryReceiveAction(SkillValuesHolders element,ref SkillComponentResolution value)
        {
           
        }

        public void OnDamageReceiveAction(SkillValuesHolders element,ref SkillComponentResolution value)
        {
            
        }

        public void OnShieldLost(SkillValuesHolders element,ref SkillComponentResolution value)
        {
            
        }

        public void OnHealthLost(SkillValuesHolders element,ref SkillComponentResolution value)
        {
           
        }

        public void OnMortalityDeath(SkillValuesHolders element,ref SkillComponentResolution value)
        {
            var target = element.Target;
            target.Team.Events.OnMemberDeath(target);
        }
    }
}
